using Calendar.Model;
using CalendarProject.ViewModel;
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
            appointmentDatabase = Utils.DeserializeAppointmentsFile(PathToAppointmentsFile);
            userDatabase = Utils.DeserializeUsersFile(PathToUsersFile);

            SetUsersInListBox();
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
            DateTime startDate = new DateTime(selectedYear, selectedMonth, selectedDay, startHour, startMinute, DateSeconds);
            DateTime endDate = new DateTime(selectedYear, selectedMonth, selectedDay, endHour, endMinute, DateSeconds);
            selectedUsersAppointments = Utils.GetParticipantsAppointments(selectedUsers, appointmentDatabase);

            if (Utils.IsAppointmentInputValid(startDate, endDate, selectedUsersAppointments))
            {
                appointment = new Appointment();
                CreateAppointmentsObject(appointment);
                appointmentDatabase.Serialize(PathToAppointmentsFile);
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

        private void ButtonCancel_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }
        #endregion
    }
}
