using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Labb6pub
{


    class Waitress
    {
        public Action<string> WaitressPrint;
        private BlockingCollection<Glass> filledWithBeerGlasses;
        private BlockingCollection<Glass> glassesOnShelve;
        public Stack<Glass> dirtyGlasses = new Stack<Glass>();

        private  int speed = 1;
        private int pickingGlassesTime = 10000;
        private int dishingGlassesTime = 15000;
        private bool waitressWorksFaster;


        public Waitress(Action<string> CallBack, BlockingCollection<Glass> FilledWithBeerGlasses, BlockingCollection<Glass> GlassesOnShelve, bool WaitressWorksFaster)
        {
            WaitressPrint = CallBack;
            this.filledWithBeerGlasses = FilledWithBeerGlasses;
            this.glassesOnShelve = GlassesOnShelve;
            this.waitressWorksFaster = WaitressWorksFaster;
            

        }
        public void ChangeSpeed(int Speed)
        {
            this.speed = Speed;
        }

        public void AddEmptyGlasses()
        {
            dirtyGlasses.Push(filledWithBeerGlasses.Take());
        }

        public void PickUpEmptyGlasses()
        {   if (waitressWorksFaster)
            {
                Thread.Sleep(pickingGlassesTime / speed / 10);
            }
            else
            {
                Thread.Sleep(pickingGlassesTime / speed);
            }


            if (dirtyGlasses.Count > 0)
            {
                WaitressPrint($"Picks up {dirtyGlasses.Count} glasses.");
                DishEmptyGlasses();
            }

            else 
                WaitressPrint("Found no glasses.");
        }
        public void DishEmptyGlasses()
        {   if (waitressWorksFaster)
            {
                Thread.Sleep(dishingGlassesTime / speed/10);
            }
            else
            {
                Thread.Sleep(dishingGlassesTime / speed);
            }
            WaitressPrint("Washes the glasses.");

            for (int i = 0; i < dirtyGlasses.Count; i++)
            {   
                glassesOnShelve.Add(dirtyGlasses.Pop());
            }
          
          
        }
        public void WaitressGoHome()
        {
            WaitressPrint("Waitress goes home.");
        }
    }
}
