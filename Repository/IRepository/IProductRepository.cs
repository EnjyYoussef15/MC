using MCSHiPPERS_Task.DTO;
using MCSHiPPERS_Task.Models;
using System.Linq.Expressions;

namespace MCSHiPPERS_Task.Repository.IProduct
{
    public interface IProductRepository <T> where T : class
    {
       
       

        void Add(T DTO);

        void Update(T DTO);

        void Delete(int id);

        Task<T> GetOne(int id);

        Task<IEnumerable<T>> GetAll();

        Task<IEnumerable<T>> GetAll(int skip, int take);
        Task<IEnumerable<T>> Search(Expression<Func<T, bool>> match);


    }
}
