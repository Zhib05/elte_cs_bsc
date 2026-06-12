using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PackOpening
{
    public class MyClub
    {
        private List<Card> cards;

        public MyClub()
        {
            this.cards = new List<Card>();
        }

        public void JoinMyClub(Card card)
        {
            cards.Add(card);
        }

        public int AverageRating()
        {
            if (cards.Count == 0)
            {
                return 0;
            }

            int ratingSum = 0;
            foreach(Card card in cards)
            {
                ratingSum += card.Rating();
            }
            return ratingSum / cards.Count;
        }

        public int EnglishPlayersCount()
        {
            int count = 0;
            foreach(Card card in cards)
            {
                if (card.player!.nationallity == Nation.ENGLAND)
                {
                    count++;
                }
            }
            return count;
        }

        public List<string> AllEnglishDefenders()
        {
            List<string> defenderNames = new List<string>();
            foreach(Card card in cards)
            {
                if (card.player!.nationallity == Nation.ENGLAND && card.isDefender())
                {
                    defenderNames.Add(card.player.name);
                }
            }
            return defenderNames;
        }

        public bool BestSpanishPlayer(out string spanishPlayerName)
        {
            bool l = false;
            int bestStat = 0;
            spanishPlayerName = "";
            foreach(Card card in cards)
            {
                if (card.player!.nationallity == Nation.SPAIN)
                {
                    if (card.Rating() > bestStat)
                    {
                        bestStat = card.Rating();
                        spanishPlayerName = card.player.name;
                        l = true;
                    }
                }
            }
            return l;
        }

        public bool MostExpensiveDefenderPrice(out int expensiveDefender)
        {
            bool l = false;
            expensiveDefender = 0;
            foreach(Card card in cards)
            {
                if (card.isDefender())
                {
                    if (card.cost > expensiveDefender)
                    {
                        expensiveDefender = card.cost;
                        l = true;
                    }
                }
            }
            return l;
        }

        public bool BestRarePlayerName(out string bestRareName)
        {
            bool l = false;
            int rating = 0;
            bestRareName = "";
            foreach(Card card in cards)
            {
                if (card.cardType == Rare.Instance() && card.Rating() > rating)
                {
                    l = true;
                    rating = card.Rating();
                    bestRareName = card.player!.name;
                }
            }
            return l;
        }

        public bool MostExpensiveSpecialCardCost(out int specialPrice)
        {
            bool l = false;
            specialPrice = 0;
            foreach(Card card in cards)
            {
                if (card.cardType != Rare.Instance() && card.cost > specialPrice)
                {
                    l = true;
                    specialPrice = card.cost;
                }
            }
            return l;
        }
    }
}
