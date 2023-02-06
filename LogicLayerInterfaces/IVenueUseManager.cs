using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DataObjects;

namespace LogicLayerInterfaces
{
    public interface IVenueUseManager
    {
        bool AddVenueUse(VenueVM venue, DateTime startDate, DateTime endDate, Employee employee);

        List<VenueUseVM> RetreiveVenueUses();

        bool AddUseDay(VenueUse venueUse, DateTime date, int ticketsSold, decimal revenue, Employee employee);

        bool UpdateVenueUseAdCampaign(int useId, int campaignId);
    }
}
