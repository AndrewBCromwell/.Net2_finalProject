using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DataAccessLayerInterfaces;
using DataObjects;

namespace DataAccessLayerFakes
{
    public class VenueAccessorFake : IVenueAccessor
    {
        private List<VenueVM> fakeVenues = new List<VenueVM>();

        public VenueAccessorFake()
        {
            fakeVenues.Add(new VenueVM()
            {
                VenueID = 999999,
                VenueName = "fakeItTillMakeIt",
                StreetAddress = "000 doesn't matter",
                ZipCode = "00000",
                PhoneNumber = "000-000-0000",
                TermsOfUse = "does not matter",
                AverageTicketsSold = 0,
                AverageRevenue = 0.00M,
                LastUsedOn = new DateTime(2019, 05, 09)
            });
            fakeVenues.Add(new VenueVM()
            {
                VenueID = 999998,
                VenueName = "another fake",
                StreetAddress = "000 doesn't matter",
                ZipCode = "00000",
                PhoneNumber = "000-000-0000",
                TermsOfUse = "does not matter",
                AverageTicketsSold = 0,
                AverageRevenue = 0.00M,
                LastUsedOn = new DateTime(2022, 12, 25)
            });
        }

        

        public List<VenueVM> SelectVenuesWithStats()
        {
            return fakeVenues;
        }

        public int InsertVenue(Venue venue)
        {
             throw new NotImplementedException();
        }

        public VenueVM SelectCityAndStateByZipCode(VenueVM venue)
        {
            throw new NotImplementedException();
        }
    }
}
