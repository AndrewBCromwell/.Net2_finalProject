using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DataAccessLayerInterfaces;
using DataObjects;

namespace DataAccessLayerFakes
{
    public class VenueUseAccessorFakes : IVenueUseAccessor
    {
        private List<VenueUseVM> fakeUses = new List<VenueUseVM>();

        public VenueUseAccessorFakes()
        {
            fakeUses.Add(new VenueUseVM
            {
                UseId = 999999,
                VenueId = 999999,
                StartDate = DateTime.Now,
                EndDate = DateTime.Now.AddDays(1),
                TotalRevenue = 0,
                TotalTicketsSold = 0
            });
            fakeUses.Add(new VenueUseVM
            {
                UseId = 999998,
                VenueId = 999998,
                StartDate = DateTime.Now,
                EndDate = DateTime.Now.AddDays(1),
                TotalRevenue = 0,
                TotalTicketsSold = 0
            });
        }

        public int InsertUseDay(UseDay useDay)
        {
            throw new NotImplementedException();
        }

        public int InsertVenueUse(int VenueId, DateTime startDate, DateTime endDate, int employeeId)
        {
            throw new NotImplementedException();
        }

        public VenueUseVM SelectUseDaysByUseId(VenueUseVM venueUse)
        {
            throw new NotImplementedException();
        }

        public List<VenueUseVM> SelectVenueUses()
        {
            throw new NotImplementedException();
        }

        public int UpdateVenueUseAdCampaign(int useId, int campaignId)
        {
            throw new NotImplementedException();
        }
    }
}
