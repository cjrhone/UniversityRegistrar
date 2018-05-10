using Microsoft.VisualStudio.TestTools.UnitTesting;
using UniversityRegistrar.Models;
using System.Collections.Generic;
using System;

namespace UniversityRegistrar.Tests
{
      [TestClass]
      public class StudentTests : IDisposable
      {

        public void Dispose()
        {
          Student.DeleteAll();
          Course.DeleteAll();
        }

        [TestMethod]
        public void GetAll_DbStartsEmpty_0()
        {

          int result = Student.GetAll().Count;

          Assert.AreEqual(0, result);
        }

        [TestMethod]
        public void Equals_ReturnsTrueIfNamesAreTheSame_Student()
        {
          Student firstStudent = new Student("James");
          Student secondStudent = new Student("James");


          Assert.AreEqual(firstStudent, secondStudent);
        }

        [TestMethod]
        public void Save_SavesToDatabase_StudentList()
        {
          Student testStudent = new Student("James");

          testStudent.Save();
          List<Student> result = Student.GetAll();
          List<Student> testList = new List<Student>{testStudent};
          Console.WriteLine("result " + result.Count);
          Console.WriteLine("testList " + testList.Count);


          CollectionAssert.AreEqual(testList, result);
        }

        [TestMethod]
        public void Save_AssignsStudentIdToObject_Id()
        {
          //Arrange
          Student testStudent = new Student("James");
          Student testStudent2 = new Student("Alex");

          //Act
          testStudent.Save();
          testStudent2.Save();
          Student savedStudent = Student.GetAll()[1];

          int result = savedStudent.GetId();
          int testId = testStudent2.GetId();

          //Assert
          Assert.AreEqual(testId, result);
        }

        [TestMethod]
        public void AddCourse_AddsCoursetoStudents_CourseList()
        {
          //Arrange
          Student testStudent = new Student("James");
          testStudent.Save();

          Course testCourse = new Course("Algebra");
          testCourse.Save();

          //Act
          testStudent.AddCourse(testCourse);

          List<Course> result = testStudent.GetCourses();
          //CALL GetCourses()
          List<Course> testList = new List<Course>{testCourse};

          //Assert
          CollectionAssert.AreEqual(testList, result);
        }

        [TestMethod]
        public void GetCourses_ReturnsAllStudentsCourses_CoursesList()
        {
          //Arrange
          Student testStudent = new Student("James");
          testStudent.Save();

          Course testCourse1 = new Course("Algebra");
          testCourse1.Save();

          Course testCourse2 = new Course("English");
          testCourse2.Save();

          //Act
          testStudent.AddCourse(testCourse1);
          //CALL AddCourse()
          List<Course> result = testStudent.GetCourses();
          List<Course> testList = new List<Course> {testCourse1};

          //Assert
          CollectionAssert.AreEqual(testList, result);
        }

        [TestMethod]
        public void Delete_DeletesStudentAssociationsFromDatabase_StudentList()
        {
          //Arrange
          Course testCourse = new Course("Algebra");
          testCourse.Save();

          string testName = "James";
          Student testStudent = new Student(testName);
          testStudent.Save();

          //Act
          testStudent.AddCourse(testCourse);
          testStudent.Delete();
          //CALL Delete()

          List<Student> resultCourseStudents = testCourse.GetStudents();
          List<Student> testCourseStudents = new List<Student> {};

          //Assert
          CollectionAssert.AreEqual(testCourseStudents, resultCourseStudents);
        }

        [TestMethod]
        public void Find_FindsStudentInDatabase_Student()
        {
          //Arrange
          Student testStudent = new Student("Oprah");
          testStudent.Save();

          //Act
          Student result = Student.Find(testStudent.GetId());

          //Assert
          Assert.AreEqual(testStudent, result);
        }
    }
}
