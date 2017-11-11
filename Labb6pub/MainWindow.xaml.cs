﻿using System;
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
        private BlockingCollection<Glass> glassesFilledWithBeer;
        private BlockingCollection<Glass> glassesOnShelve;

        private BlockingCollection<Chair> chairs;

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
                NumberOfGuestsLabel.Content = "Number of guests: " + GuestListBox.Items.Count.ToString();
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

        private void AddToQueueToBar(Patron patron) 
        {
            Thread.Sleep(walkToBarTime); // tid för gästen att komma till kön
            {
                queueToBar.Add(patron);             
            }
        }


        private void OpenOrCloseBarButton_Click(object sender, RoutedEventArgs e)
        {
            OpenOrCloseBarButton.Content = "Close bar";


            IsBarOpen = true;

            SetStartValues();

            b = new Bouncer(AddToGuestListBox);

            p = new Patron(AddToGuestListBox,chairs,queueToBar);

            w = new Waitress(AddToWaitressListBox,glassesFilledWithBeer, glassesOnShelve);

            b.PatronArrived += AddToQueueToBar;
            b.PatronArrived += bar.GetGlass;
            bar.GotBeer += p.PatronSearchForChair;
            p.PatronLeaved += w.AddEmptyGlasses;
            

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
            {   while (IsBarOpen)
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
                    NumberOfGuestsLabel.Content = "Number of guests: ";
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

            });
        }

        

      
    }
}
















