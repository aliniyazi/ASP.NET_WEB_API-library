using API.Common;
using API.DataAccess.Enums;
using API.DataAccess.Models;
using API.Repositories.Contracts;
using API.Services.DTOs;
using API.Services.Mappers;
using API.Services.Requests;
using API.Services.Responses;
using API.Services.ServiceContracts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;

namespace API.Services.Service
{
    public class BookService : IBookService
    {
        private readonly IBookRepository bookRepository;
        private readonly IAuthorRepository authorRepository;
        private readonly IFileService fileService;
        private readonly IUserService userService;

        public BookService(IBookRepository bookRepository, IAuthorRepository authorRepository, IUserService userService, IFileService fileService)
        {
            this.bookRepository = bookRepository;
            this.authorRepository = authorRepository;
            this.fileService = fileService;
            this.userService = userService;
        }

        public async Task<Response<BookDTO>> CreateBookAsync(CreateBookRequest userInput)
        {
            if (userInput.ImageFile == null)
            {
                return (new Response<BookDTO>(HttpStatusCode.BadRequest, null, GlobalConstants.CREATE_OR_EDIT_BOOK_NO_IMAGE));
            }
            if (await bookRepository.GetBookByTitleAndAuthorNamesAsync(userInput.Title, userInput.AuthorFirstName, userInput.AuthorLastName) != null)
            {
                return (new Response<BookDTO>(HttpStatusCode.BadRequest, null, GlobalConstants.BOOK_ALREADY_EXISTS_ERROR));
            }

            var imageNameUpload = await fileService.UploadImageToAzureStorage(userInput.ImageFile);
            if (imageNameUpload == null)
            {
                return (new Response<BookDTO>(HttpStatusCode.FailedDependency, null, GlobalConstants.UPLOAD_IMAGE_ERROR));
            }

            var author = await authorRepository.GetAuthorByAuthorNamesAsync(userInput.AuthorFirstName, userInput.AuthorLastName);
            Book bookEntity = null;
           
            if (author != null)
            {
                bookEntity = Mapper.MapFrom(userInput, imageNameUpload, author.Id); 
            }
            else
            {
                bookEntity = Mapper.MapFrom(userInput, imageNameUpload);
            }
            
            var bookFromDB = await bookRepository.InsertAsync(bookEntity);
            await bookRepository.SaveAsync();
           
            var imageNameGet =  await fileService.GetImageFromAzureStorage(bookFromDB.ImageName);
            if (imageNameGet == null)
            {
                return (new Response<BookDTO>(HttpStatusCode.FailedDependency, null, GlobalConstants.GET_IMAGE_ERROR));
            }

            var bookDTO = Mapper.MapFromBookEntity(bookFromDB, imageNameGet);
           
            return new Response<BookDTO>(HttpStatusCode.OK, bookDTO);           
        }

        public async Task<Response<int>> DeleteBookByIdAsync(int id)
        {
            var bookEntity = await bookRepository.GetByIdAsync(id);
            if (bookEntity == null)
            {
                return (new Response<int>(HttpStatusCode.NotFound, id, GlobalConstants.BOOK_NOT_FOUND_ERROR));
            }

            var deletedBookEntity = await bookRepository.DeleteAsync(id);
            var deletedBookEntityId = deletedBookEntity.Id;
            await bookRepository.SaveAsync();
            return new Response<int>(HttpStatusCode.OK, deletedBookEntityId);
        }

        public async Task<Response<BookDTO>> GetBookByIdAsync(int id)
        {
            var bookEntity = await bookRepository.GetByIdAsync(id);
            if (bookEntity == null)
            {
                return (new Response<BookDTO>(HttpStatusCode.NotFound, null, GlobalConstants.BOOK_NOT_FOUND_ERROR));
            }
            var imageNameGet = string.Empty;
            if (bookEntity.ImageName!=null)
            {
                imageNameGet = await fileService.GetImageFromAzureStorage(bookEntity.ImageName);
            }
            if (imageNameGet == null)
            {
                return (new Response<BookDTO>(HttpStatusCode.FailedDependency, null, GlobalConstants.GET_IMAGE_ERROR));
            }

            var author = await authorRepository.GetByIdAsync(bookEntity.AuthorId);
            var bookDTO = Mapper.MapFromBookEntity(bookEntity, imageNameGet, author);
            return new Response<BookDTO>(HttpStatusCode.OK, bookDTO);
        }

