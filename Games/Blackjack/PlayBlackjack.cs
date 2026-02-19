using System.Collections.Generic;
using System.Linq;

namespace Chuds2Chads.Games.Blackjack
{
    public class BlackjackGame
    {
        public List<Player> Players { get; private set; } = new();
        public Player Dealer { get; private set; }
        public Deck Deck { get; private set; }

        public int CurrentPlayerIndex { get; private set; }
        public GamePhase Phase { get; private set; }
        public bool DealerRevealed { get; private set; }

        public BlackjackGame(List<string> playerNames)
        {
            Deck = new Deck();
            Phase = GamePhase.PlayerTurn;
            CurrentPlayerIndex = 0;
            DealerRevealed = false;

            foreach (var name in playerNames.Take(4))
            {
                Players.Add(new Player
                {
                    Name = name,
                    IsDealer = false
                });
            }

            Dealer = new Player
            {
                Name = "Dealer",
                IsDealer = true
            };

            DealInitialCards();
        }

        private void DealInitialCards()
        {
            foreach (var player in Players)
            {
                player.Hit(Deck.DrawCard());
                player.Hit(Deck.DrawCard());
            }

            Dealer.Hit(Deck.DrawCard());
            Dealer.Hit(Deck.DrawCard());
        }

        public Player CurrentPlayer =>
            Players[CurrentPlayerIndex];

        public void PlayerHit()
        {
            if (Phase != GamePhase.PlayerTurn)
                return;

            var player = CurrentPlayer;
            player.Hit(Deck.DrawCard());

            if (player.IsBusted)
            {
                player.HasStood = true;
                AdvanceTurn();
            }
        }

        public void PlayerStand()
        {
            if (Phase != GamePhase.PlayerTurn)
                return;

            CurrentPlayer.HasStood = true;
            AdvanceTurn();
        }

        private void AdvanceTurn()
        {
            if (Players.All(p => p.HasStood))
            {
                StartDealerTurn();
                return;
            }

            do
            {
                CurrentPlayerIndex++;
            }
            while (CurrentPlayerIndex < Players.Count &&
                   Players[CurrentPlayerIndex].HasStood);

            if (CurrentPlayerIndex >= Players.Count)
            {
                StartDealerTurn();
            }
        }

        private void StartDealerTurn()
        {
            Phase = GamePhase.DealerTurn;
            DealerRevealed = true;

            while (Dealer.GetHandValue() < 17)
            {
                Dealer.Hit(Deck.DrawCard());
            }

            Phase = GamePhase.Finished;
        }

        public string GetResult(Player player)
        {
            if (player.IsBusted)
                return "Bust";

            if (Dealer.IsBusted)
                return "Win";

            if (player.GetHandValue() > Dealer.GetHandValue())
                return "Win";

            if (player.GetHandValue() < Dealer.GetHandValue())
                return "Lose";

            return "Push";
        }
    }
}
