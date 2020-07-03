using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Calendar.Model
{
    [Serializable]
    public class Appointment
    {
        #region Fields
        private DateTime startDate;
        private DateTime endDate;
        private string title;
        private string description;
        private User creator;
        private List<User> participants;
        #endregion

        #region Properties
        public Appointment()
        {
            participants = new List<User>() { };
        }

        public DateTime StartDate
        {
            get
            {
                return startDate;
            }
            set
            {
                startDate = value;
            }
        }

        public DateTime EndDate
        {
            get
            {
                return endDate;
            }
            set
            {
                endDate = value;
            }
        }

        public string Title
        {
            get
            {
                return title;
            }
            set
            {
                title = value;
            }
        }

        public string Description
        {
            get
            {
                return description;
            }
            set
            {
                description = value;
            }
        }

        public User Creator
        {
            get
            {
                return creator;
            }
            set
            {
                creator = value;
            }
        }

        public List<User> Participants
        {
            get
            {
                return participants;
            }
        }
        #endregion

        #region Methods
        public bool IsUserAppointmentCreator(User user)
        {
            bool isSelectedUserAppointment = false;

            if (user != null)
            {
                if (this.Creator.Name == user.Name)
                {
                    isSelectedUserAppointment = true;
                }
            }

            return isSelectedUserAppointment;
        }

        public bool IsUserAppointment(User user)
        {
            bool isUserAppointment = false;

            if (this.Participants.Find(u => u.Name == user.Name) != null)
            {
                isUserAppointment = true;
            }

            return isUserAppointment;
        }

        public bool IsBetweenDates(DateTime startDateToCheck, DateTime endDateToCheck)
        {
            bool betweenDates = false;

            bool isStartBeforeAppointmentStart = startDateToCheck <= this.StartDate;
            bool isEndAfterAppointmentStart = endDateToCheck >= this.StartDate;
            bool isStartBeforeAppointmentEnd = startDateToCheck <= this.EndDate;
            bool isEndAfterAppointmentEnd = endDateToCheck >= this.EndDate;
            bool isStartAfterAppointmentStart = startDateToCheck >= this.StartDate;
            bool isEndBeforeAppointmentEnd = endDateToCheck <= this.EndDate;

            bool isBetweenDatesLower = isStartBeforeAppointmentStart && isEndAfterAppointmentStart;
            bool isBetweenDatesUpper = isStartBeforeAppointmentEnd && isEndAfterAppointmentEnd;
            bool isBetweenDatesMiddle = isStartAfterAppointmentStart && isEndBeforeAppointmentEnd;

            bool hasCollisionBetweenDates = isBetweenDatesLower || isBetweenDatesMiddle || isBetweenDatesUpper;

            if (hasCollisionBetweenDates)
            {
                betweenDates = true;
            }

            return betweenDates;
        }
        #endregion
    }
}
