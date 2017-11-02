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
        private ConcurrentQueue<Patron> queueToBar;
        private Stack<Glass> stackGlasses;

        public Bartender(Action<string> Callback, ConcurrentQueue<Patron> QueueToBar, Stack<Glass> StackGlasses)
        {
            this.queueToBar = QueueToBar;
            this.stackGlasses = StackGlasses;
            BartenderPrint = Callback;
        }

        public void WaitsForPatron()
        {
            BartenderPrint("1. Waits for patrons.");
            if (queueToBar != null)
            {
                
                while (queueToBar.Count == 0)
                {
                    Thread.Sleep(10);
                }

                   bool isPossible = queueToBar.TryDequeue(out Patron p); //använd blocking collection

            }

        }

        public void GetGlass(Patron patron)
        {
            Thread.Sleep(3000); //tid att ta glaset
            BartenderPrint("Gets the glass from the shelve");
            if (stackGlasses != null)
            {
                Glass g1 = stackGlasses.Pop();
                PourBeer(patron.Name);
            }
        }
        public void PourBeer(string PatronName)
        {
            Task.Run(() => 
            {
                Thread.Sleep(3000);
                BartenderPrint("Pour a glass of beer to "+PatronName);
               
            });
            
        }


    }
}
