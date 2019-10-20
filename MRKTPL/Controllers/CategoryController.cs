using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MRKTPL.Data.Entities;
using MRKTPL.Data.ViewModel;
using MRKTPL.Services.CategoryServices;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Security.Claims;



namespace MRKTPL.Controllers
{
    [Route("api/[controller]")]
    [Authorize]
    public class CategoryController : Controller
    {
        #region Field
        private readonly ICategoryServices _categoryServices;
        private readonly IMapper _mapper;

        #endregion
        #region CTor
        public CategoryController(ICategoryServices categoryServices, IMapper mapper)
        {
            _categoryServices = categoryServices;
            _mapper = mapper;
        }
        #endregion
        #region Create / Edit / Delete
        [Authorize]
        [HttpPost("CreateCategory")]
        public HttpResponseMessage Create([FromBody] CategoryViewModel categoryVM)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    if (_categoryServices.CheckCategoryExits(categoryVM.CategoryTitle))
                    {
                        HttpResponseMessage responce = new HttpResponseMessage()
                        {
                            StatusCode = System.Net.HttpStatusCode.Conflict
                        };
                        return responce;
                    }
                    else
                    {
                        string userId = User.FindFirstValue(ClaimTypes.Name);
                        CategoryMaster category = _mapper.Map<CategoryMaster>(categoryVM);
                        category.IsActive = true;
                        category.CreatedDate = DateTime.Now;
                        category.CreatedBy = Convert.ToInt32(userId);
                        if (category != null)
                        {
                            _categoryServices.InsertCategory(category);
                        }
                        HttpResponseMessage responce = new HttpResponseMessage()
                        {
                            StatusCode = System.Net.HttpStatusCode.OK
                        };
                        return responce;
                    }
                }
                else
                {
                    HttpResponseMessage responce = new HttpResponseMessage()
                    {
                        StatusCode = System.Net.HttpStatusCode.BadRequest
                    };
                    return responce;
                }
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }
        [HttpGet]
        [Route("getAllCategory")]
        public IActionResult GetAll()
        {
            IEnumerable<CategoryMaster> category = _categoryServices.GetAllCategories();
            List<CategoryViewModel> roleVM = new List<CategoryViewModel>();

            try
            {
                if (category != null)
                {
                    foreach (CategoryMaster r in category)
                    {
                        MapperConfiguration config = new MapperConfiguration(cfg =>
                        {
                            cfg.CreateMap<CategoryMaster, CategoryViewModel>();
                        });
                        IMapper mapper = config.CreateMapper();
                        CategoryViewModel dest = Mapper.Map<CategoryMaster, CategoryViewModel>(r);
                        roleVM.Add(dest);
                    }
                }
                return Ok(roleVM);
            }
            catch (Exception ex)
            {

                throw ex;
            }

        }
        [HttpPut("{id}")]
        public IActionResult UpdateRole(int id, [FromBody] CategoryViewModel categoryVM)
        {
            try
            {
                CategoryMaster category = _mapper.Map<CategoryMaster>(categoryVM);
                category.CategoryID = id;
                category.ModifiedDate = DateTime.UtcNow;
                _categoryServices.UpdateCategory(category);
                return Ok();
            }
            catch (Exception ex)
            {

                return BadRequest(new { message = ex.Message });
            }
        }
        [HttpDelete("{id}")]
        public IActionResult DeleteRole(int id)
        {
            try
            {
                _categoryServices.DeleteCategory(id);
                return Ok();
            }
            catch (Exception ex)
            {

                return BadRequest(new { message = ex.Message });
            }
        }
        #endregion
        #region Utility
        public IActionResult GetFormattedBreadCrumb(CategoryMaster category, IList<CategoryMaster> allCategories = null,
            string separator = ">>", int languageId = 0)
        {
            string result = string.Empty;
            IList<CategoryMaster> breadcrumb = _categoryServices.GetCategoryBreadCrumb(category, allCategories, true);
            for (int i = 0; i <= breadcrumb.Count - 1; i++)
            {
                CategoryMaster categoryName = breadcrumb[i];
                result = string.IsNullOrEmpty(result) ? categoryName : $"{result} {separator} {categoryName}";
                //result + separator + categoryName;

            }
            return Ok(result);
        }
        #endregion
    }
}
