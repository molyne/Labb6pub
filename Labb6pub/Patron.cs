﻿using System;
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

        BlockingCollection<Chair> chairs;
        private BlockingCollection<Patron> queueToBar;


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
        public void PatronSearchForChair(string name)
        {
            Thread.Sleep(1000);
            PatronPrint(name+" search for a chair.");
          
        
           if(chairs.Count>0)
            PatronSits(name);                      
        }
        public void PatronSits(string name)
        {
            Thread.Sleep(4000);
            PatronPrint(name+" sits and drink his/hers beer");

            chairs.TryTake(out Chair c);

            PatronLeaves(name);
        }
        public void PatronLeaves(string name)
        {

            Random r = new Random();

            int randomTime = r.Next(10000, 20000);

            Thread.Sleep(50000);

            PatronPrint(name + " finished the beer and leaves the bar.");
            chairs.Add(new Chair());
        }
       
        
        

    }


   
 

}

