﻿using SIMS_HCI_Project.Applications.Services;
using SIMS_HCI_Project.Domain.Models;
using SIMS_HCI_Project.WPF.Commands;
using SIMS_HCI_Project.WPF.Validations;
using SIMS_HCI_Project.WPF.Views.OwnerViews;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows;

namespace SIMS_HCI_Project.WPF.ViewModels.OwnerViewModels
{
    public class AddRenovationViewModel: INotifyPropertyChanged
    {
        private RenovationService _renovationService;
        public Accommodation Accommodation { get; set; }
        public RenvationDateValidation ValidatedRenovationDate { get; set; }
        public Renovation SelectedRenovation { get; set; }
        public AddRenovationView AddRenovationView { get; set; }
        public SelectAccommodationForRenovationView SelectAccommodationForRenovationView { get; set; }
        public RenovationsViewModel RenovationsVM { get; set; }
        

        #region OnPropertyChanged

        private string _description;
        public string Description
        {
            get => _description;
            set
            {
                if (value != _description)
                {
                    _description = value;
                    OnPropertyChanged(nameof(Description));
                }
            }
        }

        private List<Renovation> _availableRenovations;
        public List<Renovation> AvailableRenovations
        {
            get => _availableRenovations;
            set
            {
                if (value != _availableRenovations)
                {

                    _availableRenovations = value;
                    OnPropertyChanged(nameof(AvailableRenovations));
                }
            }
        }

        private string _availableDatesText;
        public string AvailableDatesText
        {
            get => _availableDatesText;
            set
            {
                if (value != _availableDatesText)
                {
                    _availableDatesText = value;
                    OnPropertyChanged(nameof(AvailableDatesText));
                }
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;
        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        #endregion

        public RelayCommand AddNewRenovationCommand { get; set; }
        public RelayCommand CloseAddRenovationViewCommand { get; set; }
        public RelayCommand SearchAvailableRenovationsCommand { get; set; }
        
        public AddRenovationViewModel(AddRenovationView addRenovationView, SelectAccommodationForRenovationView selectAccommodationView, RenovationsViewModel renovationsVM, Accommodation selectedAccommodation)
        {
            InitCommands();

            _renovationService = new RenovationService();

            AddRenovationView = addRenovationView;
            SelectAccommodationForRenovationView = selectAccommodationView;
            RenovationsVM = renovationsVM;

            ValidatedRenovationDate = new RenvationDateValidation();
            Accommodation = selectedAccommodation;

            AvailableRenovations = new List<Renovation>();
            AvailableDatesText = "";
        }

        #region Commands

        public void Executed_SearchAvailableRenovationsCommand(object obj)
        {
            ValidatedRenovationDate.Validate();
            if (ValidatedRenovationDate.IsValid)
            {
                AvailableRenovations = _renovationService.GetAvailableRenovations(Accommodation, ValidatedRenovationDate.EnteredStart, ValidatedRenovationDate.EnteredEnd, ValidatedRenovationDate.DaysNumber ?? 1);
                if (AvailableRenovations.Count() == 0)
                {
                    AvailableDatesText = "There are not any available dates on those days.";
                }
                else 
                {
                    AvailableDatesText = "There are available dates on those days.";
                }
            }
            else 
            {
                MessageBox.Show("Not all fields are field in correctly.");
            }
        }

        public bool CanExecute_SearchAvailableRenovationsCommand(object obj)
        {
            return true;
        }

        private MessageBoxResult ConfirmAddNewRenovation()
        {
            string sMessageBoxText = $"Are you sure you want to add this renovation?";
            string sCaption = "Add Renovation Confirmation";

            MessageBoxButton btnMessageBox = MessageBoxButton.YesNo;
            MessageBoxImage icnMessageBox = MessageBoxImage.Warning;

            MessageBoxResult result = MessageBox.Show(sMessageBoxText, sCaption, btnMessageBox, icnMessageBox);
            return result;
        }

        public void Executed_AddNewRenovationCommand(object obj)
        {
            if (SelectedRenovation != null)
            {
                if (ConfirmAddNewRenovation() == MessageBoxResult.Yes)
                {
                    SelectedRenovation.Description = Description;
                    SelectedRenovation.AccommodationId = Accommodation.Id;
                    SelectedRenovation.Accommodation = Accommodation;
                    _renovationService.Add(SelectedRenovation);
                    AddRenovationView.Close();
                    SelectAccommodationForRenovationView.Close();
                    RenovationsVM.UpdateRenovations();
                }
            }
            else 
            {
                MessageBox.Show("No date range has been selected.");
            }
        }

        public bool CanExecute_AddNewRenovationCommand(object obj)
        {
            return true;
        }

        public void Executed_CloseAddRenovationViewCommand(object obj)
        {
            AddRenovationView.Close();
        }

        public bool CanExecute_CloseAddRenovationViewCommand(object obj)
        {
            return true;
        }
        #endregion

        public void InitCommands()
        {
            AddNewRenovationCommand = new RelayCommand(Executed_AddNewRenovationCommand, CanExecute_AddNewRenovationCommand);
            CloseAddRenovationViewCommand = new RelayCommand(Executed_CloseAddRenovationViewCommand, CanExecute_CloseAddRenovationViewCommand);
            SearchAvailableRenovationsCommand = new RelayCommand(Executed_SearchAvailableRenovationsCommand, CanExecute_SearchAvailableRenovationsCommand);
        }

    }
}
