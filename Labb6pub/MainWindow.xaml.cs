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

        public MainWindow()
        {
            InitializeComponent();

            //en lista på vilken ordning gästerna kommer i
           

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

            Bartender bar = new Bartender();
            Bouncer b = new Bouncer(AddToGuestListBox);

            b.PatronArrived += GiveGlass; //bartender prenumenerar på att en gäst kommit

            //prenumenera här på events
            Task.Run(() =>
            {
                bar.WaitsForPatron(AddToBartenderListBox);



            });


            Task.Run(() =>
            {
                b.Work(GiveGlass);
            });
            //Task.Run(() =>
            //{

            //    bar.GetGlass(CheckTest);
            //});



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
        private void GiveGlass(string bartenderInformation)
        {
            Dispatcher.Invoke(() =>
            {
                BartenderListbox.Items.Insert(0, bartenderInformation);
            });
        }
    }
}
















