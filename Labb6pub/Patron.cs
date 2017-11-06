using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Labb6pub
{
    class Patron
    {

        public Action<string> PatronPrint;
        BlockingCollection<Chair> chairs;


        public string Name{ get; set; }

        public Patron(Action<string> PatronPrint, BlockingCollection<Chair> Chairs)
        {
            this.chairs = Chairs;
            this.PatronPrint = PatronPrint;
        }

        public string PatronEnters()
        {
            
            return Name + " enters and goes to the bar.";
        }
        public void PatronSearchForChair(string name)
        {
            PatronPrint(name+" search for a chair.");

           if(chairs.Count>0)
            PatronSits();
        }
        public void PatronSits()
        {
            PatronPrint("Patron sits and drink his/hers beer");
            chairs.TryTake(out Chair c);
        }
       
        
        

    }


   
 

}

