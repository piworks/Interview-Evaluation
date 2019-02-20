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
    public class IntervieweeController : ApiController
    {
        private readonly String connectionString = 
            ConfigurationManager.ConnectionStrings["SqlServerStr"].ConnectionString;

        [HttpGet]
        [ActionName("GetInterviewee")]
        public List<Interviewee> GetInterviewees()
        {
            List<Interviewee> listOfInterviewee = new List<Interviewee>();

            try
            {
                using (var connection = new SqlConnection(connectionString))
                {
                    connection.Open();

                    using (var cmd = new SqlCommand())
                    {
                        cmd.CommandType = System.Data.CommandType.Text;
                        cmd.CommandText = "Select * from interviewee";
                        cmd.Connection = connection;

                        SqlDataReader reader = cmd.ExecuteReader();

                        while (reader.Read())
                        {
                            Interviewee interviewee = new Interviewee();
                            interviewee.extranotes = new List<ExtraNote>();

                            interviewee.id = Convert.ToInt32(reader.GetValue(0));
                            interviewee.firstname = reader.GetValue(1).ToString();
                            interviewee.lastname = reader.GetValue(2).ToString();
                            interviewee.email = reader.GetValue(3).ToString();
                            interviewee.university = reader.GetValue(4).ToString();
                            interviewee.githublink = reader.GetValue(5).ToString();
                            interviewee.bamboolink = reader.GetValue(6).ToString();
                            interviewee.backendnote = Convert.ToInt32(reader.GetValue(7));
                            interviewee.frontend = Convert.ToInt32(reader.GetValue(8));
                            interviewee.algorithms = Convert.ToInt32(reader.GetValue(9));
                            interviewee.specialnote = reader.GetValue(10).ToString();

                            // the notes for this interviewee
                            using (var cmdNote = new SqlCommand())
                            {
                                cmdNote.CommandType = System.Data.CommandType.Text;
                                cmdNote.CommandText = "Select * from extranotes";
                                cmdNote.Connection = connection;

                                SqlDataReader readerNote = cmdNote.ExecuteReader();

                                while (readerNote.Read())
                                {
                                    ExtraNote extraNote = new ExtraNote();
                                    extraNote.id = Convert.ToInt32(readerNote.GetValue(0));
                                    extraNote.intervieweeid = Convert.ToInt32(readerNote.GetValue(1));
                                    extraNote.columnname = readerNote.GetValue(2).ToString();
                                    extraNote.note = readerNote.GetValue(3).ToString();

                                    // here we check whether the extra note is for this interviewee or not
                                    if (interviewee.id == extraNote.intervieweeid)
                                    {
                                        interviewee.extranotes.Add(extraNote);
                                    }
                                }

                                readerNote.Close();
                            }


                            listOfInterviewee.Add(interviewee);
                        }

                        reader.Close();
                    }
    
                }
            }
            catch (SqlException ex)
            {
                Console.WriteLine(ex.Message);
            }

            return listOfInterviewee;
        }

        [HttpGet]
        [ActionName("GetIntervieweeByID")]
        public Interviewee GetIntervieweeById(int id)
        {
            Interviewee interviewee = null;

            try
            {
                using (var connection = new SqlConnection(connectionString))
                {
                    connection.Open();

                    using (var cmd = new SqlCommand())
                    {
                        cmd.CommandType = System.Data.CommandType.Text;
                        cmd.CommandText = "Select * from interviewee where id = " + id;
                        cmd.Connection = connection;

                        SqlDataReader reader = cmd.ExecuteReader();

                        while (reader.Read())
                        {
                            interviewee = new Interviewee();
                            interviewee.extranotes = new List<ExtraNote>();


                            interviewee.id = Convert.ToInt32(reader.GetValue(0));
                            interviewee.firstname = reader.GetValue(1).ToString();
                            interviewee.lastname = reader.GetValue(2).ToString();
                            interviewee.email = reader.GetValue(3).ToString();
                            interviewee.university = reader.GetValue(4).ToString();
                            interviewee.githublink = reader.GetValue(5).ToString();
                            interviewee.bamboolink = reader.GetValue(6).ToString();
                            interviewee.backendnote = Convert.ToInt32(reader.GetValue(7));
                            interviewee.frontend = Convert.ToInt32(reader.GetValue(8));
                            interviewee.algorithms = Convert.ToInt32(reader.GetValue(9));
                            interviewee.specialnote = reader.GetValue(10).ToString();

                            // the notes for this interviewee
                            using (var cmdNote = new SqlCommand())
                            {
                                cmdNote.CommandType = System.Data.CommandType.Text;
                                cmdNote.CommandText = "Select * from extranotes where intervieweeid = " + interviewee.id;
                                cmdNote.Connection = connection;

                                SqlDataReader readerNote = cmdNote.ExecuteReader();

                                while (readerNote.Read())
                                {
                                    ExtraNote extraNote = new ExtraNote();
                                    extraNote.id = Convert.ToInt32(readerNote.GetValue(0));
                                    extraNote.intervieweeid = Convert.ToInt32(readerNote.GetValue(1));
                                    extraNote.columnname = readerNote.GetValue(2).ToString();
                                    extraNote.note = readerNote.GetValue(3).ToString();

                                    // here we check whether the extra note is for this interviewee or not
                                    if (interviewee.id == extraNote.intervieweeid)
                                    {
                                        interviewee.extranotes.Add(extraNote);
                                    }
                                }

                                readerNote.Close();
                            }                      
                        }

                        reader.Close();
                    }
                }
            }
            catch (SqlException ex)
            {
                Debug.WriteLine(ex.Message);
            }
           
            return interviewee;
        }

        [HttpPost]
        [ActionName("PostInterviewee")]
        public void AddInterviewee(Interviewee interviewee)
        {     
            try
            {
                using (var connection = new SqlConnection(connectionString))
                {
                    connection.Open();

                    using (var cmd = new SqlCommand())
                    {
                        cmd.CommandType = System.Data.CommandType.Text;
                        cmd.CommandText = "insert into interviewee (firstname, lastname, email, university" +
                            ", githublink, bamboolink, backendnote, frontend, algorithms, specialnote) " +
                            "values (@firstname, @lastname, @email, @university" +
                            ", @githublink, @bamboolink, @backendnote, @frontend, @algorithms, @specialnote)";

                        Debug.WriteLine(cmd.CommandText);

                        cmd.Connection = connection;

                        cmd.Parameters.AddWithValue("@firstname", interviewee.firstname);
                        cmd.Parameters.AddWithValue("@lastname", interviewee.lastname);
                        cmd.Parameters.AddWithValue("@email", interviewee.email);
                        cmd.Parameters.AddWithValue("@university", interviewee.university);
                        cmd.Parameters.AddWithValue("@githublink", interviewee.githublink);
                        cmd.Parameters.AddWithValue("@bamboolink", interviewee.bamboolink);
                        cmd.Parameters.AddWithValue("@backendnote", interviewee.backendnote);
                        cmd.Parameters.AddWithValue("@frontend", interviewee.frontend);
                        cmd.Parameters.AddWithValue("@algorithms", interviewee.algorithms);
                        cmd.Parameters.AddWithValue("@specialnote", interviewee.specialnote);

                        cmd.ExecuteNonQuery();
                    }
                }
            }
            catch (SqlException ex)
            {

                Debug.WriteLine(ex.Message);
            }
        }

        [HttpPut]
        [ActionName("PutInterviewee")]
        public void EditInterviewee(Interviewee interviewee, int id)
        {
            try
            {
                using (var connection = new SqlConnection(connectionString))
                {
                    connection.Open();

                    using (var cmd = new SqlCommand())
                    {
                        cmd.CommandType = System.Data.CommandType.Text;
                        cmd.CommandText = "update interviewee set " +
                            "firstname = @firstname ," +
                            "lastname = @lastname ," +
                            "email = @email ," +
                            "university = @university ," +
                            "githublink = @githublink ," +
                            "bamboolink = @bamboolink ," +
                            "backendnote = @backendnote ," +
                            "frontend = @frontend ," +
                            "algorithms = @algorithms ," +
                            "specialnote = @specialnote " +
                            "where id = " + id;

                        cmd.Connection = connection;

                        cmd.Parameters.AddWithValue("@firstname", interviewee.firstname);
                        cmd.Parameters.AddWithValue("@lastname", interviewee.lastname);
                        cmd.Parameters.AddWithValue("@email", interviewee.email);
                        cmd.Parameters.AddWithValue("@university", interviewee.university);
                        cmd.Parameters.AddWithValue("@githublink", interviewee.githublink);
                        cmd.Parameters.AddWithValue("@bamboolink", interviewee.bamboolink);
                        cmd.Parameters.AddWithValue("@backendnote", interviewee.backendnote);
                        cmd.Parameters.AddWithValue("@frontend", interviewee.frontend);
                        cmd.Parameters.AddWithValue("@algorithms", interviewee.algorithms);
                        cmd.Parameters.AddWithValue("@specialnote", interviewee.specialnote);

                        cmd.ExecuteNonQuery();
                    }
                }
            }
            catch (SqlException ex)
            {

                Debug.WriteLine(ex.Message);
            }
        }

        [HttpDelete]
        [ActionName("DeleteInterviewee")]
        public void DeleteInterviewee(int id)
        {
            try
            {
                using (var connection = new SqlConnection(connectionString))
                {
                    connection.Open();
                    // deletes the interviewee
                    using (var cmd = new SqlCommand())
                    {
                        cmd.CommandType = System.Data.CommandType.Text;
                        cmd.CommandText = "delete from interviewee where id = " + id;
                        cmd.Connection = connection;
                        cmd.ExecuteNonQuery();
                    }

                    //deletes extra notes of the interviewee
                    using (var cmd = new SqlCommand())
                    {
                        cmd.CommandType = System.Data.CommandType.Text;
                        cmd.CommandText = "delete from extranotes where intervieweeid = " + id;
                        cmd.Connection = connection;
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
