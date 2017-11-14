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
    //använd cancelltoken grej för att avsluta.


    public partial class MainWindow : Window

        // number of taken chairs slutar alltid på 1 eftersom listbox måste uppdateras en gång till?
        // samma sak med number of guests in pub..
        // tiden måste uppdateras hela tiden?
        
    {   
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

        bool IsGlassAvailable = true;
        private int numberOfGlasses = 8;
        private int numberOfChairs = 9;
        int walkToBarTime = 1000;

        Bartender bar;
        Bouncer b;
        Patron p;
        Waitress w;
        
  
        public MainWindow()
        {
            InitializeComponent();

            //en lista på vilken ordning gästerna kommer i
            //queueToBar = new ConcurrentQueue<Patron>();
            queueToBar = new BlockingCollection<Patron>(); // concurrentqueue är standardklass
            guestsInPub = new Queue<Patron>();
            //stackGlasses = new Stack<Glass>();
            glassesFilledWithBeer = new BlockingCollection<Glass>(new ConcurrentStack<Glass>());
            glassesOnShelve = new BlockingCollection<Glass>(new ConcurrentStack<Glass>());

            chairs = new BlockingCollection<Chair>();
            bar = new Bartender(AddToBartenderListBox, queueToBar, glassesFilledWithBeer, IsGlassAvailable, glassesOnShelve,speed);

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
            if (guestsInPub.Count == 0)

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
        void timer_Tick(object sender, EventArgs e)
        {
          

            if (timeToClosing != 0)
                timeToClosing--;
            else
                timerToClosing.Stop();

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
            timerToClosing = new DispatcherTimer();
            upDateLabelEverySecond = new DispatcherTimer();

            upDateLabelEverySecond.Interval = TimeSpan.FromSeconds(1);
            upDateLabelEverySecond.Tick += timerTvå_Tick;
            upDateLabelEverySecond.Start();

            timerToClosing.Interval = TimeSpan.FromSeconds(1);
            timerToClosing.Tick += timer_Tick;
            timerToClosing.Start();

            printTime.Start();

            OpenOrCloseBarButton.IsEnabled = false;
            FastForwardButton.IsEnabled = false;
            InfoTextLabel.Content = string.Empty;

            SetStartValues();

            b = new Bouncer(AddToGuestListBox);

            p = new Patron(AddToGuestListBox,chairs,speed);

            w = new Waitress(AddToWaitressListBox,glassesFilledWithBeer, glassesOnShelve, speed);

            b.PatronArrived += AddToQueueInBar;
            b.PatronArrived += bar.GetGlass;
            b.PatronArrived += AddToGuestsInPub;
            bar.GotBeer += p.PatronSearchForChair;
            p.PatronLeaved += w.AddEmptyGlasses;
            p.PatronLeaved += RemoveGuestInPub;
            AllGuestsLeft += bar.BartenderGoesHome;


            Task bartender = Task.Run(() =>
            {
              
                bar.WaitsForPatron();
                
            });

            Task bouncer = Task.Run(() =>
            {
                b.Work(chairs,printTime,speed);

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

        private void FastForward(object sender, RoutedEventArgs e)
        {

        }

        private void FastForwardButton_Click(object sender, RoutedEventArgs e)
        {
            speed = 2;
            w.changespeed(speed);
            FastForwardButton.IsEnabled = false;
            InfoTextLabel.Content = "Speed x2 is now choosen";
            // skicka in farten som inparameter
        }
    }
}
















