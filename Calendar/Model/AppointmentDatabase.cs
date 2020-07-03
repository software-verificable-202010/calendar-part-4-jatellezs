using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Formatters.Binary;
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

        #region Methods
        public void Serialize(string path)
        {
            IFormatter writeFormatter = new BinaryFormatter();
            Stream writeStream = new FileStream(path, FileMode.Create, FileAccess.Write);

            writeFormatter.Serialize(writeStream, this);
            writeStream.Close();
        }

        public List<Appointment> GetSelectedUserWantedAppointments(User user, Appointment unwantedAppointment)
        {
            List<Appointment> userAppointments = new List<Appointment>();

            foreach (Appointment appointment in this.Appointments)
            {
                bool isParticipant = appointment.Participants.Find(u => u.Name == user.Name) != null;
                bool isSelectedAppointment = appointment == unwantedAppointment;
                bool isValidAppointment = isParticipant && !isSelectedAppointment;
                if (isValidAppointment)
                {
                    userAppointments.Add(appointment);
                }
            }

            return userAppointments;
        }

        public List<Appointment> GetSelectedUserAppointments(User user)
        {
            List<Appointment> userAppointments = new List<Appointment>();

            foreach (Appointment appointment in this.Appointments)
            {
                if (appointment.Participants.Find(u => u.Name == user.Name) != null)
                {
                    userAppointments.Add(appointment);
                }
            }

            return userAppointments;
        }
        #endregion
    }
}
