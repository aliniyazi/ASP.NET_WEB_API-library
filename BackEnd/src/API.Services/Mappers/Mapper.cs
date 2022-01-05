using API.DataAccess.Models;
using API.Services.Requests;
using System;
using API.DataAccess.Enums;
using API.Services.DTOs;

namespace API.Services.Mappers
{
    public class Mapper
    {
        public static User MapFrom(RegisterUserRequest entity)
        {
            return new User
            {
                FirstName = entity.FirstName,
                Lastname = entity.LastName,
                Email = entity.Email,
                UserName = entity.Email,
                PhoneNumber = entity.PhoneNumber,
                Address = new Address
                {
                    Country = entity.Address.Country,
                    City = entity.Address.City,
                    StreetName = entity.Address.StreetName,
                    StreetNumber = entity.Address.StreetNumber,
                    BuildingNumber = entity.Address.BuildingNumber,
                    AppartmentNumber = entity.Address.AppartmentNumber,
                    AdditionalInfo = entity.Address.AdditionalInfo
                }
            };
        }

        public static Request MapFromRequest(User user, Book book)
        {
            return new Request
            {
                User = user,
                UserId = user.Id,
                Book = book,
                BookId = book.Id,
                DateToReturnBook = null,
                RequestApproved = false,
                ModifiedOn = DateTime.UtcNow,
                CreatedOn = DateTime.UtcNow
            };
        }

        public static Book MapFrom(CreateBookRequest request, string imageName, int authorId = 0)
        {
            if (authorId != 0)
            {
                return new Book
                {
                    Title = request.Title,
                    Description = request.Description,
                    Quantity = (int)request.Quantity,
                    Genre = (Genres)Enum.Parse(typeof(Genres), request.Genre),
                    Status = Status.Available,
                    ImageName = imageName,
                    AuthorId = authorId,
                    ModifiedOn = DateTime.UtcNow,
                    CreatedOn = DateTime.UtcNow
                };
            }

            return new Book
            {
                Title = request.Title,
                Description = request.Description,
                Quantity = (int)request.Quantity,
                Genre = (Genres)Enum.Parse(typeof(Genres), request.Genre),
                Status = Status.Available,
                ImageName = imageName,
                Author = new Author
                {
                    FirstName = request.AuthorFirstName,
                    LastName = request.AuthorLastName,
                    CreatedOn = DateTime.UtcNow
                },
                ModifiedOn = DateTime.UtcNow,
                CreatedOn = DateTime.UtcNow
            };
        }

        public static CreateBookRequest MapFrom(Book bookEntity)
        {
            return new CreateBookRequest
            {
                Title = bookEntity.Title,
                Description = bookEntity.Description,
                Quantity = bookEntity.Quantity,
                Genre = bookEntity.Genre.ToString(),
                AuthorFirstName = bookEntity.Author.FirstName,
                AuthorLastName = bookEntity.Author.LastName,
            };
        }

        public static BookDTO MapFromBookEntity(Book bookEntity, string ImageName)
        {
            return new BookDTO
            {
                Id = bookEntity.Id,
                Title = bookEntity.Title,
                Description = bookEntity.Description,
                Quantity = bookEntity.Quantity,
                ImageName = ImageName,
                Genre = bookEntity.Genre.ToString(),
                AuthorFirstName = bookEntity.Author.FirstName,
                AuthorLastName = bookEntity.Author.LastName,
            };
        }

        public static BookDTO MapFromBookEntity(Book bookEntity, string ImageName, Author authorEntity)
        {
            return new BookDTO
            {
                Id = bookEntity.Id,
                Title = bookEntity.Title,
                Description = bookEntity.Description,
                Quantity = bookEntity.Quantity,
                ImageName = ImageName,
                Genre = bookEntity.Genre.ToString(),
                AuthorFirstName = authorEntity.FirstName,
                AuthorLastName = authorEntity.LastName,
            };
        }

        public static BookDTO MapFromBookEntity(Book bookEntity)
        {
            return new BookDTO
            {
                Id = bookEntity.Id,
                Title = bookEntity.Title,
                Description = bookEntity.Description,
                Quantity = bookEntity.Quantity,
                ImageName = bookEntity.ImageName,
                Genre = bookEntity.Genre.ToString(),
                AuthorFirstName = bookEntity.Author.FirstName,
                AuthorLastName = bookEntity.Author.LastName,
            };
        }
        public static BookDelayedDTO MapFromBookEntityDelayed(Book bookEntity)
        {
            return new BookDelayedDTO
            {
                Id = bookEntity.Id,
                Title = bookEntity.Title,
                UserFirstName = bookEntity.User.FirstName,
                UserLastName = bookEntity.User.Lastname,
                ImageName = bookEntity.ImageName,
                Genre = bookEntity.Genre.ToString(),
                Email = bookEntity.User.Email,
                PhoneNumber = bookEntity.User.PhoneNumber,
            };
        }
        public static RequestDTO MapFrom(Request request)
        {
            return new RequestDTO
            {
                UserId = request.UserId,
                BookId = request.BookId,
                FirstName = request.User.FirstName,
                LastName = request.User.Lastname,
                PhoneNumber = request.User.PhoneNumber,
                Email = request.User.Email,
                BookTitle = request.Book.Title,
                Quantity = request.Book.Quantity,
            };
        }

    }
}
