using Microsoft.eShopWeb.ApplicationCore.Entities;

namespace ApplicationCore.Specifications
{
    public class BasketWithItemsSpecification : BaseSpecification<Basket>
    {
        // todo IntId
        public BasketWithItemsSpecification(ulong basketId)
            :base(b => b.IntId == basketId)
        {
            AddInclude(b => b.Items);
        }
        public BasketWithItemsSpecification(string buyerId)
            :base(b => b.BuyerId == buyerId)
        {
            AddInclude(b => b.Items);
        }
    }
}
