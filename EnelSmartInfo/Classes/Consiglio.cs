using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EnelSmartInfo.Classes
{
    class Consiglio
    {
        private string rigaConsiglio;

          public string Name
          {
              get { return rigaConsiglio; }
              set { rigaConsiglio = value; }
          }

          public Consiglio(string cons)
          {
              rigaConsiglio = cons;
          }
    }
}
