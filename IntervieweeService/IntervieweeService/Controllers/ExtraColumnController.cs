using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.SqlClient;
using System.Diagnostics;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using IntervieweeService.Models;

namespace IntervieweeService.Controllers
{
    public class ExtraColumnController : ApiController
    {
        private readonly String connectionString = 
            ConfigurationManager.ConnectionStrings["SqlServerStr"].ConnectionString;

        [HttpGet]
        [ActionName("GetExtraTable")]
        public List<ExtraColumn> Get()
        {
            List<ExtraColumn> listOfExtraColumns = new List<ExtraColumn>();

            try
            {
                using (var connection = new SqlConnection(connectionString))
                {
                    connection.Open();

                    using (var cmd = new SqlCommand())
                    {
                        cmd.CommandType = System.Data.CommandType.Text;
                        cmd.CommandText = "Select * from extracolumn";
                        cmd.Connection = connection;

                        SqlDataReader reader = cmd.ExecuteReader();

                        while (reader.Read())
                        {
                            ExtraColumn extraColumn = new ExtraColumn();

                            extraColumn.id = Convert.ToInt32(reader.GetValue(0));
                            extraColumn.columnname = reader.GetValue(1).ToString();

                            listOfExtraColumns.Add(extraColumn);
                        }

                        reader.Close();
                    }
                }
            }
            catch (SqlException ex)
            {
                Console.WriteLine(ex.Message);
            }

            return listOfExtraColumns;
        }

        [HttpPost]
        [ActionName("PostExtraColumn ")]
        public void AddInterviewee(ExtraColumn extraColumn)
        {
            try
            {
                using (var connection = new SqlConnection(connectionString))
                {
                    connection.Open();

                    using (var cmd = new SqlCommand())
                    {
                        cmd.CommandType = System.Data.CommandType.Text;
                        cmd.CommandText = "insert into extracolumn (columnname) values (@columnname)";
                        cmd.Connection = connection;
                        cmd.Parameters.AddWithValue("@columnname", extraColumn.columnname);
                        cmd.ExecuteNonQuery();
                    }
                }
            }
            catch (SqlException ex)
            {
                Debug.WriteLine(ex.Message);
            }
        }
    }
}
