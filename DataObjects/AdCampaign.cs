using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataObjects
{
    public class AdCampaign
    {
        public int CampaignId { get; set; }
        public int AdCompany { get; set; }
        public decimal TotalCost { get; set; }
        public int EmployeeId { get; set; }
        public override string ToString()
        {
            return CampaignId.ToString();
        }
    }

    public class AdItem
    {
        public int CampaignId { get; set; }
        public string AdType { get; set; }
        public string FocusAct { get; set; }
        public decimal Cost { get; set; }
        public override string ToString()
        {
            string str;
            if (FocusAct == null)
            {
                str = "Type = " + AdType + ". Cost = " + Cost + ". No single focus.";
            }
            else
            {
                str = "Type = " + AdType + ". Focus Act = " + FocusAct + ". Cost = " + Cost + ". ";
            }
            return str;
        }
    }

    public class AdCampaignVM : AdCampaign
    {
        public List<AdItem> AdItems { get; set; }
        public string AdCompanyName { get; set; }
        public int? AverageTicketsSold { get; set; }
        public decimal? AverageRevenue { get; set; }
    }
}
