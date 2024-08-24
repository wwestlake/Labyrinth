using Labyrinth.API.Entities.Characters;
using Labyrinth.API.Services;
using Labyrinth.API.Utilities;
using Mongo2Go;
using MongoDB.Bson;
using MongoDB.Driver;
using Moq.AutoMock;

namespace Labyrinth.API.UnitTests
{
    public class CharacterServiceTests : IClassFixture<MongoDbFixture>
    {
        private readonly MongoDbFixture _mongoDbFixture;
        private readonly AutoMocker _mocker;
        private readonly CharacterService _characterService;
        private readonly IMongoCollection<Character> _charactersCollection;

        public CharacterServiceTests(MongoDbFixture mongoDbFixture)
        {
            _mongoDbFixture = mongoDbFixture;
            _mocker = new AutoMocker();
            _charactersCollection = _mongoDbFixture.Database.GetCollection<Character>("Characters");
            _characterService = new CharacterService(_mongoDbFixture.Client);
        }

        [Fact]
        public async Task GetCharactersAsync_ShouldReturnAllCharacters()
        {
            // Arrange
            var characters = new List<Character>
            {
                CharacterGenerator.GenerateCharacter("Character1", CharacterClass.Warrior, Race.Human),
                CharacterGenerator.GenerateCharacter("Character2", CharacterClass.Mage, Race.Elf)
            };

            await _charactersCollection.InsertManyAsync(characters);

            // Act
            var result = await _characterService.GetCharactersAsync();

            // Assert
            Assert.NotNull(result);
            Assert.Equal(2, result.Count);
            Assert.Contains(result, c => c.Name == "Character1");
            Assert.Contains(result, c => c.Name == "Character2");
        }

        [Fact]
        public async Task CreateCharacterAsync_ShouldAddCharacterToDatabase()
        {
            // Arrange
            var name = "New Character";
            var characterClass = CharacterClass.Warrior;
            var race = Race.Human;

            // Act
            var result = await _characterService.CreateCharacterAsync(name, characterClass, race);

            var charactersFromDb = await _charactersCollection.Find(c => c.Name == name).FirstOrDefaultAsync();

            // Assert
            Assert.NotNull(result);
            Assert.Equal(name, result.Name);
            Assert.NotNull(charactersFromDb);
            Assert.Equal(name, charactersFromDb.Name);
        }

        [Fact]
        public async Task SubmitCharacterAsync_ShouldUpdateCharacterStats_WhenValidStatsAreSubmitted()
        {
            // Arrange
            var character = CharacterGenerator.GenerateCharacter("Character1", CharacterClass.Warrior, Race.Human);
            await _charactersCollection.InsertOneAsync(character);


            var submittedStats = new Dictionary<string, int>
            {
                { "Health", character.Health.BaseValue },
                { "Mana", character.Mana.BaseValue },
                { "Strength", character.Strength.BaseValue },
                { "Dexterity", character.Dexterity.BaseValue },
                { "Constitution", character.Constitution.BaseValue },
                { "Intelligence", character.Intelligence.BaseValue },
                { "Wisdom", character.Wisdom.BaseValue },
                { "Charisma", character.Charisma.BaseValue },
                { "Luck", character.Luck.BaseValue }
            };

            // Act
            var result = await _characterService.SubmitCharacterAsync(character.Id, submittedStats);

            var updatedCharacter = await _charactersCollection.Find(c => c.Id == character.Id).FirstOrDefaultAsync();

            // Assert
            Assert.True(result.IsSuccess);
            Assert.Equal(character.Health.BaseValue, updatedCharacter.Health.BaseValue);
            Assert.Equal(character.Mana.BaseValue, updatedCharacter.Mana.BaseValue);
        }

        [Fact]
        public async Task SubmitCharacterAsync_ShouldReturnFailure_WhenCharacterNotFound()
        {
            // Arrange
            var submittedStats = new Dictionary<string, int>
            {
                { "Health", 100 }
            };

            // Act
            var result = await _characterService.SubmitCharacterAsync(ObjectId.GenerateNewId().ToString(), submittedStats);

            // Assert
            Assert.False(result.IsSuccess);
            Assert.Equal("Character not found.", result.Errors.First().Message);
        }

        [Fact]
        public async Task SubmitCharacterAsync_ShouldReturnFailure_WhenStatsAreInvalid()
        {
            // Arrange
            var character = CharacterGenerator.GenerateCharacter("Character1", CharacterClass.Warrior, Race.Human);
            await _charactersCollection.InsertOneAsync(character);

            var submittedStats = new Dictionary<string, int>
            {
                { "Health", 100 }
            };

            // Act
            var result = await _characterService.SubmitCharacterAsync(character.Id, submittedStats);

            // Assert
            Assert.False(result.IsSuccess);
            Assert.Equal("Invalid character stats. Possible fraud attempt detected.", result.Errors.First().Message);
        }

        [Fact]
        public async Task UpdateCharacterAsync_ShouldUpdateCharacter_WhenCharacterExists()
        {
            // Arrange
            var character = CharacterGenerator.GenerateCharacter("Character1", CharacterClass.Warrior, Race.Human);
            await _charactersCollection.InsertOneAsync(character);

            var updatedCharacter = CharacterGenerator.GenerateCharacter("UpdatedCharacter", CharacterClass.Warrior, Race.Human);
            updatedCharacter.Id = character.Id; // Ensure we are updating the correct character

            // Act
            await _characterService.UpdateCharacterAsync(character.Id, updatedCharacter);

            var characterFromDb = await _charactersCollection.Find(c => c.Id == character.Id).FirstOrDefaultAsync();

            // Assert
            Assert.NotNull(characterFromDb);
            Assert.Equal("UpdatedCharacter", characterFromDb.Name);
        }

        [Fact]
        public async Task UpdateCharacterAsync_ShouldThrowKeyNotFoundException_WhenCharacterDoesNotExist()
        {
            // Arrange
            var updatedCharacter = CharacterGenerator.GenerateCharacter("UpdatedCharacter", CharacterClass.Warrior, Race.Human);
            updatedCharacter.Id = ObjectId.GenerateNewId().ToString(); // Ensure this ID does not exist in the collection

            // Act & Assert
            await Assert.ThrowsAsync<KeyNotFoundException>(() => _characterService.UpdateCharacterAsync(ObjectId.GenerateNewId().ToString(), updatedCharacter));
        }

        [Fact]
        public async Task GetCharacterByIdAsync_ShouldReturnCharacter_WhenCharacterExists()
        {
            // Arrange
            var character = CharacterGenerator.GenerateCharacter("Character1", CharacterClass.Warrior, Race.Human);
            await _charactersCollection.InsertOneAsync(character);

            // Act
            var result = await _characterService.GetCharacterByIdAsync(character.Id);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(character.Id, result.Id);
            Assert.Equal(character.Name, result.Name);
        }

        [Fact]
        public async Task GetCharacterByIdAsync_ShouldReturnNull_WhenCharacterDoesNotExist()
        {
            // Act
            var result = await _characterService.GetCharacterByIdAsync(ObjectId.GenerateNewId().ToString());

            // Assert
            Assert.Null(result);
        }
    }

    public class MongoDbFixture : IDisposable
    {
        public MongoDbRunner Runner { get; private set; }
        public IMongoClient Client { get; private set; }
        public IMongoDatabase Database { get; private set; }

        public MongoDbFixture()
        {
            Runner = MongoDbRunner.Start();
            Client = new MongoClient(Runner.ConnectionString);
            Database = Client.GetDatabase("Labyrinth");
        }

        public void Dispose()
        {
            Runner.Dispose();
        }
    }
}
