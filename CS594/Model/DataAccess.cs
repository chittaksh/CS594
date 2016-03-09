using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using Immensity.Data.SQLHelper;
using System.Configuration;

namespace CS594Web.Model
{
    public class DataAccess
    {
        /// <summary>
        /// To Retrive distinct web service and it's url.
        /// </summary>
        /// <returns></returns>
        public static DataTable getServices()
        {
            DataTable dt = SQLHelper.ExecuteDataset(ConfigurationManager.ConnectionStrings["DefaultConnection"].ConnectionString, CommandType.StoredProcedure, "sp_GetServices", null).Tables[0];

            DataRow dr = dt.NewRow();

            dr["STRSERVICE"] = "None";
            dr["STRSERVICEURL"] = 0;

            dt.Rows.InsertAt(dr,0);

            return dt;
        }

        /// <summary>
        /// Insert new record in a wen service..
        /// </summary>
        /// <param name="strService"></param>
        /// <param name="strServiceUrl"></param>
        /// <param name="intTimesCalled"></param>
        /// <param name="floatInterval"></param>
        /// <returns></returns>
        public static int NewRecord( string strService, string strServiceUrl, int intTimesCalled, float floatInterval)
        {
            try
            {
                SqlParameter[] sqlParams = new SqlParameter[4];

                SqlParameter SP1 = new SqlParameter("@strService", SqlDbType.VarChar);
                SP1.Value = strService;
                sqlParams[0] = SP1;

                SqlParameter SP2 = new SqlParameter("@strServiceUrl", SqlDbType.VarChar);
                SP2.Value = strServiceUrl;
                sqlParams[1] = SP2;

                SqlParameter SP3 = new SqlParameter("@intTimesCalled", SqlDbType.Int);
                SP3.Value = intTimesCalled;
                sqlParams[2] = SP3;

                SqlParameter SP4 = new SqlParameter("@floatInterval", SqlDbType.Float);
                SP4.Value = floatInterval;
                sqlParams[3] = SP4;

                return SQLHelper.ExecuteNonQuery(ConfigurationManager.ConnectionStrings["DefaultConnection"].ConnectionString, CommandType.StoredProcedure, "sp_AddRecord", sqlParams);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        /// <summary>
        /// get records for comparison.
        /// </summary>
        /// <param name="intCount"></param>
        /// <param name="strService"></param>
        /// <returns></returns>
        public static DataSet getRecords(int intCount, int intTimes ,string strService)
        {
            SqlParameter[] sqlParams = new SqlParameter[3];

            SqlParameter SP1 = new SqlParameter("@intCount", SqlDbType.Int);
            SP1.Value = intCount;
            sqlParams[0] = SP1;

            SqlParameter SP2 = new SqlParameter("@intTimes", SqlDbType.Int);
            SP2.Value = intTimes;
            sqlParams[1] = SP2;

            SqlParameter SP3 = new SqlParameter("@strService", SqlDbType.VarChar);
            SP3.Value = strService;
            sqlParams[2] = SP3;

            return SQLHelper.ExecuteDataset(ConfigurationManager.ConnectionStrings["DefaultConnection"].ConnectionString, CommandType.StoredProcedure, "sp_GetRecords", sqlParams);
        }
    }
}
