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
        TextBlock[] weekGrid;
        static string[] months = { "January", "February", "March", "April", "May", "June", "July", "August", "September", "October", "November", "December" };
        static readonly List<String> monthsOfYear = new List<String>(months);
        static string[] weekDayNames = { "Monday", "Tuesday", "Wednesday", "Thursday", "Friday", "Saturday", "Sunday" };
        static readonly List<String> daysOfWeek = new List<string>(weekDayNames);
        Int32 currentMonth;
        Int32 currentYear;
        Int32 CURRENT_DAY_OF_MONTH = 1;
        Int32 firstDayOfCurrentWeek;
        Int32 FIRST_DAY_OF_MONTH = 1;
        Int32 ADD_DAY = 1;
        Int32 SUBSTRACT_DAY = -1;
        Int32 DAYS_IN_WEEK = 7;
        Int32 TWO_WEEKS_AGO = -14;
        Int32 FIRST_DAY_OF_WEEK_POSITION = 0;
        DateTime currentDate;
        string firstWeekDayOfMonth;
        Int32 firstDayPositionInWeek;
        DateTime nextWeekDate;
        DateTime previousWeekDate;
        List<string> monthsInWeek = new List<string>();
        List<string> yearsInWeek = new List<string>();
        Int32 BI_MONTH_WEEK = 2;
        Int32 BI_YEAR_WEEK = 2;
        Int32 LIST_OFFSET = 1;
        Int32 FIRST_MONTH_IN_LIST = 0;
        Int32 SECOND_MONTH_IN_LIST = 1;
        Int32 FIRST_YEAR_IN_LIST = 0;
        Int32 SECOND_YEAR_IN_LIST = 1;
        string monthsInWeekText;
        string yearsInWeekText;
        string dash = "-";
        List<DateTime> datesInCurrentWeek = new List<DateTime>() { };
        List<Brush> appointmentsBackgroundColors = new List<Brush>() { Brushes.LightGreen };
        Dictionary<Int32, Dictionary<Int32, List<Int32>>> numberOfEventsInCell = new Dictionary<int, Dictionary<int, List<int>>> ();
        Int32 ADD_EVENT = 1;
        Int32 EVENT_POSITION = 0;
        Int32 EVENTS_REMAINING_POSITION = 1;
        Int32[] DEFAULT_EVENT = { 1, 0 };
        Appointments appointments;
        Int32 SMALL_FONT = 8;
        string DAY_FORMAT = "dddd";
        string US_CULTURE_INFO = "en-US";
        Int32 MARGIN_ZERO = 0;
        string PATH_TO_SERIALIZED_FILE = "Appointments.txt";
        Int32 FIRST_DATA_COLUMN = 1;
        Int32 GREEN_BACKGORUND_COLOR = 0;
        Int32 COLUMN_OFFSET = 1;

        public WeekDetail(DateTime selectedDate)
        {
            InitializeComponent();

            SetDateVariables(selectedDate);
            CreateWeekCells();
            SetWeekView();
            DeserializeAppointments();
            SetCalendarBlocks(appointments);
        }

        private void SetDateVariables(DateTime selectedDate)
        {
            currentDate = selectedDate;
            firstDayOfCurrentWeek = selectedDate.Day;
            currentMonth = selectedDate.Month;
            currentYear = selectedDate.Year;
            if (firstDayOfCurrentWeek == FIRST_DAY_OF_MONTH)
            {
                firstWeekDayOfMonth = currentDate.ToString(DAY_FORMAT, new CultureInfo(US_CULTURE_INFO));
                firstDayPositionInWeek = daysOfWeek.IndexOf(firstWeekDayOfMonth);

                currentDate = currentDate.AddDays(SUBSTRACT_DAY * firstDayPositionInWeek);
            }

            firstDayPositionInWeek = FIRST_DAY_OF_WEEK_POSITION;
        }

        private void DeserializeAppointments()
        {
            try
            {
                IFormatter formatter = new BinaryFormatter();
                Stream stream = new FileStream(PATH_TO_SERIALIZED_FILE, FileMode.Open, FileAccess.Read);
                appointments = (Appointments)formatter.Deserialize(stream);
                stream.Close();
            }
            catch
            {
                appointments = new Appointments();
            }
        }

        private void SetCalendarBlocks(Appointments appointments)
        {
            GetAppointmentStartAndEndDateTimes(appointments.appointments);

            foreach (Appointment appointment in appointments.appointments)
            {
                if (datesInCurrentWeek.Contains(appointment.startDate.Date))
                {
                    SetBlocksInEventRange(appointment);
                }
            }
        }

        private void SetBlocksInEventRange(Appointment appointment)
        {
            TextBlock appointmentBlock;
            Int32 eventStart;
            Int32 eventEnd;

            eventStart = appointment.startDate.Hour;
            eventEnd = appointment.endDate.Hour;

            for (int j = 0; j <= eventEnd - eventStart; j++)
            {
                appointmentBlock = new TextBlock();
                CreateAppointmentTextBlock(appointmentBlock, eventStart + j, appointment);
            }
        }

        private void CreateAppointmentTextBlock(TextBlock appointmentBlock, Int32 currentEventHour, Appointment appointment)
        {
            Grid weekHoursGrid = GridHoursOfDay;
            Double elementWidth;
            Double rowWidth = GridHoursOfDay.ColumnDefinitions[FIRST_DATA_COLUMN].Width.Value;
            elementWidth = rowWidth / numberOfEventsInCell[datesInCurrentWeek.IndexOf(appointment.startDate.Date)][appointment.startDate.Hour][EVENT_POSITION];

            SetTextBlockAttributes(appointmentBlock, appointment, elementWidth, currentEventHour);

            Grid.SetRow(appointmentBlock, currentEventHour);
            Grid.SetColumn(appointmentBlock, datesInCurrentWeek.IndexOf(appointment.startDate.Date) + COLUMN_OFFSET);

            weekHoursGrid.Children.Add(appointmentBlock);

            numberOfEventsInCell[datesInCurrentWeek.IndexOf(appointment.startDate.Date)][currentEventHour][EVENTS_REMAINING_POSITION] += ADD_EVENT;
        }

        private void SetTextBlockAttributes(TextBlock appointmentBlock, Appointment appointment, Double elementWidth, Int32 currentEventHour)
        {
            appointmentBlock.Text = appointment.title;
            appointmentBlock.Background = appointmentsBackgroundColors[GREEN_BACKGORUND_COLOR];
            appointmentBlock.HorizontalAlignment = HorizontalAlignment.Left;
            appointmentBlock.Margin = new Thickness(numberOfEventsInCell[datesInCurrentWeek.IndexOf(appointment.startDate.Date)][currentEventHour][EVENTS_REMAINING_POSITION] * elementWidth, MARGIN_ZERO, MARGIN_ZERO, MARGIN_ZERO);
            appointmentBlock.Width = elementWidth;
            appointmentBlock.TextWrapping = TextWrapping.Wrap;
            appointmentBlock.FontSize = SMALL_FONT;
        }

        private void AddEventToNumberOfEventsInCell(Int32 dayOfWeek, Int32 hourOfDay, Int32 eventLapse)
        {
            for (int i = hourOfDay; i <= eventLapse; i++)
                if (numberOfEventsInCell.ContainsKey(dayOfWeek))
                {
                    if (numberOfEventsInCell[dayOfWeek].ContainsKey(i))
                    {
                        numberOfEventsInCell[dayOfWeek][i][EVENT_POSITION] += ADD_EVENT;
                    }
                    else
                    {
                        numberOfEventsInCell[dayOfWeek][i] = new List<int>(DEFAULT_EVENT);
                    }
                }
                else
                {
                    numberOfEventsInCell[dayOfWeek] = new Dictionary<int, List<int>>() { { i, new List<int>(DEFAULT_EVENT) } };
                }
        }

        private void GetAppointmentStartAndEndDateTimes(List<Appointment> appointments)
        {
            Int32 startHour;
            Int32 endHour;
           
            foreach (Appointment appointment in appointments)
            {
                if (datesInCurrentWeek.Contains(appointment.startDate.Date))
                {
                    startHour = appointment.startDate.Hour;
                    endHour = appointment.endDate.Hour;
                    AddEventToNumberOfEventsInCell(datesInCurrentWeek.IndexOf(appointment.startDate.Date), startHour, endHour);
                }    
            }
        }

        private void SetWeekView()
        {
            SetCalendarDateValues();
            SetDisplayMonthText();
            SetDisplayYearText();

            nextWeekDate = currentDate;
            previousWeekDate = currentDate.AddDays(TWO_WEEKS_AGO);
        }

        private void SetDisplayYearText()
        {
            if (yearsInWeek.Count == BI_YEAR_WEEK)
            {
                yearsInWeekText = yearsInWeek[FIRST_YEAR_IN_LIST] + dash + yearsInWeek[SECOND_YEAR_IN_LIST];
            }
            else
            {
                yearsInWeekText = yearsInWeek[FIRST_YEAR_IN_LIST];
            }

            TextBlockDisplayedYears.Text = yearsInWeekText;
        }

        private void SetDisplayMonthText()
        {
            if (monthsInWeek.Count == BI_MONTH_WEEK)
            {
                monthsInWeekText = monthsInWeek[FIRST_MONTH_IN_LIST] + dash + monthsInWeek[SECOND_MONTH_IN_LIST];
            }
            else
            {
                monthsInWeekText = monthsInWeek[FIRST_MONTH_IN_LIST];
            }

            TextBlockDisplayedMonths.Text = monthsInWeekText;
        }

        private void SetCalendarDateValues()
        {
            for (int i = firstDayPositionInWeek; i < DAYS_IN_WEEK; i++)
            {
                weekGrid[i].Text = currentDate.Day.ToString();
                datesInCurrentWeek.Add(currentDate.Date);

                if (!monthsInWeek.Contains(monthsOfYear[currentDate.Month - LIST_OFFSET]))
                {
                    monthsInWeek.Add(monthsOfYear[currentDate.Month - LIST_OFFSET]);
                }

                if (!yearsInWeek.Contains(currentDate.Year.ToString()))
                {
                    yearsInWeek.Add(currentDate.Year.ToString());
                }
                currentDate = currentDate.AddDays(ADD_DAY);
            }
        }

        private void CreateWeekCells()
        {
            weekGrid = new TextBlock[] { TextBlockMondayDateDay, TextBlockTuesdayDateDay, TextBlockWednesdayDateDay,
                                        TextBlockThursdayDateDay, TextBlockFridayDateDay, TextBlockSaturdayDateDay, 
                                        TextBlockSundayDateDay};
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
                CreateAndDisplayMainWindow(new DateTime(currentYear, currentMonth, CURRENT_DAY_OF_MONTH));
                this.Close();
            }
            else if (e.Key == Key.Enter)
            {
                CreateAndDisplayCreateAppointmentWindow();
                //this.Close();
            }
        }

        private void CreateAndDisplayCreateAppointmentWindow()
        {
            CreateAppointment createAppointment = new CreateAppointment();
            createAppointment.Show();
        }

        private void CreateAndDisplayMainWindow(DateTime selectedDate)
        {
            MainWindow mainWindow = new MainWindow(selectedDate);
            mainWindow.Show();
        }

        private void CreateAndDisplayWeekDetail(DateTime weekDate)
        {
            WeekDetail weekDetail = new WeekDetail(weekDate);
            weekDetail.Show();
        }
    }
}
