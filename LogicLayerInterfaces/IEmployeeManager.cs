using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DataObjects;

namespace LogicLayerInterfaces
{
    public interface IEmployeeManager
    {
        Employee LoginUser(string email, string password);
        string HashSha256(string source);
    }
}
