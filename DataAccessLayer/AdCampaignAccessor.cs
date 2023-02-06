using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DataObjects;
using DataAccessLayerInterfaces;
using System.Data.SqlClient;
using System.Data;

namespace DataAccessLayer
{
    public class AdCampaignAccessor : IAdCampaignAccessor
    {
        public int InsertAdCampaign(AdCampaign adCampaign)
        {
            int newCampaignId = -1;
            DBConnection connectionFactory = new DBConnection();
            var conn = connectionFactory.GetConnection();

            var cmdText = "sp_insert_ad_campaign";

            var cmd = new SqlCommand(cmdText, conn);

            cmd.CommandType = CommandType.StoredProcedure;

            cmd.Parameters.Add("@company_id", SqlDbType.Int);
            cmd.Parameters.Add("@total_cost", SqlDbType.Money);
            cmd.Parameters.Add("@employee_id", SqlDbType.Int);

            cmd.Parameters["@company_id"].Value = adCampaign.AdCompany;
            cmd.Parameters["@total_cost"].Value = adCampaign.TotalCost;
            cmd.Parameters["@employee_id"].Value = adCampaign.EmployeeId;

            try
            {
                conn.Open();

                var reader = cmd.ExecuteReader();
                if (reader.HasRows)
                {
                    while (reader.Read())
                    {
                        
                        newCampaignId = reader.GetInt32(0);
                        
                    }
                }

            }
            catch (Exception ex)
            {

                throw ex;
            }
            finally
            {
                conn.Close();
            }

            return newCampaignId;
        }

        public int InsertAdItem(AdItem item)
        {
            int rowsAffected = 0;
            DBConnection connectionFactory = new DBConnection();
            var conn = connectionFactory.GetConnection();

            var cmdText = "sp_insert_ad_item";

            var cmd = new SqlCommand(cmdText, conn);

            cmd.CommandType = CommandType.StoredProcedure;

            cmd.Parameters.Add("@campaign_id", SqlDbType.Int);
            cmd.Parameters.Add("@ad_type", SqlDbType.NVarChar, 50);
            cmd.Parameters.Add("@focus_act", SqlDbType.NVarChar, 50);
            cmd.Parameters.Add("@cost", SqlDbType.Money);

            cmd.Parameters["@campaign_id"].Value = item.CampaignId;
            cmd.Parameters["@ad_type"].Value = item.AdType;
            cmd.Parameters["@focus_act"].IsNullable = true;
            if(item.FocusAct != null)
            {
                cmd.Parameters["@focus_act"].Value = item.FocusAct;
            }
            else
            {
                cmd.Parameters["@focus_act"].Value = DBNull.Value; // Thank You, Justin Niessner on stackoverflow, https://stackoverflow.com/questions/1207404/how-to-pass-a-null-variable-to-a-sql-stored-procedure-from-c-net-code
            }
            cmd.Parameters["@cost"].Value = item.Cost;

            try
            {
                conn.Open();

                rowsAffected = cmd.ExecuteNonQuery();
            }
            catch (Exception ex)
            {

                throw ex;
            }
            finally
            {
                conn.Close();
            }

            return rowsAffected;
        }

        public List<string> SelectActs()
        {
            List<string> acts = new List<string>();

            var connectionFactory = new DBConnection();
            var conn = connectionFactory.GetConnection();

            var cmdText = "sp_select_acts";


            var cmd = new SqlCommand(cmdText, conn);

            cmd.CommandType = CommandType.StoredProcedure;
                        
            try
            {
                conn.Open();

                var reader = cmd.ExecuteReader();

                if (reader.HasRows)
                {
                    while (reader.Read())
                    {
                        acts.Add(reader.GetString(0));
                    }
                }
                
            }
            catch (Exception ex)
            {

                throw ex;
            }
            finally
            {
                conn.Close();
            }

            return acts;
        }

        public List<AdCampaignVM> SelectAdCampaigns()
        {
            List<AdCampaignVM> adCampaigns = new List<AdCampaignVM>();
            DBConnection connectionFactory = new DBConnection();
            var conn = connectionFactory.GetConnection();

            var cmdText = "sp_select_ad_campaigns";

            var cmd = new SqlCommand(cmdText, conn);

            cmd.CommandType = CommandType.StoredProcedure;
            try
            {
                conn.Open();
                var reader = cmd.ExecuteReader();
                if (reader.HasRows)
                {
                    while (reader.Read())
                    {
                        AdCampaignVM adCampaign = new AdCampaignVM();
                        adCampaign.CampaignId = reader.GetInt32(0);
                        adCampaign.AdCompany = reader.GetInt32(1);
                        adCampaign.AdCompanyName = reader.GetString(2);
                        adCampaign.TotalCost = reader.GetDecimal(3);                        
                        if (reader.IsDBNull(4)) // idea from marc_s on StackOverflow, https://stackoverflow.com/questions/1772025/sql-data-reader-handling-null-column-values
                        {
                            adCampaign.AverageTicketsSold = null;
                        }
                        else
                        {
                            adCampaign.AverageTicketsSold = reader.GetInt32(4);
                        }
                        if (reader.IsDBNull(5))
                        {
                            adCampaign.AverageRevenue = null;
                        }
                        else
                        {
                            adCampaign.AverageRevenue = reader.GetDecimal(5);
                        }
                        adCampaigns.Add(adCampaign);
                    }
                }
            }
            catch (Exception ex)
            {

                throw ex;
            }
            finally
            {
                conn.Close();
            }

            return adCampaigns;
        }

        public AdCampaignVM SelectAdItemsByCampaignId(AdCampaignVM adCampaign)
        {
            DBConnection connectionFactory = new DBConnection();
            var conn = connectionFactory.GetConnection();

            var cmdText = "sp_select_ad_items_by_campaign_id";

            var cmd = new SqlCommand(cmdText, conn);

            cmd.CommandType = CommandType.StoredProcedure;

            cmd.Parameters.Add("@campaign_id", SqlDbType.Int);
            cmd.Parameters["@campaign_id"].Value = adCampaign.CampaignId;
            try
            {
                conn.Open();
                var reader = cmd.ExecuteReader();
                if (reader.HasRows)
                {
                    adCampaign.AdItems = new List<AdItem>();
                    while (reader.Read())
                    {
                        
                        AdItem adItem = new AdItem();
                        adItem.CampaignId = reader.GetInt32(0);
                        adItem.AdType = reader.GetString(1);
                        if (reader.IsDBNull(2))
                        {
                            adItem.FocusAct = null;
                        }
                        else
                        {
                            adItem.FocusAct = reader.GetString(2);
                        }
                        
                        adItem.Cost = reader.GetDecimal(3);
                        adCampaign.AdItems.Add(adItem);
                    }
                }
            }
            catch (Exception ex)
            {

                throw ex;
            }
            finally
            {
                conn.Close();
            }
            return adCampaign;
        }

        public List<string> SelectAdTypes()
        {
            List<string> adTypes = new List<string>();

            var connectionFactory = new DBConnection();
            var conn = connectionFactory.GetConnection();

            var cmdText = "sp_select_ad_types";


            var cmd = new SqlCommand(cmdText, conn);

            cmd.CommandType = CommandType.StoredProcedure;

            try
            {
                conn.Open();

                var reader = cmd.ExecuteReader();

                if (reader.HasRows)
                {
                    while (reader.Read())
                    {
                        adTypes.Add(reader.GetString(0));
                    }
                }

            }
            catch (Exception ex)
            {

                throw ex;
            }
            finally
            {
                conn.Close();
            }

            return adTypes;
        }
    }
}
