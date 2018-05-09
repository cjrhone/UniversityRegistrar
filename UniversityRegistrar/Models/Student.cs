using System.Collections.Generic;
using System;
using MySql.Data.MySqlClient;

namespace UniversityRegistrar.Models
{
  public class Student
  {
    private int _id;
    private string _name;

    public Student (string name, int id=0)
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

    public static List<Student> GetAll()
    {
      List<Student> allStudents = new List<Student> {};
      MySqlConnection conn = DB.Connection();

      conn.Open();

      MySqlCommand cmd = conn.CreateCommand() as MySqlCommand;

      cmd.CommandText = @"SELECT * FROM students;";
      MySqlDataReader rdr = cmd.ExecuteReader() as MySqlDataReader;

      while(rdr.Read())
      {
        int studentId = rdr.GetInt32(0);
        string studentName = rdr.GetString(1);

        Student newStudent = new Student(studentName, studentId);

        allStudents.Add(newStudent);

      }

      conn.Close();
      if (conn != null)
      {
        conn.Dispose();
      }
      return allStudents;
    }

    public static void DeleteAll()
    {
      MySqlConnection conn = DB.Connection();
      conn.Open();
      //Creates conn object representing our connection to the database

      //manually opens the connection ( conn ) with conn.Open()
      var cmd = conn.CreateCommand() as MySqlCommand;
      cmd.CommandText = @"DELETE FROM students;";
      //Define cmd as --> creating command --> MySqlCommand... then...
      cmd.ExecuteNonQuery();
      //...Define CommandText property using SQL statement, which will clear the items table in our database

      //Executes SQL statements that modify data (like deletion)
      conn.Close();
      if (conn != null)
      //Finally, we make sure to close our connection with Close()...
      {
        conn.Dispose();
      }
    }
  }
}
