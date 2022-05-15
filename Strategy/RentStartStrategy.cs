using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Strategy
{
    //Annahme:
    //ich brauche mind. 3000, maximal 5000

    //rente: 500

    //brauche mind. 2500, maximal 4500

    //angenommener aktien crash: 50%

    //d.h.
    //minimal scenario:
    // 2 gleichungen mit 2 unbekannten -> lösbar

    //3000 = rente + festgeld + (aktien* 0.5)
    //5000 = rente + festgeld + aktien

    //2500 = festgeld + 0.5* aktien
    //4500 = festgeld + aktien

    //festgeld = 4500 - aktien

    //2500 = 4500 -aktien + 0.5aktien

    //4000 = aktien
    //500 = festgeld

    internal class RentStartStrategy
    {
    }
}
