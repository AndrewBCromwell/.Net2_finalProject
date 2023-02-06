using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DataObjects;

namespace DataAccessLayerInterfaces
{
    public interface IAdCompanyAccessor
    {
        List<AdCompanyVM> SelectAdCompanies();
    }
}
