using RiverSight.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RiverSight.Domain.Interfaces
{
    public interface IScreenReader
    {
        // O dominio pede: "Me dê as cartas que estão na mão do Hero"
        // Ele não quer saber de Bitmap ou mat, ele quer um array de Card[]

        IEnumerable<Card> ReadHeroHands();
        IEnumerable<Card> ReadCommunityCards();
    }
}
