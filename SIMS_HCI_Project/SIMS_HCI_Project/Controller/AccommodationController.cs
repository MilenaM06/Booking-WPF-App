﻿using SIMS_HCI_Project.FileHandler;
using SIMS_HCI_Project.Observer;
using SIMS_HCI_Project.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SIMS_HCI_Project.Controller
{
    public class AccommodationController: ISubject
    {
        private readonly List<IObserver> _observers;
        private readonly AccommodationFileHandler _fileHandler;

        private static List<Accommodation> _accommodations;

        private readonly OwnerController _ownerController;

        public AccommodationController() 
        {
            if (_accommodations == null)
            {
                _accommodations= new List<Accommodation>();
            }

            _fileHandler = new AccommodationFileHandler();
            _observers = new List<IObserver>();

            _ownerController = new OwnerController();
        
        }

        public List<Accommodation> GetAll()
        {
            return _accommodations;
        }


        public void Load() // load from file
        {
            _accommodations = _fileHandler.Load();
        }


        public void Save() //save to file
        {
            _fileHandler.Save(_accommodations);
        }

        public int GenerateId()
        {
            if (_accommodations.Count == 0)
            {
                return 1;
            }
            else 
            {
                return _accommodations[_accommodations.Count - 1].Id + 1;
            }
        }

        public void Add(Accommodation accommodation) //adds accommodation to all accommodations list and to corresponding owner list
        {
            _accommodations.Add(accommodation);
            _ownerController.AddAccommodationToOwner(accommodation);
            NotifyObservers();
            Save();
        }

        public void Remove(Accommodation accommodation)
        {
            // TO DO
        }

        public void Edit(Accommodation accommodation) 
        {
            // TO DO
        }

        public Accommodation FindById(int id) 
        {
            return _accommodations.Find(a => a.Id == id);
        }

        public void NotifyObservers()
        {
            foreach (var observer in _observers)
            {
                observer.Update();
            }
        }

        public void Subscribe(IObserver observer)
        {
            _observers.Add(observer);
        }

        public void Unsubscribe(IObserver observer)
        {
            _observers.Remove(observer);
        }
    }
}
