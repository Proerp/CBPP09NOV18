using TotalModel;
using TotalModel.Models;

using TotalDTO;
using TotalDTO.Commons;

using TotalCore.Repositories.Commons;
using TotalCore.Services.Commons;

namespace TotalService.Commons
{
    public class CommodityService<TDto, TPrimitiveDto> : GenericService<Commodity, TDto, TPrimitiveDto>, ICommodityService<TDto, TPrimitiveDto>
        where TDto : class, TPrimitiveDto
        where TPrimitiveDto : BaseDTO, IPrimitiveEntity, IPrimitiveDTO, new()
    {
        public CommodityService(ICommodityRepository commodityRepository)
            : base(commodityRepository, null, "CommoditySaveRelative")
        {
        }

    }

    public class MaterialService : CommodityService<CommodityDTO<CMDMaterial>, CommodityPrimitiveDTO<CMDMaterial>>, IMaterialService
    {
        public MaterialService(ICommodityRepository commodityRepository)
            : base(commodityRepository) { }
    }

    public class ItemService : CommodityService<CommodityDTO<CMDItem>, CommodityPrimitiveDTO<CMDItem>>, IItemService
    {
        public ItemService(ICommodityRepository commodityRepository)
            : base(commodityRepository) { }
    }

    public class ProductService : CommodityService<CommodityDTO<CMDProduct>, CommodityPrimitiveDTO<CMDProduct>>, IProductService
    {
        public ProductService(ICommodityRepository commodityRepository)
            : base(commodityRepository) { }
    }

}