using System.Security.Cryptography.X509Certificates;
using AutoMapper;
using Shelfy.Core.Domain;
using Shelfy.Infrastructure.DTO.Author;
using Shelfy.Infrastructure.DTO.Book;
using Shelfy.Infrastructure.DTO.Test;
using Shelfy.Infrastructure.DTO.User;

namespace Shelfy.Infrastructure.AutoMapper
{
    public static class AutoMapperConfig
    {
        public static IMapper Initialize()
            => new MapperConfiguration(cfg =>
                {
                    cfg.CreateMap<Book, BookDto>().ForSourceMember(x => x.Authors, opt => opt.DoNotValidate())
                        .ForMember(x => x.Authors, opt => opt.Ignore());
                    cfg.CreateMap<User, UserDto>();
                    cfg.CreateMap<Author, AuthorDto>();
                    cfg.CreateMap<Author, AuthorSearchDto>();
                    cfg.CreateMap<Author, BookAuthorDto>();
                })
                .CreateMapper();
    }
}
