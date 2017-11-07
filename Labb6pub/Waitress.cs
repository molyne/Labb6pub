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
        public BlockingCollection<Glass> dirtyGlasses = new BlockingCollection<Glass>(new ConcurrentStack<Glass>());

        public Waitress(Action<string> CallBack, BlockingCollection<Glass> FilledWithBeerGlasses, BlockingCollection<Glass> GlassesOnShelve)
        {
            WaitressPrint = CallBack;
            this.filledWithBeerGlasses = FilledWithBeerGlasses;
        }

        public void PickUpEmptyGlasses()
        {
            Thread.Sleep(40000);
            WaitressPrint("Picks up empty glasses");
            // emptyGlassesOnTables
            if (filledWithBeerGlasses.Count > 0)
                DishEmptyGlasses();
            else
                WaitressPrint("Found no glasses");
        }
        public void DishEmptyGlasses(/*List<Glass> dirtyGlasses*/)
        {
            Thread.Sleep(15000);
            WaitressPrint("Dishes the glasses");
          
                dirtyGlasses.Add(filledWithBeerGlasses.Take());
                glassesOnShelve.Add(dirtyGlasses.Take());
            
        }
    }
}
