using RiverSight.Domain.Entities;
using RiverSight.Domain.Enums;

namespace RiverSight.Infrastructure.Vision.Services
{
    public static class CardParser
    {
        public static Card? Parse(string filename)
        {
            // Esperado: "Ah", "Ks", "Td" (Ten of Diamonds)
            if(string.IsNullOrWhiteSpace(filename)|| filename.Length < 2) return null;

            char rankChar = filename[0];
            char suitChar = filename[1];

            Rank rank = rankChar switch
            {              
                'A' => Rank.Ace,
                'K' => Rank.King,
                'Q' => Rank.Queen,
                'J' => Rank.Jack,
                'T' => Rank.Ten,
                _ => (Rank)int.Parse(rankChar.ToString())
            };

            Suit suit = suitChar switch
            {
                'h' => Suit.Hearts,
                'd' => Suit.Diamonds,
                'c' => Suit.Clubs,
                's' => Suit.Spades,
                _ => throw new ArgumentException($"Naipe desconhecido: {suitChar}")
            };

            return new Card(rank, suit);
        }
    }
}
