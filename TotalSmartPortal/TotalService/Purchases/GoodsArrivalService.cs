using System;
using System.Linq;
using System.Collections.Generic;
using System.Data.Entity.Core.Objects;

using System.Drawing;
using DataMatrix.net;

using TotalModel.Models;
using TotalDTO.Purchases;
using TotalCore.Repositories.Purchases;
using TotalCore.Services.Purchases;


namespace TotalService.Purchases
{
    public class GoodsArrivalService : GenericWithViewDetailService<GoodsArrival, GoodsArrivalDetail, GoodsArrivalViewDetail, GoodsArrivalDTO, GoodsArrivalPrimitiveDTO, GoodsArrivalDetailDTO>, IGoodsArrivalService
    {
        private IGoodsArrivalRepository goodsArrivalRepository;
        public GoodsArrivalService(IGoodsArrivalRepository goodsArrivalRepository)
            : base(goodsArrivalRepository, "GoodsArrivalPostSaveValidate", "GoodsArrivalSaveRelative", "GoodsArrivalToggleApproved", null, null, "GetGoodsArrivalViewDetails")
        {
            this.goodsArrivalRepository = goodsArrivalRepository;
        }

        public override ICollection<GoodsArrivalViewDetail> GetViewDetails(int goodsArrivalID)
        {
            ObjectParameter[] parameters = new ObjectParameter[] { new ObjectParameter("GoodsArrivalID", goodsArrivalID) };
            return this.GetViewDetails(parameters);
        }

        public override bool Save(GoodsArrivalDTO goodsArrivalDTO)
        {
            goodsArrivalDTO.GoodsArrivalViewDetails.RemoveAll(x => x.Quantity == 0);
            return base.Save(goodsArrivalDTO);
        }

        protected override GoodsArrival SaveThis(GoodsArrivalDTO dto)
        {
            GoodsArrival goodsArrival = base.SaveThis(dto);

            if (goodsArrival.Approved)
            {
                List<BarcodeBase> barcodeBases = this.goodsArrivalRepository.GetBarcodeBases(goodsArrival.GoodsArrivalID);
                foreach (BarcodeBase barcodeBase in barcodeBases)
                {
                    string symbologies = this.getSymbologies(barcodeBase.Code);
                    this.goodsArrivalRepository.SetBarcodeSymbologies(barcodeBase.BarcodeID, symbologies);
                }
            }

            return goodsArrival;
        }

        private string getSymbologies(string barcode)
        {
            DmtxImageEncoder encoder = new DmtxImageEncoder();
            Bitmap bmp = encoder.EncodeImage(barcode);

            byte[] bitmapData;

            using (System.IO.MemoryStream memoryStream = new System.IO.MemoryStream())
            {
                bmp.Save(memoryStream, System.Drawing.Imaging.ImageFormat.Bmp);
                bitmapData = memoryStream.ToArray();
            }

            return Convert.ToBase64String(bitmapData);
        }
    }
}
