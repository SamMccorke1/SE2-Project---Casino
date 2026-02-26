using System;
using System.Collections.Generic;
using System.Linq;

namespace Chuds2Chads.Games.Blackjack
{
    public class BlackjackEngine
    {
        public List<int> PlayerHand { get; private set; } = new();
        public List<int> DealerHand { get; private set; } = new();

        public bool GameOver { get; private set; }
        public bool PlayerTurn { get; private set; } = true;

        public string GameMessage { get; private set; } = "";

        public void StartGame()
        {
            PlayerHand.Clear();
            DealerHand.Clear();
            GameOver = false;
            PlayerTurn = true;

            PlayerHand.Add(DrawCard());
            PlayerHand.Add(DrawCard());

            DealerHand.Add(DrawCard());
            DealerHand.Add(DrawCard());
        }

        public void Hit()
        {
            if (!PlayerTurn || GameOver)
                return;

            PlayerHand.Add(DrawCard());

            if (PlayerScore > 21)
            {
                GameMessage = "Bust! Dealer wins.";
                GameOver = true;
                PlayerTurn = false;
            }
        }

        public void Stand()
        {
            if (!PlayerTurn || GameOver)
                return;

            PlayerTurn = false;

            while (DealerScore < 17)
            {
                DealerHand.Add(DrawCard());
            }

            DetermineWinner();
            GameOver = true;
        }

        private void DetermineWinner()
        {
            if (DealerScore > 21 || PlayerScore > DealerScore)
                GameMessage = "You win!";
            else if (PlayerScore == DealerScore)
                GameMessage = "Push!";
            else
                GameMessage = "Dealer wins.";
        }

        private int DrawCard()
        {
            return Random.Shared.Next(1, 11);
        }

        public int PlayerScore => PlayerHand.Sum();
        public int DealerScore => DealerHand.Sum();
    }
}