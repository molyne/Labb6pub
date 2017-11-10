using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Labb6pub
{
    class Patron
    {

        public Action<string> PatronPrint;
        public event Action PatronLeaved;

        BlockingCollection<Chair> chairs;
        private BlockingCollection<Patron> queueToBar;
        private string name;

       

        public string Name{ get; set; }

        public Patron(Action<string> PatronPrint, BlockingCollection<Chair> Chairs, BlockingCollection<Patron> QueueToBar)
        {
            this.chairs = Chairs;
            this.PatronPrint = PatronPrint;
            this.queueToBar = QueueToBar;

         
        }

        public string PatronEnters()
        {
            
            return Name + " enters and goes to the bar.";
        }
        public void PatronSearchForChair(string Name)
        {
            this.name = Name;

            Thread.Sleep(1000);
            PatronPrint(name+" search for a chair.");
          
        
           if(chairs.Count>0)
            PatronSits();                      
        }
        public void PatronSits()
        {
            Thread.Sleep(4000);
            PatronPrint(name+" sits and drink his/hers beer");

            chairs.TryTake(out Chair c);

            PatronLeaves();
        }
        public void PatronLeaves()
        {

            Random r = new Random();

            int randomTime = r.Next(10000, 20000);

            Thread.Sleep(randomTime);

            PatronPrint(name + " finished the beer and leaves the bar.");
            chairs.Add(new Chair());
            PatronLeaved?.Invoke();
            

        }
       
        
        

    }


   
 

}

