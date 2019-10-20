using AutoMapper;
using MRKTPL.Data.Entities;
using MRKTPL.Data.ViewModel;

namespace MRKTPL.Repository.Mapping
{
    public class MappingsProfile: Profile
    {
        //public MappingsProfile()
        //{
        //    Mapper.Initialize(cfg =>
        //    {
        //        cfg.CreateMap<UserMaster, UserViewModel>().ReverseMap();
        //        cfg.CreateMap<RoleMaster, RoleViewModel>().ReverseMap();

        //        //cfg.CreateMap<CategoryMaster, CategoryViewModel>().ReverseMap();
        //        //cfg.CreateMap<ProductMaster, ProductViewModel>().ReverseMap();
        //        //cfg.CreateMap<ProductCategoryMapping, ProductCategoryModel>().ReverseMap();
        //        //cfg.CreateMap<RelatedProduct, RelatedProductViewModel>().ReverseMap();

        //    });

        //}
        public static void Initialize()
        {
            Mapper.Initialize(cfg =>
            {
                cfg.CreateMap<UserMaster, UserViewModel>().ReverseMap();
                cfg.CreateMap<RoleMaster, RoleViewModel>().ReverseMap();
                cfg.CreateMap<CategoryMaster, CategoryViewModel>().ReverseMap();
                cfg.CreateMap<ProductMaster, ProductViewModel>().ReverseMap();
                cfg.CreateMap<ProductCategoryMapping, ProductCategoryModel>().ReverseMap();
                cfg.CreateMap<RelatedProduct, RelatedProductViewModel>().ReverseMap();
            });
        }
    }
}
