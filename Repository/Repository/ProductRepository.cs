
using MCSHiPPERS_Task.DTO;
using MCSHiPPERS_Task.Models;
using MCSHiPPERS_Task.Repository.IProduct;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Linq.Expressions;

namespace MCSHiPPERS_Task.Repository.Repository
{
    public class ProductRepository <T> : IProductRepository <T> where T : class
    {
        private readonly DataContext _context;

        public ProductRepository(DataContext context)
        {
            _context = context;
        }
        public void Add(T DTO)
        {
            _context.Set<T>().Add(DTO);
            _context.SaveChanges();
        }

        public async void Delete(int id)
        {
            T product = await _context.Set<T>().FindAsync(id);
            if (product != null)
            {
                _context.Set<T>().Remove(product);
                _context.SaveChanges();
            }

        }


        public void Update(T DTO)
        {
            _context.Set<T>().Update(DTO);
            _context.SaveChanges();
        }

        public async Task<T> GetOne(int id)
        {
            return await _context.Set<T>().FindAsync(id);
        }
        public async Task<IEnumerable<T>> GetAll()
        {
            return await _context.Set<T>().ToListAsync();
        }
       

        public async Task<IEnumerable<T>> GetAll( int skip, int take)
        {
            return await _context.Set<T>().Skip((skip-1)*take).Take(take).ToListAsync();
        }


        public async Task<IEnumerable<T>> Search(Expression<Func<T, bool>> match)
        {
            return await _context.Set<T>().Where(match).ToListAsync();
        }





        //[HttpGet("{id}", Name = "GetOneDept")]
        //public IActionResult GetDeptWithEmpNames(int id)
        //{
        //    Cat DeptEmpDTO = new DeptWithListEmpInfo();
        //    Department Dept = context.Departments.Include(e => e.Employees).FirstOrDefault(d => d.ID == id);
        //    DeptEmpDTO.DeptName = Dept.Name;
        //    DeptEmpDTO.DeptId = Dept.ID;

        //    foreach (var item in Dept.Employees)
        //    {
        //        DeptEmpDTO.EmployeeNames.Add(item.Name);
        //    }

        //    return Ok(DeptEmpDTO);


        //}


        //public void Update(T item)
        //{
        //   _context.Set<T>().Update(item);
        //    _context.SaveChanges();
        //}
    }
}
