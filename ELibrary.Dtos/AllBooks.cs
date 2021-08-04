using ELibrary.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace ELibrary.Dtos
{
    public class AllBooks
    {
        public List<GetBookDto> books { get; set; }
    }
}
