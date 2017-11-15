using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Windows.Threading;

namespace Labb6pub
{

    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>



    public partial class MainWindow : Window

 
    {
        public event Action BarIsClosed;

        private BlockingCollection<Patron> queueToBar;
        private Queue<Patron> guestsInPub;
        private BlockingCollection<Glass> glassesFilledWithBeer;
        private BlockingCollection<Glass> glassesOnShelve;

        private BlockingCollection<Chair> chairs;
        Stopwatch printTime = new Stopwatch();
        string elapsedtime;

        DispatcherTimer timerToClosing;
        DispatcherTimer upDateLabelEverySecond;

        int timeToClosing = 120;
        int upDateLabelTime = 0;
        int speed = 1;

        public event Action AllGuestsLeft;

        bool barIsOpen = false;

        private int numberOfGlasses = 3;
        private int numberOfChairs = 9;
        int walkToBarTime = 1000;

        Bartender bar;
        Bouncer b;
        Patron p;
        Waitress w;
        
  
        public MainWindow()
        {
            InitializeComponent();

 
            queueToBar = new BlockingCollection<Patron>();
            guestsInPub = new Queue<Patron>();

            glassesFilledWithBeer = new BlockingCollection<Glass>(new ConcurrentStack<Glass>());
            glassesOnShelve = new BlockingCollection<Glass>(new ConcurrentStack<Glass>());

            chairs = new BlockingCollection<Chair>();
            bar = new Bartender(AddToBartenderListBox, queueToBar, glassesFilledWithBeer, glassesOnShelve);

        }

       

        private void SetStartValues()
        {
            FillShelveWithGlasses();
        
            CreateChairs();
            
        }
     

        public void FillShelveWithGlasses()
        {
            for (int i = 1; i <= numberOfGlasses; i++)
            {
                glassesOnShelve.Add(new Glass());
            }
        }



        private void CreateChairs()
        {
            for (int i = 0; i < numberOfChairs; i++)
            {
                chairs.Add(new Chair());
            }
        }

        private void AddToQueueInBar(Patron patron) 
        {
            Thread.Sleep(walkToBarTime); // tid för gästen att komma till kön
            {
                queueToBar.Add(patron);
                
            }
        }

        

            private void AddToGuestsInPub(Patron patron)
        {
            guestsInPub.Enqueue(patron);

           
        
        }

        private void RemoveGuestInPub()
        {
            if (guestsInPub != null)
                guestsInPub.Dequeue();
            if (guestsInPub.Count == 0 && !barIsOpen)

            {
                AllGuestsLeft?.Invoke();
            }
        }

        private static string FormatTime(int time)
        {
                if(time%60<10)
                    return $"[0{time / 60}:0{time % 60}]";
                else
                    return $"[0{ time / 60}:{ time % 60}]";
        }

        private void StartClocks()
        {
            timerToClosing = new DispatcherTimer();
            upDateLabelEverySecond = new DispatcherTimer();

            upDateLabelEverySecond.Interval = TimeSpan.FromMilliseconds(1);
            upDateLabelEverySecond.Tick += timerTvå_Tick;
            upDateLabelEverySecond.Start();
            timerToClosing.Interval = TimeSpan.FromMilliseconds(1000);
            timerToClosing.Tick += timer_Tick;
            timerToClosing.Start();

            printTime.Start();
        }

        public void ChangeTimeSpeed()
        {
            timerToClosing.Interval = TimeSpan.FromMilliseconds(1000/speed);

        }

        void timer_Tick(object sender, EventArgs e)
        {


            if (timeToClosing != 0)
                timeToClosing--;
            else
            {   
                timerToClosing.Stop();
                barIsOpen = false;
                BarIsClosed?.Invoke();
            }
            if (timeToClosing <= 10)
            {
                    ClosingTimeLabel.Foreground = Brushes.Red;       
            }

            ClosingTimeLabel.Content = FormatTime(timeToClosing);

            
        }
        void timerTvå_Tick (object sender, EventArgs e)
        {
            UpdateLabelsValues();
            upDateLabelTime++;
        }


