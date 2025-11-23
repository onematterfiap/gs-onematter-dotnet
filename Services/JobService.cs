using OneMatter.Data.Repositories;
using OneMatter.Models;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace OneMatter.Services
{
    public class JobService
    {
        private readonly IJobRepository _repository;

        public JobService(IJobRepository repository)
        {
            _repository = repository;
        }

        public async Task<IEnumerable<Job>> GetAllJobsAsync()
        {
            return await _repository.GetAllAsync();
        }

        public async Task<Job?> GetJobByIdAsync(int id)
        {
            return await _repository.GetByIdAsync(id);
        }

        public async Task CreateJobAsync(string title, string description, string location)
        {
            // Regra de negócio: A criação da entidade Job já valida os dados pelo construtor
            var newJob = new Job(title, description, location);

            await _repository.AddAsync(newJob);
        }

        public async Task UpdateJobAsync(int id, string title, string description, string location)
        {
            var job = await _repository.GetByIdAsync(id);

            if (job == null)
            {
                throw new KeyNotFoundException("Vaga não encontrada.");
            }

            // Regra de negócio: O método UpdateDetails do domínio protege as invariantes
            // (ex: não permitir update em vaga fechada)
            job.UpdateDetails(title, description, location);

            await _repository.UpdateAsync(job);
        }

        public async Task CloseJobAsync(int id)
        {
            var job = await _repository.GetByIdAsync(id);

            if (job != null)
            {
                job.CloseJob();
                await _repository.UpdateAsync(job);
            }
        }

        public async Task DeleteJobAsync(int id)
        {
            var job = await _repository.GetByIdAsync(id);

            if (job != null)
            {
                await _repository.DeleteAsync(job);
            }
        }
    }
}