using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DataObjects;

namespace DataAccessLayerInterfaces
{
    public interface IEmployeeAccessor
    {
        int AuthenticateUserWithEmailAndPasswordHash(
            string email, string passwordHash);
        Employee SelectUserByEmail(string email);
        List<string> SelectRolesByEmployeeID(int employeeID);
    }
}
