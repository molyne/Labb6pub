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

        public Waitress(Action<string> CallBack, BlockingCollection<Glass> FilledWithBeerGlasses, BlockingCollection<Glass> GlassesOnShelve)
        {
            WaitressPrint = CallBack;
            this.filledWithBeerGlasses = FilledWithBeerGlasses;
            this.glassesOnShelve = GlassesOnShelve;
        }

        public void AddEmptyGlasses()
        {
            dirtyGlasses.Push(filledWithBeerGlasses.Take());
        }

        public void PickUpEmptyGlasses()
        {
            Thread.Sleep(10000);

            if (dirtyGlasses.Count > 0)
            {
                WaitressPrint("Picks up empty glasses");
                DishEmptyGlasses();
            }
            else //todo gör så det skrivs ut en gång
                WaitressPrint("Found no glasses");
        }
        public void DishEmptyGlasses(/*List<Glass> dirtyGlasses*/)
        {
            Thread.Sleep(15000);
            WaitressPrint("Dishes the glasses");

            for (int i = 0; i < dirtyGlasses.Count; i++)
            {   
                glassesOnShelve.Add(dirtyGlasses.Pop());
              
                
            }
            
        }
    }
}
