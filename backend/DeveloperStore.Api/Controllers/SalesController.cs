using DeveloperStore.API.DTOs;
using DeveloperStore.Application.UseCases.Sales;
using DeveloperStore.Domain.Entities;
using Microsoft.AspNetCore.Mvc;

namespace DeveloperStore.Api.Controllers
{
    [ApiController]
    [Route("api/sales")]
    public class SalesController : ControllerBase
    {
        private readonly CreateSaleUseCase _createSaleUseCase;
        private readonly GetSaleByIdUseCase _getSaleByIdUseCase;
        private readonly GetAllSalesUseCase _getAllSalesUseCase;
        private readonly UpdateSaleUseCase _updateSaleUseCase;
        private readonly CancelSaleUseCase _cancelSaleUseCase;

        public SalesController(
            CreateSaleUseCase createSaleUseCase,
            GetSaleByIdUseCase getSaleByIdUseCase,
            GetAllSalesUseCase getAllSalesUseCase,
            UpdateSaleUseCase updateSaleUseCase,
            CancelSaleUseCase cancelSaleUseCase)
        {
            _createSaleUseCase = createSaleUseCase;
            _getSaleByIdUseCase = getSaleByIdUseCase;
            _getAllSalesUseCase = getAllSalesUseCase;
            _updateSaleUseCase = updateSaleUseCase;
            _cancelSaleUseCase = cancelSaleUseCase;
        }

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var sales = await _getAllSalesUseCase.ExecuteAsync();
            return Ok(sales);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(Guid id)
        {
            var sale = await _getSaleByIdUseCase.ExecuteAsync(id);
            if (sale == null)
                return NotFound();

            return Ok(sale);
        }

        [HttpPost]
        public async Task<IActionResult> Create([FromBody] SaleDto saleDto)
        {
            var sale = new Sale(saleDto.SaleNumber, saleDto.Customer, saleDto.Branch, saleDto.Date);

            foreach (var itemDto in saleDto.Items)
            {
                var item = new SaleItem(
                    product: itemDto.Product,
                    quantity: itemDto.Quantity,
                    unitPrice: itemDto.UnitPrice
                )
                {
                    SaleId = sale.Id
                };
                
                sale.AddItem(item);
            }

            var createdSale = await _createSaleUseCase.ExecuteAsync(sale);

            return CreatedAtAction(nameof(GetById), new { id = createdSale.Id }, createdSale);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Update(Guid id, SaleDto saleDto)
        {
            if (id != saleDto.Id)
                return BadRequest("Id da URL diferente do Id da venda.");

            var sale = new Sale((Guid)saleDto.Id, saleDto.SaleNumber, saleDto.Customer, saleDto.Branch, saleDto.Date);

            foreach (var itemDto in saleDto.Items)
            {
                var saleItem = new SaleItem(
                    itemDto.Product,
                    itemDto.Quantity,
                    itemDto.UnitPrice
                )
                {
                    Id = itemDto.Id
                };

                sale.AddItem(saleItem);
            }

            var updatedSale = await _updateSaleUseCase.ExecuteAsync(sale);
            return Ok(updatedSale);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Cancel(Guid id)
        {
            var cancelledSale = await _cancelSaleUseCase.ExecuteAsync(id);
            return Ok(cancelledSale);
        }
    }
}
