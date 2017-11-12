using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Threading;
using System.Timers;
using System.Diagnostics;
using System.Collections.Concurrent;

namespace Labb6pub
{

    class Bouncer
    {
        private Action<string> Callback;
        public event Action<Patron> PatronArrived;
        public event Action<Patron> AddToGuestInBar;

        List<string> GuestList;
        string elapsedtime;
        int numberOfGuestsOnList;

        public Bouncer(Action<string> CallBack)
        {
            this.Callback = CallBack;

            GuestList = new List<string>();
            {

                GuestList.Add("Kalle");
                GuestList.Add("Lisa");
                GuestList.Add("Olle");
                GuestList.Add("Viktor");
                GuestList.Add("Anna");
                GuestList.Add("Camilla");
                GuestList.Add("Silvio");
                GuestList.Add("Molyn");
                GuestList.Add("John");
                GuestList.Add("Fredrik");
                GuestList.Add("Sara");
                GuestList.Add("Emma");
                GuestList.Add("Nils");
                GuestList.Add("Magnus");
                GuestList.Add("Elias");
                GuestList.Add("Tobias");

            };

        }
        //gör en funktion som heter work. Vänta ett tag släpp in en gäst. Använd en loop.
        public void Work(BlockingCollection<Chair> Chairs,BlockingCollection<Patron> QueueToBar)
        {
          
            Random r = new Random();
            Stopwatch s = new Stopwatch();

           
            Task patron = Task.Run(() =>
                {
                s.Start();


                    while (s.Elapsed < TimeSpan.FromSeconds(120) && GuestList.Count > 0)//tiden har tagit slut 2 min. 120 sekunder.
                {
                      
                        
                            int randomTime = r.Next(3000, 10000);
                            Thread.Sleep(randomTime);

                            int randomNumber = r.Next(0, numberOfGuestsOnList); // slumpa mellan namnen som finns kvar på listan


                            string elapsedminutes = s.Elapsed.Minutes.ToString("00:");
                            string elapsedseconds = s.Elapsed.Seconds.ToString("00");


                            elapsedtime = elapsedminutes + elapsedseconds;


                            Patron p = new Patron(Callback, Chairs, QueueToBar);

                            p.Name = GuestList[randomNumber];

                            GuestList.RemoveAt(randomNumber); //ta bort gäst från gästlistan

                            numberOfGuestsOnList = GuestList.Count(); // antal namn kvar på listan

                            AddToGuestInBar?.Invoke(p);

                            Callback($"[{elapsedtime}] {p.PatronEnters()}");
                            PatronArrived?.Invoke(p);

                        }

                    

                });

        }    
        public void BouncerGoesHome()
        {
            Callback("Bouncer goes home");
        }

    }
}
