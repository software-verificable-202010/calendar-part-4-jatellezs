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

namespace Calendar
{
    public partial class MainWindow : Window
    {
        static string[] months = { "January", "February", "March", "April", "May", "June", "July", "August", "September", "October", "November", "December" };
        static readonly List<String> monthsOfYear = new List<String>(months);
        static string[] weekDayNames = { "Monday", "Tuesday", "Wednesday", "Thursday", "Friday", "Saturday", "Sunday" };
        static readonly List<String> daysOfWeek = new List<string>(weekDayNames);
        String currentMonthName;
        DateTime firstDayOfMonth;
        String firstWeekDayOfMonth;
        DateTime currentDate = DateTime.Now;
        Int32 currentYear;
        Int32 currentMonthNumber;
        Int32 LIST_POSITION_OFFSET = -1;
        static Int32 firstPosition = 1;
        static string space = " ";
        static Int32 next = 1;
        static Int32 previous = -1;
        static Int32 firstDay = 1;
        TextBlock currentCell;
        Int32 startingPoint;
        Int32 endingPoint;
        Int32 dayNumber;
        TextBlock[] calendarGrid;
        Int32 FIRST_DAY_OF_MONTH = 1;
        Appointments appointments;
        Int32 SMALL_FONT = 8;
        List<ItemsControl> itemsControlsEvents = new List<ItemsControl>() { };
        string TIME_FORMAT = "HH:mm";
        string DAY_FORMAT = "dddd";
        string US_CULTURE_INFO = "en-US";
        string RIGHT_ARROW = "->";
        string PATH_TO_SERIALIZED_FILE = "Appointments.txt";
        string EMPTY_TEXT = "";

        public MainWindow(DateTime calendarDate)
        {
            InitializeComponent();

            currentDate = calendarDate;
            DeserializeCalendarContent();
            SetCalendarCells();
            DeleteCellsDayNumber();
            DeleteEvents();
            SetCalendarView(currentDate, appointments);
        }

        private void DeserializeCalendarContent()
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

        private void SetEventsInDay(Appointments appointments, DateTime selectedDate, Int32 cellNumber)
        {
            ItemsControl appointmentItemControl = new ItemsControl();
            TextBlock appointmentBlock;
            Grid.SetRow(appointmentItemControl, Grid.GetRow(calendarGrid[cellNumber]));
            Grid.SetColumn(appointmentItemControl, Grid.GetColumn(calendarGrid[cellNumber]));

            foreach (Appointment appointment in appointments.appointments)
            {
                if (appointment.startDate.Date == selectedDate)
                {
                    appointmentBlock = new TextBlock();
                    CreateTextBlockElement(appointmentBlock, appointment, appointmentItemControl);
                }
            }
            appointmentItemControl.VerticalAlignment = VerticalAlignment.Bottom;
            daysOfMonth.Children.Add(appointmentItemControl);
            itemsControlsEvents.Add(appointmentItemControl);
        }

        private void CreateTextBlockElement(TextBlock appointmentBlock, Appointment appointment, ItemsControl appointmentItemControl)
        {
            appointmentBlock.Text = appointment.title + space + appointment.startDate.ToString(TIME_FORMAT) + RIGHT_ARROW + appointment.endDate.ToString(TIME_FORMAT);
            appointmentBlock.FontSize = SMALL_FONT;
            appointmentItemControl.Items.Add(appointmentBlock);
        }

        private void SetCalendarView(DateTime selectedDate, Appointments appointments)
        {
            SetDateGlobalVariables(selectedDate);

            for ( int cellNumber = startingPoint; cellNumber < endingPoint; cellNumber++ )
            {
                dayNumber = cellNumber - startingPoint + firstPosition;
                currentCell = calendarGrid[cellNumber];
                currentCell.Text = dayNumber.ToString();
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

            TextBlockDisplayedDate.Text = currentMonthName + space + currentYear.ToString();

            startingPoint = GetStartingCallendarCell(firstWeekDayOfMonth);
            endingPoint = GetDaysInMonth(currentYear, currentMonthNumber) + startingPoint;
        }

        private void WindowKeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Left)
            {
                DeleteCellsDayNumber();
                DeleteEvents();
                currentDate = currentDate.AddMonths(previous);
                SetCalendarView(currentDate, appointments);
            }
            else if (e.Key == Key.Right)
            {
                DeleteCellsDayNumber();
                DeleteEvents();
                currentDate = currentDate.AddMonths(next);
                SetCalendarView(currentDate, appointments);
            }
            else if (e.Key == Key.Space)
            {
                WeekDetail weekDetail = new WeekDetail(new DateTime(currentYear, currentMonthNumber, FIRST_DAY_OF_MONTH));
                weekDetail.Show();
                this.Close();
            }
        }

        private DateTime GetFirstDayOfMonth(DateTime selectedDate)
        {
            return new DateTime(GetYear(selectedDate), GetMonthNumber(selectedDate), firstDay);
        }

        private Int32 GetYear(DateTime selectedDate)
        {
            return selectedDate.Year;
        }

        private string GetMonthName(DateTime selectedDate)
        {
            return monthsOfYear[GetMonthNumber(selectedDate) + LIST_POSITION_OFFSET];
        }

        private Int32 GetMonthNumber(DateTime selectedDate)
        {
            return selectedDate.Month;
        }

        private string GetDayOfWeek(DateTime selectedDate)
        {
            return selectedDate.ToString(DAY_FORMAT, new CultureInfo(US_CULTURE_INFO));
        }

        private Int32 GetStartingCallendarCell(String dayOfWeekName)
        {
            return daysOfWeek.IndexOf(dayOfWeekName);
        }

        private Int32 GetDaysInMonth(Int32 year, Int32 monthNUmber)
        {
            return DateTime.DaysInMonth(year, monthNUmber);
        }

        private void DeleteCellsDayNumber()
        {
            foreach (TextBlock cell in calendarGrid)
            {
                cell.Text = EMPTY_TEXT;
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

        private void SetCalendarCells()
        {
            calendarGrid = new TextBlock[] { TextBlockCell0, TextBlockCell1, TextBlockCell2, TextBlockCell3, TextBlockCell4,
                                            TextBlockCell5, TextBlockCell6, TextBlockCell7, TextBlockCell8, TextBlockCell9,
                                            TextBlockCell10, TextBlockCell11, TextBlockCell12, TextBlockCell13, TextBlockCell14,
                                            TextBlockCell15, TextBlockCell16, TextBlockCell17, TextBlockCell18, TextBlockCell19,
                                            TextBlockCell20, TextBlockCell21, TextBlockCell22, TextBlockCell23, TextBlockCell24,
                                            TextBlockCell25, TextBlockCell26, TextBlockCell27, TextBlockCell28, TextBlockCell29,
                                            TextBlockCell30, TextBlockCell31, TextBlockCell32, TextBlockCell33, TextBlockCell34,
                                            TextBlockCell35, TextBlockCell36, TextBlockCell37, TextBlockCell38, TextBlockCell39,
                                            TextBlockCell40, TextBlockCell41};
        }
    }
}
