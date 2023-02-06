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
    public class VenueAccessor : IVenueAccessor
    {
        public int InsertVenue(Venue venue)
        {
            throw new NotImplementedException();
        }

        public List<VenueVM> SelectVenuesWithStats()
        {

            List<VenueVM> venues = new List<VenueVM>();

            DBConnection connectionFactory = new DBConnection();
            var conn = connectionFactory.GetConnection();

            var cmdText = "sp_select_venues_with_stats";

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
                        VenueVM venue = new VenueVM();
                        venue.VenueID = reader.GetInt32(0);
                        venue.VenueName = reader.GetString(1);
                        venue.StreetAddress = reader.GetString(2);
                        venue.ZipCode = reader.GetString(3);
                        venue.TermsOfUse = reader.GetString(4);
                        venue.PhoneNumber = reader.GetString(5);
                        if (reader.IsDBNull(6)) // idea from marc_s on StackOverflow, https://stackoverflow.com/questions/1772025/sql-data-reader-handling-null-column-values
                        {
                            venue.AverageTicketsSold = null;
                        }
                        else
                        {
                            venue.AverageTicketsSold = reader.GetInt32(6);
                        }
                        if (reader.IsDBNull(7))
                        {
                            venue.AverageRevenue = null;
                        }
                        else
                        {
                            venue.AverageRevenue = reader.GetDecimal(7);
                        }
                        if (reader.IsDBNull(8))
                        {
                            venue.LastUsedOn = null;
                        }
                        else
                        {
                            venue.LastUsedOn = reader.GetDateTime(8);
                        }
                        venues.Add(venue);
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
            return venues;
        }

        public VenueVM SelectCityAndStateByZipCode(VenueVM venue)
        {
            DBConnection connectionFactory = new DBConnection();
            var conn = connectionFactory.GetConnection();

            var cmdText = "sp_select_city_and_state_by_zipcode";

            var cmd = new SqlCommand(cmdText, conn);

            cmd.CommandType = CommandType.StoredProcedure;

            cmd.Parameters.Add("@zip_code", SqlDbType.Char, 5);
            cmd.Parameters["@zip_code"].Value = venue.ZipCode;
            try
            {
                conn.Open();
                var reader = cmd.ExecuteReader();
                if (reader.HasRows)
                {
                    reader.Read();
                    venue.City = reader.GetString(0);
                    venue.State = reader.GetString(1);
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

            return venue;
        }
    }
}
