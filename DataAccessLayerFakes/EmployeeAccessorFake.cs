using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DataAccessLayerInterfaces;
using DataObjects;

namespace DataAccessLayerFakes
{
    public class EmployeeAccessorFake : IEmployeeAccessor
    {
        private List<Employee> fakeEmployees = new List<Employee>();
        private List<string> fakePaswordHashes = new List<string>();

        public EmployeeAccessorFake()
        {
            fakeEmployees.Add(new Employee()
            {
                EmployeeID = 999999,
                Email = "tess@company.com",
                GivenName = "Tess",
                FamilyName = "Employee",
                Phone = "1234567890",
                Active = true,
                Roles = new List<string>()
            });
            fakeEmployees[0].Roles.Add("Tour Planer");
            fakeEmployees[0].Roles.Add("Performer");

            fakeEmployees.Add(new Employee()
            {
                EmployeeID = 999998,
                Email = "fake2@company.com",
                GivenName = "Tess",
                FamilyName = "Employee",
                Phone = "1234567890",
                Active = true,
                Roles = new List<string>()
            });
            fakeEmployees.Add(new Employee()
            {
                EmployeeID = 999997,
                Email = "fake3@company.com",
                GivenName = "Tess",
                FamilyName = "Employee",
                Phone = "1234567890",
                Active = true,
                Roles = new List<string>()
            });

            fakePaswordHashes.Add("9c9064c59f1ffa2e174ee754d2979be80dd30db552ec03e7e327e9b1a4bd594e");
            fakePaswordHashes.Add("bad hash");
            fakePaswordHashes.Add("bad hash");
        }

        public int AuthenticateUserWithEmailAndPasswordHash(string email, string passwordHash)
        {
            int numAuthenticated = 0;

            // check for user record in fake data
            for (int i = 0; i < fakeEmployees.Count; i++)
            {
                if (fakeEmployees[i].Email == email &&
                    fakePaswordHashes[i] == passwordHash &&
                    fakeEmployees[i].Active == true)
                {
                    numAuthenticated++;
                }
            }
            return numAuthenticated;
        }

        public List<string> SelectRolesByEmployeeID(int employeeID)
        {
            List<string> roles = new List<string>();

            foreach (var fakeEmployee in fakeEmployees)
            {
                if (fakeEmployee.EmployeeID == employeeID)
                {
                    roles = fakeEmployee.Roles;
                    break;
                }
            }

            return roles;
        }

        public Employee SelectUserByEmail(string email)
        {
            Employee user = null;
            foreach (var fakeEmployee in fakeEmployees)
            {
                if (fakeEmployee.Email == email)
                {
                    user = fakeEmployee;
                }
            }
            if (user == null)
            {
                throw new ApplicationException("User not found.");
            }

            return user;
        }
    }
}
