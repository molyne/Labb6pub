using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
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

    {
       private Stack<Glass> stackGlasses;
       private ConcurrentQueue<Patron> queueToBar;
       Bartender bar;
  

        public MainWindow()
        {
            InitializeComponent();

            //en lista på vilken ordning gästerna kommer i
            queueToBar = new ConcurrentQueue<Patron>();
            stackGlasses = new Stack<Glass>();
            bar = new Bartender(AddToBartenderListBox, queueToBar, stackGlasses);

        }

        private void SetStartValues()
        {
            FillShelveWithGlasses();
            Dispatcher.Invoke(() =>
            {
                NumberOfGuestsLabel.Content = "Number of guests: " + GuestListBox.Items.Count.ToString();
                NumberOfEmptyGlassesLabel.Content = "Number of glasses left: " + stackGlasses.Count();

            });
        }

        private void FillShelveWithGlasses()
        {
            //gör en concurrent stack
            for (int i = 1; i <= 8; i++)
            {
            stackGlasses.Push(new Glass());

            }    

        }

        private void AddToQueueToBar(Patron patron) 
        {
            if(queueToBar!=null)
            {
                queueToBar.Enqueue(patron);
               
            }
        }


        private void OpenOrCloseBarButton_Click(object sender, RoutedEventArgs e)
        {
            SetStartValues();

            Bouncer b = new Bouncer(AddToGuestListBox);

            b.PatronArrived += AddToQueueToBar;
            b.PatronArrived += bar.GetGlass;
         
            //prenumenera här på events
            Task.Run(() =>
            {
                bar.WaitsForPatron();
            });


            Task.Run(() =>
            {
                b.Work(AddToBartenderListBox);
            });
          

            Task.Run(() =>
            {
                //gör en task åt waitress.
            });

        }

        private void AddToGuestListBox(string patronInformation)
        {

            Dispatcher.Invoke(() =>
                {
                    GuestListBox.Items.Insert(0, patronInformation);
                    NumberOfGuestsLabel.Content = "Number of guest: " + GuestListBox.Items.Count.ToString();

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
















