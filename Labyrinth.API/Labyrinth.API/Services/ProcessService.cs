using Labyrinth.API.Entities.Processes;
using MongoDB.Bson;
using MongoDB.Driver;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Labyrinth.API.Services
{
    public class ProcessService : IProcessService
    {
        private readonly IMongoCollection<Process> _processCollection;

        public ProcessService(IMongoClient mongoClient)
        {
            var database = mongoClient.GetDatabase("Labyrinth"); // Get the Labyrinth database
            _processCollection = database.GetCollection<Process>("Processes"); // Initialize the Process collection
        }

        // Create a new process document in the database
        public async Task<Process> CreateProcessAsync(Process process)
        {
            if (process == null)
                throw new ArgumentNullException(nameof(process));

            await _processCollection.InsertOneAsync(process);
            return process;
        }

        // Retrieve a process document by its unique identifier
        public async Task<Process> GetProcessByIdAsync(ObjectId id)
        {
            return await _processCollection.Find(p => p.Id == id).FirstOrDefaultAsync();
        }

        // Retrieve all process documents from the collection
        public async Task<IEnumerable<Process>> GetAllProcessesAsync()
        {
            return await _processCollection.Find(_ => true).ToListAsync();
        }

        // Update an existing process document with new data
        public async Task<Process> UpdateProcessAsync(ObjectId id, Process updatedProcess)
        {
            if (updatedProcess == null)
                throw new ArgumentNullException(nameof(updatedProcess));

            // Replace the existing document with the updated one
            var result = await _processCollection.ReplaceOneAsync(p => p.Id == id, updatedProcess);

            if (result.IsAcknowledged && result.ModifiedCount > 0)
            {
                return updatedProcess;
            }

            // Return null if no document was updated
            return null;
        }

        // Delete a process document by its unique identifier
        public async Task<bool> DeleteProcessAsync(ObjectId id)
        {
            var result = await _processCollection.DeleteOneAsync(p => p.Id == id);
            return result.IsAcknowledged && result.DeletedCount > 0;
        }
    }
}
