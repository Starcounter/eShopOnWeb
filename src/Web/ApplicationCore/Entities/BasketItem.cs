namespace Microsoft.eShopWeb.ApplicationCore.Entities
{
    public class BasketItem : BaseEntity
    {
        public virtual decimal UnitPrice { get; set; }
        public virtual int Quantity { get; set; }
        public virtual ulong CatalogItemId { get; set; }
        public virtual ulong BasketId { get; set; }
    }
}
