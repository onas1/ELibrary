using AutoMapper;
using ELibrary.Core.Abstractions;
using ELibrary.Data.Repositories.Abstractions;
using ELibrary.Data.Repositories.Implementations;
using ELibrary.Dtos;
using Microsoft.Extensions.Configuration;
using System.Linq;
using System.Threading.Tasks;

namespace ELibrary.Core.Implementations
{
    public class BookService : IBookService
    {
        private readonly IMapper _mapper;
        private readonly IBookRepository _bookRepo;
        private readonly IConfiguration _config;

        public BookService(IMapper mapper, IBookRepository BookRepo, IConfiguration config)
        {
            _mapper = mapper;
            _bookRepo = BookRepo;
            _config = config;
        }

        public ResponseDto<AllBooks> GetByCategory(string CategoryName, int pageIndex=1)
        {
           
           
                

            var books = _bookRepo.GetByCategoryName(CategoryName);

            if (books == null)
            {
                var response1 = new ResponseDto<AllBooks>
                {
                    Data = null,
                    Success = true,
                    StatusCode = 404,

                };
            }

            var bookDto = books.Select(book => _mapper.Map<GetBookDto>(book));
            var books1 = new AllBooks { books = bookDto.ToList() };
          

            var response = new ResponseDto<AllBooks>
            {
                Data = books1,
                Success = true,
                StatusCode = 200,

            };

            return response;
        }

        public ResponseDto<AllBooks> SortByDate(int pageIndex = 1)
        {
            var books = _bookRepo.Get().OrderByDescending(e => e.PublishedDate).Take(8);

            if (books == null)
            {
                var response1 = new ResponseDto<AllBooks>
                {
                    Data = null,
                    Success = true,
                    StatusCode = 404,

                };
            }

            var bookDto = books.Select(book => _mapper.Map<GetBookDto>(book));
            var books1 = new AllBooks { books = bookDto.ToList() };

            var response = new ResponseDto<AllBooks>
            {
                Data = books1,
                Success = true,
                StatusCode = 200,

            };

            return response;
        }


        public ResponseDto<AllBooks> SortByViews(int pageIndex = 1)
        {

            var books = _bookRepo.Get().OrderByDescending(e => e.Views).Take(8);
            if (books == null)
            {
                var response1 = new ResponseDto<AllBooks>
                {
                    Data = null,
                    Success = true,
                    StatusCode = 404,

                };
            }

            var bookDto = books.Select(book => _mapper.Map<GetBookDto>(book));
            var books1 = new AllBooks { books = bookDto.ToList() };

            var response = new ResponseDto<AllBooks>
            {
                Data = books1,
                Success = true,
                StatusCode = 200,

            };

            return response;
        }


    }
}
