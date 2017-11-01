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
        Stack<Glass> stackGlasses;
        public event Action<Action<string>> RemovedGlass;
        Bartender bar = new Bartender();

        public MainWindow()
        {
            InitializeComponent();

            //en lista på vilken ordning gästerna kommer i
           

        }

        private void FillShelveWithGlasses()
        {
            stackGlasses = new Stack<Glass>();

            Glass glass1 = new Glass();
            Glass glass2 = new Glass();
            Glass glass3 = new Glass();
            Glass glass4 = new Glass();
            Glass glass5 = new Glass();
            Glass glass6 = new Glass();
            Glass glass7 = new Glass();
            Glass glass8 = new Glass();

            stackGlasses.Push(glass1);
            stackGlasses.Push(glass2);
            stackGlasses.Push(glass3);
            stackGlasses.Push(glass4);
            stackGlasses.Push(glass5);
            stackGlasses.Push(glass6);
            stackGlasses.Push(glass7);
            stackGlasses.Push(glass8);



            Dispatcher.Invoke(() =>
            {
                NumberOfEmptyGlassesLabel.Content = "Number of glasses left: " + stackGlasses.Count();
            });

        }
        private void RemoveGlass()
        {
            

            if (stackGlasses.Count != 0)
            {
                Glass g1 = stackGlasses.Pop(); // ta bort glas
            }
            Dispatcher.Invoke(() =>
            {
                NumberOfEmptyGlassesLabel.Content = "Number of glasses left: " + stackGlasses.Count();
                RemovedGlass?.Invoke(AddToBartenderListBox);
            });

            
        }


        private void QueueToBar()
        {
            ConcurrentQueue<string> queue = new ConcurrentQueue<string>();

            for (int i = 0; i < GuestListBox.Items.Count; i++)
            {
                queue.Enqueue(((Patron)GuestListBox.Items.GetItemAt(i)).Name);

            }
        }

        private void OpenOrCloseBarButton_Click(object sender, RoutedEventArgs e)
        {

           
            Bouncer b = new Bouncer(AddToGuestListBox);

            b.PatronArrived += bar.GetGlass;
            bar.TookGlass += RemoveGlass;
            RemovedGlass += bar.PourBeer;


            FillShelveWithGlasses();

            //prenumenera här på events
            Task.Run(() =>
            {
                bar.WaitsForPatron(AddToBartenderListBox);
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

                });

        }
        private void AddToBartenderListBox(string bartenderInformation)
        {
            Dispatcher.Invoke(() =>
            {
                BartenderListbox.Items.Insert(0, bartenderInformation);

            });

        }
        
    }
}
















