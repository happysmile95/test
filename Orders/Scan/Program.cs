using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using AutoMapper;
using Data;
using Infrastructure;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Internal;
using Microsoft.Extensions.DependencyInjection;
using OfficeOpenXml;
using OfficeOpenXml.ConditionalFormatting;

namespace Scan
{
    class Program
    {
        private static readonly string defaultPath = "C:\\Users\\Ашот\\source\\repos\\Orders";
        private static readonly string incomingDirectory = "C:\\Users\\Ашот\\source\\repos\\Orders\\INCOMING";
        private static readonly string processingDirectory = "C:\\Users\\Ашот\\source\\repos\\Orders\\PROCESSED";
        private static readonly string errorDirectory = "C:\\Users\\Ашот\\source\\repos\\Orders\\ERROR";
        private static IMapper _mapper;
        private static CoreContext _context = new CoreContext();
        static void Main(string[] args)
        {
            if (!HasFiles())
            {
                //MoveFilesToProcessingFolder();
                //CheckConsistencyData();
                var goods = ParseFile();

                var config = new MapperConfiguration(cfg => cfg.CreateMap<GoodDto, Good>());
                _mapper = new Mapper(config);

                CreateOrUpdate(goods);
            }
        }

        private static bool HasFiles()
        {
            return Directory.GetFiles(incomingDirectory).Any();
        }
        private static void MoveFilesToProcessingFolder()
        {
            foreach (var file in Directory.GetFiles(incomingDirectory))
            {
                try
                {
                    File.Move(file, string.Format("{0}\\{1}", processingDirectory, new FileInfo(file).Name));
                }
                catch
                {
                    RollbackFiles(processingDirectory, incomingDirectory);
                    break;
                }
            }
        }
        private static void RollbackFiles(string dirFrom, string dirTo)
        {
            foreach (var file in Directory.GetFiles(dirFrom).Where(e=> (new FileInfo(e).CreationTime.Minute - DateTime.Now.Minute) < 5))
            {
                File.Move(file, string.Format("{0}\\{1}", dirTo, new FileInfo(file).Name));
            }
        }
        private static void MoveFileToErrorFolder()
        {
            foreach (var file in Directory.GetFiles(processingDirectory))
            {
                File.Move(file, string.Format("{0}\\{1}", errorDirectory, new FileInfo(file).Name));
            }
        }

        private static void CheckConsistencyData()
        {
            try
            {
                ExcelPackage.LicenseContext = LicenseContext.NonCommercial;
                foreach (var file in Directory.GetFiles(processingDirectory).Select(e => new FileInfo(e)))
                {
                    using (var package = new ExcelPackage(file))
                    {
                        foreach (var worksheet in package.Workbook.Worksheets)
                        {
                            ValidationData(worksheet, worksheet.Dimension.Rows, worksheet.Dimension.Columns);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error message {0}", ex.Message);
                MoveFileToErrorFolder();
            }
        }
        private static void ValidationData(ExcelWorksheet worksheet, int rows, int columns)
        {
            for (int i = 2; i <= rows; i++)
            {
                for (int j = 1; j <= columns; j++)
                {
                    if(!ValidationValue(worksheet, i, j))
                    {
                        throw new ApplicationException("The data is not consistent");
                    }
                }
            }
        }
        private static bool ValidationValue(ExcelWorksheet worksheet, int i, int j)
        {
            try
            {
                switch (j)
                {
                    case 12:
                    case 13:
                    case 14:
                        {
                            return int.TryParse(worksheet.Cells[i, j].Value.ToString(), out int value) && value >= 0;
                        }
                    default:
                        {
                            return !string.IsNullOrEmpty(worksheet.Cells[i, j].Value.ToString());
                        }
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        private static List<GoodDto> ParseFile()
        {
            List<GoodDto> goods = new List<GoodDto>();
            ExcelPackage.LicenseContext = LicenseContext.NonCommercial;
            foreach (var file in Directory.GetFiles(processingDirectory).Select(e => new FileInfo(e)))
            {
                using (var package = new ExcelPackage(file))
                {
                    foreach (var worksheet in package.Workbook.Worksheets)
                    {
                        goods.AddRange(ParseFile(worksheet, worksheet.Dimension.Rows));
                    }
                }
            }

            return goods;
        }
        private static List<GoodDto> ParseFile(ExcelWorksheet worksheet, int rows)
        {
            List<GoodDto> goods = new List<GoodDto>();
            for (int i = 2; i <= rows; i++)
            {
                goods.Add(GetGood(worksheet, i));
            }
            return goods;
        }
        private static GoodDto GetGood(ExcelWorksheet worksheet, int row)
        {
            int cell = 0;
            return new GoodDto()
            {
                SoldTo = worksheet.Cells[row, ++cell].Value.ToString(),
                CustName = worksheet.Cells[row, ++cell].Value.ToString(),
                ShipTo = worksheet.Cells[row, ++cell].Value.ToString(),
                ShipToNa = worksheet.Cells[row, ++cell].Value.ToString(),
                OrderType = worksheet.Cells[row, ++cell].Value.ToString(),
                Dv = worksheet.Cells[row, ++cell].Value.ToString(),
                OrderNum = worksheet.Cells[row, ++cell].Value.ToString(),
                Material = worksheet.Cells[row, ++cell].Value.ToString(),
                MatDes = worksheet.Cells[row, ++cell].Value.ToString(),
                Size = worksheet.Cells[row, ++cell].Value.ToString(),
                AltSize = worksheet.Cells[row, ++cell].Value.ToString(),
                OnOrdQty = int.Parse(worksheet.Cells[row, ++cell].Value.ToString()),
                ShipQty = int.Parse(worksheet.Cells[row, ++cell].Value.ToString()),
                RejectQty = int.Parse(worksheet.Cells[row, ++cell].Value.ToString())
            };
        }


        private static void CreateOrUpdate(List<GoodDto> goodDtos)
        {
            var list = goodDtos.GroupBy(e => new
            {
                e.OrderNum,
                e.Material,
                e.Size
            });

            foreach (var item in list)
            {
                var goods = _context.Goods.AsNoTracking()
                            .Where(e => e.OrderNum == item.Key.OrderNum && e.Material == item.Key.Material && e.Size == item.Key.Size)
                            .ToList();

                var sessionRows = item.Select(e => e.OnOrdQty + e.ShipQty).Sum();
                var good = item.FirstOrDefault();

                if (!goods.Any())
                {
                    CreateGood(good, sessionRows, goods.Count);
                }
                else
                {
                    //step by update
                    bool hasDifferent = goods.Any(e =>
                        !e.OrderType.Equals(good.OrderType)
                        || !e.SoldTo.Equals(good.SoldTo)
                        || !e.CustName.Equals(good.CustName));

                    if (hasDifferent)
                    {
                        foreach (var existGood in goods)
                        {
                            good.Id = existGood.Id;
                            _context.Update(_mapper.Map(good, existGood));
                        }
                    }

                    //step by new create
                    if (sessionRows > goods.Count) 
                    {
                        good.Id = Guid.Empty;
                        CreateGood(good, sessionRows, goods.Count);
                    }
                }
                _context.SaveChanges();
            }
        }

        private static void CreateGood(GoodDto good, int sessionRows, int goodsTotal)
        {
            for (int i = 0; i < (sessionRows - goodsTotal); i++)
            {
                _context.Add(_mapper.Map<GoodDto, Good>(good));
            }
        }
    }
}
