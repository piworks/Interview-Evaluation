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
    public class ExtraNoteController : ApiController
    {
        private readonly String connectionString = 
            ConfigurationManager.ConnectionStrings["MySqlServerStr"].ConnectionString;

        [HttpGet]
        [ActionName("GetExtraNoteById")]
        public List<ExtraNote> Get()
        {
            List<ExtraNote> listOfExtraNotes = new List<ExtraNote>();

            try
            {
                using (var connection = new MySqlConnection(connectionString))
                {
                    connection.Open();

                    using (var cmd = new MySqlCommand())
                    {
                        cmd.CommandType = System.Data.CommandType.Text;
                        cmd.CommandText = "Select * from extranotes";
                        cmd.Connection = connection;

                        MySqlDataReader reader = cmd.ExecuteReader();

                        while (reader.Read())
                        {
                            ExtraNote extraNote = new ExtraNote();

                            extraNote.id = Convert.ToInt32(reader.GetValue(0));
                            extraNote.intervieweeid = Convert.ToInt32(reader.GetValue(1));
                            extraNote.columnname = reader.GetValue(2).ToString();
                            extraNote.note = reader.GetValue(3).ToString();

                            listOfExtraNotes.Add(extraNote);
                        }

                        reader.Close();
                    }
                }
            }
            catch (MySqlException ex)
            {
                Console.WriteLine(ex.Message);
            }

            return listOfExtraNotes;
        }

        [HttpPost]
        [ActionName("PostExtraNote")]
        public void AddExtraNote(List<ExtraNote> extraNotes)
        {

            int intervieweeId = -1;

            try
            {
                using (var connection = new MySqlConnection(connectionString))
                {
                    connection.Open();

                    using (var cmd = new MySqlCommand())
                    {
                        cmd.CommandType = System.Data.CommandType.Text;
                        cmd.CommandText = "Select * from interviewee order by id DESC";
                        cmd.Connection = connection;

                        MySqlDataReader reader = cmd.ExecuteReader();

                        while (reader.Read())
                        {
                            intervieweeId = Convert.ToInt32(reader.GetValue(0));
                            break;
                        }

                        reader.Close();
                    }
                }
            }
            catch (MySqlException ex)
            {
                Console.WriteLine(ex.Message);
            }

            try
            {
                using (var connection = new MySqlConnection(connectionString))
                {
                    connection.Open();

                    using (var cmd = new MySqlCommand())
                    {
                        foreach(var extraNote in extraNotes)
                        {
                            Debug.WriteLine(extraNote.note);
                            cmd.CommandType = System.Data.CommandType.Text;
                            cmd.CommandText = "insert into extranotes (intervieweeid, columnname, note) " +
                                "values (@intervieweeid, @columnname, @note)";

                            cmd.Connection = connection;

                            if(extraNote.intervieweeid != -1)
                            {
                                cmd.Parameters.AddWithValue("@intervieweeid", extraNote.intervieweeid);
                            }
                            else
                            {
                                cmd.Parameters.AddWithValue("@intervieweeid", intervieweeId);
                            }
                            cmd.Parameters.AddWithValue("@columnname", extraNote.columnname);
                            cmd.Parameters.AddWithValue("@note", extraNote.note);

                            cmd.ExecuteNonQuery();

                            cmd.Parameters.Clear();
                        }
                    }
                }
            }
            catch (MySqlException ex)
            {
                Debug.WriteLine(ex.Message);
            }
        }
        


        [HttpPut]
        [ActionName("PutExtraNote")]
        public void EditExtraNote(List<ExtraNote> extraNotes)
        {
            List<int> listOfIds = new List<int>();
            try
            {

                using (var connection = new MySqlConnection(connectionString))
                {
                    connection.Open();

                    // getting the extra columns
                    if(extraNotes.Count > 0)
                    {
                        using (var cmd = new MySqlCommand())
                        {
                            cmd.CommandType = System.Data.CommandType.Text;
                            cmd.CommandText = "Select * from extranotes where intervieweeid = " + extraNotes[0].intervieweeid;
                            cmd.Connection = connection;

                            MySqlDataReader reader = cmd.ExecuteReader();

                            while (reader.Read())
                            {
                                ExtraNote extraNote = new ExtraNote();

                                extraNote.id = Convert.ToInt32(reader.GetValue(0));
                                extraNote.intervieweeid = Convert.ToInt32(reader.GetValue(1));
                                extraNote.columnname = reader.GetValue(2).ToString();
                                extraNote.note = reader.GetValue(3).ToString();
                                
                                listOfIds.Add(extraNote.id);
                            }

                            reader.Close();
                        }
                    }                 

                    // so the extra column for this interviewee is new
                    // if interviewee is created without this extra column we will create
                    // extrar columns for this interviewee with placeholder values
                    if (listOfIds.Count() < extraNotes.Count())
                    {
                        int difference = extraNotes.Count() - listOfIds.Count();

                        // how many extra column the interviewee lack
                        for(int i = 0; i < difference; ++i)
                        {                   
                            // Here I need to add a new extra notes to this particular interviewee of which is the object to be edited                           
                            using (var connectionExtraNote = new MySqlConnection(connectionString))
                            {
                                connectionExtraNote.Open();

                                using (var cmdExtraNote = new MySqlCommand())
                                {
                                    cmdExtraNote.CommandType = System.Data.CommandType.Text;
                                    cmdExtraNote.CommandText = "insert into extranotes (intervieweeid, columnname, note) " +
                                        "values (@intervieweeid, @columnname, @note)";

                                    cmdExtraNote.Connection = connection;

                                    int intervieweeId = extraNotes[0].intervieweeid;
                                    string columnname = extraNotes[0].columnname;

                                    cmdExtraNote.Parameters.AddWithValue("@intervieweeid", intervieweeId);
                                    cmdExtraNote.Parameters.AddWithValue("@columnname", columnname);
                                    cmdExtraNote.Parameters.AddWithValue("@note", "--"); // placeholder value untill it is edited

                                    Debug.WriteLine("placeholder added for interviewee witd id of " + intervieweeId);
                                    cmdExtraNote.ExecuteNonQuery();


                                }
                            }

                        }


                        // now we have as much extra columns as needed to go further into this operation
                        // we need to fill the list of ids again, and before doing this, we msut empty the lsit first
                        listOfIds.Clear();
                        using (var cmd = new MySqlCommand())
                        {
                            cmd.CommandType = System.Data.CommandType.Text;
                            cmd.CommandText = "Select * from extranotes where intervieweeid = " + extraNotes[0].intervieweeid;
                            cmd.Connection = connection;

                            MySqlDataReader reader = cmd.ExecuteReader();

                            while (reader.Read())
                            {
                                ExtraNote extraNote = new ExtraNote();

                                extraNote.id = Convert.ToInt32(reader.GetValue(0));
                                extraNote.intervieweeid = Convert.ToInt32(reader.GetValue(1));
                                extraNote.columnname = reader.GetValue(2).ToString();
                                extraNote.note = reader.GetValue(3).ToString();

                                listOfIds.Add(extraNote.id);
                            }

                            cmd.Parameters.Clear();
                            reader.Close();
                        }

                    }

                    // now we have the listOfIds

                    // editing
                    using (var cmd = new MySqlCommand())
                    {
                        int index = 0;

                        foreach (var extraNote in extraNotes)
                        {
                            int noteIndex = listOfIds.ElementAt(index);

                            cmd.CommandType = System.Data.CommandType.Text;
                            cmd.CommandText = "update extranotes set " +
                                              "columnname = @columnname ," +
                                              "note = @note " +
                                              "where id = @id";

                            cmd.Connection = connection;

                            cmd.Parameters.AddWithValue("@columnname", extraNote.columnname);
                            cmd.Parameters.AddWithValue("@note", extraNote.note);
                            cmd.Parameters.AddWithValue("@id", noteIndex);

                            cmd.ExecuteNonQuery();

                            cmd.Parameters.Clear();
                            index++;
                        }

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
