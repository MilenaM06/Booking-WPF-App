﻿using SIMS_HCI_Project.Applications.Services;
using SIMS_HCI_Project.Domain.Models;
using SIMS_HCI_Project.WPF.Commands;
using SIMS_HCI_Project.WPF.Views.OwnerViews;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace SIMS_HCI_Project.WPF.ViewModels.OwnerViewModels
{
    public class RenovationsViewModel
    {
        private readonly RenovationService _renovationService;
        public RenovationsView RenovationsView { get; set; }
        public Owner Owner { get; set; }
        public ObservableCollection<Renovation> Renovations { get; set; }
        public Renovation SelectedRenovation { get; set; } 
        public RelayCommand AddRenovationCommand { get; set; }
        public RelayCommand CancelRenovationCommand { get; set; }
        public RelayCommand ShowGuestSuggestionsCommand { get; set; }
        public RelayCommand CreatePDFReportCommand { get; set; }
        public RelayCommand CloseRenovationsViewCommand { get; set; }

        public RenovationsViewModel(RenovationsView renovationsView, Owner owner)
        {
            InitCommands();

            _renovationService = new RenovationService();

            RenovationsView = renovationsView;
            Owner = owner;
            Renovations = new ObservableCollection<Renovation>(_renovationService.GetByOwnerId(Owner.Id));
        }

        #region Commands
        public void Executed_AddRenovationCommand(object obj)
        {
            Window selectAccommodationForRenovation = new SelectAccommodationForRenovationView(this, Owner);
            selectAccommodationForRenovation.ShowDialog();
        }

        public bool CanExecute_AddRenovationCommand(object obj)
        {
            return true;
        }

        private MessageBoxResult ConfirmCancelRenovation()
        {
            string sMessageBoxText = $"Are you sure you want to cancel this renovation?";
            string sCaption = "Cancel Renovation Confirmation";

            MessageBoxButton btnMessageBox = MessageBoxButton.YesNo;
            MessageBoxImage icnMessageBox = MessageBoxImage.Warning;

            MessageBoxResult result = MessageBox.Show(sMessageBoxText, sCaption, btnMessageBox, icnMessageBox);
            return result;
        }

        public void Executed_CancelRenovationCommand(object obj)
        {
            if (SelectedRenovation != null)
            {
                if (_renovationService.CancelRenovation(SelectedRenovation))
                {
                    if (ConfirmCancelRenovation() == MessageBoxResult.Yes)
                    {
                        UpdateRenovations();
                    }
                }
                else if (SelectedRenovation.End < DateTime.Today)
                {
                    MessageBox.Show("The renovation can't be cancelled as it has already been completed.");
                }
                else 
                {
                    MessageBox.Show("The renovation can't be canceled as the start date is less than 5 days from now.");
                }
            }
            else 
            {
                MessageBox.Show("No renovation has been selected.");
            }
        }

        public bool CanExecute_CancelRenovationCommand(object obj)
        {
            return true;
        }

        public void Executed_ShowGuestSuggestionsCommand(object obj)
        {
            Window suggestionsForRenovation = new SuggestionsForRenovationView(Owner);
            suggestionsForRenovation.ShowDialog();
        }

        public bool CanExecute_ShowGuestSuggestionsCommand(object obj)
        {
            return true;
        }

        public void Executed_CreatePDFReportCommand(object obj)
        {
            Window createPDFView = new CreatePDFView(Owner);
            createPDFView.ShowDialog();
        }

        public bool CanExecute_CreatePDFReportCommand(object obj)
        {
            return true;
        }

        public void Executed_CloseRenovationsViewCommand(object obj)
        {
            RenovationsView.Close();
        }

        public bool CanExecute_CloseRenovationsViewCommand(object obj)
        {
            return true;
        }
        #endregion

        public void InitCommands()
        {
            AddRenovationCommand = new RelayCommand(Executed_AddRenovationCommand, CanExecute_AddRenovationCommand);
            CancelRenovationCommand = new RelayCommand(Executed_CancelRenovationCommand, CanExecute_CancelRenovationCommand);
            ShowGuestSuggestionsCommand = new RelayCommand(Executed_ShowGuestSuggestionsCommand, CanExecute_ShowGuestSuggestionsCommand);
            CreatePDFReportCommand = new RelayCommand(Executed_CreatePDFReportCommand, CanExecute_CreatePDFReportCommand);
            CloseRenovationsViewCommand = new RelayCommand(Executed_CloseRenovationsViewCommand, CanExecute_CloseRenovationsViewCommand);
        }

        public void UpdateRenovations()
        {
            Renovations.Clear();
            foreach (Renovation renovation in _renovationService.GetByOwnerId(Owner.Id))
            {
                Renovations.Add(renovation);
            }
        }
    }
}
