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

    public void Delete()
    {
      MySqlConnection conn = DB.Connection();
      conn.Open();
      var cmd = conn.CreateCommand() as MySqlCommand;
      cmd.CommandText = @"DELETE FROM students WHERE id = @StudentId; DELETE FROM courses_students WHERE student_id = @StudentId;";

      MySqlParameter studentIdParameter = new MySqlParameter();
      studentIdParameter.ParameterName = "@StudentId";
      studentIdParameter.Value = this.GetId();
      cmd.Parameters.Add(studentIdParameter);

      cmd.ExecuteNonQuery();
      if (conn != null)
      {
        conn.Close();
      }
    }

    public static void DeleteAll()
    {
      MySqlConnection conn = DB.Connection();
      conn.Open();
      var cmd = conn.CreateCommand() as MySqlCommand;
      cmd.CommandText = @"DELETE FROM students;";
      cmd.ExecuteNonQuery();

      conn.Close();
      if (conn != null)
      {
        conn.Dispose();
      }
    }

    public override bool Equals(System.Object otherStudent)
    {
      if (!(otherStudent is Student))
      {
        return false;
      }
      else
      {
        Student newStudent = (Student) otherStudent;
        bool idEquality = (this.GetId() == newStudent.GetId());
        bool nameEquality = (this.GetName() == newStudent.GetName());
        return (idEquality && nameEquality);
      }
    }

    public void Save()
    {
      MySqlConnection conn = DB.Connection();
      conn.Open();

      var cmd = conn.CreateCommand() as MySqlCommand;
      cmd.CommandText = @"INSERT INTO students (name) VALUES (@name);";

      MySqlParameter name = new MySqlParameter();
      name.ParameterName = "@name";
      name.Value = _name;
      cmd.Parameters.Add(name);

      cmd.ExecuteNonQuery();
      _id = (int) cmd.LastInsertedId;

      conn.Close();
      if (conn != null)
      {
        conn.Dispose();
      }
    }

    public void AddCourse(Course newCourse)
    {
      MySqlConnection conn = DB.Connection();
            conn.Open();
            var cmd = conn.CreateCommand() as MySqlCommand;
            cmd.CommandText = @"INSERT INTO courses_students (course_id, student_id) VALUES (@CourseId, @StudentId);";

            MySqlParameter course_id = new MySqlParameter();
            course_id.ParameterName = "@CourseId";
            course_id.Value = newCourse.GetId();
            cmd.Parameters.Add(course_id);

            MySqlParameter student_id = new MySqlParameter();
            student_id.ParameterName = "@StudentId";
            student_id.Value = _id;
            cmd.Parameters.Add(student_id);

            cmd.ExecuteNonQuery();
            conn.Close();
            if (conn != null)
            {
                conn.Dispose();
            }

    }

    public List<Course> GetCourses()
    {
      MySqlConnection conn = DB.Connection();
          conn.Open();
          var cmd = conn.CreateCommand() as MySqlCommand;
          cmd.CommandText = @"SELECT course_id FROM courses_students WHERE student_id = @StudentId;";

          MySqlParameter studentIdParameter = new MySqlParameter();
          studentIdParameter.ParameterName = "@StudentId";
          studentIdParameter.Value = _id;
          cmd.Parameters.Add(studentIdParameter);

          var rdr = cmd.ExecuteReader() as MySqlDataReader;

          List<int> courseIds = new List<int> {};
          while(rdr.Read())
          {
              int courseId = rdr.GetInt32(0);
              courseIds.Add(courseId);
          }
          rdr.Dispose();

          List<Course> courses = new List<Course> {};
          foreach (int courseId in courseIds)
          {
              var courseQuery = conn.CreateCommand() as MySqlCommand;
              courseQuery.CommandText = @"SELECT * FROM courses WHERE id = @CategoryId;";

              MySqlParameter courseIdParameter = new MySqlParameter();
              courseIdParameter.ParameterName = "@CategoryId";
              courseIdParameter.Value = courseId;
              courseQuery.Parameters.Add(courseIdParameter);

              var categoryQueryRdr = courseQuery.ExecuteReader() as MySqlDataReader;
              while(categoryQueryRdr.Read())
              {
                  int thisCourseId = categoryQueryRdr.GetInt32(0);
                  string courseName = categoryQueryRdr.GetString(1);
                  Course foundCourse = new Course(courseName, thisCourseId);
                  courses.Add(foundCourse);
              }
              categoryQueryRdr.Dispose();
          }
          conn.Close();
          if (conn != null)
          {
              conn.Dispose();
          }
          return courses;

    }
  }
}
