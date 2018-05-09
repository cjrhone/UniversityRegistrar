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
    }

    [TestMethod]
    public void GetAll_DbStartsEmpty_0()
    {
      //Arrange
      //Act

      int result = Course.GetAll().Count;

      //Assert
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
  }
}
