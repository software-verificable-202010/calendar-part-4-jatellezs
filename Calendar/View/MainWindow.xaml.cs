using System;
using System.Globalization;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Formatters.Binary;
using System.IO;
using Calendar.Model;
using Calendar.View;

namespace Calendar
{
    public partial class MainWindow : Window
    {
        #region Constants
        internal int ListPositionOffset = 1;
        internal int FirstDayOfMonth = 1;
        internal int SmallFont = 8;
        internal static int NextMonth = 1;
        internal static int PreviousMonth = -1;
        internal static int FirstDay = 1;
        internal static int FirstPositionOffset = 1;
        internal string TimeFormat = "HH:mm";
        internal string DayFormat = "dddd";
        internal string PathToAppointmentsFile = "Appointments.txt";
        internal string EmptyText = "";
        internal CultureInfo USCultureInfo = new CultureInfo("en-US");
        #endregion

        #region Fields
        private TextBlock textBlockCurrentCell;
        private TextBlock textBlockAppointmentDisplay;
        private TextBlock[] textBlocksCalendarGrid;
        private static readonly string[] months = { "January", "February", "March", "April", "May", "June", "July", "August", "September", "October", "November", "December" };
        private static readonly List<string> monthsOfYear = new List<String>(months);
        private static readonly string[] weekDayNames = { "Monday", "Tuesday", "Wednesday", "Thursday", "Friday", "Saturday", "Sunday" };
        private static readonly List<string> daysOfWeek = new List<string>(weekDayNames);
        private List<ItemsControl> itemsControlsEvents = new List<ItemsControl>() { };
        private int startingPoint;
        private int endingPoint;
        private int dayNumber;
        private int currentYear;
        private int currentMonthNumber;
        private string currentMonthName;
        private string firstWeekDayOfMonth;
        private DateTime firstDayOfMonth;
        private DateTime currentDate = DateTime.Now;
        private AppointmentDatabase appointmentDatabase;
        private readonly User currentUser;
        #endregion

        #region Methods
        public MainWindow(DateTime calendarDate, User user)
        {
            InitializeComponent();

            currentDate = calendarDate;
            currentUser = user;

            DeserializeCalendarContent();
            SetCalendarCells();
            DeleteCellsDayNumber();
            DeleteEvents();
            SetCalendarView(currentDate, appointmentDatabase);
        }

        private void DeserializeCalendarContent()
        {
            try
            {
                IFormatter formatter = new BinaryFormatter();
                Stream stream = new FileStream(PathToAppointmentsFile, FileMode.Open, FileAccess.Read);
                appointmentDatabase = formatter.Deserialize(stream) as AppointmentDatabase;
                stream.Close();
            }
            catch (Exception ex) when (ex is FileNotFoundException || ex is SerializationException)
            {
                appointmentDatabase = new AppointmentDatabase();
            }
        }

        private void SetCalendarCells()
        {
            textBlocksCalendarGrid = new TextBlock[] {
                TextBlockCell0, TextBlockCell1, TextBlockCell2, TextBlockCell3, TextBlockCell4,
                TextBlockCell5, TextBlockCell6, TextBlockCell7, TextBlockCell8, TextBlockCell9,
                TextBlockCell10, TextBlockCell11, TextBlockCell12, TextBlockCell13, TextBlockCell14,
                TextBlockCell15, TextBlockCell16, TextBlockCell17, TextBlockCell18, TextBlockCell19,
                TextBlockCell20, TextBlockCell21, TextBlockCell22, TextBlockCell23, TextBlockCell24,
                TextBlockCell25, TextBlockCell26, TextBlockCell27, TextBlockCell28, TextBlockCell29,
                TextBlockCell30, TextBlockCell31, TextBlockCell32, TextBlockCell33, TextBlockCell34,
                TextBlockCell35, TextBlockCell36, TextBlockCell37, TextBlockCell38, TextBlockCell39,
                TextBlockCell40, TextBlockCell41
            };
        }

        private void DeleteCellsDayNumber()
        {
            foreach (TextBlock cell in textBlocksCalendarGrid)
            {
                cell.Text = EmptyText;
            }
        }

        private void DeleteEvents()
        {
            foreach (ItemsControl itemsControl in itemsControlsEvents)
            {
                itemsControl.Items.Clear();
            }

            itemsControlsEvents.Clear();
        }

        private void SetCalendarView(DateTime selectedDate, AppointmentDatabase appointments)
        {
            SetDateGlobalVariables(selectedDate);

            for (int cellNumber = startingPoint; cellNumber < endingPoint; cellNumber++)
            {
                dayNumber = cellNumber - startingPoint + FirstPositionOffset;
                textBlockCurrentCell = textBlocksCalendarGrid[cellNumber];
                textBlockCurrentCell.Text = dayNumber.ToString(USCultureInfo);
                SetEventsInDay(appointments, new DateTime(currentYear, currentMonthNumber, dayNumber), cellNumber);
            }
        }

