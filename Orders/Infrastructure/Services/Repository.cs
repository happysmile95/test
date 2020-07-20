using Data;
using Infrastructure.Mapper;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Infrastructure.Services
{
    /// <summary>
    /// Репозпиторий для работы с базой
    /// </summary>
    public class Repository : IRepository
    {
        /// <summary>
        /// Контекст базы данных
        /// </summary>
        private readonly CoreContext _context;

        /// <summary>
        /// Маппер
        /// </summary>
        private readonly AutoMapperConfiguration _mapper;

        public Repository(CoreContext context)
        {
            _context = context;
            _mapper = new AutoMapperConfiguration();
        }

        /// <summary>
        /// Создать или обновить данные в базе
        /// </summary>
        /// <param name="goodDtos">Товары полученные из файлов</param>
        public void CreateOrUpdate(List<GoodDto> goodDtos)
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
                            _context.Update(_mapper.Mapper.Map(good, existGood));
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

        public void Dispose()
        {
            _context.Dispose();
        }

        /// <summary>
        /// Создать товар
        /// </summary>
        /// <param name="good">Товар</param>
        /// <param name="sessionRows">Кол-во товара в рамках сессии</param>
        /// <param name="goodsTotal">Кол-во товаров в системе</param>
        private void CreateGood(GoodDto good, int sessionRows, int goodsTotal)
        {
            for (int i = 0; i < (sessionRows - goodsTotal); i++)
            {
                _context.Add(_mapper.Mapper.Map<GoodDto, Good>(good));
            }
        }
    }
}
