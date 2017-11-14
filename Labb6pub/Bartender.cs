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
        public event Action<string> GotBeer;

        private int takeGlassTime = 3000;
        private int pourBeerTime = 3000;
        private int waitingTime = 0;
        private int speed = 1;

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

        public void ChangeSpeed(int Speed)
        {
            this.speed = Speed;
        }



        public void WaitsForPatron()
        {
            BartenderPrint("Waiting for patrons.");             

        }

            public void DequePatron()
            {

                while (queueToBar.Count == 0)
                {
                    Thread.Sleep(10); // davids 
                }

            if (queueToBar != null)
            {
                Task.Run(() =>
                {
                    Thread.Sleep(waitingTime); //så att den skrivs ut efter alla fått sin öl
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
           
            while (glassesOnShelve.Count <= 0) { Thread.Sleep(10); }
            {
                Thread.Sleep(takeGlassTime/speed); //tid att ta glaset
                glassesFilledWithBeer.Add(glassesOnShelve.Take());
                BartenderPrint("Gets the glass from the shelve.");
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
                Thread.Sleep(pourBeerTime/speed);
                DequePatron();
                BartenderPrint("Pour a glass of beer to " + FirstInQueue.Name + ".");
                GotBeer?.Invoke(FirstInQueue.Name);
            });
            
        }
        public void BartenderGoesHome()
        {
            BartenderPrint("Bartender goes home.");
        }

      


    }
}
