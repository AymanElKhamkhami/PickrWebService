using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.SqlClient;
using System.Linq;
using System.Web;

namespace JSONWebService
{
    public class DBConnect
    {
        private static SqlConnection NewCon;
        private static string conStr = ConfigurationManager.ConnectionStrings["ConString"].ConnectionString;

        public static SqlConnection getConnection()
        {
            NewCon = new SqlConnection(conStr);
            return NewCon;

        }
        public DBConnect()
        {

        }
    }
}