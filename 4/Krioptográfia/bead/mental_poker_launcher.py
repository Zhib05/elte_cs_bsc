"""
Mental Poker – Indítóprogram
============================
Futtatás:
  python mental_poker_launcher.py          → mindkét szimuláció
  python mental_poker_launcher.py --draw   → csak 5 lapos csere póker
  python mental_poker_launcher.py --holdem → csak Texas Hold'em
"""

import sys

def main():
    args = sys.argv[1:]

    if not args or "--draw" in args:
        from mental_poker_core import run_five_card_draw
        run_five_card_draw()

    if not args or "--holdem" in args:
        if not args:
            print("\n\n")
        from mental_poker_holdem import run_texas_holdem
        run_texas_holdem()

if __name__ == "__main__":
    main()
