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
        private bool barIsOpen = true;
        public event Action<Patron> PatronArrived;
        public event Action PatronLeaved;

        private BlockingCollection<Chair> chairs;
        private BlockingCollection<Chair> takenChairs;
        Patron p;
        Random r;

        private int speed = 1;
        private int timeUntilBusLoadArrives = 20000;

        List<string> GuestList;
        int numberOfGuestsOnList;

        private bool couplesNight;
        private bool bouncerWorksSlower;
        private bool busLoad;



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

        public void Work(BlockingCollection<Chair> Chairs, BlockingCollection<Chair> TakenChairs, bool CouplesNight, bool BusLoad, bool BouncerWorksSlower)
        {
            this.chairs = Chairs;
            this.takenChairs = TakenChairs;
            this.couplesNight = CouplesNight;
            this.busLoad = BusLoad;
            this.bouncerWorksSlower = BouncerWorksSlower;
    


            r = new Random();
            Stopwatch s = new Stopwatch();

            Task patron = Task.Run(() =>
            {
                while (barIsOpen)
                {


                    int randomTime = r.Next(3000, 10000);
                    numberOfGuestsOnList = GuestList.Count(); // antal namn på gästlistan


                    BusLoadEnters();




                    if (bouncerWorksSlower)
                    {
                        Thread.Sleep(randomTime * 2 / speed);
                    }

                    else { Thread.Sleep(randomTime / speed); }


                    if (barIsOpen)
                    {


                        int randomNumber = r.Next(0, numberOfGuestsOnList); // slumpa mellan namnen som finns i listan

                        p = new Patron(Callback, chairs, takenChairs);

                        p.PatronLeaved += PatronLeft;
                        p.Name = GuestList[randomNumber];


                        Callback(p.PatronEnters());
                        Task.Run(() => { PatronArrived?.Invoke(p); });


                        IsitCouplesNight();


                    }
                }



                Callback("Bouncer goes home");


            });

        }
        public void IsBarClosed()
        {
            barIsOpen = false;
        }

        public void GotBeer(string name)
        {
            p.PatronSearchForChair(name);
        }
        public void PatronLeft()
        {
            PatronLeaved?.Invoke();
        }

        public void BusLoadEnters()
        {
            if (busLoad)
            {
                Task.Run(() =>
                {
                    Thread.Sleep(timeUntilBusLoadArrives/speed);
                    for (int i = 0; i < 15; i++)
                    {

                        int randomNumber = r.Next(0, numberOfGuestsOnList);

                        Patron onemorepatron = new Patron(Callback, chairs, takenChairs);

                        onemorepatron.Name = GuestList[randomNumber];

                        Callback(onemorepatron.PatronEnters());



                        Task.Run(() => { PatronArrived?.Invoke(onemorepatron); });


                    }

                });
                busLoad = false;
            }
        }
        public void IsitCouplesNight()
        {
            if (couplesNight)
            {
                int randomNumber2 = r.Next(0, numberOfGuestsOnList);

                Patron onemorepatron = new Patron(Callback, chairs, takenChairs);

                onemorepatron.Name = GuestList[randomNumber2];

                Callback(onemorepatron.PatronEnters());

                Task.Run(() => { PatronArrived?.Invoke(onemorepatron); });
            }
        }

    }

}