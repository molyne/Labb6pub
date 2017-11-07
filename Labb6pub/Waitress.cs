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
        private BlockingCollection<Glass> glassesOnShelf;

        public Waitress(Action<string> CallBack, BlockingCollection<Glass> StackGlasses)
        {
            WaitressPrint = CallBack;
            this.glassesOnShelf = StackGlasses;
        }

        public void PickUpEmptyGlasses()
        {
            Thread.Sleep(40000);
            WaitressPrint("Picks up empty glasses");
            // emptyGlassesOnTables

            

            DishEmptyGlasses();
        }
        public void DishEmptyGlasses(/*List<Glass> dirtyGlasses*/)
        {
            Thread.Sleep(15000);
            WaitressPrint("Dishes the glasses");
            for (int i = 1; i <= (9-glassesOnShelf.Count); i++)
            {
                glassesOnShelf.Add(new Glass());
            }
        }
    }
}
