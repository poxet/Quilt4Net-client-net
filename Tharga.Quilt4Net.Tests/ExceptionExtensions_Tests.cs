using System;
using NUnit.Framework;

namespace Tharga.Quilt4Net.Tests
{
    [TestFixture]
    public class ExceptionExtensions_Tests
    {
        [Test]
        public void When_adding_the_same_data_twice()
        {
            //Arrange
            var e = new Exception();
            e.AddData("A", "A1");

            //Act
            e.AddData("A", "A2");

            //Assert
            Assert.AreEqual("A2", e.Data["A"]);
        }
    }
}