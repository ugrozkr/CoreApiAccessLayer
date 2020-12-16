using Microsoft.EntityFrameworkCore;
using System;

namespace Wall.Service.DataAccessLayer.Interface
{
    public interface IDbContext : IDisposable
    {
        DbContext Instance { get; }
    }
}
