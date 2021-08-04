using System.Threading.Tasks;
using ELibrary.Data.Repositories.Implementations;
using ELibrary.Dtos;
using ELibrary.Models;

namespace ELibrary.Core.Abstractions
{
    public interface IBookServices
    {
        ResponseDto<AllBooks> GetAll(int pageIndex = 1);
        Task<ResponseDto<bool>> DeleteById(int bookId);
        Task<ResponseDto<AddBookResponseDto>> AddBook(AddBookDto model);
        Task<ResponseDto<Book>> UpdateBook(UpdateBookDto model);
        Task<ResponseDto<Pagination<GetBookDto>>> GetBookByTitle(BookByTitleResourceParameters bookResource);
        Task<ResponseDto<Book>> GetBookById(string Id);
        public Task<ResponseDto<GetBookDto>> UpdatePhotoBook(AddPhotoDto photo);
        //public Task<ResponseDto<Pagination<GetBookDto>>> GetBookByCategory(string categoryName, int pageIndex);
        ResponseDto<AllBooks> GetBookBySearchTerm(string query, string searchProperty, int pageIndex);


    }
}
