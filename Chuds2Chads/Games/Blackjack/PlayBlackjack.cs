<<<<<<< HEAD
using System;
=======
>>>>>>> 2095be124ef035a32198381033e370e98052dfda
using System.Collections.Generic;
using System.Linq;

namespace Chuds2Chads.Games.Blackjack
{
<<<<<<< HEAD
    public class PlayBlackjack
    {
        public string LobbyName { get; set; }
        public GamePhase Phase { get; private set; } = GamePhase.WaitingForPlayers;
        
        public Deck Deck { get; private set; } = new Deck();
        public Player Dealer { get; private set; } = new Player { Name = "Dealer", IsDealer = true };
        public List<Player> Players { get; set; } = new();
        
        public int CurrentPlayerIndex { get; private set; } = 0;
        public Player? CurrentPlayer => Players.ElementAtOrDefault(CurrentPlayerIndex);
        public bool DealerRevealed { get; private set; } = false;

        public Action? OnGameStateChanged;

        public PlayBlackjack(string lobbyName)
        {
            LobbyName = lobbyName;
        }

        public void StartNewRound()
        {
            if (Players.Count == 0) return;

            Phase = GamePhase.Dealing;
            Deck.InitializeAndShuffle();
            Dealer.Reset();
            DealerRevealed = false;

            foreach (var player in Players)
            {
                player.Reset();
=======
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
>>>>>>> 2095be124ef035a32198381033e370e98052dfda
                player.Hit(Deck.DrawCard());
                player.Hit(Deck.DrawCard());
            }

            Dealer.Hit(Deck.DrawCard());
            Dealer.Hit(Deck.DrawCard());
<<<<<<< HEAD

            CurrentPlayerIndex = 0;
            Phase = GamePhase.PlayerTurn;
            
            CheckCurrentPlayerStatus(); 
            NotifyStateChanged();
        }

        public void PlayerHit(string playerName)
        {
            if (Phase != GamePhase.PlayerTurn || CurrentPlayer?.Name != playerName) return;

            CurrentPlayer.Hit(Deck.DrawCard());
            CheckCurrentPlayerStatus();
            NotifyStateChanged();
        }

        public void PlayerStand(string playerName)
        {
            if (Phase != GamePhase.PlayerTurn || CurrentPlayer?.Name != playerName) return;

            CurrentPlayer.HasStood = true;
            NextPlayerTurn();
        }

        private void CheckCurrentPlayerStatus()
        {
            if (CurrentPlayer == null) return;

            if (CurrentPlayer.IsBusted || CurrentPlayer.Score == 21)
            {
                CurrentPlayer.HasStood = true;
                NextPlayerTurn();
            }
        }

        private void NextPlayerTurn()
        {
            CurrentPlayerIndex++;

            if (CurrentPlayerIndex >= Players.Count)
            {
                ExecuteDealerTurn();
            }
            else
            {
                CheckCurrentPlayerStatus();
                NotifyStateChanged();
            }
        }

        private void ExecuteDealerTurn()
        {
            Phase = GamePhase.DealerTurn;
            DealerRevealed = true;
            NotifyStateChanged();

            while (Dealer.Score < 17)
=======
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
>>>>>>> 2095be124ef035a32198381033e370e98052dfda
            {
                Dealer.Hit(Deck.DrawCard());
            }

<<<<<<< HEAD
            DetermineWinners();
        }

        private void DetermineWinners()
        {
            int dealerScore = Dealer.Score;
            bool dealerBusted = Dealer.IsBusted;

            foreach (var player in Players)
            {
                if (player.IsBusted) { player.HasWon = false; }
                else if (dealerBusted || player.Score > dealerScore) { player.HasWon = true; }
                else if (player.Score == dealerScore) { player.IsPush = true; }
                else { player.HasWon = false; }
            }

            Phase = GamePhase.GameOver;
            NotifyStateChanged();
        }

        private void NotifyStateChanged() => OnGameStateChanged?.Invoke();
    }
}
=======
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
>>>>>>> 2095be124ef035a32198381033e370e98052dfda
