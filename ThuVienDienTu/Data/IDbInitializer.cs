﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ThuVienDienTu.Data
{
    public interface IDbInitializer
    {
        Task Initialize();
    }
}
