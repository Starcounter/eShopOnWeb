using System.Collections.Generic;
using System.Threading.Tasks;

namespace ApplicationCore.Interfaces
{
    public interface IBasketService
    {
        Task<int> GetBasketItemCountAsync(string userName);
        Task TransferBasketAsync(string anonymousId, string userName);
        Task AddItemToBasket(ulong basketId, ulong catalogItemId, decimal price, int quantity);
        Task SetQuantities(ulong basketId, Dictionary<string, int> quantities);
        Task DeleteBasketAsync(ulong basketId);
    }
}
