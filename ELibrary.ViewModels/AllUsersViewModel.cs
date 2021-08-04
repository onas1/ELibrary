using System;
using System.Collections.Generic;
using System.Text;

namespace ELibrary.ViewModels
{
    public class AllUsersViewModel
    {
        public List<UserViewModel> UserViewModels { get; set; } = new List<UserViewModel>();
        public SearchViewModel searchViewModel { get; set; } = new SearchViewModel();
        public DeleteViewModel DeleteViewModel { get; set; } = new DeleteViewModel();
        public bool HasNext { get; set; }
        public bool HasPrevious { get; set; }
        public int PageIndex { get; set; }
    }
}
