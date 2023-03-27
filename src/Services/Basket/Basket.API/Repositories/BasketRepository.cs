using System.Text.Json;
using Basket.API.Entities;
using System.Threading.Tasks;
using Microsoft.Extensions.Caching.Distributed;
using System;

namespace Basket.API.Repositories
{
    public class BasketRepository : IBasketRepository
    {
        private readonly IDistributedCache _redisCache;

        public BasketRepository(IDistributedCache redisCache)
        {
            _redisCache = redisCache ?? throw new ArgumentNullException(nameof(redisCache));
        }

        public async Task DeleteBasket(string userName)
        {
            await _redisCache.RemoveAsync(userName);
        }

        public async Task<ShoppingCart> GetBasket(string userName)
        {
            var basket = await _redisCache.GetStringAsync(userName);
            if (string.IsNullOrEmpty(basket)) 
            {
                return null;
            }
            return JsonSerializer.Deserialize<ShoppingCart>(basket);
        }

        public async Task<ShoppingCart> UpdateBasket(ShoppingCart cart)
        {
            await _redisCache.SetStringAsync(cart.UserName, JsonSerializer.Serialize<ShoppingCart>(cart));
            return await GetBasket(cart.UserName);
        }

    }
}
