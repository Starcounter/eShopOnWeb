using Starcounter.Nova;

namespace ApplicationCore.Entities.OrderAggregate
{
    /// <summary>
    /// Represents the item that was ordered. If catalog item details change, details of
    /// the item that was part of a completed order should not change.
    /// </summary>
    [Database]
    public class CatalogItemOrdered // ValueObject
    {
        public CatalogItemOrdered(ulong catalogItemId, string productName, string pictureUri)
        {
            CatalogItemId = catalogItemId;
            ProductName = productName;
            PictureUri = pictureUri;
        }

        public CatalogItemOrdered()
        {
            // required by EF
        }
        public virtual ulong CatalogItemId { get; set; }
        public virtual string ProductName { get; set; }
        public virtual string PictureUri { get; set; }
    }
}
