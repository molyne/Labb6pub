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
        List<string> GuestList;
        string elapsedtime;

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
        public void Work(Action<string> CallBack)
        {

            Random r = new Random();
            Stopwatch s = new Stopwatch();

           
            Task patron = Task.Run(() =>
                {
                s.Start();

                while (s.Elapsed < TimeSpan.FromSeconds(120))//tiden har tagit slut 2 min. 120 sekunder.
                {

                        int randomTime = r.Next(3000, 10000);
                        Thread.Sleep(randomTime);

                        int randomNumber = r.Next(0, 15);


                        string elapsedminutes = s.Elapsed.Minutes.ToString("00:");
                        string elapsedseconds = s.Elapsed.Seconds.ToString("00");


                        elapsedtime = elapsedminutes + elapsedseconds;
                        

                        Patron p = new Patron(CallBack);
                        p.Name = GuestList[randomNumber];

                        
                        Callback($"[{elapsedtime}] {p.PatronEnters()}");


                    
                        PatronArrived?.Invoke(p);
                    }
                    s.Stop();

                });
            
        }       

    }
}
