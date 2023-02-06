using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DataObjects;
using LogicLayerInterfaces;
using DataAccessLayer;
using DataAccessLayerInterfaces;
using System.Security.Cryptography;

namespace LogicLayer
{
    public class EmployeeManager : IEmployeeManager
    {
        private IEmployeeAccessor employeeAccessor = null;

        public EmployeeManager()
        {
            employeeAccessor = new EmployeeAccessor();
        }

        public EmployeeManager(IEmployeeAccessor ea)
        {
            employeeAccessor = ea;
        }
        public string HashSha256(string source)
        {
            string result = "";

            // check for missing input
            if (source == "" || source == null)
            {
                throw new ArgumentNullException("Missing Input");
            }

            // create a byte array
            byte[] data;

            // create a .NET hash provider object
            using (SHA256 sha256hasher = SHA256.Create())
            {
                // hash the input
                data = sha256hasher.ComputeHash(
                    Encoding.UTF8.GetBytes(source));
            }

            // create output with a stringbuilder object
            var s = new StringBuilder();

            // loop through the hashed output making characters
            // from the values in the byte array
            for (int i = 0; i < data.Length; i++)
            {
                s.Append(data[i].ToString("x2"));
            }

            // convert the stringbuilder into a string
            result = s.ToString();
            result = result.ToLower();

            return result;
        }

        public Employee LoginUser(string email, string password)
        {
            Employee user = null;

            try
            {
                password = HashSha256(password);
                if (1 == employeeAccessor.AuthenticateUserWithEmailAndPasswordHash(
                    email, password))
                {
                    user = employeeAccessor.SelectUserByEmail(email);
                    user.Roles = employeeAccessor.SelectRolesByEmployeeID(user.EmployeeID);
                }
                else
                {
                    throw new ApplicationException("User not found.");
                }
            }
            catch (Exception ex)
            {
                throw new ApplicationException("Bad Username or Password", ex);
            }

            return user;
        }
    }
}
