﻿using System;
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
using System.Windows.Shapes;

namespace SIMS_HCI_Project.View
{
    /// <summary>
    /// Interaction logic for TourReservationView.xaml
    /// </summary>
    public partial class TourReservationView : Window
    {
        public TourReservationView()
        {
            InitializeComponent();
        }

        

        private void btnConfirmReservation_Click(object sender, RoutedEventArgs e)
        {
            //TODO:  show message box if it is successfully reserved, or partially/fully reserved
            //      1. if successfull - add tour to myReservations, message that it is added to myReservations
            //      2. partially - show message: how many places are left
            //      3. fully reserved - show window with   
        }
    }
}
