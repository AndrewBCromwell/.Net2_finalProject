using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DataObjects;

namespace DataAccessLayerInterfaces
{
    public interface IAdCampaignAccessor
    {
        int InsertAdCampaign(AdCampaign adCampaign);
        int InsertAdItem(AdItem item);
        List<string> SelectAdTypes();
        List<string> SelectActs();

        List<AdCampaignVM> SelectAdCampaigns();
        AdCampaignVM SelectAdItemsByCampaignId(AdCampaignVM adCampaign);
    }
}
