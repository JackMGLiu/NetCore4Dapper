using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using WebApiCore.Infrastructure.Data;
using WebApiCore.Model;
using WebApiCore.Repository;

namespace WebApiCore.Controllers
{
    [Produces("application/json")]
    [Route("api/Product")]
    public class ProductController : Controller
    {
        private readonly IUnitOfWorkFactory _uowFactory;
        private readonly IProductRepository _productRepository;

        public ProductController(IUnitOfWorkFactory uowFactory,IProductRepository productRepository)
        {
            _uowFactory = uowFactory;
            _productRepository = productRepository;
        }

        [HttpGet]
        public async Task<IActionResult> Get()
        {
            var data = await _productRepository.GetAllAsync();
            return Ok(data);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> Get(int id)
        {
            var data = await _productRepository.GetByIDAsync(id);
            return Ok(data);
        }

        [HttpPost]
        public async Task<IActionResult> Post([FromBody] Product prod)
        {
            /*
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            await _productRepository.AddAsync(prod);
            return NoContent();
            */
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            using (var uow = _uowFactory.Create())
            {
                await _productRepository.AddAsync(prod);
                uow.SaveChanges();
            }
            return NoContent();
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Put(int id, [FromBody] Product prod)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var model = await _productRepository.GetByIDAsync(id);
            model.Name = prod.Name;
            model.Quantity = prod.Quantity;
            model.Price = prod.Price;
            await _productRepository.UpdateAsync(model);

            return NoContent();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            await _productRepository.DeleteAsync(id);
            return NoContent();
        }

        [HttpGet("getlist")]
        public async Task<IActionResult> ProjectList()
        {
            return Ok("111222");
        }
    }
}