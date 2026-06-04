"""
Mental Poker – SRA (Shamir–Rivest–Adleman) protokoll implementációja
=====================================================================
Struktúra:
  - Közös p prím és g generátor (Zp*-ban)
  - Kártyák: 52 véletlen szám ∈ (p/2, p-1)
  - shuffle(deck): összekeveri + titkosítja: d → g^(x·d) mod p
    (valójában: d → d^x mod p, mert az SRA protokollban x az exp)
  - reveal(card): c → c^y mod p ahol x·y ≡ 1 (mod p-1)

Megjegyzés: A klasszikus SRA Mental Pokerben a titkosítás:
  E_x(d) = d^x mod p   (commutative encryption)
  D_x(d) = d^y mod p   ahol y = x^{-1} mod (p-1)
"""

import random
import math
from typing import Optional

# ── Segédfüggvények ─────────────────────────────────────────────────────────

def mod_inverse(a: int, m: int) -> Optional[int]:
    """Kiterjesztett Euklidészi algoritmus: a^{-1} mod m, vagy None ha nem létezik."""
    g, x, _ = extended_gcd(a, m)
    return x % m if g == 1 else None

def extended_gcd(a: int, b: int):
    if a == 0:
        return b, 0, 1
    g, x, y = extended_gcd(b % a, a)
    return g, y - (b // a) * x, x

def is_prime(n: int, k: int = 20) -> bool:
    """Miller–Rabin prímteszt."""
    if n < 2: return False
    if n == 2 or n == 3: return True
    if n % 2 == 0: return False
    r, d = 0, n - 1
    while d % 2 == 0:
        r += 1
        d //= 2
    for _ in range(k):
        a = random.randrange(2, n - 1)
        x = pow(a, d, n)
        if x in (1, n - 1): continue
        for _ in range(r - 1):
            x = pow(x, 2, n)
            if x == n - 1: break
        else:
            return False
    return True

def generate_safe_prime(bits: int = 128) -> int:
    """Generál egy safe prime-ot: p = 2q+1 ahol q is prím."""
    print(f"  [*] Safe prím generálása ({bits} bit)...")
    while True:
        q = random.getrandbits(bits - 1) | (1 << (bits - 2)) | 1
        if is_prime(q):
            p = 2 * q + 1
            if is_prime(p):
                return p

def find_generator(p: int) -> int:
    """Talál egy primitív gyököt mod p (safe prime esetén hatékony)."""
    q = (p - 1) // 2
    for g in range(2, p):
        if pow(g, 2, p) != 1 and pow(g, q, p) != 1:
            return g
    raise ValueError("Nem található generátor")

def generate_private_key(p: int) -> int:
    """Generál egy x ∈ [2, p-2] értéket, amelynek van inverze mod (p-1)."""
    phi = p - 1
    while True:
        x = random.randrange(2, p - 1)
        if math.gcd(x, phi) == 1:
            return x


# ── Játékos osztály ──────────────────────────────────────────────────────────

class Player:
    def __init__(self, name: str, p: int):
        self.name = name
        self.p = p
        self.x: Optional[int] = None   # privát kulcs
        self.y: Optional[int] = None   # inverz kulcs
        self._generate_keys()

    def _generate_keys(self):
        self.x = generate_private_key(self.p)
        self.y = mod_inverse(self.x, self.p - 1)
        assert self.y is not None
        assert (self.x * self.y) % (self.p - 1) == 1

    def encrypt(self, card: int) -> int:
        """Titkosít: c → c^x mod p"""
        return pow(card, self.x, self.p)

    def decrypt(self, card: int) -> int:
        """Visszafejt: c → c^y mod p"""
        return pow(card, self.y, self.p)

    def shuffle(self, deck: list[int]) -> list[int]:
        """Összekeveri és titkosítja a paklibeli kártyákat."""
        shuffled = deck[:]
        random.shuffle(shuffled)
        return [self.encrypt(card) for card in shuffled]

    def reveal(self, card: int) -> int:
        """Részleges felfedés: eltávolítja a saját titkosítási réteget."""
        return self.decrypt(card)

    def reveal_key(self) -> int:
        """Felfedi a privát kulcsot (showdown esetén)."""
        return self.x


# ── Pakli előkészítése ───────────────────────────────────────────────────────

def prepare_deck(p: int, size: int = 52) -> list[int]:
    """52 egyedi véletlen számot generál a (p/2, p-1) intervallumból."""
    lo = p // 2 + 1
    hi = p - 2
    assert hi - lo >= size, f"p túl kicsi a {size} egyedi kártyához"
    # random.sample nem működik nagy range-re, ezért kézzel szúrjuk ki
    chosen = set()
    while len(chosen) < size:
        v = random.randint(lo, hi)
        chosen.add(v)
    return list(chosen)


# ══════════════════════════════════════════════════════════════════════════════
# 5-LAPOS CSERE PÓKER SZIMULÁCIÓ
# ══════════════════════════════════════════════════════════════════════════════

# Kézértékek (egyszerűsített – csak a kombináció típusa)
HAND_RANKS = {
    "Royal Flush": 9, "Straight Flush": 8, "Four of a Kind": 7,
    "Full House": 6, "Flush": 5, "Straight": 4,
    "Three of a Kind": 3, "Two Pair": 2, "One Pair": 1, "High Card": 0
}

def card_index_to_rank_suit(idx: int):
    """0-51 → (rank_str, suit_str)"""
    suits = ["♠", "♥", "♦", "♣"]
    ranks = ["2","3","4","5","6","7","8","9","10","J","Q","K","A"]
    return ranks[idx % 13], suits[idx // 13]

def card_display(idx: int) -> str:
    r, s = card_index_to_rank_suit(idx)
    return f"{r}{s}"

def original_to_card_index(orig_value: int, original_deck: list[int]) -> int:
    """Az eredeti pakliból visszakeresi a kártya indexét (0-51)."""
    return original_deck.index(orig_value)

def evaluate_hand(indices: list[int]) -> tuple[int, str]:
    """Kiértékeli az 5 lapból álló kezet; visszaadja (pontszám, leírás)."""
    ranks_num = sorted([i % 13 for i in indices], reverse=True)
    suits_set = [i // 13 for i in indices]
    is_flush = len(set(suits_set)) == 1
    is_straight = (max(ranks_num) - min(ranks_num) == 4 and len(set(ranks_num)) == 5) or \
                  (sorted(ranks_num) == [0, 1, 2, 3, 12])  # A-2-3-4-5
    from collections import Counter
    cnt = Counter(ranks_num)
    counts = sorted(cnt.values(), reverse=True)
    top_rank = max(ranks_num)

    if is_flush and is_straight and top_rank == 12:
        return (HAND_RANKS["Royal Flush"], "Royal Flush")
    if is_flush and is_straight:
        return (HAND_RANKS["Straight Flush"], f"Straight Flush ({top_rank+2} high)")
    if counts[0] == 4:
        return (HAND_RANKS["Four of a Kind"], f"Four of a Kind ({[r for r,c in cnt.items() if c==4][0]+2}s)")
    if counts[:2] == [3, 2]:
        return (HAND_RANKS["Full House"], "Full House")
    if is_flush:
        return (HAND_RANKS["Flush"], f"Flush ({top_rank+2} high)")
    if is_straight:
        return (HAND_RANKS["Straight"], f"Straight ({top_rank+2} high)")
    if counts[0] == 3:
        return (HAND_RANKS["Three of a Kind"], "Three of a Kind")
    if counts[:2] == [2, 2]:
        return (HAND_RANKS["Two Pair"], "Two Pair")
    if counts[0] == 2:
        return (HAND_RANKS["One Pair"], "One Pair")
    return (HAND_RANKS["High Card"], f"High Card ({top_rank+2})")


def run_five_card_draw():
    print("=" * 65)
    print("  MENTAL POKER – 5 LAPOS CSERE PÓKER SZIMULÁCIÓ")
    print("=" * 65)

    # ── 1. Paraméterek ──────────────────────────────────────────────
    print("\n[1] Kriptográfiai paraméterek inicializálása...")
    p = generate_safe_prime(bits=128)
    g = find_generator(p)
    print(f"  p = {p}")
    print(f"  g = {g}")

    # ── 2. Eredeti pakli ────────────────────────────────────────────
    original_deck = prepare_deck(p, 52)
    print(f"\n[2] Eredeti pakli előkészítve (52 kártya, értékek: {original_deck[:3]}...)")

    # ── 3. Játékosok ────────────────────────────────────────────────
    alice = Player("Alice", p)
    bob   = Player("Bob",   p)
    print(f"\n[3] Játékosok kulcsai generálva (privát értékek titokban maradnak)")

    # ── 4. Kétszeres shuffle ────────────────────────────────────────
    print("\n[4] Pakli keverése...")
    deck_after_alice = alice.shuffle(original_deck)
    print(f"  Alice megkeverte és titkosította a paklit.")
    deck_after_bob = bob.shuffle(deck_after_alice)
    print(f"  Bob megkeverte és titkosította a paklit.")
    shared_deck = deck_after_bob  # mindkét titkosítás aktív

    # ── 5. Lapok kiosztása ──────────────────────────────────────────
    print("\n[5] Lapok kiosztása (5-5 lap, kétszer titkosítva)...")
    alice_encrypted = shared_deck[:5]
    bob_encrypted   = shared_deck[5:10]

    # ── 6. Kereszt-felfedés ─────────────────────────────────────────
    # Alice adja át Bobnak a saját lapjait → Bob alkalmazza a reveal-t
    alice_half_revealed = [bob.reveal(c) for c in alice_encrypted]
    # Bob adja át Alice-nek a saját lapjait → Alice alkalmazza a reveal-t
    bob_half_revealed   = [alice.reveal(c) for c in bob_encrypted]

    # ── 7. Saját réteg eltávolítása ─────────────────────────────────
    alice_plain = [alice.reveal(c) for c in alice_half_revealed]
    bob_plain   = [bob.reveal(c) for c in bob_half_revealed]

    def decode(plain_vals):
        return [original_to_card_index(v, original_deck) for v in plain_vals]

    alice_indices = decode(alice_plain)
    bob_indices   = decode(bob_plain)

    print(f"\n  Alice lapjai: {[card_display(i) for i in alice_indices]}")
    print(f"  Bob lapjai:   {[card_display(i) for i in bob_indices]}")

    # ── 8. Csere (Alice cserél max 2 lapot, Bob max 2 lapot) ────────
    print("\n[6] Csere fázis...")
    deck_ptr = 10  # következő kártya indexe

    for player_name, player, other_player, indices_ref in [
        ("Alice", alice, bob, alice_indices),
        ("Bob",   bob,  alice, bob_indices)
    ]:
        swap_count = random.randint(0, 2)
        if swap_count == 0:
            print(f"  {player_name} nem cserél lapot.")
            continue
        swap_pos = random.sample(range(5), swap_count)
        new_encrypted = shared_deck[deck_ptr:deck_ptr + swap_count]
        deck_ptr += swap_count
        # Másik játékos eltávolítja saját rétegét
        new_half = [other_player.reveal(c) for c in new_encrypted]
        # Játékos eltávolítja saját rétegét
        new_plain = [player.reveal(c) for c in new_half]
        new_idx = decode(new_plain)
        for i, pos in enumerate(swap_pos):
            indices_ref[pos] = new_idx[i]
        print(f"  {player_name} lecserélt {swap_count} lapot ({swap_pos} pozíció(k)).")

    print(f"\n  Alice végső lapjai: {[card_display(i) for i in alice_indices]}")
    print(f"  Bob végső lapjai:   {[card_display(i) for i in bob_indices]}")

    # ── 9. Showdown ─────────────────────────────────────────────────
    print("\n[7] Showdown – privát értékek felfedése és kézértékelés...")
    print(f"  Alice x-je: {alice.x}")
    print(f"  Bob x-je:   {bob.x}")

    alice_score, alice_hand = evaluate_hand(alice_indices)
    bob_score,   bob_hand   = evaluate_hand(bob_indices)

    print(f"\n  Alice: {[card_display(i) for i in alice_indices]}  → {alice_hand}")
    print(f"  Bob:   {[card_display(i) for i in bob_indices]}  → {bob_hand}")

    print("\n" + "─" * 65)
    if alice_score > bob_score:
        print(f"  🏆  GYŐZTES: ALICE  ({alice_hand})")
    elif bob_score > alice_score:
        print(f"  🏆  GYŐZTES: BOB  ({bob_hand})")
    else:
        print("  🤝  DÖNTETLEN")
    print("=" * 65)


if __name__ == "__main__":
    run_five_card_draw()
