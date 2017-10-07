using System;
using System.Threading.Tasks;

namespace Kleu.Utility.Data
{
    public interface IDeleteRepository<in TKey> : IRepository
    {
        Task<bool> Delete(TKey id);
    }
}
