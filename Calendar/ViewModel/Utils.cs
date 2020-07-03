using Calendar.Model;
using Microsoft.Win32.SafeHandles;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Formatters.Binary;
using System.Text;
using System.Threading.Tasks;

namespace CalendarProject.ViewModel
{
    public static class Utils
    {
        #region Constants
        internal static int FirstDay = 1;
        internal static int ListPositionOffset = 1;
        internal static string DayFormat = "dddd";
        internal static CultureInfo USCultureInfo = new CultureInfo("en-US");
        #endregion

        #region Fields
        private static UserDatabase userDatabase;
        private static AppointmentDatabase appointmentDatabase;
        private static readonly string[] months = { "January", "February", "March", "April", "May", "June", "July", "August", "September", "October", "November", "December" };
        private static readonly List<string> monthsOfYear = new List<String>(months);
        private static readonly string[] weekDayNames = { "Monday", "Tuesday", "Wednesday", "Thursday", "Friday", "Saturday", "Sunday" };
        private static readonly List<string> daysOfWeek = new List<string>(weekDayNames);
        #endregion

        #region Methods
        public static UserDatabase DeserializeUsersFile(string path)
        {
            try
            {
                IFormatter readFormatter = new BinaryFormatter();
                Stream readStream = new FileStream(path, FileMode.OpenOrCreate, FileAccess.Read);
                userDatabase = readFormatter.Deserialize(readStream) as UserDatabase;
                readStream.Close();
            }
            catch (Exception ex) when (ex is FileNotFoundException || ex is SerializationException)
            {
                userDatabase = new UserDatabase();
            }

            return userDatabase;
        }

        public static AppointmentDatabase DeserializeAppointmentsFile(string path)
        {
            try
            {
                IFormatter formatter = new BinaryFormatter();
                Stream stream = new FileStream(path, FileMode.Open, FileAccess.Read);
                appointmentDatabase = formatter.Deserialize(stream) as AppointmentDatabase;
                stream.Close();
            }
            catch (Exception ex) when (ex is FileNotFoundException || ex is SerializationException)
            {
                appointmentDatabase = new AppointmentDatabase();
            }

            return appointmentDatabase;
        }

        public static string GetMonthName(DateTime selectedDate)
        {
            return monthsOfYear[GetMonthNumber(selectedDate) - ListPositionOffset];
        }

        public static DateTime GetFirstDayOfMonth(DateTime selectedDate)
        {
            return new DateTime(GetYear(selectedDate), GetMonthNumber(selectedDate), FirstDay);
        }

        public static int GetMonthNumber(DateTime selectedDate)
        {
            return selectedDate.Month;
        }

        public static int GetYear(DateTime selectedDate)
        {
            return selectedDate.Year;
        }

        public static string GetDayOfWeek(DateTime selectedDate)
        {
            return selectedDate.ToString(DayFormat, USCultureInfo);
        }

        public static int GetStartingCallendarCell(string dayOfWeekName)
        {
            return daysOfWeek.IndexOf(dayOfWeekName);
        }

        public static int GetDaysInMonth(int year, int monthNumber)
        {
            return DateTime.DaysInMonth(year, monthNumber);
        }

        public static List<Appointment> GetParticipantsWantedAppointments(List<User> selectedUsers, AppointmentDatabase appointmentDatabase, Appointment selectedAppointment)
        {
            List<Appointment> selectedUsersAppointments = new List<Appointment>();

            foreach (User user in selectedUsers)
            {
                selectedUsersAppointments.AddRange(appointmentDatabase.GetSelectedUserWantedAppointments(user, selectedAppointment));
            }

            return selectedUsersAppointments;
        }

        public static bool IsAppointmentInputValid(DateTime startDate, DateTime endDate, List<Appointment> usersAppointments)
        {
            bool isValid = true;

            if (endDate <= startDate)
            {
                isValid = false;
            }
            else if (HasDateCollision(startDate, endDate, usersAppointments))
            {
                isValid = false;
            }

            return isValid;
        }

        public static bool HasDateCollision(DateTime startDate, DateTime endDate, List<Appointment> usersAppointments)
        {
            bool hasCollision = false;

            foreach (Appointment appointment in usersAppointments)
            {
                if (appointment.IsBetweenDates(startDate, endDate))
                {
                    hasCollision = true;
                }
            }

            return hasCollision;
        }

        public static List<Appointment> GetParticipantsAppointments(List<User> selectedUsers, AppointmentDatabase appointmentDatabase)
        {
            List<Appointment> selectedUsersAppointments = new List<Appointment>();

            foreach (User user in selectedUsers)
            {
                selectedUsersAppointments.AddRange(appointmentDatabase.GetSelectedUserAppointments(user));
            }

            return selectedUsersAppointments;
        }
        #endregion
    }
}
