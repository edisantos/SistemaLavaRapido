using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;

namespace SkywalkerAutoWash.DAO
{
   public class Conexao
    {
        protected SqlConnection con;
        protected SqlCommand cmd;
        protected SqlDataReader Dr;
        protected SqlDataAdapter Adp;

        protected SqlConnection OpenCon()
        {
            //HERE OPEN THE CONNECTION WITH THE DATABASE
            string strCon = string.Format(@"Data Source=LAPTOP\SQLSERVERLOCAL;Initial Catalog=DbSkywalker_LavaRapido;Integrated Security=True");
            //string strCon = string.Format(@"workstation id=DbSkywalker.mssql.somee.com;packet size=4096;user id=edisantos_SQLLogin_1;pwd=oikdvpx65y;data source=DbSkywalker.mssql.somee.com;persist security info=False;initial catalog=DbSkywalker");
            con = new SqlConnection(strCon);
            if (con.State == ConnectionState.Closed)
            {
                con.Open();
            }
            return con;
        }
        protected SqlConnection CloseCon()
        {
            //HERE CLOSE THE CONNECTION WITH THE DATABASE.
            /* THIS METHOD HE CHECK IF THE CONNECTION IT OPEN AND CLOSE*/
            if (con.State == ConnectionState.Open)
            {
                con.Close();
            }
            return con;
        }

    }
}
