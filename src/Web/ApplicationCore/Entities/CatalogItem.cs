namespace Microsoft.eShopWeb.ApplicationCore.Entities
{
    public class CatalogItem : BaseEntity
    {
        public virtual string Name { get; set; }
        public virtual string Description { get; set; }
        public virtual decimal Price { get; set; }
        public virtual string PictureUri { get; set; }
        public virtual int CatalogTypeId { get; set; }
        public virtual CatalogType CatalogType { get; set; }
        public virtual int CatalogBrandId { get; set; }
        public virtual CatalogBrand CatalogBrand { get; set; }
    }
}