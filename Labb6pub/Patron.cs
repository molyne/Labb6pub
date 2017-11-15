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
        private BlockingCollection<Chair> takenChairs;

        private int searchForChairTime = 1000;
        private int walkToTableTime = 4000;
        private int speed = 1;

      

       

        public string Name{ get; set; }
        public string Name2 { get; set; }

        public Patron(Action<string> PatronPrint, BlockingCollection<Chair> Chairs, BlockingCollection<Chair> TakenChairs )
        {
            this.chairs = Chairs;
            this.takenChairs = TakenChairs;
            this.PatronPrint = PatronPrint;


         
        }

        public void ChangeSpeed(int Speed)
        {
            this.speed = Speed;
        }

        public string PatronEnters()
        {
            
            
            return Name + " enters and goes to the bar.";

        }


        public void PatronSearchForChair(string FirstInQueueName)
        {
            Task.Run(() =>
           {

            
               Thread.Sleep(searchForChairTime / speed);
                   PatronPrint(FirstInQueueName + " searches for a chair.");


                   PatronSits(FirstInQueueName);
               
           });
        }
        public void PatronSits(string FirstInQueueName2)
        {
            
            Thread.Sleep(walkToTableTime/speed);
            takenChairs.Add(chairs.Take());
            PatronPrint(FirstInQueueName2+" sits and drinks his/hers beer.");
            

            PatronLeaves(FirstInQueueName2);
        }
        public void PatronLeaves(string FirstInQueueName3)
        {

            Random r = new Random();

            int randomTime = r.Next(10000, 20000);

            Thread.Sleep(randomTime/speed);

            PatronPrint(FirstInQueueName3 + " finished the beer and leaves the bar.");
            chairs.Add(takenChairs.Take());
            PatronLeaved?.Invoke();
            

        }
       
      
        

    }


   
 

}

