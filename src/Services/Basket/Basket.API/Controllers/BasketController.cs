using Basket.API.Entities;
using Basket.API.Repositories;

using Microsoft.AspNetCore.Mvc;

using System.Threading.Tasks;

namespace Basket.API.Controllers
{
    [ApiController]
    [Route("api/v1/[controller]")]
    public class BasketController: ControllerBase
    {
        private readonly IBasketRepository _repository;

        public BasketController(IBasketRepository repository)
        {
            _repository = repository;
        }

        [HttpGet("{userName}")]
        [ProducesResponseType(typeof(ShoppingCart), 200)]
        public async Task<ActionResult<ShoppingCart>> GetBasket(string userName)
        {
            var basket = await _repository.GetBasket(userName);
            return Ok(basket ?? new ShoppingCart { UserName = userName });
        }

        [HttpPost]
        [ProducesResponseType(typeof(ShoppingCart), 200)]
        public async Task<ActionResult<ShoppingCart>> SetBasket(ShoppingCart basket)
        {
            var result = await _repository.UpdateBasket(basket);
            return Ok(result);
        }

        [HttpDelete]
        [ProducesResponseType(200)]
        public async Task<ActionResult> DeleteBasket(string userName)
        {
            await _repository.DeleteBasket(userName);
            return Ok();
        }
    }
}
