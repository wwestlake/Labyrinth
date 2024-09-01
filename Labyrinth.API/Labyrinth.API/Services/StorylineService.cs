using Labyrinth.API.Entities.Storyline;
using MongoDB.Driver;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Labyrinth.API.Services
{
    public interface IStorylineService
    {
        Task<List<Chapter>> GetChaptersAsync();
        Task<Chapter> GetChapterByIdAsync(string id);
        Task CreateChapterAsync(Chapter chapter);
        Task UpdateChapterAsync(string id, Chapter updatedChapter);
        Task DeleteChapterAsync(string id);

        // Add similar methods for other entities (Character, Event, PlayerAction, UserProgress)
    }

    public class StorylineService : IStorylineService
    {
        private readonly IMongoCollection<Chapter> _chaptersCollection;

        public StorylineService(IMongoClient mongoClient)
        {
            var database = mongoClient.GetDatabase("Labyrinth");
            _chaptersCollection = database.GetCollection<Chapter>("Chapters");
        }

        public async Task<List<Chapter>> GetChaptersAsync()
        {
            return await _chaptersCollection.Find(_ => true).ToListAsync();
        }

        public async Task<Chapter> GetChapterByIdAsync(string id)
        {
            return await _chaptersCollection.Find(chapter => chapter.Id == id).FirstOrDefaultAsync();
        }

        public async Task CreateChapterAsync(Chapter chapter)
        {
            await _chaptersCollection.InsertOneAsync(chapter);
        }

        public async Task UpdateChapterAsync(string id, Chapter updatedChapter)
        {
            await _chaptersCollection.ReplaceOneAsync(chapter => chapter.Id == id, updatedChapter);
        }

        public async Task DeleteChapterAsync(string id)
        {
            await _chaptersCollection.DeleteOneAsync(chapter => chapter.Id == id);
        }

        // Implement similar methods for other entities (Character, Event, PlayerAction, UserProgress)
    }
}
