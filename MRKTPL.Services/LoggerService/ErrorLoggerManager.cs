using Microsoft.Extensions.Configuration;
using MRKTPL.Data.Entities;
using MRKTPL.Data.ViewModel;
using System;
using System.Data;
using System.Data.SqlClient;
using System.Net;

//using NLog;

namespace MRKTPL.LoggerService
{
    public class ErrorLoggerManager //: IErrorLoggerManager
    {
        #region Field
        private readonly MarketPlaceCoreContext context;
        private readonly IConfiguration configuration;
        #endregion
        #region CTor
        public ErrorLoggerManager(MarketPlaceCoreContext _context, IConfiguration _configuration)
        {
            context = _context;
            configuration = _configuration;
        }
        #endregion
        
        public string LogError()
        {
            ErrorDetails error = new ErrorDetails();
            try
            {
                string connectionString = configuration.GetConnectionString("DatabaseConnStr");
                using (SqlConnection con = new SqlConnection(connectionString))
                {
                    SqlCommand cmd = new SqlCommand("USP_MRKTPL_InsertErrorLog", con)
                    {
                        CommandType = CommandType.StoredProcedure
                    };
                    cmd.Parameters.AddWithValue("@StatusCode", error.StatusCode);
                    cmd.Parameters.AddWithValue("@ErrMessage", error.StatusCode);
                    cmd.Parameters.AddWithValue("@IPAddress", Dns.GetHostName());

                    con.Open();
                    SqlDataReader sdr = cmd.ExecuteReader(CommandBehavior.CloseConnection);

                    if (sdr.Read())
                    {
                        error.StatusCode = Convert.ToInt32(sdr["StatusCode"]);
                        
                    }

                    con.Close();
                }
                return error.ToString();
            }
            catch (Exception ex)
            {

                throw ex;
            }

        }

       
    }


}
