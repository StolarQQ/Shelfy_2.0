using AutoMapper;
using Shelfy.Core.Domain;
using Shelfy.Infrastructure.Commands.Author;
using Shelfy.Infrastructure.Commands.Book;
using Shelfy.Infrastructure.Commands.Review;
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
                    cfg.CreateMap<Book, BookDto>();
                    cfg.CreateMap<Book, BookDetailsDto>();
                    cfg.CreateMap<AuthorShortcut, AuthorShortcutDto>();
                    cfg.CreateMap<Author, AuthorDto>();
                    cfg.CreateMap<Author, AuthorSearchDto>();
                    cfg.CreateMap<Author, AuthorShortcutDto>();
                    cfg.CreateMap<Author, AuthorNameDto>();
                    cfg.CreateMap<UpdateAuthor, Author>();
                    cfg.CreateMap<UpdateBook, Book>();
                    cfg.CreateMap<User, UserDto>();
                    cfg.CreateMap<Review, ReviewDto>();
                    cfg.CreateMap<Review, UpdateReview>();
                    cfg.CreateMap<UpdateReview, Review>();
                })
                .CreateMapper();
    }
}
