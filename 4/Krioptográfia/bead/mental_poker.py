"""
Mental Poker implementáció (SRA protokoll)
---------------------------------------------------------
Tartalmazza az 5-lapos cserés pókert és a Texas Hold'em-et,
beleértve a kártyák kiértékelését és a nyertes kihirdetését.
"""

import random
import math
import itertools

# ─── 1. Kriptográfiai alapok ───────────────────────────────────────────

P = 4787  # Biztonságos prím
G = 2     # Generátor
PHI = P - 1

def mod_inverse(a: int, m: int) -> int:
    """Kiszámolja 'a' moduláris inverzét 'm'-re nézve."""
    return pow(a, -1, m)

class Player:
    def __init__(self, name: str):
        self.name = name
        # Privát kulcs (x) választása (relatív prím PHI-vel)
        while True:
            self.x = random.randrange(2, PHI)
            if math.gcd(self.x, PHI) == 1:
                break
        
        # Inverz kulcs (y) kiszámítása
        self.y = mod_inverse(self.x, PHI)

    def shuffle(self, deck: list[int]) -> list[int]:
        """Összekeveri a paklit és titkosítja az elemeket: d -> d^x mod p"""
        shuffled = deck.copy()
        random.shuffle(shuffled)
        return [pow(card, self.x, P) for card in shuffled]

    def reveal(self, card: int) -> int:
        """Eltávolítja a játékos titkosítását: c -> c^y mod p"""
        return pow(card, self.y, P)

# ─── 2. Kártyakezelés és Kiértékelés ───────────────────────────────────

def prepare_deck() -> list[int]:
    """Generál 52 egyedi kártyát a (P/2, P-1) intervallumból."""
    lo = P // 2 + 1
    hi = P - 2
    return random.sample(range(lo, hi + 1), 52)

def display_cards(indices: list[int]) -> str:
    """Kártyaindexek (0-51) átalakítása olvasható szöveggé."""
    ranks = ["2","3","4","5","6","7","8","9","T","J","Q","K","A"]
    suits = ["♠", "♥", "♦", "♣"]
    return "[" + ", ".join(f"{ranks[i % 13]}{suits[i // 13]}" for i in indices) + "]"

def evaluate_hand(indices: list[int]):
    """
    Rövid kártyaértékelő (Párok, Drill, Póker, Magas lap alapján).
    Visszaad egy listát, amivel a kezek közvetlenül összehasonlíthatók.
    """
    ranks = [i % 13 for i in indices]
    freq = {}
    for r in ranks:
        freq[r] = freq.get(r, 0) + 1
    # Rendezzük: először gyakoriság szerint (csökkenő), majd a lap értéke szerint
    return sorted(freq.items(), key=lambda x: (x[1], x[0]), reverse=True)

def best_holdem_hand(indices: list[int]):
    """Kiválasztja a legjobb 5 lapot a 7-ből Texas Hold'em esetén."""
    return max(evaluate_hand(combo) for combo in itertools.combinations(indices, 5))

# ─── 3. Szimulációk ────────────────────────────────────────────────────

def run_five_card_draw():
    print("\n" + "="*55)
    print("  5-LAPOS CSERÉS PÓKER (MENTAL POKER)")
    print("="*55)
    
    alice = Player("Alice")
    bob = Player("Bob")
    original_deck = prepare_deck()
    
    # Keverés és titkosítás
    shared_deck = bob.shuffle(alice.shuffle(original_deck))
    
    # Lapok kiosztása (Kereszt-felfedéssel és saját felfedéssel)
    alice_plain = [alice.reveal(bob.reveal(c)) for c in shared_deck[0:5]]
    bob_plain = [bob.reveal(alice.reveal(c)) for c in shared_deck[5:10]]
    
    alice_idx = [original_deck.index(c) for c in alice_plain]
    bob_idx = [original_deck.index(c) for c in bob_plain]
    
    print(f"Alap kiosztás Alice: {display_cards(alice_idx)}")
    print(f"Alap kiosztás Bob:   {display_cards(bob_idx)}")
    
    # Csere fázis (Alice lecseréli az első lapját)
    print("\n[Csere] Alice cserél 1 lapot...")
    new_card_enc = shared_deck[10]
    new_card_half = bob.reveal(new_card_enc)       # Bob átadja felfedve
    new_card_plain = alice.reveal(new_card_half)   # Alice is felfedi magának
    alice_idx[0] = original_deck.index(new_card_plain)
    
    print("[Csere] Bob nem cserél lapot.")
    
    # Showdown és eredményhirdetés
    print("\n--- SHOWDOWN ---")
    print(f"Alice privát kulcsa (x): {alice.x}")
    print(f"Bob privát kulcsa (x):   {bob.x}")
    print(f"Végső lapok Alice: {display_cards(alice_idx)}")
    print(f"Végső lapok Bob:   {display_cards(bob_idx)}")
    
    score_alice = evaluate_hand(alice_idx)
    score_bob = evaluate_hand(bob_idx)
    
    print("\nEREDMÉNY:")
    if score_alice > score_bob:
        print("🏆 ALICE nyert!")
    elif score_bob > score_alice:
        print("🏆 BOB nyert!")
    else:
        print("🤝 Döntetlen!")

def run_texas_holdem():
    print("\n" + "="*55)
    print("  TEXAS HOLD'EM (MENTAL POKER)")
    print("="*55)
    
    alice = Player("Alice")
    bob = Player("Bob")
    original_deck = prepare_deck()
    
    shared_deck = bob.shuffle(alice.shuffle(original_deck))
    
    def decode_card(enc_card):
        """Mindkét titkosítás levétele egy adott kártyáról."""
        return original_deck.index(alice.reveal(bob.reveal(enc_card)))
    
    # Lyuk lapok kiosztása
    alice_hole = [decode_card(c) for c in shared_deck[0:2]]
    bob_hole = [decode_card(c) for c in shared_deck[2:4]]
    print(f"Alice lyuk lapjai: {display_cards(alice_hole)}")
    print(f"Bob lyuk lapjai:   {display_cards(bob_hole)}")
    
    # Egyszerűsített Licit -> Mindenki megadja (CALL)
    print("\n[Licit] Mindkét játékos CALL. Jöhetnek a közös lapok!")
    
    # Közös lapok
    flop = [decode_card(c) for c in shared_deck[5:8]]
    turn = [decode_card(shared_deck[9])]
    river = [decode_card(shared_deck[11])]
    
    community = flop + turn + river
    print(f"Közös lapok az asztalon: {display_cards(community)}")
    
    # Showdown (Mivel mindenki tartotta a tétet)
    print("\n--- SHOWDOWN ---")
    # A privát értékeket itt fedjük fel egymásnak
    print(f"Mindenki megadta a tétet, privát értékek felfedése kötelező!")
    print(f"Alice (x): {alice.x} | Bob (x): {bob.x}")
    
    alice_full = alice_hole + community
    bob_full = bob_hole + community
    
    score_alice = best_holdem_hand(alice_full)
    score_bob = best_holdem_hand(bob_full)
    
    print("\nEREDMÉNY:")
    if score_alice > score_bob:
        print(f"🏆 ALICE nyerte a kört a legjobb 5 lappal!")
    elif score_bob > score_alice:
        print(f"🏆 BOB nyerte a kört a legjobb 5 lappal!")
    else:
        print("🤝 Döntetlen (Split pot)!")

if __name__ == "__main__":
    run_five_card_draw()
    run_texas_holdem()