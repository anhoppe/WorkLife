using Moq;
using NUnit.Framework;
using WorkLife.Model.Contract;

namespace WorkLife.Dal.Test
{
    [TestFixture]
    public class DbAccessTest
    {
        [Test]
        public void Connect_WhenCalled_ThenShouldReturnTrue()
        {
            // Arrange
            var sut = new DbAccess();

            // Act
            var result = sut.Connect();

            // Assert
            Assert.That(result);
        }

        [Test]
        public void LoadPersons_WhenDbConnected_ThenPersonsCanBeLoaded()
        { 
            // Arrange   
            var sut = new DbAccess();
            var result = sut.Connect();
            var personBuilder = MockPersonBuilder();

            // Act
            var persons = sut.LoadPersons(personBuilder.Object);

            // Assert
            personBuilder.Verify(m => m.Build(), Times.Exactly(5));
        }

        [Test]
        public void LoadPersons_WhenDbNotConnected_ThenPersonsIsEmpty()
        { 
            // Arrange   
            var sut = new DbAccess();
            var personBuilder = MockPersonBuilder();

            // Act
            var persons = sut.LoadPersons(personBuilder.Object);

            // Assert
            personBuilder.Verify(m => m.Build(), Times.Never);
        }

        [Test]
        public void LoadTarketTimeWeeks_WhenDbConnected_ThenWorkingWeeksCanBeLoaded()
        {
            // Arrange   
            var sut = new DbAccess();
            var result = sut.Connect();
            
            // Act
            var targetTimesWeek = sut.LoadTargetTimeWeeks();

            // Assert
            Assert.That(targetTimesWeek.Count, Is.EqualTo(15), "Expected the 15 working weeks from the DB");
        }

        private Mock<IPersonBuilder> MockPersonBuilder()
        {
            var personBuilder = new Mock<IPersonBuilder>();

            personBuilder.Setup(m => m.WithName(It.IsAny<string>()))
                .Returns(personBuilder.Object);
            personBuilder.Setup(m => m.WithId(It.IsAny<int>()))
                .Returns(personBuilder.Object);
            personBuilder.Setup(m => m.WithEmployeeId(It.IsAny<string>()))
                .Returns(personBuilder.Object);
            personBuilder.Setup(m => m.WithTargetTimeModel(It.IsAny<DateOnly>(), It.IsAny<TargetTimeModel>()))
                .Returns(personBuilder.Object);

            return personBuilder;
        }
    }
}
