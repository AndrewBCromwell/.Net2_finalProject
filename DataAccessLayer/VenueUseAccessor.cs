using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DataAccessLayerInterfaces;
using DataObjects;

namespace DataAccessLayer
{
    public class VenueUseAccessor : IVenueUseAccessor
    {
        public int InsertVenueUse(int venueId, DateTime startDate, DateTime endDate, int employeeId)
        {
            int rowsAffected = 0;
            DBConnection connectionFactory = new DBConnection();
            var conn = connectionFactory.GetConnection();

            var cmdText = "sp_insert_venue_use";

            var cmd = new SqlCommand(cmdText, conn);

            cmd.CommandType = CommandType.StoredProcedure;

            cmd.Parameters.Add("@venue_id", SqlDbType.Int);
            cmd.Parameters.Add("@start_date", SqlDbType.Date);
            cmd.Parameters.Add("@end_date", SqlDbType.Date);
            cmd.Parameters.Add("@employee_id", SqlDbType.Int);

            cmd.Parameters["@venue_id"].Value = venueId;
            cmd.Parameters["@start_date"].Value = startDate;
            cmd.Parameters["@end_date"].Value = endDate;
            cmd.Parameters["@employee_id"].Value = employeeId;

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

        public List<VenueUseVM> SelectVenueUses()
        {
            List<VenueUseVM> venueUses = new List<VenueUseVM>();
            DBConnection connectionFactory = new DBConnection();
            var conn = connectionFactory.GetConnection();

            var cmdText = "sp_select_venue_uses";

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
                        VenueUseVM venueUse = new VenueUseVM();
                        venueUse.UseId = reader.GetInt32(0);
                        venueUse.VenueId = reader.GetInt32(1);
                        venueUse.VenueName = reader.GetString(2);
                        venueUse.StreetAddress = reader.GetString(3);
                        venueUse.ZipCode = reader.GetString(4);
                        venueUse.City = reader.GetString(5);
                        venueUse.State = reader.GetString(6);
                        venueUse.StartDate = reader.GetDateTime(7);
                        venueUse.EndDate = reader.GetDateTime(8);
                        if (reader.IsDBNull(9)) // idea from marc_s on StackOverflow, https://stackoverflow.com/questions/1772025/sql-data-reader-handling-null-column-values
                        {
                            venueUse.AdCampain = null;
                        }
                        else
                        {
                            venueUse.AdCampain = reader.GetInt32(9);
                        }
                        venueUse.TotalTicketsSold = reader.GetInt32(10);
                        venueUse.TotalRevenue = reader.GetDecimal(11);
                        venueUses.Add(venueUse);
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

            return venueUses;
        }


        public VenueUseVM SelectUseDaysByUseId(VenueUseVM venueUse)
        {
            DBConnection connectionFactory = new DBConnection();
            var conn = connectionFactory.GetConnection();

            var cmdText = "sp_select_use_days_by_use_id";

            var cmd = new SqlCommand(cmdText, conn);

            cmd.CommandType = CommandType.StoredProcedure;

            cmd.Parameters.Add("@use_id", SqlDbType.Int);
            cmd.Parameters["@use_id"].Value = venueUse.UseId;
            try
            {
                conn.Open();
                var reader = cmd.ExecuteReader();
                if (reader.HasRows)
                {
                    while (reader.Read())
                    {
                        venueUse.useDays = new List<UseDay>();
                        UseDay day = new UseDay();
                        day.UseId = reader.GetInt32(0);
                        day.UseDate = reader.GetDateTime(1);
                        day.TicketsSold = reader.GetInt32(2);
                        day.Revenue = reader.GetDecimal(3);
                        venueUse.useDays.Add(day);
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
            return venueUse;
        }

        public int InsertUseDay(UseDay useDay)
        {
            int rowsAffected = 0;
            DBConnection connectionFactory = new DBConnection();
            var conn = connectionFactory.GetConnection();

            var cmdText = "sp_insert_use_day";

            var cmd = new SqlCommand(cmdText, conn);

            cmd.CommandType = CommandType.StoredProcedure;

            cmd.Parameters.Add("@use_id", SqlDbType.Int);
            cmd.Parameters.Add("@date", SqlDbType.Date);
            cmd.Parameters.Add("@tickets_sold", SqlDbType.Int);
            cmd.Parameters.Add("@revenue", SqlDbType.Money);
            cmd.Parameters.Add("@employee_id", SqlDbType.Int);

            cmd.Parameters["@use_id"].Value = useDay.UseId;
            cmd.Parameters["@date"].Value = useDay.UseDate;
            cmd.Parameters["@tickets_sold"].Value = useDay.TicketsSold;
            cmd.Parameters["@revenue"].Value = useDay.Revenue;
            cmd.Parameters["@employee_id"].Value = useDay.EmployeeId;

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

        public int UpdateVenueUseAdCampaign(int useId, int campaignId)
        {
            int rowsAffected = 0;
            DBConnection connectionFactory = new DBConnection();
            var conn = connectionFactory.GetConnection();

            var cmdText = "sp_update_venueuse_adcampaign";

            var cmd = new SqlCommand(cmdText, conn);

            cmd.CommandType = CommandType.StoredProcedure;

            cmd.Parameters.Add("@use_id", SqlDbType.Int);
            cmd.Parameters.Add("@campaign_id", SqlDbType.Int);

            cmd.Parameters["@use_id"].Value = useId;
            cmd.Parameters["@campaign_id"].Value = campaignId;

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
    }
}
