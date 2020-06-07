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
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Markup;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace Calendar.View
{
    /// <summary>
    /// Lógica de interacción para CreateAppointment.xaml
    /// </summary>
    public partial class CreateAppointment : Window
    {
        #region Constants
        internal int DateSeconds = 0;
        internal int EmptyListLength = 0;
        internal string PathToAppointmentsFile = "Appointments.txt";
        internal string PathToUsersFile = "Users.txt";
        internal string ErrorMessage = "Invalid Input";
        internal string MessageTitle = "Calendar";
        internal CultureInfo USCultureInfo = new CultureInfo("en-US");
        #endregion

        #region Fields
        private Appointment appointment;
        private List<Appointment> selectedUsersAppointments = new List<Appointment>() { };
        private AppointmentDatabase appointmentDatabase;
        private UserDatabase userDatabase;
        private List<User> availableUsers = new List<User>() { };
        private List<User> selectedUsers = new List<User>() { };
        private readonly User currentUser;
        private int selectedYear;
        private int selectedMonth;
        private int selectedDay;
        private int startHour;
        private int startMinute;
        private int endHour;
        private int endMinute;
        #endregion

        #region Methods
        public CreateAppointment(User user)
        {
            InitializeComponent();

            currentUser = user;

            DeserializeAppointmentsFile();
            DeserializeUsersFile();
            SetUsersInListBox();
        }

        private void DeserializeAppointmentsFile()
        {
            IFormatter readFormatter = new BinaryFormatter();
            Stream readStream = new FileStream(PathToAppointmentsFile, FileMode.OpenOrCreate, FileAccess.Read);

            try
            {
                appointmentDatabase = readFormatter.Deserialize(readStream) as AppointmentDatabase;
            }
            catch (Exception ex) when (ex is FileNotFoundException || ex is SerializationException)
            {
                appointmentDatabase = new AppointmentDatabase();
            }

            readStream.Close();
        }

        private void DeserializeUsersFile()
        {
            IFormatter readFormatter = new BinaryFormatter();
            Stream readStream = new FileStream(PathToUsersFile, FileMode.OpenOrCreate, FileAccess.Read);

            try
            {
                userDatabase = readFormatter.Deserialize(readStream) as UserDatabase;
            }
            catch (Exception ex) when (ex is FileNotFoundException || ex is SerializationException)
            {
                userDatabase = new UserDatabase();
            }

            readStream.Close();
        }

        private void SetUsersInListBox()
        {
            SetAvailableUsersData();

            ListBoxUsers.ItemsSource = availableUsers;
        }

        private void SetAvailableUsersData()
        {
            foreach (User user in userDatabase.RegisteredUsers)
            {
                if (user.Name != currentUser.Name)
                {
                    availableUsers.Add(user);
                }
            }
        }

        private void ButtonCreate_Click(object sender, RoutedEventArgs e)
        {
            SaveInputInformation();
            
            if (IsValidInput())
            {
                appointment = new Appointment();
                CreateAppointmentsObject(appointment);
                SerializeAppointmentsFile(appointmentDatabase);
                this.Close();
            }
            else
            {
                MessageBox.Show(ErrorMessage, MessageTitle);
            }       
        }

        private void SaveInputInformation()
        {
            selectedYear = DatePickerDateOfEvent.SelectedDate.Value.Year;
            selectedMonth = DatePickerDateOfEvent.SelectedDate.Value.Month;
            selectedDay = DatePickerDateOfEvent.SelectedDate.Value.Day;
            startHour = Convert.ToInt32(ComboBoxInitialHour.Text, USCultureInfo);
            startMinute = Convert.ToInt32(ComboBoxInitialMinute.Text, USCultureInfo);
            endHour = Convert.ToInt32(ComboBoxFinalHour.Text, USCultureInfo);
            endMinute = Convert.ToInt32(ComboBoxFinalMinute.Text, USCultureInfo);

            foreach (User user in ListBoxUsers.SelectedItems)
            {
                selectedUsers.Add(user);
            }
        }

        public bool IsValidInput()
        {
            DateTime startDate = new DateTime(selectedYear, selectedMonth, selectedDay, startHour, startMinute, DateSeconds);
            DateTime endDate = new DateTime(selectedYear, selectedMonth, selectedDay, endHour, endMinute, DateSeconds);

            SetParticipantsAppointments(selectedUsers);

            bool isValid = true;
            if (endDate <= startDate)
            {
                isValid = false;
            }
            else if (HasDateCollision(startDate, endDate))
            {
                isValid = false;
            }

            return isValid;
        }

        private void SetParticipantsAppointments(List<User> selectedUsers)
        {
            foreach (User user in selectedUsers)
            {
                SetSelectedUserAppointments(user);
            }
        }

        private void SetSelectedUserAppointments(User user)
        {
            foreach (Appointment appointment in appointmentDatabase.Appointments)
            {
                if (appointment.Participants.Find(u => u.Name == user.Name) != null)
                {
                    selectedUsersAppointments.Add(appointment);
                }
            }
        }

        private bool HasDateCollision(DateTime startDate, DateTime endDate)
        {
            bool hasCollision = false;

            foreach (Appointment appointment in selectedUsersAppointments)
            {
                if (IsBetweenDates(appointment, startDate, endDate))
                {
                    hasCollision = true;
                }
            }

            return hasCollision;
        }

        private bool IsBetweenDates(Appointment appointment, DateTime startDateToCheck, DateTime endDateToCheck)
        {
            bool betweenDates = false;

            bool isStartBeforeAppointmentStart = startDateToCheck <= appointment.StartDate;
            bool isEndAfterAppointmentStart = endDateToCheck >= appointment.StartDate;
            bool isStartBeforeAppointmentEnd = startDateToCheck <= appointment.EndDate;
            bool isEndAfterAppointmentEnd = endDateToCheck >= appointment.EndDate;
            bool isStartAfterAppointmentStart = startDateToCheck >= appointment.StartDate;
            bool isEndBeforeAppointmentEnd = endDateToCheck <= appointment.EndDate;

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

        private void CreateAppointmentsObject(Appointment appointment)
        {
            appointment.Title = TextBoxTitle.Text;
            appointment.Description = TextBoxDescription.Text;
            appointment.StartDate = new DateTime(selectedYear, selectedMonth, selectedDay, startHour, startMinute, DateSeconds);
            appointment.EndDate = new DateTime(selectedYear, selectedMonth, selectedDay, endHour, endMinute, DateSeconds);
            appointment.Creator = currentUser;
            appointment.Participants.Add(currentUser);

            foreach (User user in selectedUsers)
            {
                appointment.Participants.Add(user);
            }

            appointmentDatabase.Appointments.Add(appointment);
        }

        private void SerializeAppointmentsFile(AppointmentDatabase appointments)
        {
            IFormatter writeFormatter = new BinaryFormatter();
            Stream writeStream = new FileStream(PathToAppointmentsFile, FileMode.Create, FileAccess.Write);

            writeFormatter.Serialize(writeStream, appointments);
            writeStream.Close();
        }

        private void ButtonCancel_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }
        #endregion
    }
}
