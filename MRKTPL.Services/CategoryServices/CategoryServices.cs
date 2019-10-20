using Microsoft.Extensions.Configuration;
using MRKTPL.Data.Entities;
using MRKTPL.Data.ViewModel;
using System;
using System.Collections.Generic;
using System.Linq;

namespace MRKTPL.Services.CategoryServices
{
    public class CategoryServices : ICategoryServices
    {
        #region Fields
        private readonly MarketPlaceCoreContext context;
        private readonly IConfiguration configuration;
        #endregion
        #region CTor
        public CategoryServices(MarketPlaceCoreContext _context, IConfiguration _configuration)
        {
            context = _context;
            configuration = _configuration;
        }
        #endregion
        #region Method's
        public bool CheckCategoryExits(string CategoryName)
        {
            try
            {
                if (string.IsNullOrEmpty(CategoryName))
                {
                    throw new ArgumentException("Category Name cannot be null or empty string");
                }
                var query = (from c in context.Category
                             where c.CategoryTitle == CategoryName
                             select c).Count();
                return query > 0 ? true : false;
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }
        public void DeleteCategory(int id)
        {
            try
            {
                CategoryMaster category = GetCategoryById(id);
                
                if (category != null)
                {
                    context.Remove(category);
                    context.SaveChanges();
                }
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }
        public CategoryMaster GetCategoryById(int categoryId)
        {
            try
            {
                if (categoryId == 0)
                {
                    return null;
                }
                else
                {
                    return context.Category.Find(categoryId);
                }
            }
            catch (System.Exception ex)
            {

                throw ex;
            }
        }
        public IList<CategoryMaster> GetAllCategories()
        {
            try
            {
                List<CategoryMaster> query = (from c in context.Category select c).ToList();
                return query;
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }
        public void InsertCategory(CategoryMaster category)
        {
            try
            {
                context.Category.Add(category);
                context.SaveChanges();
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }
        public void UpdateCategory(CategoryMaster category)
        {
            try
            {
                context.Update(category);
                context.SaveChanges();
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }
        public List<CategoryMaster> GetCategoriesByIds(long[] categoryIds)
        {
            try
            {
                if (categoryIds == null || categoryIds.Length == 0)
                {
                    return new List<CategoryMaster>();
                }
                List<CategoryMaster> query = (from p in context.Category
                                              where categoryIds.Contains(p.CategoryID)
                                              && p.IsActive
                                              select p).ToList();
                return query;
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }
        public virtual IList<CategoryMaster> GetCategoryBreadCrumb(CategoryMaster category, IList<CategoryMaster> allCategories = null, bool showHidden = false)
        {
            try
            {
                if (category == null)
                {
                    throw new ArgumentNullException(nameof(category));
                }

                List<CategoryMaster> result = new List<CategoryMaster>();

                //used to prevent circular references
                List<int> alreadyProcessedCategoryIds = new List<int>();

                while (category != null && //not null
                    category.IsActive &&
                    !alreadyProcessedCategoryIds.Contains(Convert.ToInt32(category.CategoryID))) //prevent circular references
                {
                    result.Add(category);

                    alreadyProcessedCategoryIds.Add(Convert.ToInt32(category.CategoryID));

                    category = allCategories != null ? allCategories.FirstOrDefault(c => c.CategoryID == category.ParentCategoryId)
                        : GetCategoryById(category.ParentCategoryId);
                }

                result.Reverse();
                return result;
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }
        

        #endregion
    }
}
