using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ThuVienDienTu.Models;

namespace ThuVienDienTu.DesignPatterns.RepositoryPatterns
{
    public interface IUnitOfWork
    {
        IRepository<Country> Countries { get; }
        IRepository<Genres> Genres { get; }
        void Commit();
    }
}
