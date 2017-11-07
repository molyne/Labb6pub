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
        public Action<string> BartenderPrint;
        private BlockingCollection<Glass> glassesOnShelve;
        private BlockingCollection<Glass> glassesFilledWithBeer;
        private BlockingCollection<Patron> queueToBar;
        public event Action GotBeer;

        Patron FirstInQueue;

        bool isGlassAvailable;


        public Bartender(Action<string> Callback, BlockingCollection<Patron> QueueToBar, BlockingCollection<Glass> GlassesFilledWithBeer, bool IsGlassAvailable, BlockingCollection<Glass> GlassesOnShelve)
        {
            this.queueToBar = QueueToBar;
            this.glassesFilledWithBeer = GlassesFilledWithBeer;
            this.isGlassAvailable = IsGlassAvailable;
            this.glassesOnShelve = GlassesOnShelve;
            BartenderPrint = Callback;
        }


       

        public void WaitsForPatron()
        {
            BartenderPrint("1. Waiting for patrons.");             

        }

            public void DequePatron()
            {

                while (queueToBar.Count == 0)
                {
                    Thread.Sleep(10);
                }

            if (queueToBar != null)
            {
                Task.Run(() =>
                {
                    Thread.Sleep(4000);
                    if (glassesOnShelve.Count == 0)
                    {
                        BartenderPrint("Waiting for new glasses");
                    }
                });
            }

            FirstInQueue = queueToBar.Take();
            //använd blocking collection     
        
           
            }

        public void GetGlass(Patron patron)
        {
            if (glassesOnShelve.Count > 0)
            {
                Thread.Sleep(3000); //tid att ta glaset
                glassesFilledWithBeer.Add(glassesOnShelve.Take());
                BartenderPrint("Gets the glass from the shelve");
                PourBeer();
            }

            //else {
            //    BartenderPrint("Waiting for new glasses");
            //}
        }
        public void PourBeer()
        {
            Task.Run(() => 
            {
                Thread.Sleep(3000);
                DequePatron();
                BartenderPrint("Pour a glass of beer to " + FirstInQueue.Name);
                GotBeer?.Invoke();
            });
            
        }

      


    }
}
