using MRKTPL.Data.Entities;

namespace MRKTPL.Services.ProductCategoryServices
{

    public interface IProductCategoryServices
    {
       
        ProductCategoryMapping GetProductCategoryById(int productCategoryId);
    }
}
