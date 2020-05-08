using Calendar.Model;
using System;
using System.Collections.Generic;
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
        Appointment appointment;
        Appointments appointments;
        Int32 DATE_SECONDS = 0;
        Int32 selectedYear;
        Int32 selectedMonth;
        Int32 selectedDay;
        Int32 startHour;
        Int32 startMinute;
        Int32 endHour;
        Int32 endMinute;
        string PATH_TO_SERIALIZED_FILE = "Appointments.txt";
        string ERROR_MESSAGE = "Invalid Input";
        string MESSAGE_TITLE = "Calendar";

        public CreateAppointment()
        {
            InitializeComponent();
        }

        private void ButtonCreate_Click(object sender, RoutedEventArgs e)
        {
            DeserializeAppointmentsFile();
            SaveInputInformation();
            
            if (IsValidInput())
            {
                appointment = new Appointment();
                CreateAppointmentsObject(appointment);
                SerializeAppointmentsFile(appointments);
                this.Close();
            }
            else
            {
                MessageBox.Show(ERROR_MESSAGE, MESSAGE_TITLE);
            }

            
        }

        private bool IsValidInput()
        {
            DateTime startDate = new DateTime(selectedYear, selectedMonth, selectedDay, startHour, startMinute, DATE_SECONDS);
            DateTime endDate = new DateTime(selectedYear, selectedMonth, selectedDay, endHour, endMinute, DATE_SECONDS);

            if(endDate <= startDate)
            {
                return false;
            }
            return true;
        }

        private void SaveInputInformation()
        {
            selectedYear = DatePickerDateOfEvent.SelectedDate.Value.Year;
            selectedMonth = DatePickerDateOfEvent.SelectedDate.Value.Month;
            selectedDay = DatePickerDateOfEvent.SelectedDate.Value.Day;
            startHour = Convert.ToInt32(ComboBoxInitialHour.Text);
            startMinute = Convert.ToInt32(ComboBoxInitialMinute.Text);
            endHour = Convert.ToInt32(ComboBoxFinalHour.Text);
            endMinute = Convert.ToInt32(ComboBoxFinalMinute.Text);
        }

        private void CreateAppointmentsObject(Appointment appointment)
        {
            appointment.title = TextBoxTitle.Text;
            appointment.description = TextBoxDescription.Text;
            appointment.startDate = new DateTime(selectedYear, selectedMonth, selectedDay, startHour, startMinute, DATE_SECONDS);
            appointment.endDate = new DateTime(selectedYear, selectedMonth, selectedDay, endHour, endMinute, DATE_SECONDS);
            appointments.appointments.Add(appointment);
        }

        private void SerializeAppointmentsFile(Appointments appointments)
        {
            IFormatter writeFormatter = new BinaryFormatter();
            Stream writeStream = new FileStream(PATH_TO_SERIALIZED_FILE, FileMode.Create, FileAccess.Write);

            writeFormatter.Serialize(writeStream, appointments);
            writeStream.Close();

            this.Close();
        }

        private void DeserializeAppointmentsFile()
        {
            IFormatter readFormatter = new BinaryFormatter();
            Stream readStream = new FileStream(PATH_TO_SERIALIZED_FILE, FileMode.OpenOrCreate, FileAccess.Read);

            try
            {
                appointments = (Appointments)readFormatter.Deserialize(readStream);
            }
            catch
            {
                appointments = new Appointments();
            }

            readStream.Close();
        }

        private void ButtonCancel_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }
    }
}
