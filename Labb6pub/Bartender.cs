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
        private bool isBarOpen = true;

        private int takeGlassTime = 3000;
        private int pourBeerTime = 3000;

        private int speed = 1;

        Patron FirstInQueue;




        public Bartender(Action<string> Callback, BlockingCollection<Patron> QueueToBar, BlockingCollection<Glass> GlassesFilledWithBeer, BlockingCollection<Glass> GlassesOnShelve)
        {
            this.queueToBar = QueueToBar;
            this.glassesFilledWithBeer = GlassesFilledWithBeer;
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


            FirstInQueue = queueToBar.Take();
        }

        public void GetGlass()
        {

            while (glassesOnShelve.Count <= 0) { Thread.Sleep(10); }

            while (queueToBar.Count == 0) { Thread.Sleep(10); }
           
                Thread.Sleep(takeGlassTime / speed); //tid att ta glaset
                glassesFilledWithBeer.Add(glassesOnShelve.Take());
                BartenderPrint("Gets the glass from the shelve.");
                PourBeer();

       

        }
        public void PourBeer()
        {
            
                Thread.Sleep(pourBeerTime/speed);
                DequePatron();
                GotBeer?.Invoke(FirstInQueue.Name);
                BartenderPrint("Pours a glass of beer to " + FirstInQueue.Name + ".");
                
            
            
        }
        public void BartenderGoesHome()
        {
            BartenderPrint("Bartender goes home.");
        }

        public void BarIsOpen()
        {
            isBarOpen = false;
        }


    }
}
