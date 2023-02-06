using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DataObjects;
using LogicLayerInterfaces;
using DataAccessLayer;
using DataAccessLayerInterfaces;

namespace LogicLayer
{
    public class VenueUseManager : IVenueUseManager
    {
        private IVenueUseAccessor venueUseAccessor = null;

        public VenueUseManager()
        {
            venueUseAccessor = new VenueUseAccessor();
        }

        public VenueUseManager(IVenueUseAccessor vua)
        {
            venueUseAccessor = vua;
        }

        

        public bool AddVenueUse(VenueVM venue, DateTime startDate, DateTime endDate, Employee employee)
        {
            bool success = false;
            int venueId = venue.VenueID;
            try
            {
                int rowsAfected = venueUseAccessor.InsertVenueUse(venueId, startDate, endDate, employee.EmployeeID);
                if(rowsAfected == 1)
                {
                    success = true;
                }
            }
            catch (Exception ex)
            {

                throw new ApplicationException("Failed to add venue use", ex);
            }

            return success;
        }

        public List<VenueUseVM> RetreiveVenueUses()
        {
            List<VenueUseVM> venueUses = new List<VenueUseVM>();
            try
            {
                venueUses = venueUseAccessor.SelectVenueUses();
                foreach(VenueUseVM venueUse in venueUses)
                {
                    venueUseAccessor.SelectUseDaysByUseId(venueUse);
                }
            }
            catch (Exception ex)
            {

                throw new ApplicationException("Venue uses could not be retrieved.", ex);
            }
            return venueUses;
        }

        public bool AddUseDay(VenueUse venueUse, DateTime date, int ticketsSold, decimal revenue, Employee employee)
        {
            bool success = false;
            UseDay day = new UseDay();
            day.UseId = venueUse.UseId;
            day.UseDate = date;
            day.TicketsSold = ticketsSold;
            day.Revenue = revenue;
            day.EmployeeId = employee.EmployeeID;

            try
            {
                if (venueUseAccessor.InsertUseDay(day) == 2)
                {
                    success = true;
                }
            }
            catch (Exception ex)
            {

                throw new ApplicationException("Failed to add use day.", ex);
            }

            return success;
        }

        public bool UpdateVenueUseAdCampaign(int useId, int campaignId)
        {
            bool success = false;
            try
            {
                if (venueUseAccessor.UpdateVenueUseAdCampaign(useId, campaignId) == 1)
                {
                    success = true;
                }
            }
            catch (Exception ex)
            {

                throw new ApplicationException("Failed to update the use's ad campaign.", ex);
            }

            return success;
        }
    }
}
