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
        private BlockingCollection<Glass> stackGlasses;
        private BlockingCollection<Patron> queueToBar;
        public event Action<string> GotBeer;

        bool isGlassAvailable;

       



        public Bartender(Action<string> Callback, BlockingCollection<Patron> QueueToBar, BlockingCollection<Glass> StackGlasses, bool IsGlassAvailable)
        {
            this.queueToBar = QueueToBar;
            this.stackGlasses = StackGlasses;
            this.isGlassAvailable = IsGlassAvailable;
            BartenderPrint = Callback;
        }



        public void WaitsForPatron()
        {
            BartenderPrint("1. Waiting for patrons.");             

        }

            public void DequePatron()
            {
                 if (queueToBar != null)
                 {

                   while (queueToBar.Count == 0)
                    {
                    Thread.Sleep(10);

                    }


                queueToBar.TryTake(out Patron p); //använd blocking collection            

            }
            }

        public void GetGlass(Patron patron)
        {
            

            if (stackGlasses.Count > 0)
            {
                Thread.Sleep(3000); //tid att ta glaset
                stackGlasses.TryTake(out Glass g1);
                BartenderPrint("Gets the glass from the shelve");
                PourBeer(patron.Name);
            }

            else {
                BartenderPrint("Waiting for new glasses");
            }
        }
        public void PourBeer(string PatronName)
        {
            Task.Run(() => 
            {
                Thread.Sleep(3000);
                BartenderPrint("Pour a glass of beer to " + PatronName);
                DequePatron();
                GotBeer?.Invoke(PatronName);
            });
            
        }

      


    }
}
