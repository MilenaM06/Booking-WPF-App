﻿using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SIMS_HCI_Project.Model
{
    // Yes, this class is probably wrong for the pattern
    public class Guide : User
    {
        public ObservableCollection<Tour> Tours { get; set; } // this feels illegal

        public Guide()
        {
            Tours = new ObservableCollection<Tour>();
        }

        public Guide(string id, string username, string password)
        {
            Id = id;
            Username = username;
            Password = password;
            Tours = new ObservableCollection<Tour>();
        }

        public void AddTour(Tour tour) // This should probably be in controller?
        {
            Tours.Add(tour);
        }
    }
}