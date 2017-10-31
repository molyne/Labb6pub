using System;
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
        bool IsBarOpen = false;

        public MainWindow()
        {
            InitializeComponent();

            
        }

        private void OpenOrCloseBarButton_Click(object sender, RoutedEventArgs e)
        {

                Bartender bar = new Bartender();
                Bouncer b = new Bouncer(AddToGuestListBox);

                b.PatronArrived += bar.GetGlass;

            //prenumenera här på events
            Task.Run(() => 
            {
                bar.WaitsForPatron(AddToBartenderListBox);
                
                
            });

           
                Task.Run(() =>
                {
                    b.Work();
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
















