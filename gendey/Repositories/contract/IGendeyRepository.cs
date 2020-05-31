using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace gendey.Repositories.contract
{
    public interface IGendeyRepository<T>
    {
        Task<T> GetAll();

        Task<T> Get(int id);

        Task<T> Update(int id, object obj);

        bool Exists(int id);

        Task<T> Add(object obj);

        Task<T> Delete(object obj);
    }
}
