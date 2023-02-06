using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataObjects
{
    public class Venue
    {
        public int VenueID { get; set; }
        public string VenueName { get; set; }
        public string StreetAddress { get; set; }
        public string ZipCode { get; set; }
        public string PhoneNumber { get; set; }
        public string TermsOfUse { get; set; }

        public override string ToString()
        {
            return VenueName;
        }
    }

    public class VenueVM : Venue
    {
        public int? AverageTicketsSold { get; set; }
        public decimal? AverageRevenue { get; set; }
        public DateTime? LastUsedOn { get; set; }
        public string City { get; set; }
        public string State { get; set; }
    }
}
