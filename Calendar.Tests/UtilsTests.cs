using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using NUnit.Framework;
using Calendar.View;
using Calendar.Model;
using System.Windows;
using CalendarProject.ViewModel;

namespace Calendar.Tests
{
    [TestFixture]
    class UtilsTests
    {
        #region Constants
        internal static string PathToUsersTestFile = "TestUsers.txt";
        internal static string PathToAppointmentsTestFile = "TestAppointments.txt";
        internal static string PathToInexistentUsersTestFile = "TestUsersInexistent.txt";
        internal static string PathToInexistentAppointmentsTestFile = "TestAppointmentsInexistent.txt";
        internal static DateTime testStartDate = new DateTime(2020, 1, 1, 0, 0, 0);
        internal static DateTime testEndDate = new DateTime(2020, 1, 1, 5, 0, 0);
        #endregion

        #region Fields
        private static UserDatabase userDatabase;
        private static AppointmentDatabase appointmentDatabase;
        private static Appointment appointment;
        private static User user;
        #endregion

        #region Methods
        [Test]
        public void DeserializeUsersFile_FileExists_ReturnedDatabaseContainsTestUser()
        {
            user = new User("test");
            userDatabase = new UserDatabase();
            userDatabase.RegisteredUsers.Add(user);
            userDatabase.Serialize(PathToUsersTestFile);

            bool isUserInDeserializedFile = Utils.DeserializeUsersFile(PathToUsersTestFile).RegisteredUsers.Exists(u => u.Name == user.Name);
            Assert.IsTrue(isUserInDeserializedFile);
        }

        [Test]
        public void DeserializeUsersFile_FileDoesNotExist_ReturnsEmptyUserDatabase()
        {
            bool isUserDatabaseEmpty = Utils.DeserializeUsersFile(PathToInexistentUsersTestFile).RegisteredUsers.Count == 0;
            Assert.IsTrue(isUserDatabaseEmpty);
        }

        [Test]
        public void DeserializeAppointmentsFile_FileExists_ReturnedDatabaseContainsTestAppointment()
        {
            user = new User("test");
            appointment = new Appointment()
            {
                StartDate = testStartDate,
                EndDate = testEndDate,
                Creator = user,
                Title = "Title",
                Description = "Description"
            };

            appointment.Participants.Add(user);

            appointmentDatabase = new AppointmentDatabase();
            appointmentDatabase.Appointments.Add(appointment);
            appointmentDatabase.Serialize(PathToAppointmentsTestFile);

            bool isAppointmentInDatabase = Utils.DeserializeAppointmentsFile(PathToAppointmentsTestFile).Appointments.Exists(
                a => a.Title == appointment.Title && a.Creator.Name == appointment.Creator.Name);
            Assert.IsTrue(isAppointmentInDatabase);
        }

        [Test]
        public void DeserializeAppointmentsFile_FileDoesNotExists_ReturnsEmptyAppointmentDatabase()
        {
            bool isAppointmentDatabaseEmpty = Utils.DeserializeAppointmentsFile(PathToInexistentAppointmentsTestFile).Appointments.Count == 0;
            Assert.IsTrue(isAppointmentDatabaseEmpty);
        }

        [Test]
        public void GetMonthName_JanuaryDate_ReturnsJanuary()
        {
            string result = Utils.GetMonthName(testStartDate);
            Assert.AreEqual("January", result);
        }

        [Test]
        public void GetFirstDayOfMonth_DateOfFirstDayOfMonth_ReturnsSameInputDate()
        {
            DateTime result = Utils.GetFirstDayOfMonth(testStartDate);
            Assert.AreEqual(testStartDate, result);
        }

        [Test]
        public void GetDayOfWeek_FirstDayOf2020_ReturnsWednesday()
        {
            string result = Utils.GetDayOfWeek(testStartDate);
            Assert.AreEqual("Wednesday", result);
        }

        [Test]
        public void GetStartingCallendarCell_InputIsMonday_ReturnsZero()
        {
            int result = Utils.GetStartingCallendarCell("Monday");
            Assert.AreEqual(0, result);
        }

        [Test]
        public void GetDaysInMonth_JanuaryDate_Returns31()
        {
            int result = Utils.GetDaysInMonth(testStartDate.Year, testStartDate.Month);
            Assert.AreEqual(31, result);
        }

        [Test]
        public void GetParticipantsWantedAppointments_TestUserAsOnlyUserAndSelectedAppointmentIsOnlyAppoointment_ReturnsEmptyList()
        {
            appointment = new Appointment()
            {
                StartDate = testStartDate,
                EndDate = testEndDate,
                Creator = user,
                Title = "Title",
                Description = "Description"
            };

            appointment.Participants.Add(user);

            appointmentDatabase = new AppointmentDatabase();
            appointmentDatabase.Appointments.Add(appointment);

            bool isResultEmpty = Utils.GetParticipantsWantedAppointments(new List<User>() { user }, appointmentDatabase, appointment).Count == 0;
            Assert.IsTrue(isResultEmpty);
        }

        [Test]
        public void IsAppointmentInputValid_DateCollidesWithAppointment_ReturnsFalse()
        {
            user = new User("test");
            User testUser = new User("testUser2");
            Appointment testAppointment = new Appointment()
            {
                StartDate = testStartDate,
                EndDate = testEndDate,
                Creator = testUser
            };
            testAppointment.Participants.Add(testUser);
            testAppointment.Participants.Add(user);
            appointmentDatabase.Appointments.Add(testAppointment);

            bool isAppointmentInputValidResult = Utils.IsAppointmentInputValid(testStartDate, testEndDate, appointmentDatabase.Appointments);
            Assert.IsFalse(isAppointmentInputValidResult);
        }

        [Test]
        public void HasDateCollision_InputHasDateCollisionForUsersAppointments_ReturnsTrue()
        {
            appointment = new Appointment()
            {
                StartDate = testStartDate,
                EndDate = testEndDate,
                Creator = user,
                Title = "Title",
                Description = "Description"
            };

            appointment.Participants.Add(user);

            appointmentDatabase = new AppointmentDatabase();
            appointmentDatabase.Appointments.Add(appointment);

            bool hasDateCollisionResult = Utils.HasDateCollision(testStartDate, testEndDate, appointmentDatabase.Appointments);
            Assert.IsTrue(hasDateCollisionResult);
        }

        [Test]
        public void GetParticipantsAppointments_TestUserAsOnlyUser_ReturnsListWithTestAppointment()
        {
            appointment = new Appointment()
            {
                StartDate = testStartDate,
                EndDate = testEndDate,
                Creator = user,
                Title = "Title",
                Description = "Description"
            };

            appointment.Participants.Add(user);

            appointmentDatabase = new AppointmentDatabase();
            appointmentDatabase.Appointments.Add(appointment);

            bool isAppointmentInResultList = Utils.GetParticipantsAppointments(new List<User>() { user }, appointmentDatabase).Contains(appointment);
            Assert.IsTrue(isAppointmentInResultList);
        }
        #endregion
    }
}
