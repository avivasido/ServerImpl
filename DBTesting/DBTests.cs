using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Collections.Generic;
using Moq;
using System.Linq;
using Entities;
using System.Data.Entity;

namespace DBTesting
{
    [TestClass]
    public class DBTests
    {
        MedTrainDBContext _service;

        [TestInitialize]
        public void TestInitialize()
        {
            Mock<MedTrainDBContext> mock = EFMock.CreateMockForDbContext<MedTrainDBContext>();
            Mock<DbSet<User>> users = EFMock.CreateMockForDbSet<User>();
        }

        public static Mock<IDbSet<T>> GetMockedDbSet<T>(IList<T> fakeData) where T : class, new()
        {
            var data = fakeData.AsQueryable();

            var mockSet = new Mock<IDbSet<T>>();
            mockSet.As<IQueryable<T>>().Setup(m => m.Provider).Returns(data.Provider);
            mockSet.As<IQueryable<T>>().Setup(m => m.Expression).Returns(data.Expression);
            mockSet.As<IQueryable<T>>().Setup(m => m.ElementType).Returns(data.ElementType);
            mockSet.As<IQueryable<T>>().Setup(m => m.GetEnumerator()).Returns(data.GetEnumerator());

            return mockSet;
        }

        [TestMethod]
        public void TestAddUser()
        {
            var data = new List<User> 
            { 
                new User { UserId = "a@a.a", userPassword = "p", userMedicalTraining = "mt", userFirstName = "fn", userLastName = "ln" } 
            }.AsQueryable();

            var mockSet = new Mock<DbSet<User>>();
            mockSet.As<IQueryable<User>>().Setup(m => m.Provider).Returns(data.Provider);
            mockSet.As<IQueryable<User>>().Setup(m => m.Expression).Returns(data.Expression);
            mockSet.As<IQueryable<User>>().Setup(m => m.ElementType).Returns(data.ElementType);
            mockSet.As<IQueryable<User>>().Setup(m => m.GetEnumerator()).Returns(data.GetEnumerator());

            var mockContext = new Mock<MedTrainDBContext>();
            mockContext.Setup(c => c.Users).Returns(mockSet.Object);

            var service = new MedTrainService(mockContext.Object);
            var user = service.getUser("a@a.a");

            Assert.IsTrue(user != null);
        }
    }
}
