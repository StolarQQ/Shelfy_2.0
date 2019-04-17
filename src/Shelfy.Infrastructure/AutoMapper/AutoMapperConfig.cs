using AutoMapper;
using Shelfy.Core.Domain;
using Shelfy.Infrastructure.DTO;

namespace Shelfy.Infrastructure.AutoMapper
{
    public static class AutoMapperConfig
    {
        public static IMapper Initialize()
            => new MapperConfiguration(cfg =>
                {
                    cfg.CreateMap<Book, BookDto>();
                    cfg.CreateMap<User, UserDto>();

                })
                .CreateMapper();

    }
}
