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
    }
}
