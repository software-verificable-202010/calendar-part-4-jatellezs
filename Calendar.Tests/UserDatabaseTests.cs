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
    class UserDatabaseTests
    {
        #region Fields
        private UserDatabase users;
        private User user;
        #endregion

        #region Methods
        [SetUp]
        public void SetUp()
        {
            users = new UserDatabase();
            user = new User("test");
        }

        [Test]
        public void SaveUser_UserNotInDatabase_AddsUserToDatabase()
        {
            users.SaveUser(user);
            bool isUserInDatabase = users.RegisteredUsers.Contains(user);
            Assert.IsTrue(isUserInDatabase);
        }
        #endregion
    }
}
