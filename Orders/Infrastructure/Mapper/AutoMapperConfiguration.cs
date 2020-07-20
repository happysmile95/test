using AutoMapper;
using Data;

namespace Infrastructure.Mapper
{
    /// <summary>
    /// Конфигурация маппера
    /// </summary>
    public class AutoMapperConfiguration
    {
        public IMapper Mapper { get; set; }

        public AutoMapperConfiguration()
        {
            InitializeConfig();
        }

        /// <summary>
        /// Инициализация маппера
        /// </summary>
        private void InitializeConfig()
        {
            var config = new MapperConfiguration(c =>
            {
                c.CreateMap<Good, GoodDto>();
                c.CreateMap<GoodDto, Good>();
            });

            Mapper = config.CreateMapper();
        }
    }
}
