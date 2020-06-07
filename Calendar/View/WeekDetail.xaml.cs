using Calendar.Model;
using Calendar.View;
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
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace Calendar
{
    /// <summary>
    /// Lógica de interacción para WeekDetail.xaml
    /// </summary>
    public partial class WeekDetail : Window
    {
        #region Constants
        internal int CurrentDayOfMonth = 1;
        internal int FirstDayOfMonth = 1;
        internal int AddDay = 1;
        internal int SubstractDay = -1;
        internal int DaysInWeek = 7;
        internal int TwoWeeksAgo = -14;
        internal int FirstDayOfWeekPosition = 0;
        internal int BiMonthWeek = 2;
        internal int BiYearWeek = 2;
        internal int ListOffset = 1;
        internal int FirstMonthInList = 0;
        internal int SecondMonthInList = 1;
        internal int FirstYearInList = 0;
        internal int SecondYearInList = 1;
        internal int AddEvent = 1;
        internal int EventPosition = 0;
        internal int EventsRemainingPosition = 1;
        internal int SmallFont = 8;
        internal int NoMargin = 0;
        internal int FirstDataColumn = 1;
        internal int GreenBackgroundColor = 0;
        internal int ColumnOffset = 1;
        internal int[] DefaultEvent = { 1, 0 };
        internal string DayFormat = "dddd";
        internal string PathToAppointmentsFile = "Appointments.txt";
        internal CultureInfo USCultureInfo = new CultureInfo("en-US");
        #endregion

        #region Fields
        private TextBlock textBlockAppointmentDisplay;
        private TextBlock[] textBlocksWeekGrid;
        private static readonly string[] months = { "January", "February", "March", "April", "May", "June", "July", "August", "September", "October", "November", "December" };
        private static readonly List<string> monthsOfYear = new List<String>(months);
        private static readonly string[] weekDayNames = { "Monday", "Tuesday", "Wednesday", "Thursday", "Friday", "Saturday", "Sunday" };
        private static readonly List<string> daysOfWeek = new List<string>(weekDayNames);
        private readonly List<Brush> appointmentsBackgroundColors = new List<Brush>() { Brushes.LightGreen };
        private List<string> monthsInWeek = new List<string>();
        private List<string> yearsInWeek = new List<string>();
        private List<DateTime> datesInCurrentWeek = new List<DateTime>() { };
        private Dictionary<int, Dictionary<int, List<int>>> numberOfEventsInCell = new Dictionary<int, Dictionary<int, List<int>>>();
        private int currentMonth;
        private int currentYear;
        private int firstDayOfCurrentWeek;
        private int firstDayPositionInWeek;
        private int startHour;
        private int endHour;
        private string firstWeekDayOfMonth;
        private string monthsInWeekText;
        private string yearsInWeekText;
        private DateTime currentDate;
        private DateTime nextWeekDate;
        private DateTime previousWeekDate;
        private AppointmentDatabase appointmentDatabase;
        private readonly User currentUser;
        #endregion

        #region Methods
        public WeekDetail(DateTime selectedDate, User user)
        {
            InitializeComponent();

            currentUser = user;

            SetDateVariables(selectedDate);
            CreateWeekCells();
            SetWeekView();
            DeserializeAppointments();
            SetCalendarBlocks();
        }

        private void SetDateVariables(DateTime selectedDate)
        {
            currentDate = selectedDate;
            firstDayOfCurrentWeek = selectedDate.Day;
            currentMonth = selectedDate.Month;
            currentYear = selectedDate.Year;

            if (firstDayOfCurrentWeek == FirstDayOfMonth)
            {
                firstWeekDayOfMonth = currentDate.ToString(DayFormat, USCultureInfo);
                firstDayPositionInWeek = daysOfWeek.IndexOf(firstWeekDayOfMonth);

                currentDate = currentDate.AddDays(SubstractDay * firstDayPositionInWeek);
            }

            firstDayPositionInWeek = FirstDayOfWeekPosition;
        }

        private void CreateWeekCells()
        {
            textBlocksWeekGrid = new TextBlock[] {
                TextBlockMondayDateDay, TextBlockTuesdayDateDay, TextBlockWednesdayDateDay,
                TextBlockThursdayDateDay, TextBlockFridayDateDay, TextBlockSaturdayDateDay,
                TextBlockSundayDateDay
            };
        }

        private void SetWeekView()
        {
            SetCalendarDateValues();
            SetDisplayMonthText();
            SetDisplayYearText();

            nextWeekDate = currentDate;
            previousWeekDate = currentDate.AddDays(TwoWeeksAgo);
        }

        private void SetCalendarDateValues()
        {
            for (int i = firstDayPositionInWeek; i < DaysInWeek; i++)
            {
                textBlocksWeekGrid[i].Text = currentDate.Day.ToString(USCultureInfo);
                datesInCurrentWeek.Add(currentDate.Date);

                if (!monthsInWeek.Contains(monthsOfYear[currentDate.Month - ListOffset]))
                {
                    monthsInWeek.Add(monthsOfYear[currentDate.Month - ListOffset]);
                }

                if (!yearsInWeek.Contains(currentDate.Year.ToString(USCultureInfo)))
                {
                    yearsInWeek.Add(currentDate.Year.ToString(USCultureInfo));
                }

                currentDate = currentDate.AddDays(AddDay);
            }
        }

        private void SetDisplayMonthText()
        {
            if (monthsInWeek.Count == BiMonthWeek)
            {
                monthsInWeekText = String.Format(USCultureInfo, "{0} - {1}",
                    monthsInWeek[FirstMonthInList], monthsInWeek[SecondMonthInList]);
            }
            else
            {
                monthsInWeekText = monthsInWeek[FirstMonthInList];
            }

            TextBlockDisplayedMonths.Text = monthsInWeekText;
        }

        private void SetDisplayYearText()
        {
            if (yearsInWeek.Count == BiYearWeek)
            {
                yearsInWeekText = String.Format(USCultureInfo, "{0} - {1}",
                    yearsInWeek[FirstYearInList], yearsInWeek[SecondYearInList]);
            }
            else
            {
                yearsInWeekText = yearsInWeek[FirstYearInList];
            }

            TextBlockDisplayedYears.Text = yearsInWeekText;
        }

        private void DeserializeAppointments()
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

        private void SetCalendarBlocks()
        {
            SetAppointmentStartAndEndDateTimes();

            foreach (Appointment appointment in appointmentDatabase.Appointments)
            {
                if (datesInCurrentWeek.Contains(appointment.StartDate.Date) && IsUserAppointment(appointment))
                {
                    SetBlocksInEventRange(appointment);
                }
            }
        }

        private void SetAppointmentStartAndEndDateTimes()
        {
            foreach (Appointment appointment in appointmentDatabase.Appointments)
            {
                bool isStartDateInCurrentWeek = datesInCurrentWeek.Contains(appointment.StartDate.Date);
                bool isUserAppointmentInCurrentWeek = isStartDateInCurrentWeek && IsUserAppointment(appointment);

                if (isUserAppointmentInCurrentWeek)
                {
                    startHour = appointment.StartDate.Hour;
                    endHour = appointment.EndDate.Hour;
                    AddEventToNumberOfEventsInCell(datesInCurrentWeek.IndexOf(appointment.StartDate.Date), startHour, endHour);
                }
            }
        }

        private bool IsUserAppointment(Appointment appointment)
        {
            bool isUserAppointment = false;

            if (appointment.Participants.Find(u => u.Name == currentUser.Name) != null)
            {
                isUserAppointment = true;
            }

            return isUserAppointment;
        }

        private void AddEventToNumberOfEventsInCell(int dayOfWeek, int hourOfDay, int eventLapse)
        {
            for (int i = hourOfDay; i <= eventLapse; i++)
            {
                if (numberOfEventsInCell.ContainsKey(dayOfWeek))
                {
                    if (numberOfEventsInCell[dayOfWeek].ContainsKey(i))
                    {
                        numberOfEventsInCell[dayOfWeek][i][EventPosition] += AddEvent;
                    }
                    else
                    {
                        numberOfEventsInCell[dayOfWeek][i] = new List<int>(DefaultEvent);
                    }
                }
                else
                {
                    numberOfEventsInCell[dayOfWeek] = new Dictionary<int, List<int>>() { { i, new List<int>(DefaultEvent) } };
                }
            }
        }

        private void SetBlocksInEventRange(Appointment appointment)
        {
            int eventStart = appointment.StartDate.Hour;
            int eventEnd = appointment.EndDate.Hour;

            for (int j = 0; j <= eventEnd - eventStart; j++)
            {
                textBlockAppointmentDisplay = new TextBlock();
                CreateAppointmentTextBlock(eventStart + j, appointment);
            }
        }

        private void CreateAppointmentTextBlock(int currentEventHour, Appointment appointment)
        {
            Grid gridWeekHours = GridHoursOfDay;
            double rowWidth = GridHoursOfDay.ColumnDefinitions[FirstDataColumn].Width.Value;
            double elementWidth = rowWidth / numberOfEventsInCell[datesInCurrentWeek.IndexOf(appointment.StartDate.Date)][appointment.StartDate.Hour][EventPosition];

            SetTextBlockAttributes(appointment, elementWidth, currentEventHour);

            Grid.SetRow(textBlockAppointmentDisplay, currentEventHour);
            Grid.SetColumn(textBlockAppointmentDisplay, datesInCurrentWeek.IndexOf(appointment.StartDate.Date) + ColumnOffset);

            gridWeekHours.Children.Add(textBlockAppointmentDisplay);

            numberOfEventsInCell[datesInCurrentWeek.IndexOf(appointment.StartDate.Date)][currentEventHour][EventsRemainingPosition] += AddEvent;
        }

        private void SetTextBlockAttributes(Appointment appointment, double elementWidth, int currentEventHour)
        {
            textBlockAppointmentDisplay.Text = appointment.Title;
            textBlockAppointmentDisplay.Background = appointmentsBackgroundColors[GreenBackgroundColor];
            textBlockAppointmentDisplay.HorizontalAlignment = HorizontalAlignment.Left;
            textBlockAppointmentDisplay.Margin = new Thickness(numberOfEventsInCell[datesInCurrentWeek.IndexOf(appointment.StartDate.Date)][currentEventHour][EventsRemainingPosition] * elementWidth, NoMargin, NoMargin, NoMargin);
            textBlockAppointmentDisplay.Width = elementWidth;
            textBlockAppointmentDisplay.TextWrapping = TextWrapping.Wrap;
            textBlockAppointmentDisplay.FontSize = SmallFont;
        }

        private void Window_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Left)
            {
                CreateAndDisplayWeekDetail(previousWeekDate);
                this.Close();
            }
            else if (e.Key == Key.Right)
            {
                CreateAndDisplayWeekDetail(nextWeekDate);
                this.Close();
            }
            else if(e.Key == Key.Space)
            {
                CreateAndDisplayMainWindow(new DateTime(currentYear, currentMonth, CurrentDayOfMonth));
                this.Close();
            }
            else if (e.Key == Key.Enter)
            {
                CreateAndDisplayCreateAppointmentWindow();
            }
        }

        private void CreateAndDisplayWeekDetail(DateTime weekDate)
        {
            WeekDetail weekDetailWindow = new WeekDetail(weekDate, currentUser);
            weekDetailWindow.Show();
        }

        private void CreateAndDisplayMainWindow(DateTime selectedDate)
        {
            MainWindow mainWindow = new MainWindow(selectedDate, currentUser);
            mainWindow.Show();
        }

        private void CreateAndDisplayCreateAppointmentWindow()
        {
            CreateAppointment createAppointmentWindow = new CreateAppointment(currentUser);
            createAppointmentWindow.Show();
        }

        private void ButtonManageAppointments_Click(object sender, RoutedEventArgs e)
        {
            CreateAndDisplayManageAppointmentsWindow();
        }

        private void CreateAndDisplayManageAppointmentsWindow()
        {
            ManageAppointmentsWindow manageAppointmentsWindow = new ManageAppointmentsWindow(currentUser);
            manageAppointmentsWindow.Show();
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
