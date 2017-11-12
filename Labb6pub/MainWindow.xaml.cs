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

namespace Labb6pub
{

    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    //använd cancelltoken grej för att avsluta.


    public partial class MainWindow : Window

    {   
        private BlockingCollection<Patron> queueToBar;
        private Queue<Patron> guestsInBar;
        private BlockingCollection<Glass> glassesFilledWithBeer;
        private BlockingCollection<Glass> glassesOnShelve;

        private BlockingCollection<Chair> chairs;
        Stopwatch timer = new Stopwatch();

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
            guestsInBar = new Queue<Patron>();
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
                NumberOfGuestsLabel.Content = "Number of guests: " + guestsInBar.Count();
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
        private void AddToGuestsInBar(Patron patron)
        {
            guestsInBar.Enqueue(patron);
        
        }
        private void RemoveGuestInBar()
        {
            if (guestsInBar != null)
                guestsInBar.Dequeue();
            if (guestsInBar.Count == 0)
                AllGuestsLeft?.Invoke();

        }


        private void OpenOrCloseBarButton_Click(object sender, RoutedEventArgs e)
        {
            timer.Start();

            OpenOrCloseBarButton.Content = "Close bar";


            IsBarOpen = true;

            SetStartValues();

            b = new Bouncer(AddToGuestListBox);

            p = new Patron(AddToGuestListBox,chairs,queueToBar);

            w = new Waitress(AddToWaitressListBox,glassesFilledWithBeer, glassesOnShelve);

            b.PatronArrived += AddToQueueInBar;
            b.PatronArrived += bar.GetGlass;
            b.AddToGuestInBar += AddToGuestsInBar;
            bar.GotBeer += p.PatronSearchForChair;
            p.PatronLeaved += w.AddEmptyGlasses;
            p.PatronLeaved += RemoveGuestInBar;
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
            {   while (timer.Elapsed < TimeSpan.FromSeconds(120))
                {
                    w.PickUpEmptyGlasses();
                }//gör en task åt waitress.
            });

        }

        private void AddToGuestListBox(string patronInformation)
        {


                Dispatcher.Invoke(() =>
           
                {
                    GuestListBox.Items.Insert(0, patronInformation);
                    NumberOfGuestsLabel.Content = "Number of guests: "+guestsInBar.Count();
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
                BartenderListbox.Items.Insert(0, bartenderInformation);
                NumberOfGuestsLabel.Content = "Number of guests: " + guestsInBar.Count();
                NumberOfEmptyGlassesLabel.Content= "Number of glasses on the shelve: " + glassesOnShelve.Count();
                NumberOfFilledGlassesLabel.Content = "Number of filled glasses: " + glassesFilledWithBeer.Count;
                

            });
        }

        private void AddToWaitressListBox(string waitressInformation)
        {
            Dispatcher.Invoke(() =>
            {
                WaitressListBox.Items.Insert(0, waitressInformation);
                NumberOfEmptyGlassesLabel.Content = "Number of glasses on the shelve: " + glassesOnShelve.Count();
                NumberofDirtyglassesLabel.Content = "Number of dirty glasses: " + w.dirtyGlasses.Count;
                NumberOfGuestsLabel.Content = "Number of guests: " + guestsInBar.Count();

            });
        }

        

      
    }
}
















