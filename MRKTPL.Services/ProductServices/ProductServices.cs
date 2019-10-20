using Microsoft.Extensions.Configuration;
using MRKTPL.Data.Entities;
using System;
using System.Linq;

namespace MRKTPL.Services.ProductServices
{
    public class ProductServices : IProductServices
    {
        #region Fields
        private readonly MarketPlaceCoreContext context;
        private readonly IConfiguration configuration;
        #endregion
        #region CTor
        public ProductServices(MarketPlaceCoreContext _context, IConfiguration _configuration)
        {
            context = _context;
            configuration = _configuration;
        }
        #endregion
        #region Method's
        public bool CheckProductExits(string productName)
        {
            try
            {
                if (string.IsNullOrEmpty(productName))
                {
                    throw new ArgumentException("produc tName cannot be null or empty string");
                }

                int Query = (from c in context.Products where c.ProductTitle == productName select c).Count();
                return Query > 0 ? true : false;

            }
            catch (Exception ex)
            {

                throw ex;
            }
        }
        #endregion
    }
}
