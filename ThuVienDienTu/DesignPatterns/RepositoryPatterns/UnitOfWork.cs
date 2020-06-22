using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ThuVienDienTu.Data;
using ThuVienDienTu.Models;

namespace ThuVienDienTu.DesignPatterns.RepositoryPatterns
{
    public class UnitOfWork : IUnitOfWork
    {
        private ApplicationDbContext _db;
        private BaseRepository<Country> _country;
        private BaseRepository<Genres> _genres;
        public UnitOfWork(ApplicationDbContext db)
        {
            _db = db;
        }
        public IRepository<Country> Countries {
            get
            {
                return _country ?? (_country = new BaseRepository<Country>(_db));
            }
        }

        public IRepository<Genres> Genres {
            get
            {
                return _genres ?? (_genres = new BaseRepository<Genres>(_db));
            }
        }

        public void Commit()
        {
            _db.SaveChanges();
        }
    }
}
