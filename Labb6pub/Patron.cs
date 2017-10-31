using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Labb6pub
{
    class Patron
    {   
        public string Name{ get; set; }

        public Patron(string name)
        {
            this.Name = name;
        }

        public string PatronEnters()
        {
            return Name + " enters and goes to the bar.";
        }

    }


   
 

}

