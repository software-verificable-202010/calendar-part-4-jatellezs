using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Calendar.Model
{
    [Serializable]
    class Appointments
    {
        internal List<Appointment> appointments { get; set; }

        public Appointments()
        {
            appointments = new List<Appointment>() { };
        }
    }
}
