using RiverSight.Domain.Enums;
using System;
using System.Collections.Generic;
using System.Text;

namespace RiverSight.Domain.Entities
{
    public class Card : IEquatable<Card>
    {
        public Rank Rank { get; }
        public Suit Suit { get; }

        // 
        public Card(Rank rank, Suit suit)
        {
            Rank = rank;
            Suit = suit;
        }

        public override string ToString()
        {
            char r = Rank switch
            {
                Rank.Ace => 'A',
                Rank.King => 'K',
                Rank.Queen => 'Q',
                Rank.Jack => 'J',
                Rank.Ten => 'T',
                _ => (char)('0' + (int)Rank)
            };

            char s = Suit.ToString().ToLower()[0]; // c, d, h, s

            return $"{r}{s}";
        }

        public bool Equals(Card? other)
        {
            if (other is null) return false;
            return this.Rank == other.Rank && this.Suit == other.Suit;
        }

        public override bool Equals(object? obj) => Equals(obj as Card);
        public override int GetHashCode() => HashCode.Combine(Rank, Suit);
    }
}
