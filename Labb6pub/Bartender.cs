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
        

public void WaitsForPatron(Action<string> Callback)
        {
            Callback("1. Waits for patrons.");       
        }

        public string GetGlass(string name)
        {
            
                return "Gets the glass from the shelve for " + name;
            
        }


    }
}