        private void OpenOrCloseBarButton_Click(object sender, RoutedEventArgs e)
        {

            FastForwardButton.IsEnabled = true;

            OpenOrCloseBarButton.IsEnabled = false;
            InfoTextLabel.Content = string.Empty;

            barIsOpen = true;
            StartClocks();
            SetStartValues();

            b = new Bouncer(AddToGuestListBox);

            p = new Patron(AddToGuestListBox,chairs);

            w = new Waitress(AddToWaitressListBox,glassesFilledWithBeer, glassesOnShelve);

           
            b.PatronArrived += AddToGuestsInPub;
            b.PatronArrived += AddToQueueInBar;
            bar.GotBeer += p.PatronSearchForChair;
            p.PatronLeaved += w.AddEmptyGlasses;
            p.PatronLeaved += RemoveGuestInPub;
            BarIsClosed += b.IsBarClosed;
            BarIsClosed += bar.BarIsOpen;



            Task bartender = Task.Run(() =>
            {
              
                bar.WaitsForPatron();
                while (timerToClosing.IsEnabled || queueToBar.Count > 0)
                {
                    bar.GetGlass();

                }
                while (guestsInPub.Count> 0) { Thread.Sleep(10); }
                bar.BartenderGoesHome();
            });

            Task bouncer = Task.Run(() =>
            {
                b.Work(chairs,printTime);

            });

            
            Task waitress = Task.Run(() =>
            {
                while (timerToClosing.IsEnabled || w.dirtyGlasses.Count > 0)
                {     
                    w.PickUpEmptyGlasses();

                        if (!timerToClosing.IsEnabled && w.dirtyGlasses.Count == 0)
                            w.WaitressGoHome();
                }

            });

        }

        private string GetElapsedTime()
        {
            string elapsedminutes = printTime.Elapsed.Minutes.ToString("00:");
            string elapsedseconds = printTime.Elapsed.Seconds.ToString("00");

            elapsedtime= elapsedminutes + elapsedseconds;

            return elapsedtime;        
        }

        private void UpdateLabelsValues()
        {
            Dispatcher.Invoke(() =>
            {
                NumberOfGuestsLabel.Content = "Number of guests in the pub: " + guestsInPub.Count();
                NumberofDirtyglassesLabel.Content = "Number of dirty glasses: " + w.dirtyGlasses.Count();
                NumberOfChairsLabel.Content = "Number of chairs: " + chairs.Count();
                NumberOfFilledGlassesLabel.Content = "Number of filled glasses: " + glassesFilledWithBeer.Count();
                NumberOfEmptyGlassesLabel.Content = "Number of glasses on the shelve: " + glassesOnShelve.Count();
                NumberOfTakenChairs.Content = "Number of taken chairs: " + p.takenChairs.Count();
         
            });
        }
        private void AddToGuestListBox(string patronInformation)
        {
                Dispatcher.Invoke(() =>
                {
                    GuestListBox.Items.Insert(0, $"[{GetElapsedTime()}] {patronInformation}");

                });
        }

        private void AddToBartenderListBox(string bartenderInformation)
        {
            Dispatcher.Invoke(() =>
            {
                BartenderListbox.Items.Insert(0, $"[{GetElapsedTime()}] {bartenderInformation}");        
            });
        }

        private void AddToWaitressListBox(string waitressInformation)
        {
            Dispatcher.Invoke(() =>         
            {
                WaitressListBox.Items.Insert(0, $"[{GetElapsedTime()}] {waitressInformation}");
            });
        }


 

        private void FastForwardButton_Click(object sender, RoutedEventArgs e)
        {
            speed = 2;

            ChangeTimeSpeed();

            w.ChangeSpeed(speed);
            b.ChangeSpeed(speed);
            p.ChangeSpeed(speed);
            bar.ChangeSpeed(speed);
            FastForwardButton.IsEnabled = false;
            InfoTextLabel.Content = "Speed x2 is now choosen";
            // skicka in farten som inparameter
        }
    }
}
















