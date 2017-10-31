using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
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


        }

        private void OpenOrCloseBarButton_Click(object sender, RoutedEventArgs e)
        {
            Task.Run(() =>
            {
                Bouncer b = new Bouncer(AddToGuestListBox);
                b.Work();
            });
        }

        private void AddToGuestListBox(string name)
        {

            Dispatcher.Invoke(() =>
                {
                    GuestListBox.Items.Insert(0, name+ " enters and goes to the bar.");

                });

        }
    }
}
















