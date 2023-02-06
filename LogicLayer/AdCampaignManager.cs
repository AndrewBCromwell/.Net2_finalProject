using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DataObjects;
using DataAccessLayerInterfaces;
using DataAccessLayer;
using DataAccessLayerFakes;
using LogicLayerInterfaces;

namespace LogicLayer
{
    public class AdCampaignManager : IAdCampaignManager
    {
        IAdCampaignAccessor adCampaignAccessor = null;

        public AdCampaignManager()
        {
            adCampaignAccessor = new AdCampaignAccessor();
        }

        public AdCampaignManager(IAdCampaignAccessor aca)
        {
            adCampaignAccessor = aca;
        }

        public bool AddAdCampaign(AdCompany adCompany, decimal cost, Employee employee, List<AdItem> adItems)
        {
            bool success = false;
            AdCampaign adCampaign = new AdCampaign();
            adCampaign.AdCompany = adCompany.CompanyId;
            adCampaign.TotalCost = cost;
            adCampaign.EmployeeId = employee.EmployeeID;
            try
            {
                int newCampaignId = adCampaignAccessor.InsertAdCampaign(adCampaign);
                if (newCampaignId != -1)
                {
                    int expectedRowsInserted = adItems.Count;
                    int rowsInserted = 0;
                    foreach(AdItem item in adItems)
                    {
                        item.CampaignId = newCampaignId;
                        rowsInserted += adCampaignAccessor.InsertAdItem(item);
                    }
                    if(rowsInserted == expectedRowsInserted)
                    {
                        success = true;
                    }
                } 
            }
            catch (Exception ex)
            {

                throw new ApplicationException("Failed to add ad campaign.", ex);
            }

            return success;
        }

        public List<string> RetreiveActs()
        {
            List<string> acts = new List<string>();
            try
            {
                acts = adCampaignAccessor.SelectActs();
            }
            catch (Exception ex)
            {

                throw new ApplicationException("The show acts could not be retrieved.", ex);
            }
            return acts;
        }

        public List<AdCampaignVM> RetreiveAdCampaigns()
        {
            List<AdCampaignVM> adCampaigns = new List<AdCampaignVM>();
            try
            {
                adCampaigns = adCampaignAccessor.SelectAdCampaigns();
                foreach (AdCampaignVM adCampaign in adCampaigns)
                {
                    adCampaignAccessor.SelectAdItemsByCampaignId(adCampaign);
                }
            }
            catch (Exception ex)
            {

                throw new ApplicationException("Ad campaigns could not be retrieved.", ex);
            }
            return adCampaigns;
        }

        public List<string> RetreiveAdTypes()
        {
            List<string> adTypes = new List<string>();
            try
            {
                adTypes = adCampaignAccessor.SelectAdTypes();
            }
            catch (Exception ex)
            {

                throw new ApplicationException("The ad types could not be retrieved.", ex);
            }
            return adTypes;
        }
    }
}
