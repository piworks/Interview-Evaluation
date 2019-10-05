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
    public class IntervieweeController : ApiController
    {
        private readonly String connectionString = 
            ConfigurationManager.ConnectionStrings["MySqlServerStr"].ConnectionString;

        [HttpGet]
        [ActionName("GetInterviewee")]
        public List<Interviewee> GetInterviewees()
        {
            List<Interviewee> listOfInterviewee = new List<Interviewee>();
            Debug.WriteLine("get interviewee");
            try
            {
                using (var connection = new MySqlConnection(connectionString))
                {
                    connection.Open();
                    Debug.WriteLine("connection is opened");
                    using (var cmd = new MySqlCommand())
                    {
                        cmd.CommandType = System.Data.CommandType.Text;
                        cmd.CommandText = "Select * from interviewee";
                        cmd.Connection = connection;

                        MySqlDataReader reader = cmd.ExecuteReader();

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
                            using (var connectionNote = new MySqlConnection(connectionString))
                            {
                                connectionNote.Open();
                                using (var cmdNote = new MySqlCommand())
                                {
                                    cmdNote.CommandType = System.Data.CommandType.Text;
                                    cmdNote.CommandText = "Select * from extranotes";
                                    cmdNote.Connection = connectionNote;

                                    MySqlDataReader readerNote = cmdNote.ExecuteReader();

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
                            

                            Debug.WriteLine(interviewee.firstname + " is added");
                            listOfInterviewee.Add(interviewee);
                        }

                        reader.Close();
                    }
    
                }
            }
            catch (MySqlException ex)
            {

                Debug.WriteLine(ex);
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
                using (var connection = new MySqlConnection(connectionString))
                {
                    connection.Open();

                    using (var cmd = new MySqlCommand())
                    {
                        cmd.CommandType = System.Data.CommandType.Text;
                        cmd.CommandText = "Select * from interviewee where id = @id";
						cmd.Parameters.AddWithValue("@id", id);
                        cmd.Connection = connection;

                        MySqlDataReader reader = cmd.ExecuteReader();

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

                            Debug.WriteLine(interviewee.firstname);

                            // the notes for this interviewee
                            using (var connectionNote = new MySqlConnection(connectionString))
                            {
                                connectionNote.Open();
                                using (var cmdNote = new MySqlCommand())
                                {
                                    cmdNote.CommandType = System.Data.CommandType.Text;
									cmdNote.CommandText = "Select * from extranotes where intervieweeid = @intervieweeid";
									cmdNote.Parameters.AddWithValue("@intervieweeid", interviewee.id);
                                    cmdNote.Connection = connectionNote;

                                    MySqlDataReader readerNote = cmdNote.ExecuteReader();

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
                              
                        }

                        reader.Close();
                    }
                }
            }
            catch (MySqlException ex)
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
                using (var connection = new MySqlConnection(connectionString))
                {
                    connection.Open();

                    using (var cmd = new MySqlCommand())
                    {
                        cmd.CommandType = System.Data.CommandType.Text;
                        cmd.CommandText = "insert into interviewee (firstname, lastname, email, university" +
                            ", githublink, bamboolink, backend, frontend, algorithms, specialnote) " +
                            "values (@firstname, @lastname, @email, @university" +
                            ", @githublink, @bamboolink, @backend, @frontend, @algorithms, @specialnote)";

                        Debug.WriteLine(cmd.CommandText);

                        cmd.Connection = connection;

                        cmd.Parameters.AddWithValue("@firstname", interviewee.firstname);
                        cmd.Parameters.AddWithValue("@lastname", interviewee.lastname);
                        cmd.Parameters.AddWithValue("@email", interviewee.email);
                        cmd.Parameters.AddWithValue("@university", interviewee.university);
                        cmd.Parameters.AddWithValue("@githublink", interviewee.githublink);
                        cmd.Parameters.AddWithValue("@bamboolink", interviewee.bamboolink);
                        cmd.Parameters.AddWithValue("@backend", interviewee.backendnote);
                        cmd.Parameters.AddWithValue("@frontend", interviewee.frontend);
                        cmd.Parameters.AddWithValue("@algorithms", interviewee.algorithms);
                        cmd.Parameters.AddWithValue("@specialnote", interviewee.specialnote);

                        cmd.ExecuteNonQuery();
                    }
                }
            }
            catch (MySqlException ex)
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
                using (var connection = new MySqlConnection(connectionString))
                {
                    connection.Open();

                    using (var cmd = new MySqlCommand())
                    {
                        cmd.CommandType = System.Data.CommandType.Text;
                        cmd.CommandText = "update interviewee set " +
                            "firstname = @firstname ," +
                            "lastname = @lastname ," +
                            "email = @email ," +
                            "university = @university ," +
                            "githublink = @githublink ," +
                            "bamboolink = @bamboolink ," +
                            "backend = @backend ," +
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
                        cmd.Parameters.AddWithValue("@backend", interviewee.backendnote);
                        cmd.Parameters.AddWithValue("@frontend", interviewee.frontend);
                        cmd.Parameters.AddWithValue("@algorithms", interviewee.algorithms);
                        cmd.Parameters.AddWithValue("@specialnote", interviewee.specialnote);

                        cmd.ExecuteNonQuery();
                    }
                }
            }
            catch (MySqlException ex)
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
                using (var connection = new MySqlConnection(connectionString))
                {
                    connection.Open();
                    // deletes the interviewee
                    using (var cmd = new MySqlCommand())
                    {
                        cmd.CommandType = System.Data.CommandType.Text;
                        cmd.CommandText = "delete from interviewee where id = @id";
						cmd.Parameters.AddWithValue("@id", id);
                        cmd.Connection = connection;
                        cmd.ExecuteNonQuery();
                    }

                    //deletes extra notes of the interviewee
                    using (var cmd = new MySqlCommand())
                    {
                        cmd.CommandType = System.Data.CommandType.Text;
                        cmd.CommandText = "delete from extranotes where intervieweeid = @id";
						cmd.Parameters.AddWithValue("@id", id);
                        cmd.Connection = connection;
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
