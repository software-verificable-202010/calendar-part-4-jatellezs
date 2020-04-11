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

namespace Calendar
{
    public partial class MainWindow : Window
    {
        static string[] months = { "January", "February", "March", "April", "May", "June", "July", "August", "September", "October", "November", "December" };
        static readonly List<String> monthsOfYear = new List<String>(months);
        static string[] weekDayNames = { "Monday", "Tuesday", "Wednesday", "Thursday", "Friday", "Saturday", "Sunday" };
        static readonly List<String> daysOfWeek = new List<string>(weekDayNames);
        String currentMonthName;
        Int32 currentMonthNumber;
        DateTime firstDayOfMonth;
        String firstWeekDayOfMonth;
        Int32 currentYear;
        DateTime currentDate;
        static Int32 listFirstPosition = -1;
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

        public MainWindow()
        {
            InitializeComponent();

            currentDate = DateTime.Now;
            SetCalendarCells();
            SetCalendarView(currentDate);
        }

        private void SetCalendarView(DateTime selectedDate)
        {
            DeleteCellsDayNumber();

            currentMonthName = GetMonthName(selectedDate);
            currentMonthNumber = GetMonthNumber(selectedDate);
            currentYear = GetYear(selectedDate);
            firstDayOfMonth = GetFirstDayOfMonth(selectedDate);
            firstWeekDayOfMonth = GetDayOfWeek(firstDayOfMonth);

            TextBlockDisplayedDate.Text = currentMonthName + space + currentYear.ToString();

            startingPoint = GetStartingCallendarCell(firstWeekDayOfMonth);
            endingPoint = GetDaysInMonth(currentYear, currentMonthNumber) + startingPoint;

            for ( int cellNumber = startingPoint; cellNumber < endingPoint; cellNumber++ )
            {
                dayNumber = cellNumber - startingPoint + firstPosition;
                currentCell = calendarGrid[cellNumber];
                currentCell.Text = dayNumber.ToString();
            }
        }

        private void WindowKeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Left)
            {
                currentDate = currentDate.AddMonths(previous);
                SetCalendarView(currentDate);
            }
            else if (e.Key == Key.Right)
            {
                currentDate = currentDate.AddMonths(next);
                SetCalendarView(currentDate);
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
            return monthsOfYear[GetMonthNumber(selectedDate) + listFirstPosition];
        }

        private Int32 GetMonthNumber(DateTime selectedDate)
        {
            return selectedDate.Month;
        }

        private string GetDayOfWeek(DateTime selectedDate)
        {
            return selectedDate.ToString("dddd", new CultureInfo("en-EN"));
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
                cell.Text = "";
            }
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
