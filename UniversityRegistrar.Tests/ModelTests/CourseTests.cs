using Microsoft.VisualStudio.TestTools.UnitTesting;
using UniversityRegistrar.Models;
using System.Collections.Generic;
using System;

namespace UniversityRegistrar.Tests
{
  [TestClass]
  public class CourseTests : IDisposable
  {

    public void Dispose()
    {
      Course.DeleteAll();
      Student.DeleteAll();
    }

    [TestMethod]
    public void GetAll_DbStartsEmpty_0()
    {

      int result = Course.GetAll().Count;

      Assert.AreEqual(0, result);
    }

    [TestMethod]
    public void Equals_ReturnsTrueIfNamesAreTheSame_Course()
    {
      Course firstCourse = new Course("Algebra");
      Course secondCourse = new Course("Algebra");


      Assert.AreEqual(firstCourse, secondCourse);
    }

    [TestMethod]
    public void Save_SavesToDatabase_FlightList()
    {
      Course testCourse = new Course("Algebra");

      testCourse.Save();

      List<Course> result = Course.GetAll();
      List<Course> testList = new List<Course>{testCourse};
      //Console.WriteLine("result " + result.GetFlightName());
      Console.WriteLine("testList " + testList.Count);


      CollectionAssert.AreEqual(testList, result);
    }

    [TestMethod]
    public void Save_AssignsCourseIdToObject_Id()
    {
      //Arrange
      Course testCourse = new Course("Algebra");
      Course testCourse2 = new Course("English");

      //Act
      testCourse.Save();
      testCourse2.Save();
      Course savedCourse = Course.GetAll()[1];

      int result = savedCourse.GetId();
      int testId = testCourse2.GetId();

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
    public void Delete_DeletesCourseAssociationsFromDatabase_CourseList()
    {
      //Arrange
      Student testStudent = new Student("James");
      testStudent.Save();

      string testName = "Algebra";
      Course testCourse = new Course(testName);
      testCourse.Save();

      //Act
      testCourse.AddStudent(testStudent);
      testCourse.Delete();

      List<Course> resultStudentCourses = testStudent.GetCourses();
      List<Course> testStudentCourses = new List<Course> {};

      //Assert
      CollectionAssert.AreEqual(testStudentCourses, resultStudentCourses);
    }
  }
}
