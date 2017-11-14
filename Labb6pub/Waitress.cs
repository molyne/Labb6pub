using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Labb6pub
{
    //tre stackar med glas, en på hyllan(emptyglasses), en för (patronhasglass), en för (dirtyglasses, som bara waitress behöver veta om).

    class Waitress
    {
        public Action<string> WaitressPrint;
        private BlockingCollection<Glass> filledWithBeerGlasses;
        private BlockingCollection<Glass> glassesOnShelve;
        public Stack<Glass> dirtyGlasses = new Stack<Glass>();

       private  int speed;
        private int pickingGlassesTime = 10000;
        private int dishingGlassesTime = 15000;


        public Waitress(Action<string> CallBack, BlockingCollection<Glass> FilledWithBeerGlasses, BlockingCollection<Glass> GlassesOnShelve, int Speed)
        {
            WaitressPrint = CallBack;
            this.filledWithBeerGlasses = FilledWithBeerGlasses;
            this.glassesOnShelve = GlassesOnShelve;
            this.speed = Speed;       

        }
        public void changespeed(int speed)
        {
            this.speed = speed;
        }


        

        public void AddEmptyGlasses()
        {
            dirtyGlasses.Push(filledWithBeerGlasses.Take());
        }

        public void PickUpEmptyGlasses()
        {
            Thread.Sleep(pickingGlassesTime/speed);

            if (dirtyGlasses.Count > 0 )
            {
                WaitressPrint($"Picks up {dirtyGlasses.Count} glasses.");
                DishEmptyGlasses();
            }

            else //todo gör så det skrivs ut en gång..eller det kanske ska göra det(skrivas ut flera)?
                WaitressPrint("Found no glasses.");
        }
        public void DishEmptyGlasses(/*List<Glass> dirtyGlasses*/)
        {
            Thread.Sleep(dishingGlassesTime/speed);
            WaitressPrint("Dishes the glasses.");

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
