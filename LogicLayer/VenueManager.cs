using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DataObjects;
using DataAccessLayer;
using DataAccessLayerFakes;
using DataAccessLayerInterfaces;
using LogicLayerInterfaces;

namespace LogicLayer
{
    public class VenueManager : IVenueManager
    {
        private IVenueAccessor venueAccessor = null;

        public VenueManager()
        {
            venueAccessor = new VenueAccessor();
        }

        public VenueManager(IVenueAccessor va)
        {
            venueAccessor = va;
        }

        public bool AddVenue(Venue venue)
        {
            throw new NotImplementedException();
        }

        public List<VenueVM> RetreiveVenues()
        {
            List<VenueVM> venues = null;
            try
            {
                 venues = venueAccessor.SelectVenuesWithStats();
                foreach(VenueVM venue in venues)
                {
                    venueAccessor.SelectCityAndStateByZipCode(venue);
                }
            }
            catch(Exception ex)
            {
                throw new ApplicationException("The venues could not be found", ex);
            }
            
            return venues;
        }
    }
}
