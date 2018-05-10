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

        public void AddStudent(Student newStudent)
        {
          MySqlConnection conn = DB.Connection();
                conn.Open();
                var cmd = conn.CreateCommand() as MySqlCommand;
                cmd.CommandText = @"INSERT INTO courses_students (course_id, student_id) VALUES (@CourseId, @StudentId);";

                MySqlParameter student_id = new MySqlParameter();
                student_id.ParameterName = "@StudentId";
                student_id.Value = newStudent.GetId();
                cmd.Parameters.Add(student_id);

                MySqlParameter course_id = new MySqlParameter();
                course_id.ParameterName = "@CourseId";
                course_id.Value = _id;
                cmd.Parameters.Add(course_id);

                cmd.ExecuteNonQuery();
                conn.Close();
                if (conn != null)
                {
                    conn.Dispose();
                }

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

        public List<Student> GetStudents()
        {
            MySqlConnection conn = DB.Connection();
            conn.Open();
            var cmd = conn.CreateCommand() as MySqlCommand;
            cmd.CommandText = @"SELECT students.* FROM courses
            JOIN courses_students ON (courses.id = courses_students.course_id)
            JOIN students ON (courses_students.student_id = students.id)
            WHERE courses.id = @CourseId;";
            // @"SELECT student_id FROM courses_students WHERE course_id = @CourseId;";

            MySqlParameter courseIdParameter = new MySqlParameter();
            courseIdParameter.ParameterName = "@CourseId";
            courseIdParameter.Value = _id;
            cmd.Parameters.Add(courseIdParameter);

            MySqlDataReader rdr = cmd.ExecuteReader() as MySqlDataReader;
            List<Student> students = new List<Student>{};

            while(rdr.Read())
            {
              int studentId = rdr.GetInt32(0);
              string studentName = rdr.GetString(1);
              Student newStudent = new Student(studentName, studentId);
              students.Add(newStudent);
            }

            conn.Close();
            if (conn != null)
            {
              conn.Dispose();
            }
            return students;
        }

            // var rdr = cmd.ExecuteReader() as MySqlDataReader;




        //     List<int> studentIds = new List<int> {};
        //     while(rdr.Read())
        //     {
        //         int studentId = rdr.GetInt32(0);
        //         studentIds.Add(studentId);
        //     }
        //     rdr.Dispose();
        //
        //     List<Student> students = new List<Student> {};
        //     foreach (int studentId in studentIds)
        //     {
        //       var studentQuery = conn.CreateCommand() as MySqlCommand;
        //       studentQuery.CommandText = @"SELECT * FROM students WHERE id = @StudentId;";
        //
        //       MySqlParameter studentIdParameter = new MySqlParameter();
        //       studentIdParameter.ParameterName = "@StudentId";
        //       studentIdParameter.Value = studentId;
        //       studentQuery.Parameters.Add(studentIdParameter);
        //
        //       var studentQueryRdr = studentQuery.ExecuteReader() as MySqlDataReader;
        //       while(studentQueryRdr.Read())
        //       {
        //           int thisStudentId = studentQueryRdr.GetInt32(0);
        //           string studentName = studentQueryRdr.GetString(1);
        //           Student foundStudent = new Student(studentName, thisStudentId);
        //           students.Add(foundStudent);
        //       }
        //       studentQueryRdr.Dispose();
        //   }
        //   conn.Close();
        //   if (conn != null)
        //   {
        //       conn.Dispose();
        //   }
        //   return students;
        // }

        public void Delete()
        {
          MySqlConnection conn = DB.Connection();
          conn.Open();
          var cmd = conn.CreateCommand() as MySqlCommand;
          cmd.CommandText = @"DELETE FROM courses WHERE id = @CourseId; DELETE FROM courses_students WHERE course_id = @CourseId;";

          MySqlParameter courseIdParameter = new MySqlParameter();
          courseIdParameter.ParameterName = "@CourseId";
          courseIdParameter.Value = this.GetId();
          cmd.Parameters.Add(courseIdParameter);

          cmd.ExecuteNonQuery();
          if (conn != null)
          {
            conn.Close();
          }
        }

        public void Update(string newDescription)
        {
            MySqlConnection conn = DB.Connection();
            conn.Open();
            var cmd = conn.CreateCommand() as MySqlCommand;
            cmd.CommandText = @"UPDATE courses SET name = @newDescription WHERE id = @searchId;";

            MySqlParameter searchId = new MySqlParameter();
            searchId.ParameterName = "@searchId";
            searchId.Value = _id;
            cmd.Parameters.Add(searchId);

            MySqlParameter description = new MySqlParameter();
            description.ParameterName = "@newDescription";
            description.Value = newDescription;
            cmd.Parameters.Add(description);

            cmd.ExecuteNonQuery();
            _name = newDescription;
            conn.Close();
            if (conn != null)
            {
                conn.Dispose();
            }
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

        public static Course Find(int id)
        {
          MySqlConnection conn = DB.Connection();
          conn.Open();
          var cmd = conn.CreateCommand() as MySqlCommand;
          cmd.CommandText = @"SELECT * FROM courses WHERE id = (@searchId);";

          MySqlParameter searchId = new MySqlParameter();
          searchId.ParameterName = "@searchId";
          searchId.Value = id;
          cmd.Parameters.Add(searchId);

          var rdr = cmd.ExecuteReader() as MySqlDataReader;
          int courseId = 0;
          string courseName = "";

          while(rdr.Read())
          {
            courseId = rdr.GetInt32(0);
            courseName = rdr.GetString(1);
          }

          // Constructor below no longer includes a itemCategoryId parameter:
          Course newCourse = new Course(courseName, courseId);
          conn.Close();
          if (conn != null)
          {
              conn.Dispose();
          }

          return newCourse;
        }
      }
    }
