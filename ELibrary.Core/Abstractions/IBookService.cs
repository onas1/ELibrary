using ELibrary.Data.Repositories.Implementations;
using ELibrary.Dtos;
using System.Threading.Tasks;

namespace ELibrary.Core.Abstractions
{
   public interface IBookService
   {
        public ResponseDto<AllBooks> GetByCategory(string CategoryName, int pageSize);

        public ResponseDto<AllBooks> SortByDate(int pageIndex = 1);
        public ResponseDto<AllBooks> SortByViews(int pageIndex = 1);
    }
}
