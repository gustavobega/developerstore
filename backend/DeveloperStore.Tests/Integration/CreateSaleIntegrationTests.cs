using DeveloperStore.Application.UseCases.Sales;
using DeveloperStore.Domain.Entities;
using DeveloperStore.Infrastructure;
using DeveloperStore.Infrastructure.Repositories;
using Microsoft.EntityFrameworkCore;
using Xunit;

public class CreateSaleIntegrationTests
{
    private DeveloperStoreDbContext GetDbContext()
    {
        var options = new DbContextOptionsBuilder<DeveloperStoreDbContext>()
            .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
            .Options;

        return new DeveloperStoreDbContext(options);
    }

    [Fact]
    public async Task Should_Create_Sale_And_Apply_Discounts()
    {
        // Arrange
        var dbContext = GetDbContext();
        var repository = new SaleRepository(dbContext);
        var useCase = new CreateSaleUseCase(repository);

        var sale = new Sale("1001", "John Doe", "Main Branch", DateTime.Now);
        sale.AddItem(new SaleItem("Product A", 5, 100m));
        sale.AddItem(new SaleItem("Product B", 12, 50m));

        // Act
        var result = await useCase.ExecuteAsync(sale);

        // Assert
        Assert.Equal(5, result.Items[0].Quantity);
        Assert.Equal(0.1m, result.Items[0].Discount);
        Assert.Equal(12, result.Items[1].Quantity);
        Assert.Equal(0.2m, result.Items[1].Discount);

        var savedSale = await dbContext.Sales.Include(s => s.Items).FirstOrDefaultAsync();
        Assert.NotNull(savedSale);
        Assert.Equal(result.Id, savedSale!.Id);
    }
}
