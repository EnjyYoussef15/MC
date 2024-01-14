using MCSHiPPERS_Task.DTO;
using MCSHiPPERS_Task.Models;
using Microsoft.AspNetCore.Identity;

namespace MCSHiPPERS_Task.Repository.IRepository
{
    public interface IAccountRepository
    {

        Task<User> GetUserById(string id);
        Task<IEnumerable<User>> GetAllUsers();
      
    }

}
