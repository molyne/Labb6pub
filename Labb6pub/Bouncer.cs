﻿using System;
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
        Stopwatch stopwatch = new Stopwatch();

        List<string> GuestList;
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
        public void Work(BlockingCollection<Chair> Chairs, Stopwatch Timer)
        {
            this.stopwatch = Timer;
          
            Random r = new Random();
            Stopwatch s = new Stopwatch();
        
            Task patron = Task.Run(() =>
                {          
                    while (Timer.Elapsed < TimeSpan.FromSeconds(120) && GuestList.Count > 0)//tiden har tagit slut 2 min. 120 sekunder.
                    {

                            int randomTime = r.Next(3000, 10000);


                            Thread.Sleep(randomTime);

                            numberOfGuestsOnList = GuestList.Count(); // antal namn på gästlistan

                            int randomNumber = r.Next(0, numberOfGuestsOnList); // slumpa mellan namnen som finns kvar på listan

                            Patron p = new Patron(Callback, Chairs);

                            p.Name = GuestList[randomNumber];

                            GuestList.RemoveAt(randomNumber); //ta bort gäst från gästlistan



                            AddToGuestInBar?.Invoke(p);

                        if (Timer.Elapsed < TimeSpan.FromSeconds(119)){
                            Callback(p.PatronEnters());
                            PatronArrived?.Invoke(p);
                        }
                    }

            Callback("Bouncer goes home");
                    
                });

        }         

    }
}
