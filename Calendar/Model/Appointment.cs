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
    }
}
