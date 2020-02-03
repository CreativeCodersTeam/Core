using System;
using JetBrains.Annotations;

namespace CreativeCoders.Data
{
    [PublicAPI]
    public interface IUnitOfWork : IDisposable
    {
        void Commit();

        void Rollback();
    }
}