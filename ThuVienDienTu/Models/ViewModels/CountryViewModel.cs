using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ThuVienDienTu.Models.ViewModels
{
    public class CountryViewModel
    {
        public Country Country { get; set; } 
        public List<Country> Countries { get; set; }
        public int Total { get; set; }
    }
}
