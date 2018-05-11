using Microsoft.eShopWeb.ApplicationCore.Entities;
using Starcounter.Nova;

namespace ApplicationCore.Entities.BuyerAggregate
{
//    [Database]
    public class PaymentMethod : BaseEntity
    {
        public virtual string Alias { get; set; }
        public virtual string CardId { get; set; } // actual card data must be stored in a PCI compliant system, like Stripe
        public virtual string Last4 { get; set; }
    }
}
