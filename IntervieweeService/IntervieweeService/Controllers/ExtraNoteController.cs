﻿using System;
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
    public class ExtraNoteController : ApiController
    {
        private readonly String connectionString = 
            ConfigurationManager.ConnectionStrings["SqlServerStr"].ConnectionString;

        [HttpGet]
        [ActionName("GetExtraNoteById")]
        public List<ExtraNote> Get()
        {
            List<ExtraNote> listOfExtraNotes = new List<ExtraNote>();

            try
            {
                using (var connection = new SqlConnection(connectionString))
                {
                    connection.Open();

                    using (var cmd = new SqlCommand())
                    {
                        cmd.CommandType = System.Data.CommandType.Text;
                        cmd.CommandText = "Select * from extranotes";
                        cmd.Connection = connection;

                        SqlDataReader reader = cmd.ExecuteReader();

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
            catch (SqlException ex)
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
                using (var connection = new SqlConnection(connectionString))
                {
                    connection.Open();

                    using (var cmd = new SqlCommand())
                    {
                        cmd.CommandType = System.Data.CommandType.Text;
                        cmd.CommandText = "Select * from interviewee order by id DESC";
                        cmd.Connection = connection;

                        SqlDataReader reader = cmd.ExecuteReader();

                        while (reader.Read())
                        {
                            intervieweeId = Convert.ToInt32(reader.GetValue(0));
                            break;
                        }

                        reader.Close();
                    }
                }
            }
            catch (SqlException ex)
            {
                Console.WriteLine(ex.Message);
            }

            try
            {
                using (var connection = new SqlConnection(connectionString))
                {
                    connection.Open();

                    using (var cmd = new SqlCommand())
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
            catch (SqlException ex)
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
                using (var connection = new SqlConnection(connectionString))
                {
                    connection.Open();

                    //getting
                    if(extraNotes.Count > 0)
                    {
                        using (var cmd = new SqlCommand())
                        {
                            cmd.CommandType = System.Data.CommandType.Text;
                            cmd.CommandText = "Select * from extranotes where intervieweeid = " + extraNotes[0].intervieweeid;
                            cmd.Connection = connection;

                            SqlDataReader reader = cmd.ExecuteReader();

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

                    // editing
                    using (var cmd = new SqlCommand())
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
            catch (SqlException ex)
            {
                Debug.WriteLine(ex.Message);
            }
        }
    }
}