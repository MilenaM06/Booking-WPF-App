using SIMS_HCI_Project.Domain.DTOs;
using SIMS_HCI_Project.Domain.Models;
using SIMS_HCI_Project.Domain.RepositoryInterfaces;
using SIMS_HCI_Project.Repositories;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace SIMS_HCI_Project.Applications.Services
{
    public class GuestTourAttendanceService
    {
        private readonly IGuestTourAttendanceRepository _guestTourAttendanceRepository;

        public GuestTourAttendanceService()
        {
            _guestTourAttendanceRepository = Injector.Injector.CreateInstance<IGuestTourAttendanceRepository>();

            _userRepository = Injector.Injector.CreateInstance<IUserRepository>();
            _tourTimeRepository = Injector.Injector.CreateInstance<ITourTimeRepository>();
            _tourReservationRepository = Injector.Injector.CreateInstance<ITourReservationRepository>();
        }

        public void Add(GuestTourAttendance guestTourAttendance)
        {
            _guestTourAttendanceRepository.Add(guestTourAttendance);
        }

        public GuestTourAttendance GetById(int id)
        {
            return _guestTourAttendanceRepository.GetById(id);
        }

        public List<GuestTourAttendance> GetAll()
        {
            return _guestTourAttendanceRepository.GetAll();
        }
        
        public List<GuestTourAttendance> GetAllByTourId(int id)
        {
            return _guestTourAttendanceRepository.GetAllByTourId(id);
        }
        public GuestTourAttendance GetByGuestAndTourTimeIds(int guestId, int tourTimeId){
            return _guestTourAttendanceRepository.GetByGuestAndTourTimeIds(guestId, tourTimeId);
        }

        public bool IsPresent(int guestId, int tourTimeId)
        {
            return _guestTourAttendanceRepository.IsPresent(guestId, tourTimeId);
        }

        public List<TourTime> GetTourTimesWhereGuestWasPresent(int guestId) 
        {
            List<TourTime> tourTimes = new List<TourTime>();
            foreach (var gta in _guestTourAttendanceRepository.GetAllByGuestId(guestId))
            {
                if (gta.Status == AttendanceStatus.PRESENT)
                {
                    tourTimes.Add(_tourTimeRepository.GetById(gta.TourTimeId));
                }
            }
            return tourTimes;
        }

        public void ConfirmAttendanceForTourTime(int guestId, int tourTimeId)
        {
            _guestTourAttendanceRepository.ConfirmAttendanceForTourTime(guestId, tourTimeId);
        }

        public List<GuestTourAttendance> GetByConfirmationRequestedStatus(int guestId)
        {
            return _guestTourAttendanceRepository.GetByConfirmationRequestedStatus(guestId);
        }

        public void MarkGuestAsPresent(GuestTourAttendance guestTourAttendance)
        {
            guestTourAttendance.Status = AttendanceStatus.CONFIRMATION_REQUESTED;
            _guestTourAttendanceRepository.Update(guestTourAttendance);
        }
    }

}
