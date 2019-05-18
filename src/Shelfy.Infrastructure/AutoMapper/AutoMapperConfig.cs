using AutoMapper;
using Shelfy.Core.Domain;
using Shelfy.Infrastructure.Commands;
using Shelfy.Infrastructure.DTO.Author;
using Shelfy.Infrastructure.DTO.Book;
using Shelfy.Infrastructure.DTO.Review;
using Shelfy.Infrastructure.DTO.User;

namespace Shelfy.Infrastructure.AutoMapper
{
    public static class AutoMapperConfig
    {
        public static IMapper Initialize()
            => new MapperConfiguration(cfg =>
                {
                    cfg.CreateMap<Book, BookDto>().ForSourceMember(x => x.AuthorsIds, opt => opt.DoNotValidate())
                        .ForMember(x => x.Authors, opt => opt.Ignore());
                    cfg.CreateMap<Author, AuthorDto>();
                    cfg.CreateMap<Author, AuthorSearchDto>();
                    cfg.CreateMap<Author, AuthorFullNameDto>();
                    cfg.CreateMap<UpdateAuthor, Author>();
                    cfg.CreateMap<UpdateBook, Book>();
                    cfg.CreateMap<User, UserDto>();
                    cfg.CreateMap<Review, ReviewDto>();
                })
                .CreateMapper();
    }
}
