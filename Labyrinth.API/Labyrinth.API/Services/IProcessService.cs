using Labyrinth.API.Entities.Processes;
using MongoDB.Bson;

namespace Labyrinth.API.Services
{
    public interface IProcessService
    {
        Task<Process> CreateProcessAsync(Process process);
        Task<Process> GetProcessByIdAsync(ObjectId id);
        Task<IEnumerable<Process>> GetAllProcessesAsync();
        Task<Process> UpdateProcessAsync(ObjectId id, Process updatedProcess);
        Task<bool> DeleteProcessAsync(ObjectId id);
    }
}
