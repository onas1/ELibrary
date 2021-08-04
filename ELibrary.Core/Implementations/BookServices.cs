using AutoMapper;
using ELibrary.Core.Abstractions;
using ELibrary.Data.Repositories.Abstractions;
using ELibrary.Data.Repositories.Implementations;
using ELibrary.Dtos;
using ELibrary.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace ELibrary.Core.Implementations
{
    public class BookServices : IBookServices
    {
        private readonly IMapper _mapper;
        private readonly IBookRepository _bookRepository;
        private readonly ICloudinaryServices _cloudinaryService;
        private readonly IConfiguration _config;
        private readonly int _pageSize;

        public BookServices(IBookRepository bookRepository, ICloudinaryServices cloudinaryService, IMapper mapper, IConfiguration config)
        {
            _mapper = mapper;
            _bookRepository = bookRepository;
            _cloudinaryService = cloudinaryService;
            _pageSize = int.Parse(config.GetSection("PageSize:Default").Value);
        }

        public ResponseDto<AllBooks> GetAll(int pageIndex=1)
        {
            var books = _bookRepository.Get()
                .Select(book => _mapper.Map<GetBookDto>(book));

           
            var books1 = new AllBooks { books = books.ToList() };

            var response = new ResponseDto<AllBooks>
            {
                Data = books1,
                Success = true,
                StatusCode = 200,
              
            };

            return response;
        }

        public async Task<ResponseDto<bool>> DeleteById(int bookId)
        {
            var response = new ResponseDto<bool>();

            var book = await _bookRepository.GetById(bookId);

            if (book == null)
            {
                response.Data = false;
                response.Message = "invalid id entered";
                response.Success = false;
                response.StatusCode = 404;

                return response;
            }

            var result = await _bookRepository.DeleteById(bookId);

            if (result)
            {
                response.Data = true;
                response.Message = "deleted successfully";
                response.StatusCode = 200;
                response.Success = true;
            }
            else
            {
                response.Data = false;
                response.Message = "delete unsuccessful";
                response.StatusCode = 200;
                response.Success = false;
            }

            return response;
        }

        public async Task<ResponseDto<AddBookResponseDto>> AddBook(AddBookDto book)
        {

            var response = new ResponseDto<AddBookResponseDto>();

            if (book == null)
            {
                response.Data = null;
                response.StatusCode = 404;
                response.Success = false;
                response.Message = "Not Found";
            }

            var newBook = new Book
            {
                AddedDate = book.AddedDate,
                ISBN =book.ISBN,
                Description=book.Description,
                Author=book.Author,
                Availability=book.Availability,
                PublishedDate=book.PublishedDate,
                AvailableCopies=book.AvailableCopies,
                Copies=book.Copies,
                Language=book.Language,
                Pages=book.Pages,
                CategoryId=book.CategoryId,
                Title=book.Title
            };
           

            var success = await _bookRepository.Save(newBook);

            var bookFromDb = _mapper.Map<AddBookResponseDto>(newBook);

            response.Data = bookFromDb;
            response.StatusCode = 201;
            response.Success = true;
            response.Message = "New Book Added";

            return response;

        }

        public async Task<ResponseDto<Pagination<GetBookDto>>> GetBookByTitle(BookByTitleResourceParameters bookResource)
        {
            var response = new ResponseDto<Pagination<GetBookDto>>();

            if (string.IsNullOrEmpty(bookResource.Title))
            {
                response.Data = null;
                response.StatusCode = 404;
                response.Success = false;
                response.Message = "Not Found";
            }


            var book = _mapper.Map<Book>(bookResource);

            var bookFromDb = _bookRepository.GetBookByTitle(book.Title);

            var mappedBook = bookFromDb.Select(book => _mapper.Map<GetBookDto>(book));

            var paginatedBooks = await Pagination<GetBookDto>.CreateAsync(mappedBook, 1, 15);

            response.Data = paginatedBooks;
            response.StatusCode = 200;
            response.Success = true;
            response.Next = paginatedBooks.HasNextPage;
            response.Prev = paginatedBooks.HasPreviousPage;


            return response;
        }


        public async Task<ResponseDto<Book>> GetBookById(string bookId)
        {
            var response = new ResponseDto<Book>();

            if (String.IsNullOrEmpty(bookId))
            {
                response.Data = null;
                response.StatusCode = 404;
                response.Success = false;
                response.Message = "Not Found";
            }

            var bookFromDb = await _bookRepository.GetBookByID(Int32.Parse(bookId));

            response.Data = bookFromDb;
            response.StatusCode = 200;
            response.Success = true;

            return response;
        }


        public async Task<ResponseDto<Book>> UpdateBook(UpdateBookDto book)
        {

            var response = new ResponseDto<Book>();

            if (book == null)
            {
                response.Data = null;
                response.StatusCode = 404;
                response.Success = false;
                response.Message = "Not Found";
            }

            //var newBook = _mapper.Map<Book>(book);

            var bookAdded = await _bookRepository.UpdateBook(book);

            //var bookFromDb = _mapper.Map<UpdateBookResponseDto>(newBook);

            response.Data = bookAdded;
            response.StatusCode = 201;
            response.Success = true;
            response.Message = "New Book Added";

            return response;

        }

        public async Task<ResponseDto<GetBookDto>> UpdatePhotoBook(AddPhotoDto photo)
        {
            var response = new ResponseDto<GetBookDto>();
            var file = photo.PhotoFile;
            var book = await _bookRepository.GetById(int.Parse(photo.id));

            if (book == null)
            {
                response.Data = null;
                response.StatusCode = 404;
                response.Success = false;
                response.Message = "Not Found";
                return response;
            }

            var PhotoInfo = await _cloudinaryService.UploadImage(file);
            book.PhotoUrl = PhotoInfo.SecureUrl.ToString();

            await _bookRepository.Update(book);

            var bookDto = _mapper.Map<GetBookDto>(book);

            response.Data = bookDto;
            response.StatusCode = 200;
            response.Success = true;
            response.Message = "Image successfully updated";

            return response;
        }
        public ResponseDto<AllBooks> GetBookBySearchTerm(string query, string searchProperty, int pageIndex)
        {

            if (searchProperty == null || query == null)
            {
                return new ResponseDto<AllBooks>
                {
                    Data = null,
                    Message = "search parameter should not be null",
                    StatusCode = 400,
                    Success = false, 
                };
            }
            if (searchProperty == "ISBN")
            {
                var books = _bookRepository.Get().Where(e => e.ISBN == query);
                var bookDto = books.Select(book => _mapper.Map<GetBookDto>(book));
                // var paginatedResult = await Pagination<GetBookDto>.CreateAsync(bookDto, pageIndex, _pageSize);
                var books1 = new AllBooks { books = bookDto.ToList() };
                var response = new ResponseDto<AllBooks>
                {
                    Data = books1,
                    Message = $"you have successfuly quarried books with the ISBN {searchProperty} sesrch property.",
                    StatusCode = 200,
                    Success = true,
                  
                    
                };

                return response;
            }

            if (searchProperty == "Title")
            {
                var books = _bookRepository.Get().Where(e => e.Title.ToLower().Contains(query.ToLower()));
                var bookDto = books.Select(book => _mapper.Map<GetBookDto>(book));
                // var paginatedResult = await Pagination<GetBookDto>.CreateAsync(bookDto, pageIndex, _pageSize);
                var books1 = new AllBooks { books = bookDto.ToList() };
                var response = new ResponseDto<AllBooks>
                {
                    Data = books1,
                    Message = $"you have successfuly quarried books with the Title {query} search property.",
                    StatusCode = 200,
                    Success = true
                };
                return response;
            }

            if (searchProperty == "Author")
            {
                var books = _bookRepository.Get().Where(e => e.Author.ToLower().Contains(query.ToLower()));
                var bookDto = books.Select(book => _mapper.Map<GetBookDto>(book));
                //var paginatedResult = await Pagination<GetBookDto>.CreateAsync(bookDto, pageIndex, _pageSize);
                var books1 = new AllBooks { books = bookDto.ToList() };
                var response = new ResponseDto<AllBooks>
                {
                    Data = books1,
                    Message = $"you have successfuly quarried books with the Author {query} search property.",
                    StatusCode = 200,
                    Success = true,
                   
                };
                return response;
            }



            if (searchProperty == "Publisher")
            {
                var books = _bookRepository.Get().Where(e => e.Publisher.ToLower().Contains(query.ToLower()));
                var bookDto = books.Select(book => _mapper.Map<GetBookDto>(book));
                // var paginatedResult = await Pagination<GetBookDto>.CreateAsync(bookDto, pageIndex, _pageSize);
                var books1 = new AllBooks { books = bookDto.ToList() };
                var response = new ResponseDto<AllBooks>
                {
                    Data = books1,
                    Message = $"you have successfuly quarried books with the Publisher {query} search property.",
                    StatusCode = 200,
                    Success = true,
                   
                };
                return response;
            }

            if (searchProperty == "PublishedYear")
            {
                var books = _bookRepository.Get().Where(e => e.PublishedDate.Year == Convert.ToDateTime(query).Year);
                var bookDto = books.Select(book => _mapper.Map<GetBookDto>(book));
               // var paginatedResult = await Pagination<GetBookDto>.CreateAsync(bookDto, pageIndex, _pageSize);
                var books1 = new AllBooks { books = bookDto.ToList() };
                var response = new ResponseDto<AllBooks>
                {
                    Data = books1,
                    Message = $"you have successfuly quarried books Published in the year {Convert.ToDateTime(query).Year} search property.",
                    StatusCode = 200,
                    Success = true,
                 
                };
                return response;
            }
            return new ResponseDto<AllBooks>
            {
                Data = null,
                Message = "Not found",
                StatusCode = 400,
                Success = false
            };
        }


    }
}
