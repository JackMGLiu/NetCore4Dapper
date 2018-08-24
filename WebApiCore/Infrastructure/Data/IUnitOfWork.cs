using System;

namespace WebApiCore.Infrastructure.Data
{
    public interface IUnitOfWork : IDisposable
    {
        void SaveChanges();
    }
}