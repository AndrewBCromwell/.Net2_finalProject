using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using DataObjects;
using DataAccessLayerInterfaces;
using DataAccessLayerFakes;
using LogicLayer;

namespace LogicLayerTests
{
    [TestClass]
    public class EmployeeManagerTests
    {
        EmployeeManager employeeManager = null;

        [TestInitialize]
        public void TestSetup()
        {
            employeeManager = new EmployeeManager(new EmployeeAccessorFake());
        }

        [TestMethod]
        public void TestGetSHA256ReturnsCorrectHashValue()
        {
            // Arrange
            const string source = "newuser";
            const string expectedResult =
                "9c9064c59f1ffa2e174ee754d2979be80dd30db552ec03e7e327e9b1a4bd594e";
            string result = "";
            
            result = employeeManager.HashSha256(source);

            Assert.AreEqual(expectedResult, result);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void TestGetSHA256ThrowsArgumentNullExceptionForMissingInput()
        {
            
            const string source = null;
            
            employeeManager.HashSha256(source);
        }

        [TestMethod]
        public void TestAuthenticateUserPassesWithCorrectUsernameAndPassword()
        {
            const string email = "tess@company.com";
            const string password = "newuser";
            int expectedResult = 999999;
            int actualResult = 0;

            Employee tessEmployee = employeeManager.LoginUser(email, password);
            actualResult = tessEmployee.EmployeeID;

            Assert.AreEqual(expectedResult, actualResult);
        }

        [TestMethod]
        [ExpectedException(typeof(ApplicationException))]
        public void TestAuthenticateUserFailsWithInCorrectUsername()
        {
            const string email = "xtess@company.com";
            const string password = "newuser";

            Employee tessUser = employeeManager.LoginUser(email, password);
        }

        [TestMethod]
        [ExpectedException(typeof(ApplicationException))]
        public void TestAuthenticateUserFailsWithInCorrectPassword()
        {
            const string email = "tess@company.com";
            const string password = "xnewuser";

            Employee tessUser = employeeManager.LoginUser(email, password);
        }
    }
}
