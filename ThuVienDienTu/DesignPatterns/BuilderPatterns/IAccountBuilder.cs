using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ThuVienDienTu.Models;

namespace ThuVienDienTu.DesignPatterns.BuilderPatterns
{
    public interface IAccountBuilder
    {
        void BuildInfor();
        void BuildDate();
        Task BuildAccount();
        ApplicationUser GetAccount();
    }
}
