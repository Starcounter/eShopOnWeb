namespace Microsoft.eShopWeb.ViewModels
{
    public class BasketItemViewModel
    {
        public ulong Id { get; set; }
        public ulong CatalogItemId { get; set; }
        public string ProductName { get; set; }
        public decimal UnitPrice { get; set; }
        public decimal OldUnitPrice { get; set; }
        public int Quantity { get; set; }
        public string PictureUrl { get; set; }
    }
}
