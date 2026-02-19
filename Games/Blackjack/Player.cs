using System.Collections.Generic;
using System.Linq;

namespace Chuds2Chads.Games.Blackjack
{
    public class Player
    {
        public string Name { get; set; }
        public List<Card> Hand { get; set; } = new();
        public bool IsDealer { get; set; }
        public bool HasStood { get; set; }

        public int GetHandValue()
        {
            int total = Hand.Sum(c => c.Value);
            int aceCount = Hand.Count(c => c.Rank == Rank.Ace);

            while (total > 21 && aceCount > 0)
            {
                total -= 10;
                aceCount--;
            }

            return total;
        }

        public bool IsBusted => GetHandValue() > 21;

        public void Hit(Card card)
        {
            Hand.Add(card);
        }

        public void Reset()
        {
            Hand.Clear();
            HasStood = false;
        }
    }
}
