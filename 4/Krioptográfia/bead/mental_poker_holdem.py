"""
Mental Poker – Texas Hold'em szimuláció
========================================
Protokoll lépések:
  1. Közös p prím és g generátor; 52 kártyát tartalmazó eredeti pakli
  2. Mindkét játékos shuffle() → kétszeresen titkosított közös pakli
  3. Pre-flop: 2-2 privát lap kiosztása (hole cards)
     - Kétféle titkosítási réteg → csak a lap „tulajdonosa" és a másik fél
       együtt tudja visszafejteni
  4. Flop (3 közös lap), Turn (1), River (1) – közösek, mindkét fél reveálja
  5. Licitálás (egyszerűsített): minden utcán raise/call/fold
  6. Showdown: CSAK akkor fedik fel a privát kulcsot, ha mindkét fél megadta
"""

import random
import math
from typing import Optional
from collections import Counter
from itertools import combinations

# ─── Import a core modulból (vagy inline definíció) ───────────────────────────

def extended_gcd(a, b):
    if a == 0: return b, 0, 1
    g, x, y = extended_gcd(b % a, a)
    return g, y - (b // a) * x, x

def mod_inverse(a, m):
    g, x, _ = extended_gcd(a, m)
    return x % m if g == 1 else None

def is_prime(n, k=20):
    if n < 2: return False
    if n in (2, 3): return True
    if n % 2 == 0: return False
    r, d = 0, n - 1
    while d % 2 == 0: r += 1; d //= 2
    for _ in range(k):
        a = random.randrange(2, n - 1)
        x = pow(a, d, n)
        if x in (1, n - 1): continue
        for _ in range(r - 1):
            x = pow(x, 2, n)
            if x == n - 1: break
        else: return False
    return True

def generate_safe_prime(bits=128):
    print(f"  [*] Safe prím generálása ({bits} bit)...", flush=True)
    while True:
        q = random.getrandbits(bits - 1) | (1 << (bits - 2)) | 1
        if is_prime(q):
            p = 2 * q + 1
            if is_prime(p): return p

def find_generator(p):
    q = (p - 1) // 2
    for g in range(2, p):
        if pow(g, 2, p) != 1 and pow(g, q, p) != 1: return g

def generate_private_key(p):
    phi = p - 1
    while True:
        x = random.randrange(2, p - 1)
        if math.gcd(x, phi) == 1: return x

def prepare_deck(p, size=52):
    lo, hi = p // 2 + 1, p - 2
    chosen = set()
    while len(chosen) < size:
        chosen.add(random.randint(lo, hi))
    return list(chosen)

# ─── Kártya megjelenítés ──────────────────────────────────────────────────────

RANKS_STR = ["2","3","4","5","6","7","8","9","10","J","Q","K","A"]
SUITS_STR  = ["♠","♥","♦","♣"]

def card_display(idx):
    return f"{RANKS_STR[idx % 13]}{SUITS_STR[idx // 13]}"

def original_to_index(val, orig):
    return orig.index(val)

# ─── Kézértékelő (7 lapból a legjobb 5) ──────────────────────────────────────

def evaluate_5(indices):
    ranks = sorted([i % 13 for i in indices], reverse=True)
    suits = [i // 13 for i in indices]
    flush = len(set(suits)) == 1
    straight = (max(ranks) - min(ranks) == 4 and len(set(ranks)) == 5) or \
               (sorted(ranks) == [0,1,2,3,12])
    cnt = Counter(ranks)
    counts = sorted(cnt.values(), reverse=True)
    top = max(ranks)

    if flush and straight and top == 12: return (9, "Royal Flush")
    if flush and straight:              return (8, f"Straight Flush ({top+2})")
    if counts[0] == 4:                  return (7, f"Four of a Kind ({[r for r,c in cnt.items() if c==4][0]+2})")
    if counts[:2] == [3,2]:             return (6, "Full House")
    if flush:                           return (5, f"Flush ({top+2})")
    if straight:                        return (4, f"Straight ({top+2})")
    if counts[0] == 3:                  return (3, "Three of a Kind")
    if counts[:2] == [2,2]:             return (2, "Two Pair")
    if counts[0] == 2:                  return (1, "One Pair")
    return (0, f"High Card ({top+2})")

def best_hand_from_7(all_7):
    best = (-1, "")
    for combo in combinations(all_7, 5):
        score = evaluate_5(list(combo))
        if score[0] > best[0]: best = score
    return best

# ─── Játékos ──────────────────────────────────────────────────────────────────

class Player:
    def __init__(self, name, p, chips=1000):
        self.name = name
        self.p = p
        self.chips = chips
        self.bet_total = 0   # az aktuális körben befizetett összeg
        self.folded = False
        self.x = generate_private_key(p)
        self.y = mod_inverse(self.x, p - 1)

    def encrypt(self, c): return pow(c, self.x, self.p)
    def decrypt(self, c): return pow(c, self.y, self.p)

    def shuffle(self, deck):
        s = deck[:]
        random.shuffle(s)
        return [self.encrypt(c) for c in s]

    def reveal(self, c): return self.decrypt(c)

# ─── Licitálás ────────────────────────────────────────────────────────────────

def betting_round(players, pot, blind=0, street_name=""):
    """
    Egyszerűsített licitálási kör.
    Visszatérési érték: (pot, folytatódik-e a játék)
    """
    print(f"\n  ── {street_name} licitálás ──")
    current_bet = blind
    contributions = {p.name: 0 for p in players}

    # Blind befizetés
    if blind > 0:
        for player in players:
            amount = min(blind, player.chips)
            player.chips -= amount
            player.bet_total += amount
            contributions[player.name] = amount
            pot += amount
        print(f"  [Blind] Mindkét játékos befizet {blind} chipet. Pot: {pot}")

    # Egy kör licitálás (egyszerűsített: mindkét játékos dönt)
    for i, player in enumerate(players):
        if player.folded: continue
        opponent = players[1 - i]
        to_call = current_bet - contributions[player.name]

        # AI döntés: 70% call/raise, 30% fold (de csak ha kevés chipje van)
        action_roll = random.random()
        if action_roll < 0.15 and player.chips < 200:
            player.folded = True
            print(f"  {player.name}: FOLD")
        elif action_roll < 0.45 and player.chips >= to_call + 50:
            raise_amount = random.choice([50, 100, 150])
            actual = min(raise_amount + to_call, player.chips)
            player.chips -= actual
            player.bet_total += actual
            contributions[player.name] += actual
            current_bet = contributions[player.name]
            pot += actual
            print(f"  {player.name}: RAISE +{actual}  (chips: {player.chips}, pot: {pot})")
        else:
            if to_call > 0:
                actual = min(to_call, player.chips)
                player.chips -= actual
                player.bet_total += actual
                contributions[player.name] += actual
                pot += actual
                print(f"  {player.name}: CALL {actual}  (chips: {player.chips}, pot: {pot})")
            else:
                print(f"  {player.name}: CHECK")

    active = [p for p in players if not p.folded]
    return pot, len(active) > 1


# ─── Texas Hold'em főprogram ─────────────────────────────────────────────────

def run_texas_holdem():
    SEP = "═" * 65
    print(SEP)
    print("  MENTAL POKER – TEXAS HOLD'EM SZIMULÁCIÓ")
    print(SEP)

    # 1. Paraméterek
    print("\n[1] Kriptográfiai paraméterek inicializálása...")
    p = generate_safe_prime(bits=128)
    print(f"  p = {p}")

    # 2. Eredeti pakli
    original_deck = prepare_deck(p, 52)
    print(f"\n[2] Eredeti pakli: 52 egyedi véletlen szám (p/2, p-1) ∈ Zp")

    # 3. Játékosok
    alice = Player("Alice", p, chips=1000)
    bob   = Player("Bob",   p, chips=1000)
    players = [alice, bob]
    print(f"\n[3] Játékosok: Alice (1000 chip), Bob (1000 chip)")
    print(f"    Kulcsok generálva (privátok titokban maradnak)")

    # 4. Kétszeres shuffle
    print("\n[4] Pakli titkosított keverése...")
    deck_a = alice.shuffle(original_deck)
    deck_b = bob.shuffle(deck_a)
    shared = deck_b
    print(f"  Alice → Bob sorrendben keverve és titkosítva.")

    ptr = 0  # következő kártya pozíciója

    def deal_card():
        nonlocal ptr
        c = shared[ptr]; ptr += 1
        return c

    def fully_reveal(enc_card):
        """Két réteg eltávolítása: mindkét játékos reveálja."""
        half = bob.reveal(enc_card)
        return alice.reveal(half)

    def decode(val):
        return original_to_index(val, original_deck)

    # 5. Hole cards kiosztása
    print("\n[5] Pre-flop: lyuk lapok kiosztása...")
    alice_enc = [deal_card(), deal_card()]
    bob_enc   = [deal_card(), deal_card()]

    # Alice lapjai: Bob reveálja, majd Alice
    alice_half = [bob.reveal(c) for c in alice_enc]
    alice_plain = [alice.reveal(c) for c in alice_half]
    alice_idx = [decode(v) for v in alice_plain]

    # Bob lapjai: Alice reveálja, majd Bob
    bob_half = [alice.reveal(c) for c in bob_enc]
    bob_plain = [bob.reveal(c) for c in bob_half]
    bob_idx = [decode(v) for v in bob_plain]

    print(f"  Alice lyuk lapjai: {[card_display(i) for i in alice_idx]}  (Bob nem látja)")
    print(f"  Bob lyuk lapjai:   {[card_display(i) for i in bob_idx]}  (Alice nem látja)")

    # 6. Pre-flop licit
    pot, ongoing = betting_round(players, 0, blind=20, street_name="Pre-flop")
    if not ongoing:
        winner = next(p for p in players if not p.folded)
        print(f"\n  🏆 {winner.name} nyert {pot} chipet (ellenfél foldolt).")
        return

    # 7. Burn card + Flop
    deal_card()  # burn
    print("\n[6] Flop (3 közös lap)...")
    flop_enc = [deal_card(), deal_card(), deal_card()]
    flop_idx = [decode(fully_reveal(c)) for c in flop_enc]
    print(f"  Flop: {[card_display(i) for i in flop_idx]}")

    pot, ongoing = betting_round(players, pot, street_name="Flop")
    if not ongoing:
        winner = next(p for p in players if not p.folded)
        print(f"\n  🏆 {winner.name} nyert {pot} chipet (ellenfél foldolt).")
        return

    # 8. Turn
    deal_card()  # burn
    print("\n[7] Turn (4. közös lap)...")
    turn_enc = deal_card()
    turn_idx = decode(fully_reveal(turn_enc))
    print(f"  Turn: {card_display(turn_idx)}")

    pot, ongoing = betting_round(players, pot, street_name="Turn")
    if not ongoing:
        winner = next(p for p in players if not p.folded)
        print(f"\n  🏆 {winner.name} nyert {pot} chipet (ellenfél foldolt).")
        return

    # 9. River
    deal_card()  # burn
    print("\n[8] River (5. közös lap)...")
    river_enc = deal_card()
    river_idx = decode(fully_reveal(river_enc))
    print(f"  River: {card_display(river_idx)}")

    pot, ongoing = betting_round(players, pot, street_name="River")
    if not ongoing:
        winner = next(p for p in players if not p.folded)
        print(f"\n  🏆 {winner.name} nyert {pot} chipet (ellenfél foldolt).")
        return

    # 10. Showdown – privát kulcsok felfedése (CSAK ha mindenki megadta a tétet)
    print(f"\n[9] SHOWDOWN – mindkét játékos megadta a tétet, privát kulcsok felfedése")
    print(f"  Alice privát kulcsa (x): {alice.x}")
    print(f"  Bob privát kulcsa (x):   {bob.x}")

    community = flop_idx + [turn_idx, river_idx]
    print(f"\n  Közös lapok: {[card_display(i) for i in community]}")

    alice_all = alice_idx + community
    bob_all   = bob_idx   + community

    alice_score, alice_hand = best_hand_from_7(alice_all)
    bob_score,   bob_hand   = best_hand_from_7(bob_all)

    print(f"\n  Alice: {[card_display(i) for i in alice_idx]} + közös  →  {alice_hand}")
    print(f"  Bob:   {[card_display(i) for i in bob_idx]} + közös  →  {bob_hand}")

    print("\n" + "─" * 65)
    if alice_score > bob_score:
        alice.chips += pot
        print(f"  🏆  GYŐZTES: ALICE  ({alice_hand})  |  Nyeremény: {pot} chip")
    elif bob_score > alice_score:
        bob.chips += pot
        print(f"  🏆  GYŐZTES: BOB  ({bob_hand})  |  Nyeremény: {pot} chip")
    else:
        alice.chips += pot // 2
        bob.chips   += pot // 2
        print(f"  🤝  DÖNTETLEN – Pot felosztva")

    print(f"\n  Végső állás:  Alice: {alice.chips} chip  |  Bob: {bob.chips} chip")
    print(SEP)


if __name__ == "__main__":
    run_texas_holdem()
