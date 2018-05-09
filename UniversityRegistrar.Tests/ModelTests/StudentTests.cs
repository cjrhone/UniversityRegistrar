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
        }

        [TestMethod]
        public void GetAll_DbStartsEmpty_0()
        {
          //Arrange
          //Act

          int result = Student.GetAll().Count;

          //Assert
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
        public void Save_AssignsIdToObject_Id()
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
    }
}
