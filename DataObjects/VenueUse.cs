using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataObjects
{
    public class VenueUse
    {
        public int UseId { get; set; }
        public int VenueId { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public int? AdCampain { get; set; }
        public int TotalTicketsSold { get; set; }
        public decimal TotalRevenue { get; set; }
        public override string ToString()
        {
            return UseId + ": " + StartDate.ToShortDateString() + " - " + EndDate.ToShortDateString();
        }
    }

    public class UseDay
    {
        public int UseId { get; set; }
        public DateTime UseDate { get; set; }
        public int TicketsSold { get; set; }
        public decimal Revenue { get; set; }
        public int EmployeeId { get; set; }
    }

    public class VenueUseVM : VenueUse
    {
        public List<UseDay> useDays { get; set; }
        public string VenueName { get; set; }
        public string StreetAddress { get; set; }
        public string ZipCode { get; set; }
        public string City { get; set; }
        public string State { get; set; }
        public override string ToString()
        {
            return VenueName + ": " + StartDate.ToShortDateString() + " - " + EndDate.ToShortDateString();
        }
    }
}
