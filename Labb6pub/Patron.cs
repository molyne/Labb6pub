using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Labb6pub
{
    class Patron
    {

        public Action<string> PatronPrint;

        public string Name{ get; set; }

        public Patron(Action<string> PatronPrint)
        {
           
            this.PatronPrint = PatronPrint;
        }

        public string PatronEnters()
        {
            
            return Name + " enters and goes to the bar.";
        }
        public void PatronSearchForChair(string name)
        {
            PatronPrint(name+" search for a chair.");
        }
       
        
        

    }


   
 

}

