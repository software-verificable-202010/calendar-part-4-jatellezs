using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using NUnit.Framework;
using Calendar.View;
using Calendar.Model;
using System.Windows;

namespace Calendar.Tests
{
    [TestFixture]
    class AppointmentTests
    {
        #region Fields
        private Appointment appointment;
        private User user;
        private DateTime startDate;
        private DateTime endDate;
        #endregion

        #region Methods
        [SetUp]
        public void SetUp()
        {
            startDate = new DateTime(2020, 1, 1, 0, 0, 0);
            endDate = new DateTime(2020, 1, 1, 5, 0, 0);
            user = new User("test");
            appointment = new Appointment()
            {
                StartDate = startDate,
                EndDate = endDate,
                Creator = user,
                Title = "Title",
                Description = "Description"
            };

            appointment.Participants.Add(user);
        }

        [Test]
        public void IsUserAppointmentCreator_UserIsCreator_ReturnsTrue()
        {
            bool isUserAppointmentCreatorResult = appointment.IsUserAppointmentCreator(user);
            Assert.IsTrue(isUserAppointmentCreatorResult);
        }

        [Test]
        public void IsUserAppointment_UserIsParticipant_ReturnsTrue()
        {
            bool isUserAppointmentResult = appointment.IsUserAppointment(user);
            Assert.IsTrue(isUserAppointmentResult);
        }

        [Test]
        public void IsBetweenDates_ParametersAreBetweenDateRange_ReturnsTrue()
        {
            bool isBetweenDatesResult = appointment.IsBetweenDates(new DateTime(2020, 1, 1, 1, 0, 0), new DateTime(2020, 1, 1, 4, 0, 0));
            Assert.IsTrue(isBetweenDatesResult);
        }
        #endregion
    }
}
