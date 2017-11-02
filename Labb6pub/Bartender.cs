using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Labb6pub
{
    class Bartender
    {

        public Action<string> BartenderPrint, PatronPrint;
        private ConcurrentQueue<Patron> queueToBar;

        public Bartender(Action<string> Callback, ConcurrentQueue<Patron> QueueToBar)
        {
            this.queueToBar = QueueToBar;
            BartenderPrint = Callback;
        }


        //public void Work()
        //{
        //    while(barIsOpen)
        //    {
        //        WaitsForPatron(..);
        //        GetGlass(..);
        //        PourBeer(..);
        //    }
        //}
        public void WaitsForPatron()
        {
            BartenderPrint("1. Waits for patrons.");
            if (queueToBar != null)
            {
                
                while (queueToBar.Count == 0)
                {
                    Thread.Sleep(10);
                }
               
                 /*   bool isPossible = queueToBar.TryDequeue(out Patron p);*/ //använd blocking collection
                   
                
            }
            
        }

        public void GetGlass()
        {
            Thread.Sleep(3000); //tid att ta glaset
            BartenderPrint("Gets the glass from the shelve");
            PourBeer();
        }
        public void PourBeer()//(Patron patron) //få objekt patron
        {
            Task.Run(() => 
            {
                Thread.Sleep(3000);
                BartenderPrint("Pour a glass of beer to "/*+patron.Name*/);
               
            });
            
        }


    }
}
