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
    class UserTests
    {
        #region Constants
        internal string shortUserName = "tes";
        internal string longUserName = "testName";
        #endregion

        #region Fields
        private User validUser;
        private User invalidUser;
        #endregion

        #region Methods
        [Test]
        public void HasValidName_ValidUserName_ReturnsTrue()
        {
            validUser = new User(longUserName);
            bool hasValidNameResult = validUser.HasValidName();
            Assert.IsTrue(hasValidNameResult);
        }

        [Test]
        public void HasValidName_InvalidUserName_ReturnsFalse()
        {
            invalidUser = new User(shortUserName);
            bool hasValidNameResult = invalidUser.HasValidName();
            Assert.IsFalse(hasValidNameResult);
        }
        #endregion
    }
}
