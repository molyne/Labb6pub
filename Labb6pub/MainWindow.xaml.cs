﻿using System;
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
















