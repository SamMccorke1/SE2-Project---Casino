namespace Chuds2Chads.Games.Blackjack
{
    public enum Suit
    {
        Hearts,
        Diamonds,
        Clubs,
        Spades
    }

    public enum Rank
    {
        Two = 2,
        Three,
        Four,
        Five,
        Six,
        Seven,
        Eight,
        Nine,
        Ten,
        Jack,
        Queen,
        King,
        Ace
    }

    public class Card
    {
        public Suit Suit { get; set; }
        public Rank Rank { get; set; }

        public int Value
        {
            get
            {
                if (Rank == Rank.Ace)
                    return 11;

                if ((int)Rank <= 10)
                    return (int)Rank;

                return 10;
            }
        }

        public string ImagePath
        {
            get
            {
                string suitString = Suit.ToString().ToLower();

                string rankString = Rank switch
                {
                    Rank.Ace => "A",
                    Rank.King => "K",
                    Rank.Queen => "Q",
                    Rank.Jack => "J",
                    Rank.Ten => "10",
                    Rank.Nine => "9",
                    Rank.Eight => "8",
                    Rank.Seven => "7",
                    Rank.Six => "6",
                    Rank.Five => "5",
                    Rank.Four => "4",
                    Rank.Three => "3",
                    Rank.Two => "2",
                    _ => ""
                };
                return $"PlayingCards/{suitString}_{rankString}.png";
            }
        }
        public static string CardBackPath => "PlayingCards/back_light.png";
    }
}
