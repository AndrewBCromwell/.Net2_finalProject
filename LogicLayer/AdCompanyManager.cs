using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DataObjects;
using DataAccessLayerInterfaces;
using DataAccessLayer;
using LogicLayerInterfaces;

namespace LogicLayer
{
    public class AdCompanyManager : IAdCompanyManager
    {
        private AdCompanyAccessor adCompanyAccessor = null;

        public AdCompanyManager()
        {
            adCompanyAccessor = new AdCompanyAccessor();
        }

        public AdCompanyManager(AdCompanyAccessor aca)
        {
            adCompanyAccessor = aca;
        }

        public List<AdCompanyVM> RetreiveAdCompanies()
        {
            List<AdCompanyVM> adCompanies = new List<AdCompanyVM>();
            try
            {
                adCompanies = adCompanyAccessor.SelectAdCompanies();
            }
            catch (Exception ex)
            {

                throw new ApplicationException("The ad companies could not be retrieved", ex);
            }

            return adCompanies;
        }
    }
}
