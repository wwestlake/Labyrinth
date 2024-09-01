using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using System.Collections.Generic;

namespace Labyrinth.API.Entities.Storyline
{
    public class Chapter
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string Id { get; set; }

        public string Title { get; set; }
        public string Setting { get; set; }
        public OpeningScene OpeningScene { get; set; }
        public List<PlotDevelopment> PlotDevelopment { get; set; } = new List<PlotDevelopment>();
        public List<string> PlayerRoles { get; set; } = new List<string>();
        public string Conclusion { get; set; }
    }

    public class OpeningScene
    {
        public string Location { get; set; }
        public string Description { get; set; }
    }

    public class PlotDevelopment
    {
        public string Event { get; set; }
        public string Description { get; set; }
        public List<string> KeyPoints { get; set; } = new List<string>();
    }
}
