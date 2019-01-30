﻿using System;
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
            //GoodsArrival goodsArrival = base.SaveThis(dto);

            //string[] queryArray = new string[15];

            //queryArray[0] = "";
            //queryArray[0] = "1000";
            //queryArray[0] = "0100";
            //queryArray[0] = "1100";
            //queryArray[0] = "0010";
            //queryArray[0] = "0110";
            //queryArray[0] = "1010";
            //queryArray[0] = "1110";
            //queryArray[0] = "0001";
            //queryArray[0] = "1001";
            //queryArray[0] = "0101";
            //queryArray[0] = "1101";
            //queryArray[0] = "0011";
            //queryArray[0] = "0111";
            //queryArray[0] = "1011";
            //queryArray[0] = "1111";


            //if (goodsArrival.Approved)
            //{
            //    List<BarcodeBase> barcodeBases = this.goodsArrivalRepository.GetBarcodeBases(goodsArrival.GoodsArrivalID);
            //    foreach (BarcodeBase barcodeBase in barcodeBases)
            //    {
            //        string symbologies = this.getSymbologies(barcodeBase.Code);
            //        this.goodsArrivalRepository.SetBarcodeSymbologies(barcodeBase.BarcodeID, symbologies);
            //    }
            //}

            //return goodsArrival;




            GoodsArrival goodsArrival = base.SaveThis(dto);

            string[] queryArray = new string[15];

            queryArray[0] = "1000";
            queryArray[1] = "0100";
            queryArray[2] = "1100";
            queryArray[3] = "0010";
            queryArray[4] = "0110";
            queryArray[5] = "1010";
            queryArray[6] = "1110";
            queryArray[7] = "0001";
            queryArray[8] = "1001";
            queryArray[9] = "0101";
            queryArray[10] = "1101";
            queryArray[11] = "0011";
            queryArray[12] = "0111";
            queryArray[13] = "1011";
            queryArray[14] = "1111";
            int i = 0;

            if (goodsArrival.Approved)
            {
                List<BarcodeBase> barcodeBases = this.goodsArrivalRepository.GetBarcodeBases(goodsArrival.GoodsArrivalID);
                foreach (BarcodeBase barcodeBase in barcodeBases)
                {
                    string symbologies = this.getSymbologies(queryArray[i]);
                    this.goodsArrivalRepository.SetBarcodeSymbologies(barcodeBase.BarcodeID, queryArray[i] + symbologies);
                    i++;
                }
            }

            return goodsArrival;
        }

        private string getSymbologies(string barcode)
        {
            //DmtxImageEncoder encoder = new DmtxImageEncoder();
            //Bitmap bmp = encoder.EncodeImage(barcode);

            //byte[] bitmapData;

            //using (System.IO.MemoryStream memoryStream = new System.IO.MemoryStream())
            //{
            //    bmp.Save(memoryStream, System.Drawing.Imaging.ImageFormat.Bmp);
            //    bitmapData = memoryStream.ToArray();
            //}

            //return Convert.ToBase64String(bitmapData);


            Bitmap bmp = (Bitmap)Image.FromFile("D:\\MVC PROJECTS\\CBPP\\Documents\\BP\\Mr Hien\\ICONS\\Icons\\Icons\\" + barcode + ".jpg"); ;

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
