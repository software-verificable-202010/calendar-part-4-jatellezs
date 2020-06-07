using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Calendar.Model
{
    [Serializable]
    public class AppointmentDatabase
    {
        #region Fields
        private readonly List<Appointment> appointments;
        #endregion

        #region Properties
        public AppointmentDatabase()
        {
            appointments = new List<Appointment>() { };
        }
        
        public List<Appointment> Appointments
        {
            get
            {
                return appointments;
            }
        }
        #endregion
    }
}
