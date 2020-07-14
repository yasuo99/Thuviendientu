using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ThuVienDienTu.Models;

namespace ThuVienDienTu.DesignPatterns.BuilderPatterns
{
    public class AccountDirector
    {
        private IAccountBuilder _builder;
        public AccountDirector(IAccountBuilder builder)
        {
            _builder = builder;
        }
        public void Construct()
        {
            _builder.BuildInfor();
            _builder.BuildDate();
            _builder.BuildAccount();
        }
        public ApplicationUser GetAccount()
        {
            return _builder.GetAccount();
        }
    }
}
