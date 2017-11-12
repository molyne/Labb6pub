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

        private BlockingCollection<Chair> chairs;
        public BlockingCollection<Chair> takenChairs = new BlockingCollection<Chair>(); //ändra till private sen
        private BlockingCollection<Patron> queueToBar;

        private int searchForChairTime = 1000;
        private int walkToTableTime = 4000;

       

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
        public void PatronSearchForChair(string FirstInQueueName)
        {
           

            Thread.Sleep(searchForChairTime);
            PatronPrint(FirstInQueueName+" searches for a chair.");
          
           
            PatronSits(FirstInQueueName);                      
        }
        public void PatronSits(string FirstInQueueName2)
        {

            Thread.Sleep(walkToTableTime);
            takenChairs.Add(chairs.Take());
            PatronPrint(FirstInQueueName2+" sits and drinks his/hers beer.");
            

            PatronLeaves(FirstInQueueName2);
        }
        public void PatronLeaves(string FirstInQueueName3)
        {

            Random r = new Random();

            int randomTime = r.Next(10000, 20000);

            Thread.Sleep(randomTime);

            PatronPrint(FirstInQueueName3 + " finished the beer and leaves the bar.");
            chairs.Add(takenChairs.Take());
            PatronLeaved?.Invoke();
            

        }
       
        
        

    }


   
 

}

