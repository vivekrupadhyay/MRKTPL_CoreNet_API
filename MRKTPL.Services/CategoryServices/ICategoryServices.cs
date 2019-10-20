using MRKTPL.Data.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace MRKTPL.Services.CategoryServices
{
   public interface ICategoryServices
    {
        bool CheckCategoryExits(string CategoryName);
        void DeleteCategory(int id);
        IList<CategoryMaster> GetAllCategories();
        CategoryMaster GetCategoryById(int categoryId);
        void InsertCategory(CategoryMaster category);
        void UpdateCategory(CategoryMaster category);
        List<CategoryMaster> GetCategoriesByIds(long[] categoryIds);
        IList<CategoryMaster> GetCategoryBreadCrumb(CategoryMaster category, IList<CategoryMaster> allCategories = null, bool showHidden = false);
        //IList<CategoryMaster> GetAllCategoriesByParentCategoryId(int parentCategoryId, bool showHidden = false);

    }
}
