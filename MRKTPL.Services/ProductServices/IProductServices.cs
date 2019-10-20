using MRKTPL.Data.Entities;
using System;
using System.Collections.Generic;

namespace MRKTPL.Services.ProductServices
{
    public interface IProductServices
    {
        #region Products
        bool CheckProductExits(string productName);
        //ProductMaster GetProductById(int productId);
        //IList<ProductMaster> GetProductsByIds(int[] productIds);
        //void InsertProduct(ProductMaster product);
        //void UpdateProduct(ProductMaster product);
        //void UpdateProducts(IList<ProductMaster> products);
        //int GetNumberOfProductsInCategory(IList<int> categoryIds = null, int storeId = 0);
        //void UpdateProductReviewTotals(ProductMaster product);
        //bool ProductIsAvailable(ProductMaster product, DateTime? dateTime = null);
        //bool ProductTagExists(ProductMaster product, int productTagId);
        //int GetTotalStockQuantity(ProductMaster product, bool useReservedQuantity = true, int warehouseId = 0);
        //#endregion
        //#region Related Products
        //void DeleteRelatedProduct(RelatedProduct relatedProduct);
        //RelatedProduct GetRelatedProductById(int relatedProductId);
        //void InsertRelatedProduct(RelatedProduct relatedProduct);
        //RelatedProduct FindRelatedProduct(IList<RelatedProduct> source, int productId1, int productId2);
        #endregion
    }
}
