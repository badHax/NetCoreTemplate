using Moq;
using NetCoreTemplate.BLL;
using NetCoreTemplate.DAL.API;
using NUnit.Framework;
using System.Threading.Tasks;
using Shouldly;

namespace NetCoreTemplate.Tests
{
    [TestFixture]
    public class AbstractToolTests
    {
        private MockRepository mockRepository;

        private Mock<DAL.API.IServiceProvider> mockServiceProvider;

        [SetUp]
        public void SetUp()
        {
            this.mockRepository = new MockRepository(MockBehavior.Strict);

            this.mockServiceProvider = this.mockRepository.Create<DAL.API.IServiceProvider>();
        }

        [TearDown]
        public void TearDown()
        {
            this.mockRepository.VerifyAll();
        }

        private AbstractTool CreateAbstractTool()
        {
            return new AbstractTool(
                this.mockServiceProvider.Object,
                ResourceManagementSystem.API1);
        }

        [Test]
        public async Task GetCustomerData_StateUnderTest_ExpectedBehavior()
        {
            // Arrange
            var abstractTool = this.CreateAbstractTool();
            string customerIdentifier = null;

            // Act
            var result = await abstractTool.GetCustomerData(
                customerIdentifier);

            // Assert
            result.ShouldNotBeNull();
        }
    }
}