        public async Task<Response<BookDTO>> UpdateBookAsync(UpdateBookRequest userInput)
        {
            if (userInput.ImageFile == null)
            {
                return (new Response<BookDTO>(HttpStatusCode.BadRequest, null, GlobalConstants.CREATE_OR_EDIT_BOOK_NO_IMAGE));
            }

            var bookEntity = await bookRepository.GetByIdAsync(userInput.Id);
            if (bookEntity == null)
            {
                return (new Response<BookDTO>(HttpStatusCode.NotFound, null, GlobalConstants.BOOK_NOT_FOUND_ERROR));
            }

            var imageNameUpload=string.Empty;
            if (bookEntity.ImageName != null)
            {
                try
                {
                    await fileService.DeleteImageFromAzure(bookEntity.ImageName);
                }
                catch (Exception)
                {
                    return (new Response<BookDTO>(HttpStatusCode.FailedDependency, null, GlobalConstants.DELETE_IMAGE_ERROR));
                }
            }
                
            imageNameUpload = await fileService.UploadImageToAzureStorage(userInput.ImageFile);
            if (imageNameUpload == null)
            {
                return (new Response<BookDTO>(HttpStatusCode.FailedDependency, null, GlobalConstants.UPLOAD_IMAGE_ERROR));
            }

            var author = await authorRepository.GetAuthorByAuthorNamesAsync(userInput.AuthorFirstName, userInput.AuthorLastName);
            if (author == null)
            {
                bookEntity.Author = new Author
                {
                    FirstName = userInput.AuthorFirstName,
                    LastName = userInput.AuthorLastName,
                };
            }
            else
            {
                bookEntity.AuthorId = author.Id;
            }

            bookEntity.Title = userInput.Title;
            bookEntity.Description = userInput.Description;
            bookEntity.Quantity = userInput.Quantity;
            bookEntity.ImageName = imageNameUpload;
            bookEntity.Genre = (Genres)Enum.Parse(typeof(Genres), userInput.Genre);
            bookEntity.ModifiedOn = DateTime.UtcNow;

            bookEntity = bookRepository.Update(bookEntity);
            await bookRepository.SaveAsync();

            var imageNameGet = await fileService.GetImageFromAzureStorage(imageNameUpload);
            if (imageNameGet == null)
            {
                return (new Response<BookDTO>(HttpStatusCode.FailedDependency, null, GlobalConstants.GET_IMAGE_ERROR));
            }

            var bookDTO = Mapper.MapFromBookEntity(bookEntity, imageNameGet);

            return new Response<BookDTO>(HttpStatusCode.OK, bookDTO);
        }

        public async Task<Response<PageDTO>> GetBooksForLastTwoWeeksAsync(int page, int booksPerPage)
        {
            var latestBook = await bookRepository.GetLastestBookDate();
            var booksOnPage = await bookRepository.GetBooksForLastTwoWeeksAsync(page, booksPerPage, latestBook);
            var numOfBooks = await bookRepository.GetNumOfBooksForLastTwoWeeksAsync(latestBook);

            foreach (var book in booksOnPage)
            {
                var ImageNameGet = await fileService.GetImageFromAzureStorage(book.ImageName);
                book.ImageName = ImageNameGet;
            }

            var pageDTO = new PageDTO(booksOnPage.Select(book => new BookDTO
            {
                Id = book.Id,
                Title = book.Title,
                Description = book.Description,
                Quantity = book.Quantity,
                Genre = book.Genre.ToString(),
                ImageName = book.ImageName,
                AuthorFirstName = authorRepository.GetByIdAsync(book.AuthorId).Result.FirstName,
                AuthorLastName = authorRepository.GetByIdAsync(book.AuthorId).Result.LastName
            }).ToArray(), numOfBooks);
            return new Response<PageDTO>(HttpStatusCode.OK, pageDTO);
        }

        public async Task<Response<StatisticDTO>> GetStatisticsAsync()
        {
            int numOfBooks = await bookRepository.GetBookNumberAsync();
            int numOfUsers = await userService.GetNumOfUsersAsync();
            StatisticDTO statisticDTO = new StatisticDTO(numOfBooks, 17, numOfUsers);
            return new Response<StatisticDTO>(HttpStatusCode.OK, statisticDTO);
        }
        public async Task<Response<BookDTO[]>> GetAllBooksInPageAsync(int page, int booksPerPage)
        {
            var bookList = await bookRepository.GetBooksByPageAsync(page, booksPerPage);
            var allBooks = new List<BookDTO>();
            foreach (var book in bookList)
            {
                allBooks.Add(new BookDTO
                {
                    Id = book.Id,
                    Description = book.Description,
                    AuthorFirstName = book.Author.FirstName,
                    AuthorLastName = book.Author.LastName,
                    Genre = book.Genre.ToString(),
                    Quantity = book.Quantity,
                    Title = book.Title,
                    ImageName = await fileService.GetImageFromAzureStorage(book.ImageName)

                });
            }
            return new Response<BookDTO[]>(HttpStatusCode.OK, allBooks.ToArray());
        }

        public async Task<Response<IList<BookDTO>>> GetAllBooksToBeReturnInTwoWeekAsync()
        {
            var books = await bookRepository.GetAllBooksToBeReturnInTwoWeekAsync();

            if (books.Count == 0)
            {
                return (new Response<IList<BookDTO>>(HttpStatusCode.NotFound, null, GlobalConstants.NO_BOOKS_TO_RETURN_TWO_WEEKS_MESSAGE));
            }

            foreach (var book in books)
            {
                var imageNameGet = await fileService.GetImageFromAzureStorage(book.ImageName);
                if (imageNameGet == null)
                {
                    return (new Response<IList<BookDTO>>(HttpStatusCode.FailedDependency, null, GlobalConstants.GET_IMAGE_ERROR));
                }
                book.ImageName = imageNameGet;
            }

            var booksDto = books.Select(book => Mapper.MapFromBookEntity(book)).ToList();

            return new Response<IList<BookDTO>>(HttpStatusCode.OK, booksDto);
        }

