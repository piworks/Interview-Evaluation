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
using MySql.Data.MySqlClient;

namespace IntervieweeService.Controllers
{
    public class ExtraColumnController : ApiController
    {
        private readonly String connectionString = 
            ConfigurationManager.ConnectionStrings["MySqlServerStr"].ConnectionString;

        [HttpGet]
        [ActionName("GetExtraTable")]
        public List<ExtraColumn> Get()
        {
            List<ExtraColumn> listOfExtraColumns = new List<ExtraColumn>();

            try
            {
                using (var connection = new MySqlConnection(connectionString))
                {
                    connection.Open();

                    using (var cmd = new MySqlCommand())
                    {
                        cmd.CommandType = System.Data.CommandType.Text;
                        cmd.CommandText = "Select * from extracolumn";
                        cmd.Connection = connection;

                        MySqlDataReader reader = cmd.ExecuteReader();

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
            catch (MySqlException ex)
            {
                Console.WriteLine(ex.Message);
            }

            return listOfExtraColumns;
        }

        [HttpDelete]
        [ActionName("DeleteExtraColumn")]
        public void DeleteExtraColumn(int id)
        {
            try
            {
                using(var connection = new MySqlConnection(connectionString))
                {
                    connection.Open();

                    using(var cmd = new MySqlCommand())
                    {
                        cmd.CommandType = System.Data.CommandType.Text;
                        cmd.CommandText = "Delete from table extracolumn where id = (@id)";
                        cmd.Connection = connection;
                        cmd.Parameters.AddWithValue("@id", id);
                        cmd.ExecuteNonQuery();
                    }
                }
            }
            catch(MySqlException ex)
            {
                Debug.WriteLine(ex.Message);
            }
           
        }

        [HttpPost]
        [ActionName("PostExtraColumn ")]
        public void AddInterviewee(ExtraColumn extraColumn)
        {
            try
            {
                using (var connection = new MySqlConnection(connectionString))
                {
                    connection.Open();

                    using (var cmd = new MySqlCommand())
                    {
                        cmd.CommandType = System.Data.CommandType.Text;
                        cmd.CommandText = "insert into extracolumn (columnname) values (@columnname)";
                        cmd.Connection = connection;
                        cmd.Parameters.AddWithValue("@columnname", extraColumn.columnname);
                        cmd.ExecuteNonQuery();
                    }
                }
            }
            catch (MySqlException ex)
            {
                Debug.WriteLine(ex.Message);
            }
        }
    }
}
