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
    public partial class MainWindow : Window

    {   /* VAD JAG GJORT:
        - gjort blockingcollections
        - lags till elapsedtime för när gästerna kommer in
        - ändrat knappen till close bar efter man öppnat baren
        - flyttat på thread.sleep(1000), tiden det tar för gästen att komma fram till baren
        */



        //private Stack<Glass> stackGlasses;
        //private ConcurrentQueue<Patron> queueToBar;
        private BlockingCollection<Patron> queueToBar;
        private BlockingCollection<Glass> stackGlasses;
        private BlockingCollection<Chair> chairs;

        bool IsGlassAvailable = true;



        Bartender bar;
        Bouncer b;
        Patron p;
        
  
        public MainWindow()
        {
            InitializeComponent();

            //en lista på vilken ordning gästerna kommer i
            //queueToBar = new ConcurrentQueue<Patron>();
            queueToBar = new BlockingCollection<Patron>(); // concurrentqueue är standardklass
            //stackGlasses = new Stack<Glass>();
            stackGlasses = new BlockingCollection<Glass>(new ConcurrentStack<Glass>());
            chairs = new BlockingCollection<Chair>();
            bar = new Bartender(AddToBartenderListBox, queueToBar, stackGlasses, IsGlassAvailable);

        }

        private void SetStartValues()
        {
            
            FillShelveWithGlasses();
            CreateChairs();
           
            Dispatcher.Invoke(() =>
            {
                NumberOfGuestsLabel.Content = "Number of guests: " + GuestListBox.Items.Count.ToString();
                NumberOfEmptyGlassesLabel.Content = "Number of glasses left: " + stackGlasses.Count();
                NumberOfChairsLabel.Content = "Number of chairs: " + chairs.Count();
            });
            
        }

        private void FillShelveWithGlasses()
        {
            for (int i = 1; i <= 8; i++)
            {
            stackGlasses.Add(new Glass());
            }    
        }

        private void CreateChairs()
        {
            for (int i = 0; i < 4; i++)
            {
                chairs.Add(new Chair());
            }
        }

        private void AddToQueueToBar(Patron patron) 
        {
            Thread.Sleep(1000); // tid för gästen att komma till kön
            {
                queueToBar.Add(patron);             
              
                if (stackGlasses.Count == 0)
                    b.PatronArrived -= bar.GetGlass;
            }
        }


        private void OpenOrCloseBarButton_Click(object sender, RoutedEventArgs e)
        {
            OpenOrCloseBarButton.Content = "Close bar";

           

            SetStartValues();

            b = new Bouncer(AddToGuestListBox);

            b.PatronArrived += AddToQueueToBar;

            p = new Patron(AddToGuestListBox,chairs,queueToBar);
            bar.GotBeer += p.PatronSearchForChair;


            b.PatronArrived += bar.GetGlass;

            //prenumenera här på events


            Task bartender = Task.Run(() =>
            {
                bar.WaitsForPatron();
                   
               
            });


            Task bouncer = Task.Run(() =>
            {
                b.Work(chairs,queueToBar);

            });
          
           
            Task waitress = Task.Run(() =>
            {
                //gör en task åt waitress.
            });

        }

        private void AddToGuestListBox(string patronInformation)
        {

            Dispatcher.Invoke(() =>
                {
                    GuestListBox.Items.Insert(0, patronInformation);
                    NumberOfGuestsLabel.Content = "Number of guest: ";
                    NumberOfChairsLabel.Content = "Number of chairs: " + chairs.Count();
                });

        }

        private void AddToBartenderListBox(string bartenderInformation)
        {
            Dispatcher.Invoke(() =>
            {
                BartenderListbox.Items.Insert(0, bartenderInformation);
                NumberOfEmptyGlassesLabel.Content= "Number of glasses left: " + stackGlasses.Count();
                

            });

        }

        

      
    }
}
















