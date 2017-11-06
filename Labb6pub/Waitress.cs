﻿using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Labb6pub
{
    

    class Waitress
    {
        public Action<string> WaitressPrint;
        private BlockingCollection<Glass> stackGlasses;

        public Waitress(Action<string> CallBack, BlockingCollection<Glass> StackGlasses)
        {
            WaitressPrint = CallBack;
            this.stackGlasses = StackGlasses;
        }

        public void PickUpEmptyGlasses()
        {
            Thread.Sleep(10000);
            WaitressPrint("Picks up empty glasses");
        }
    }
}
