using TotalModel;
using TotalModel.Models;

using TotalDTO;
using TotalDTO.Commons;

namespace TotalCore.Services.Commons
{
    public interface ICommodityService<TDto, TPrimitiveDto> : IGenericService<Commodity, TDto, TPrimitiveDto>
        where TDto : class, TPrimitiveDto
        where TPrimitiveDto : BaseDTO, IPrimitiveEntity, IPrimitiveDTO, new()
    {
    }

    public interface IMaterialService : ICommodityService<CommodityDTO<CMDMaterial>, CommodityPrimitiveDTO<CMDMaterial>> { }
    public interface IItemService : ICommodityService<CommodityDTO<CMDItem>, CommodityPrimitiveDTO<CMDItem>> { }
    public interface IProductService : ICommodityService<CommodityDTO<CMDProduct>, CommodityPrimitiveDTO<CMDProduct>> { }
}