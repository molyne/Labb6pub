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
        private bool barIsOpen = true;
        public event Action<Patron> PatronArrived;


        int speed = 1;

        List<string> GuestList;
        int numberOfGuestsOnList;

        private const bool couplesNight = false;
        private const bool bouncerWorksslower = false;
        private bool busLoad = false;


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
                GuestList.Add("Klas");
                GuestList.Add("Silvia");
                GuestList.Add("Gustav");
                GuestList.Add("Victoria");


            };

        }

        public void ChangeSpeed(int Speed)
        {
            this.speed = Speed;
        }

        //gör en funktion som heter work. Vänta ett tag släpp in en gäst. Använd en loop.
        public void Work(BlockingCollection<Chair> Chairs, Stopwatch Timer)
        {
      


            Random r = new Random();
            Stopwatch s = new Stopwatch();

            Task patron = Task.Run(() =>
            {
                while (barIsOpen)
                {


                    int randomTime = r.Next(3000, 10000);
                    numberOfGuestsOnList = GuestList.Count(); // antal namn på gästlistan

                   


                        if (bouncerWorksslower)
                        {
                            Thread.Sleep(randomTime * 2);
                        }

                        else { Thread.Sleep(randomTime / speed); }


                        if (barIsOpen)
                        {
                       

                            int randomNumber = r.Next(0, numberOfGuestsOnList); // slumpa mellan namnen som finns i listan

                            Patron p = new Patron(Callback, Chairs);


                            p.Name = GuestList[randomNumber];


                            Callback(p.PatronEnters());
                            Task.Run(() => { PatronArrived?.Invoke(p); });

                            if (couplesNight)
                            {
                                

                                        int randomNumber2 = r.Next(0, numberOfGuestsOnList);

                                        Patron onemorepatron = new Patron(Callback, Chairs);

                                        onemorepatron.Name = GuestList[randomNumber2];

                                        Callback(onemorepatron.PatronEnters());

                                        Task.Run(() => { PatronArrived?.Invoke(onemorepatron); });
                            }

                        if (busLoad)
                        {

                            Thread.Sleep(20000);
                            for (int i = 0; i < 15; i++)
                            {

                                int randomNumber2 = r.Next(0, numberOfGuestsOnList);

                                Patron onemorepatron = new Patron(Callback, Chairs);

                                onemorepatron.Name = GuestList[randomNumber2];

                                Callback(onemorepatron.PatronEnters());

                                Task.Run(() => { PatronArrived?.Invoke(onemorepatron); });

                                busLoad = false;

                            }
                        }
                        }

                    }

                Callback("Bouncer goes home");


            });

        }
        public void IsBarClosed()
        {
            barIsOpen = false;
        }

    }
}