using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Threading;
using System.Timers;
using System.Diagnostics;

namespace Labb6pub
{

    class Bouncer
    {
        private Action<string> Callback;
        public event Action<Action<string>> PatronArrived;
        public event Action NewInQueue;

       

        List<string> GuestList;

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

            Task.Run(() =>
                {
                   
                    
                     s.Start();           
                    

                    //  Console.Write(sw.Elapsed.Duration());

                    while (s.Elapsed<TimeSpan.FromSeconds(120))//tiden har tagit slut 2 min. 120 sekunder.
                    {   
                        
                        int randomTime = r.Next(3000, 10000);
                        Thread.Sleep(randomTime);

                        

                        int randomNumber = r.Next(0, 15);

                        
                        string name = GuestList[randomNumber];

                        Patron p = new Patron(name);
                        Bartender b = new Bartender();
                        

                        //kö i main

                        

                        Callback(p.PatronEnters());
                        Thread.Sleep(1000); //tid att gå till baren
                        Thread.Sleep(3000); //tid att ta glaset
                        PatronArrived?.Invoke(CallBack);
                        NewInQueue?.Invoke();

                    }
                    s.Stop();

                });
            
        }

        
           
        

        }
    }
