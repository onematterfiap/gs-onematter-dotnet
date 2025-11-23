using System.Collections.Generic;
using System.Threading.Tasks;
using OneMatter.Models;

namespace OneMatter.Data.Repositories
{
    public interface IJobRepository
    {
        Task<IEnumerable<Job>> GetAllAsync();
        Task<Job?> GetByIdAsync(int id);
        Task AddAsync(Job job);
        Task UpdateAsync(Job job);
        Task DeleteAsync(Job job);
    }
}