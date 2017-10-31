using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Labb6pub
{
    class Patron
    {
        
        Bouncer b = new Bouncer();

        public string PatronInformation
        {

            get { return b.Name + " enters and goes to the bar"; }
            set { b.Name = value; }
        }
    }

    //public string PatronInformation {get; set;}

    //Bouncer b = new Bouncer();

    //public Patron()
    //{
    //    this.PatronInformation = b.Name + " goes to the bar";
    //}


}