        public async Task<Response<IList<BookDelayedDTO>>> GetAllBooksDelayedReturnAsync()
        {
            var books = await bookRepository.GetAllBooksDelayedReturnAsync();

            if (books.Count==0)
            {
                return (new Response<IList<BookDelayedDTO>>(HttpStatusCode.NotFound, null, GlobalConstants.NO_BOOKS_DELAYED_RETURN));
            }

            foreach (var book in books)
            {
                var imageNameGet = await fileService.GetImageFromAzureStorage(book.ImageName);
                if (imageNameGet == null)
                {
                    return (new Response<IList<BookDelayedDTO>>(HttpStatusCode.FailedDependency, null, GlobalConstants.GET_IMAGE_ERROR));
                }
                book.ImageName = imageNameGet;
            }

            var booksDto = books.Select(book => Mapper.MapFromBookEntityDelayed(book)).ToList();

            return new Response<IList<BookDelayedDTO>>(HttpStatusCode.OK, booksDto);
        }

        public async Task<Response<PageDTO>> Search(int page, int booksPerPage, string searchType, string search, string genres)
        {
            string[] searchTerms = search.Split().Select(x => x.ToUpper()).ToArray();
            string[] searchGenres = genres == null ? null : genres.Split().Select(x => x.ToUpper()).ToArray();
            var books = await bookRepository.GetBooksAsync();

            IList<Book> passingBooks;
            Dictionary<Book, int> sortedBooks = new Dictionary<Book, int>();
            if(searchType.ToUpper() == "AUTHOR")
            {
                passingBooks = books
                .Where(x => search == null || searchTerms.Any(x.Author.FirstName.ToUpper().Contains) || searchTerms.Any(x.Author.LastName.ToUpper().Contains))
                .Where(x => genres == null || searchGenres.Any(x.Genre.ToString().ToUpper().Contains))
                .ToList();
                foreach (var book in passingBooks)
                {
                    foreach (var searchTerm in searchTerms)
                    {
                        if (book.Author.FirstName.ToUpper().Contains(searchTerm) || book.Author.LastName.ToUpper().Contains(searchTerm))
                        {
                            if (sortedBooks.ContainsKey(book))
                            {
                                sortedBooks[book]++;
                            }
                            else
                            {
                                sortedBooks.Add(book, 1);
                            }
                        }
                    }
                }
            }
            else if(searchType.ToUpper() == "TITLE")
            {
                passingBooks = books
                .Where(x => search == null || searchTerms.Any(x.Title.ToUpper().Contains))
                .Where(x => genres == null || searchGenres.Any(x.Genre.ToString().ToUpper().Contains))
                .ToList();
                foreach (var book in passingBooks)
                {
                    foreach (var searchTerm in searchTerms)
                    {
                        if (book.Title.ToUpper().Contains(searchTerm))
                        {
                            if (sortedBooks.ContainsKey(book))
                            {
                                sortedBooks[book]++;
                            }
                            else
                            {
                                sortedBooks.Add(book, 1);
                            }
                        }
                    }
                }
            }
            else
            {
                passingBooks = books
                .Where(x => search == null || searchTerms.Any(x.Description.ToUpper().Contains))
                .Where(x => genres == null || searchGenres.Any(x.Genre.ToString().ToUpper().Contains))
                .ToList();

                foreach (var book in passingBooks)
                {
                    foreach (var searchTerm in searchTerms)
                    {
                        if (search == null || book.Description.ToUpper().Contains(searchTerm))
                        {
                            if (sortedBooks.ContainsKey(book))
                            {
                                sortedBooks[book]++;
                            }
                            else
                            {
                                sortedBooks.Add(book, 1);
                            }
                        }
                    }
                }
            }
            
            var booksOnPage = sortedBooks
                .OrderByDescending(x => x.Value)
                .Skip((page - 1) * booksPerPage)
                .Take(booksPerPage);

            foreach (var book in booksOnPage)
            {
                var ImageNameGet = await fileService.GetImageFromAzureStorage(book.Key.ImageName);
                book.Key.ImageName = ImageNameGet;
            }

            var pageDTO = new PageDTO(booksOnPage.Select(book => new BookDTO
            {
                Id = book.Key.Id,
                Title = book.Key.Title,
                Description = book.Key.Description,
                Quantity = book.Key.Quantity,
                Genre = book.Key.Genre.ToString(),
                AuthorFirstName = book.Key.Author.FirstName,
                AuthorLastName = book.Key.Author.LastName,
                ImageName = book.Key.ImageName
            }).ToArray(), passingBooks.Count());

            return new Response<PageDTO>(HttpStatusCode.OK, pageDTO);
        }
    }
}