        private void SetDateGlobalVariables(DateTime selectedDate)
        {
            currentMonthName = GetMonthName(selectedDate);
            currentMonthNumber = GetMonthNumber(selectedDate);
            currentYear = GetYear(selectedDate);
            firstDayOfMonth = GetFirstDayOfMonth(selectedDate);
            firstWeekDayOfMonth = GetDayOfWeek(firstDayOfMonth);

            TextBlockDisplayedDate.Text = String.Format(USCultureInfo, "{0} {1}",
                currentMonthName, currentYear.ToString(USCultureInfo));

            startingPoint = GetStartingCallendarCell(firstWeekDayOfMonth);
            endingPoint = GetDaysInMonth(currentYear, currentMonthNumber) + startingPoint;
        }

        private string GetMonthName(DateTime selectedDate)
        {
            return monthsOfYear[GetMonthNumber(selectedDate) - ListPositionOffset];
        }

        private int GetMonthNumber(DateTime selectedDate)
        {
            return selectedDate.Month;
        }

        private int GetYear(DateTime selectedDate)
        {
            return selectedDate.Year;
        }

        private DateTime GetFirstDayOfMonth(DateTime selectedDate)
        {
            return new DateTime(GetYear(selectedDate), GetMonthNumber(selectedDate), FirstDay);
        }

        private string GetDayOfWeek(DateTime selectedDate)
        {
            return selectedDate.ToString(DayFormat, USCultureInfo);
        }

        private int GetStartingCallendarCell(string dayOfWeekName)
        {
            return daysOfWeek.IndexOf(dayOfWeekName);
        }

        private int GetDaysInMonth(int year, int monthNUmber)
        {
            return DateTime.DaysInMonth(year, monthNUmber);
        }

        private void SetEventsInDay(AppointmentDatabase appointments, DateTime selectedDate, int cellNumber)
        {
            ItemsControl itemsControlAppointment = new ItemsControl();
            Grid.SetRow(itemsControlAppointment, Grid.GetRow(textBlocksCalendarGrid[cellNumber]));
            Grid.SetColumn(itemsControlAppointment, Grid.GetColumn(textBlocksCalendarGrid[cellNumber]));

            foreach (Appointment appointment in appointments.Appointments)
            {
                bool areEqualDates = appointment.StartDate.Date == selectedDate;
                bool isUserAppointmentInDate = areEqualDates && IsCurrentUserAppointment(appointment);
                if (isUserAppointmentInDate)
                {
                    textBlockAppointmentDisplay = new TextBlock();
                    CreateTextBlockElement(textBlockAppointmentDisplay, appointment, itemsControlAppointment);
                }
            }

            itemsControlAppointment.VerticalAlignment = VerticalAlignment.Bottom;
            daysOfMonth.Children.Add(itemsControlAppointment);
            itemsControlsEvents.Add(itemsControlAppointment);
        }

        private bool IsCurrentUserAppointment(Appointment appointment)
        {
            bool isUserAppointment = false;

            if (appointment.Participants.Find(u => u.Name == currentUser.Name) != null)
            {
                isUserAppointment = true;
            }

            return isUserAppointment;
        }

        private void CreateTextBlockElement(TextBlock appointmentBlock, Appointment appointment, ItemsControl appointmentItemControl)
        {
            appointmentBlock.Text = String.Format(USCultureInfo, "{0}: {1} -> {2}",
                appointment.Title, appointment.StartDate.ToString(TimeFormat, USCultureInfo), appointment.EndDate.ToString(TimeFormat, USCultureInfo));
            appointmentBlock.FontSize = SmallFont;
            appointmentItemControl.Items.Add(appointmentBlock);
        }

        private void WindowKeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Left)
            {
                DeleteCellsDayNumber();
                DeleteEvents();
                currentDate = currentDate.AddMonths(PreviousMonth);
                SetCalendarView(currentDate, appointmentDatabase);
            }
            else if (e.Key == Key.Right)
            {
                DeleteCellsDayNumber();
                DeleteEvents();
                currentDate = currentDate.AddMonths(NextMonth);
                SetCalendarView(currentDate, appointmentDatabase);
            }
            else if (e.Key == Key.Space)
            {
                CreateAndDisplayWeekDetailWindow();
                this.Close();
            }
        }

        private void CreateAndDisplayWeekDetailWindow()
        {
            WeekDetail weekDetailWindow = new WeekDetail(new DateTime(currentYear, currentMonthNumber, FirstDayOfMonth), currentUser);
            weekDetailWindow.Show();
        }

        private void ButtonLogOut_Click(object sender, RoutedEventArgs e)
        {
            CreateAndDisplayLoginWindow();
            this.Close();
        }

        private void CreateAndDisplayLoginWindow()
        {
            LoginWindow loginWindow = new LoginWindow();
            loginWindow.Show();
        }
        #endregion
    }
}
