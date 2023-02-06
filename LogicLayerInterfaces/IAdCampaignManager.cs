using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DataObjects;

namespace LogicLayerInterfaces
{
    public interface IAdCampaignManager
    {
        bool AddAdCampaign(AdCompany adCompany, decimal cost, Employee employee, List<AdItem> adItems);
        List<string> RetreiveAdTypes();
        List<string> RetreiveActs();

        List<AdCampaignVM> RetreiveAdCampaigns();
    }
}
