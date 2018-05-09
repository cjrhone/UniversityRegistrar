using System.Collections.Generic;
using MySql.Data.MySqlClient;
using System;

namespace UniversityRegistrar.Models
{
    public class Course
    {
        private int _id;
        private string _name;

        public Course(string name, int id=0)
        {
          _name = name;
          _id = id;

        }

        public int GetId()
        {
          return _id;
        }

        public string GetName()
        {
          return _name;
        }

        public static List<Course> GetAll()
        {
          List<Course> allCourses = new List<Course> {};
          MySqlConnection conn = DB.Connection();

          conn.Open();

          MySqlCommand cmd = conn.CreateCommand() as MySqlCommand;

          cmd.CommandText = @"SELECT * FROM courses;";
          MySqlDataReader rdr = cmd.ExecuteReader() as MySqlDataReader;

          while(rdr.Read())
          {
            int courseId = rdr.GetInt32(0);
            string courseName = rdr.GetString(1);

            Course newCourse = new Course(courseName, courseId);

            allCourses.Add(newCourse);
          }

          conn.Close();
          if (conn != null)
          {
            conn.Dispose();
          }
          return allCourses;
        }

        public static void DeleteAll()
        {
          MySqlConnection conn = DB.Connection();
          conn.Open();

          var cmd = conn.CreateCommand() as MySqlCommand;
          cmd.CommandText = @"DELETE FROM courses;";

          cmd.ExecuteNonQuery();

          conn.Close();
          if (conn != null)
          {
            conn.Dispose();
          }
        }

        public override bool Equals(System.Object otherCourse)
        {
          if (!(otherCourse is Course))
          {
            return false;
          }
          else
          {
            Course newCourse = (Course) otherCourse;
            bool idEquality = (this.GetId() == newCourse.GetId());
            //when we change an object from one type to another, its called "TYPE CASTING"
            bool nameEquality = (this.GetName() == newCourse.GetName());
            return (idEquality && nameEquality);
          }
        }

        public void Save()
        {
          MySqlConnection conn = DB.Connection();
          conn.Open();

          var cmd = conn.CreateCommand() as MySqlCommand;
          cmd.CommandText = @"INSERT INTO courses (name) VALUES (@name);";


          cmd.Parameters.Add(new MySqlParameter("@name", _name));


          cmd.ExecuteNonQuery();
          _id = (int) cmd.LastInsertedId;

          conn.Close();
          if (conn != null)
          {
              conn.Dispose();
          }
        }
      }
    }
