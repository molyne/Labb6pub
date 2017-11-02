using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Labb6pub
{
    class Bartender
    {
        public event Action TookGlass;
        public Action<string> BartenderPrint, PatronPrint;

        public Bartender(Action<string> Callback)
        {
            BartenderPrint = Callback;
        }

        public void WaitsForPatron(Action<string> Callback)
        {
            BartenderPrint("1. Waits for patrons.");       
        }

        public void GetGlass(Action<string> Callback)
        {
          
            Callback("Gets the glass from the shelve");
            TookGlass?.Invoke();
        }
        public void PourBeer(string name)
        {
            Task.Run(() => 
            {
                Thread.Sleep(3000);
                BartenderPrint("Pour a glass of beer to "+name);
            });
            
        }


    }
}
