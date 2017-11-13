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
        Stopwatch timer = new Stopwatch();
        string elapsedtime;

        DispatcherTimer timerTest;

        int time = 30;

        public event Action AllGuestsLeft;

        bool IsGlassAvailable = true;
        bool IsBarOpen = false;
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
            bar = new Bartender(AddToBartenderListBox, queueToBar, glassesFilledWithBeer, IsGlassAvailable, glassesOnShelve);

        }

       

        private void SetStartValues()
        {
            FillShelveWithGlasses();
        
            CreateChairs();
           
            Dispatcher.Invoke(() =>
            {
                NumberOfGuestsLabel.Content = "Number of guests in the pub: " + guestsInPub.Count();
                NumberOfEmptyGlassesLabel.Content = "Number of glasses left: " + glassesOnShelve.Count();
                NumberOfChairsLabel.Content = "Number of chairs: " + chairs.Count();
            });
            
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
            if (time != 0)
                time--;
            else
                timer.Stop();

            if (time <= 10)
            {
                if (time %2==0)
                {
                    ClosingTimeLabel.Foreground = Brushes.Red;
                }
                else
                    ClosingTimeLabel.Foreground = Brushes.White;
            }

            ClosingTimeLabel.Content = FormatTime(time);
            
        }


        private void OpenOrCloseBarButton_Click(object sender, RoutedEventArgs e)
        {
            timerTest = new DispatcherTimer();

            timerTest.Interval = TimeSpan.FromSeconds(1);
            timerTest.Tick += timer_Tick;
            timerTest.Start();

            timerTest.Start();
            timer.Start();

            OpenOrCloseBarButton.IsEnabled = false;


            IsBarOpen = true;

            SetStartValues();

            b = new Bouncer(AddToGuestListBox);

            p = new Patron(AddToGuestListBox,chairs,queueToBar);

            w = new Waitress(AddToWaitressListBox,glassesFilledWithBeer, glassesOnShelve);

            b.PatronArrived += AddToQueueInBar;
            b.PatronArrived += bar.GetGlass;
            b.PatronArrived += AddToGuestsInPub;
            //b.AddToGuestInBar += AddToGuestsInBar;
            bar.GotBeer += p.PatronSearchForChair;
            p.PatronLeaved += w.AddEmptyGlasses;
            p.PatronLeaved += RemoveGuestInPub;
            AllGuestsLeft += w.WaitressGoHome;
            AllGuestsLeft += bar.BartenderGoesHome;
            

            //prenumenera här på events


            Task bartender = Task.Run(() =>
            {
                bar.WaitsForPatron();     
               
            });

            Task bouncer = Task.Run(() =>
            {
                b.Work(chairs,queueToBar,timer);

            });
          
           
            Task waitress = Task.Run(() =>
            {   while (timer.Elapsed < TimeSpan.FromSeconds(120)) // tills alla gästerna gått hem?
                {
                    w.PickUpEmptyGlasses();
                }//gör en task åt waitress.
            });

        }

        private string GetElapsedTime()
        {

            string elapsedminutes = timer.Elapsed.Minutes.ToString("00:");
            string elapsedseconds = timer.Elapsed.Seconds.ToString("00");


            elapsedtime= elapsedminutes + elapsedseconds;

            return elapsedtime;

           
        }
        private void AddToGuestListBox(string patronInformation)
        {


                Dispatcher.Invoke(() =>
           
                {
                    GuestListBox.Items.Insert(0, $"[{GetElapsedTime()}] {patronInformation}");
                    NumberOfGuestsLabel.Content = "Number of guests in the pub: "+guestsInPub.Count();
                    NumberOfChairsLabel.Content = "Number of chairs: " + chairs.Count();
                    NumberofDirtyglassesLabel.Content = "Number of dirty glasses: " + w.dirtyGlasses.Count;
                    NumberOfFilledGlassesLabel.Content = "Number of filled glasses: " + glassesFilledWithBeer.Count;
                    NumberOfEmptyGlassesLabel.Content = "Number of glasses on the shelve: " + glassesOnShelve.Count();
                    NumberOfTakenChairs.Content = "Number of taken chairs: " + p.takenChairs.Count();

                });
     


        }

        private void AddToBartenderListBox(string bartenderInformation)
        {
            Dispatcher.Invoke(() =>
            {
                BartenderListbox.Items.Insert(0, $"[{GetElapsedTime()}] {bartenderInformation}");
                NumberOfEmptyGlassesLabel.Content= "Number of glasses on the shelve: " + glassesOnShelve.Count();
                NumberOfFilledGlassesLabel.Content = "Number of filled glasses: " + glassesFilledWithBeer.Count;
           
                

            });
        }

        private void AddToWaitressListBox(string waitressInformation)
        {
            Dispatcher.Invoke(() =>
            
            {
                WaitressListBox.Items.Insert(0, $"[{GetElapsedTime()}] {waitressInformation}");
                NumberOfEmptyGlassesLabel.Content = "Number of glasses on the shelve: " + glassesOnShelve.Count();
                NumberofDirtyglassesLabel.Content = "Number of dirty glasses: " + w.dirtyGlasses.Count;

            });
        }

        

      
    }
}
















