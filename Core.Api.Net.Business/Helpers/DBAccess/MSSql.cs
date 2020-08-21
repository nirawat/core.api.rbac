using System;
using System.Linq;
using System.Text;
using System.Data;
using System.Data.SqlTypes;
using System.Data.SqlClient;
using System.Collections.Generic;
using Core.Api.Net.Business.Models.DBAccess;
using Core.Api.Net.Business.Models.Configs;

namespace Core.Api.Net.Business.Helpers.DBAccess
{
    public class MSSql : IDisposable
    {
        private string oConnStr;
        public MSSql(DBConnectionType type, EnvironmentModel EnvironmentModel)
        {
            switch (type)
            {
                case DBConnectionType.RBAC:
                    oConnStr = EnvironmentModel.DBRBAC.ConnectionString;
                    break;
                case DBConnectionType.Business:
                    oConnStr = EnvironmentModel.DBBusiness.ConnectionString;
                    break;
                default:
                    break;
            }
        }
        public DataTable GetDataTableFromQueryStr(string oQueryString)
        {
            DataTable dt = new DataTable();
            using (var cnn = new SqlConnection(oConnStr))
            {
                cnn.Open();
                using (var cmd = new SqlCommand(oQueryString, cnn))
                {
                    cmd.CommandText = oQueryString;
                    cmd.CommandType = CommandType.Text;
                    cmd.CommandTimeout = 60;

                    SqlDataAdapter adp = new SqlDataAdapter(cmd);
                    adp.Fill(dt);
                }
                cnn.Close();
            }
            return dt;
        }

        public int ExcuteNonQueryStr(string oQueryString)
        {
            int resp = -1;
            using (var cnn = new SqlConnection(oConnStr))
            {
                cnn.Open();
                using (var cmd = new SqlCommand(oQueryString, cnn))
                {
                    cmd.CommandText = oQueryString;
                    cmd.CommandType = CommandType.Text;
                    cmd.CommandTimeout = 60;
                    resp = cmd.ExecuteNonQuery();
                }
                cnn.Close();
            }
            return resp;
        }

        public int ExcuteScalarQueryStr(string oConnStr, string oQueryString)
        {
            int resp = -1;
            using (var cnn = new SqlConnection(oConnStr))
            {
                cnn.Open();
                using (var cmd = new SqlCommand(oQueryString, cnn))
                {
                    cmd.CommandText = oQueryString;
                    cmd.CommandType = CommandType.Text;
                    cmd.CommandTimeout = 60;
                    resp = cmd.ExecuteNonQuery();
                }
                cnn.Close();
            }
            return resp;
        }

        public int ExecuteStoreProcedure(string oConnStr, string oStoreProcedureName, IList<StoreParameters> list_parameters)
        {
            int resp = -1;
            using (var cnn = new SqlConnection(oConnStr))
            {
                cnn.Open();

                using (var cmd = new SqlCommand(oStoreProcedureName, cnn))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.CommandTimeout = 60;

                    if (list_parameters != null && list_parameters.Count > 0)
                    {
                        foreach (var item in list_parameters)
                        {
                            cmd.Parameters.Add(new SqlParameter(string.Format("@{0}", item.pName), item.pValue));
                        }
                    }
                    cmd.Parameters.Add("@retValue", SqlDbType.Int).Direction = System.Data.ParameterDirection.ReturnValue;

                    cmd.ExecuteNonQuery();

                    resp = (int)cmd.Parameters["@retValue"].Value;
                }
                cnn.Close();
            }
            return resp;
        }


        #region IDisposable Support
        private bool disposedValue = false;
        protected virtual void Dispose(bool disposing)
        {
            if (!disposedValue)
            {
                if (disposing)
                {
                    //TODO: dispose managed state (managed objects).
                }
                disposedValue = true;
            }
        }
        public void Dispose()
        {
            Dispose(true);
        }
        #endregion
    }

}