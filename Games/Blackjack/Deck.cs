using System;
using System.Collections.Generic;
using System.Linq;

namespace Chuds2Chads.Games.Blackjack
{
    public class Deck
    {
        private List<Card> _cards;
        private Random _random = new();

        public Deck()
        {
            _cards = new List<Card>();

            foreach (Suit suit in Enum.GetValues(typeof(Suit)))
            {
                foreach (Rank rank in Enum.GetValues(typeof(Rank)))
                {
                    _cards.Add(new Card
                    {
                        Suit = suit,
                        Rank = rank
                    });
                }
            }

            Shuffle();
        }

        public void Shuffle()
        {
            _cards = _cards.OrderBy(x => _random.Next()).ToList();
        }

        public Card DrawCard()
        {
            if (_cards.Count == 0)
                throw new InvalidOperationException("Deck is empty.");

            var card = _cards[0];
            _cards.RemoveAt(0);
            return card;
        }
    }
}
