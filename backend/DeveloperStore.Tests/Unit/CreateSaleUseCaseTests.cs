using DeveloperStore.Application.UseCases.Sales;
using DeveloperStore.Domain.Entities;
using DeveloperStore.Domain.Interfaces;
using Moq;
using Xunit;

namespace DeveloperStore.Tests.Unit
{
    public class CreateSaleUseCaseTests
    {
        private readonly Mock<ISaleRepository> _mockRepo;
        private readonly CreateSaleUseCase _useCase;

        public CreateSaleUseCaseTests()
        {
            _mockRepo = new Mock<ISaleRepository>();
            _useCase = new CreateSaleUseCase(_mockRepo.Object);
        }

        [Fact]
        public async Task Should_Create_Sale_With_Discounts_Correctly()
        {
            // Arrange
            var sale = new Sale("123", "John", "Main", DateTime.Now);
            sale.AddItem(new SaleItem("Product A", 10, 100m)); 

            _mockRepo.Setup(r => r.AddAsync(It.IsAny<Sale>())).Returns(Task.CompletedTask);

            // Act
            var result = await _useCase.ExecuteAsync(sale);

            // Assert
            Assert.Equal(0.2m, result.Items[0].Discount); 
            Assert.Equal(800m, result.Items[0].Total);
            _mockRepo.Verify(r => r.AddAsync(It.IsAny<Sale>()), Times.Once);
        }

        [Fact]
        public async Task Should_Throw_When_More_Than_20_Items()
        {
            // Arrange
            var sale = new Sale("124", "Alice", "Branch", DateTime.Now);
            sale.AddItem(new SaleItem("Product B", 25, 10m)); // 25 itens => erro

            // Act & Assert
            await Assert.ThrowsAsync<InvalidOperationException>(() => _useCase.ExecuteAsync(sale));
        }
    }
}
