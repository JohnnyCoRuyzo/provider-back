﻿using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using AutoMapper;
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
                SqlCommand cmd = new SqlCommand(sqlProcedureToExecute(action), conn);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd = cmd.AddParameters(viewModel, action);
                rdr = cmd.ExecuteReader();
                return Convert.ToInt32(rdr["ID"]);
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

        public static string sqlProcedureToExecute(EnumAction action)
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
            cmd.Parameters.Add("@Business_Name", SqlDbType.VarChar).Value = viewModel.ProviderBusiness_Name;
            cmd.Parameters.Add("@NIT", SqlDbType.VarChar).Value = viewModel.ProviderNIT;
            cmd.Parameters.Add("@Address", SqlDbType.VarChar).Value = viewModel.ProviderAddress;
            cmd.Parameters.Add("@PhoneNumber", SqlDbType.VarChar).Value = viewModel.ProviderPhoneNumber;
            cmd.Parameters.Add("@Rating_Number", SqlDbType.VarChar).Value = viewModel.ProviderRating_Number;
            }
            if(action == EnumAction.Update || action == EnumAction.Delete)
            {
                cmd.Parameters.Add("@Id", SqlDbType.VarChar).Value = viewModel.ProviderID;
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
                P_ID = Convert.ToInt32(rdr["P_ID"]),
                P_Name = Convert.ToString(rdr["P_Name"]),
                P_Business_Name = Convert.ToString(rdr["P_Business_Name"]),
                P_NIT = Convert.ToString(rdr["P_NIT"]),
                P_Address = Convert.ToString(rdr["P_Address"]),
                P_PhoneNumber = Convert.ToString(rdr["P_PhoneNumber"]),
                P_Creation_Date = Convert.ToDateTime(rdr["P_Creation_Date"]),
                P_Rating_Number = Convert.ToDecimal(rdr["P_Rating_Number"])
            };
            bool parseSuccess = DateTime.TryParse(rdr["P_Modification_Date"].ToString(), out DateTime modificationDate);
            item.P_Modification_Date = !parseSuccess ? (DateTime?)null : modificationDate;
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