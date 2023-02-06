using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DataObjects;

namespace DataAccessLayerInterfaces
{
    public interface IVenueAccessor
    {
        List<VenueVM> SelectVenuesWithStats();
        VenueVM SelectCityAndStateByZipCode(VenueVM venue);
        int InsertVenue(Venue venue);
    }
}
