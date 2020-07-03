using Calendar.Model;
using CalendarProject.ViewModel;
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
    /// Lógica de interacción para ManageAppointmentsWindow.xaml
    /// </summary>
    public partial class ManageAppointmentsWindow : Window
    {
        #region Constants
        internal string PathToAppointmentsFile = "Appointments.txt";
        internal string PathToUsersFile = "Users.txt";
        internal string ErrorMessage = "Invalid Input";
        internal string MessageTitle = "Calendar";
        internal CultureInfo USCultureInfo = new CultureInfo("en-US");
        internal int DateSeconds = 0;
        #endregion

        #region Fields
        private readonly User currentUser;
        private List<User> availableUsers = new List<User>() { };
        private List<User> selectedUsers = new List<User>() { };
        private UserDatabase userDatabase;
        private AppointmentDatabase appointmentDatabase;
        private List<Appointment> userAppointments = new List<Appointment>() { };
        private List<Appointment> selectedUsersAppointments = new List<Appointment>() { };
        private Appointment selectedAppointment;
        private int selectedYear;
        private int selectedMonth;
        private int selectedDay;
        private int startHour;
        private int startMinute;
        private int endHour;
        private int endMinute;
        #endregion

        #region Methods
        public ManageAppointmentsWindow(User user)
        {
            InitializeComponent();

            currentUser = user;
            appointmentDatabase = Utils.DeserializeAppointmentsFile(PathToAppointmentsFile);
            userDatabase = Utils.DeserializeUsersFile(PathToUsersFile);

            SetAppointmentsListBoxData();
            SetUserListBoxData();
        }

        private void SetAppointmentsListBoxData()
        {
            SetUserAppointmentsData();

            ListBoxMyAppointments.ItemsSource = userAppointments;
        }

        private void SetUserAppointmentsData()
        {
            foreach (Appointment appointment in appointmentDatabase.Appointments)
            {
                if (appointment.IsUserAppointmentCreator(currentUser))
                {
                    userAppointments.Add(appointment);
                }
            }
        }

        private void SetUserListBoxData()
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

        private void ListBoxMyAppointments_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            ListBoxUsers.SelectedItems.Clear();
            SetSelectedAppointmentData();
        }

        private void SetSelectedAppointmentData()
        {
            selectedAppointment = ListBoxMyAppointments.SelectedItem as Appointment;

            TextBoxTitle.Text = selectedAppointment.Title;
            TextBoxDescription.Text = selectedAppointment.Description;
            ComboBoxStartHour.SelectedIndex = selectedAppointment.StartDate.Hour;
            ComboBoxStartMinute.SelectedIndex = selectedAppointment.StartDate.Minute;
            ComboBoxEndHour.SelectedIndex = selectedAppointment.EndDate.Hour;
            ComboBoxEndMinute.SelectedIndex = selectedAppointment.EndDate.Minute;
            DatePickerDateOfEvent.SelectedDate = selectedAppointment.StartDate;

            foreach (User user in ListBoxUsers.ItemsSource)
            {
                if (selectedAppointment.Participants.Find(u => u.Name == user.Name) != null)
                {
                    ListBoxUsers.SelectedItems.Add(user);
                }
            }
        }

        private void ButtonDelete_Click(object sender, RoutedEventArgs e)
        {
            selectedAppointment = ListBoxMyAppointments.SelectedItem as Appointment;
            appointmentDatabase.Appointments.Remove(selectedAppointment);
            appointmentDatabase.Serialize(PathToAppointmentsFile);
        }

        private void ButtonCancel_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        private void ButtonUpdate_Click(object sender, RoutedEventArgs e)
        {
            selectedAppointment = ListBoxMyAppointments.SelectedItem as Appointment;
            Appointment appointmentInDatabase = appointmentDatabase.Appointments.Find(r => r.Equals(selectedAppointment));

            SetNewAppointmentData();
            DateTime startDate = new DateTime(selectedYear, selectedMonth, selectedDay, startHour, startMinute, DateSeconds);
            DateTime endDate = new DateTime(selectedYear, selectedMonth, selectedDay, endHour, endMinute, DateSeconds);
            selectedUsersAppointments = Utils.GetParticipantsWantedAppointments(selectedUsers, appointmentDatabase, ListBoxMyAppointments.SelectedItem as Appointment);

            bool hasItemSelected = ListBoxMyAppointments.SelectedItem != null;
            bool isUpdateValid = hasItemSelected && Utils.IsAppointmentInputValid(startDate, endDate, selectedUsersAppointments);
            if (isUpdateValid)
            {
                UpdateAppointment(appointmentInDatabase);
                appointmentDatabase.Serialize(PathToAppointmentsFile);
            }
            else
            {
                MessageBox.Show(ErrorMessage, MessageTitle);
            }
        }

        private void SetNewAppointmentData()
        {
            selectedYear = DatePickerDateOfEvent.SelectedDate.Value.Year;
            selectedMonth = DatePickerDateOfEvent.SelectedDate.Value.Month;
            selectedDay = DatePickerDateOfEvent.SelectedDate.Value.Day;
            startHour = Convert.ToInt32(ComboBoxStartHour.Text, USCultureInfo);
            startMinute = Convert.ToInt32(ComboBoxStartMinute.Text, USCultureInfo);
            endHour = Convert.ToInt32(ComboBoxEndHour.Text, USCultureInfo);
            endMinute = Convert.ToInt32(ComboBoxEndMinute.Text, USCultureInfo);
            selectedUsersAppointments.Clear();
            selectedUsers.Clear();

            foreach (User user in ListBoxUsers.SelectedItems)
            {
                selectedUsers.Add(user);
            }
        }

        private void UpdateAppointment(Appointment appointment)
        {
            appointment.Title = TextBoxTitle.Text;
            appointment.Description = TextBoxDescription.Text;
            appointment.StartDate = new DateTime(selectedYear, selectedMonth, selectedDay, startHour, startMinute, DateSeconds);
            appointment.EndDate = new DateTime(selectedYear, selectedMonth, selectedDay, endHour, endMinute, DateSeconds);
            appointment.Participants.Clear();
            appointment.Participants.Add(currentUser);

            foreach (User user in ListBoxUsers.SelectedItems)
            {
                appointment.Participants.Add(user);
            }
        }
        #endregion
    }
}
