using Microsoft.Extensions.Configuration;
using MRKTPL.Data.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace MRKTPL.Services.ProductCategoryServices
{
   public class ProductCategoryServices:IProductCategoryServices
    {
        #region Fields
        private readonly MarketPlaceCoreContext context;
        private readonly IConfiguration configuration;
        #endregion
        #region CTor
        public ProductCategoryServices(MarketPlaceCoreContext _context, IConfiguration _configuration)
        {
            context = _context;
            configuration = _configuration;
        }
        #endregion
        #region Methods
        public virtual ProductCategoryMapping GetProductCategoryById(int productCategoryId)
        {
            if (productCategoryId == 0)
                return null;

            return context.ProductCategoryMapping.Find(productCategoryId);
        }
        #endregion
    }
}
