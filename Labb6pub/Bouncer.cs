﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Labb6pub
{
   
    class Bouncer
    {

      
               

    public string Name { get; set; }

        
        public Bouncer()
            {
           
            var GuestList = new List<string>();
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

            Random r = new Random();
            int randomNumber = r.Next(0, 15);

             this.Name = GuestList[randomNumber];

            }




    }
}