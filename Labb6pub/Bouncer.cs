using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Threading;

namespace Labb6pub
{

    class Bouncer
    {

        private Action<string> Callback;

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
             public void Work()
            {
            Task.Run(() =>
            {
                Random r = new Random();
                int randomNumber = r.Next(0, 15);

                Patron p = new Patron(GuestList[randomNumber]);

                Callback(p.PatronEnters());

            });
            }

        }
    }
