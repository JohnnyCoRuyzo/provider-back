using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Globalization;
using System.IO;
using provider_back.Models;
using static provider_back.Utilities;

namespace provider_back.Connection
{
    public static class Connection
    {
        public static int ExecuteStoreProcedure( ProviderViewModel viewModel, EnumAction action)
        {
            SqlConnection conn = null;
            SqlDataReader rdr = null;

            try
            {
                conn = new SqlConnection(GetDBConnectionString());
                conn.Open();
                SqlCommand cmd = new SqlCommand(SqlProcedureToExecute(action), conn);
                cmd.CommandType = CommandType.Text;
                cmd = cmd.AddParameters(viewModel, action);
                rdr = cmd.ExecuteReader();
                int id = 0;
                while (rdr.Read())
                {
                    if(action != EnumAction.Insert) {
                        id = 0;
                    }
                    else { 
                    id = Convert.ToInt32(rdr["ID"]??0);
                    }
                }
                return id;
            }
            finally
            {
                if (conn != null)
                {
                    conn.Close();
                }
                if (rdr != null)
                {
                    rdr.Close();
                }
            }
        }

        public static string SqlProcedureToExecute(EnumAction action)
        {
            switch (action)
            {
                case EnumAction.Insert:
                    return "EXEC [dbo].[PI_Provider_Insert] @Name,@Business_Name,@NIT,@Address,@PhoneNumber,@Rating_Number";
                case EnumAction.Update:
                    return "EXEC [dbo].[PI_Provider_Update] @Id,@Name,@Business_Name,@NIT,@Address,@PhoneNumber,@Rating_Number";
                case EnumAction.Delete:
                    return "EXEC [dbo].[PI_Provider_Delete] @Id";
                default:
                    return "";
            }
        }

        public static SqlCommand AddParameters(this SqlCommand cmd, ProviderViewModel viewModel, EnumAction action)
        {
            if (action == EnumAction.Insert || action == EnumAction.Update) { 
            cmd.Parameters.Add("@Name", SqlDbType.VarChar).Value = viewModel.ProviderName;
            cmd.Parameters.Add("@Business_Name", SqlDbType.VarChar).Value = viewModel.ProviderBusinessName;
            cmd.Parameters.Add("@NIT", SqlDbType.VarChar).Value = viewModel.ProviderNIT;
            cmd.Parameters.Add("@Address", SqlDbType.VarChar).Value = viewModel.ProviderAddress;
            cmd.Parameters.Add("@PhoneNumber", SqlDbType.VarChar).Value = viewModel.ProviderPhoneNumber;
            cmd.Parameters.Add("@Rating_Number", SqlDbType.VarChar).Value = viewModel.ProviderRatingNumber;
            }
            if(action == EnumAction.Update || action == EnumAction.Delete)
            {
                cmd.Parameters.Add("@Id", SqlDbType.Int).Value = viewModel.ProviderID;
            }
            return cmd;
        }

        public static List<ProviderViewModel> SelectQuery(string sqlQuery)
        {
            SqlConnection conn = null;
            SqlDataReader rdr = null;
            List<ProviderViewModel> lista = new List<ProviderViewModel>();

            try
            {
                conn = new SqlConnection(GetDBConnectionString());
                conn.Open();
                SqlCommand cmd = new SqlCommand(sqlQuery, conn);
                cmd.CommandType = CommandType.Text;
                rdr = cmd.ExecuteReader(); 
                while (rdr.Read())
                {
                    lista.Add(ProviderElement(rdr));
                }
            }
            finally
            {
                if (conn != null)
                {
                    conn.Close();
                }
                if (rdr != null)
                {
                    rdr.Close();
                }
            }
            return lista;
        }

        public static ProviderViewModel ProviderElement(SqlDataReader rdr)
        {
            ProviderViewModel item = new ProviderViewModel
            {
                ProviderID = Convert.ToInt32(rdr["P_ID"]),
                ProviderOrder = Convert.ToInt32(rdr["P_Order"]),
                ProviderName = Convert.ToString(rdr["P_Name"]),
                ProviderBusinessName = Convert.ToString(rdr["P_Business_Name"]),
                ProviderNIT = Convert.ToString(rdr["P_NIT"]),
                ProviderAddress = Convert.ToString(rdr["P_Address"]),
                ProviderPhoneNumber = Convert.ToString(rdr["P_PhoneNumber"]),
                ProviderCreationDate = Convert.ToDateTime(rdr["P_Creation_Date"]),
                ProviderRatingNumber = Decimal.Round(Convert.ToDecimal(rdr["P_Rating_Number"]),1).ToString()
            };
            bool parseSuccess = DateTime.TryParse(rdr["P_Modification_Date"].ToString(), out DateTime modificationDate);
            item.ProviderLastModificationDate = !parseSuccess ? (DateTime?)null : modificationDate;
            return item;

        }

        public static string GetDBConnectionString()
        {
            LoadEnviroment();
            return GetConnectionString();
        }

        private static void LoadEnviroment()
        {
            DotNetEnv.Env.Load();
            DotNetEnv.Env.TraversePath().Load();
            string filePath = GetFilePath();
            DotNetEnv.Env.Load(filePath.Replace("\\", "/"));
            using (var stream = File.OpenRead(filePath))
            {
                DotNetEnv.Env.Load(stream);
            }
        }

        private static string GetFilePath()
        {
            if (ConfigurationManager.AppSettings["prod"].ToUpper() == "FALSE")
            {
                return System.AppDomain.CurrentDomain.BaseDirectory + "..\\..\\..\\.enviroment";
            }
            else
            {
                return System.AppDomain.CurrentDomain.BaseDirectory + "\\.enviroment";
            }
        }

        private static string GetConnectionString()
        {
            return "Data Source=" + DotNetEnv.Env.GetString("DATA_SOURCE")
                 + ";Initial Catalog=" + DotNetEnv.Env.GetString("INITIAL_CATALOG")
                 + ";Persist Security Info=True;User ID=" + DotNetEnv.Env.GetString("USER_ID")
                 + ";Password=" + DotNetEnv.Env.GetString("PASSWORD");
        }
    }
}
