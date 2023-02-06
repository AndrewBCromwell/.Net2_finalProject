using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DataObjects;

namespace DataAccessLayerInterfaces
{
    public interface IVenueUseAccessor
    {
        int InsertVenueUse(int VenueId, DateTime startDate, DateTime endDate, int employeeId);

        List<VenueUseVM> SelectVenueUses();
        VenueUseVM SelectUseDaysByUseId(VenueUseVM venueUse);
        int UpdateVenueUseAdCampaign(int useId, int campaignId);
        int InsertUseDay(UseDay useDay);
    }
}
