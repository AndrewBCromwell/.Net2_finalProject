using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DataObjects;

namespace LogicLayerInterfaces
{
    public interface IAdCompanyManager
    {
        List<AdCompanyVM> RetreiveAdCompanies();
    }
}
