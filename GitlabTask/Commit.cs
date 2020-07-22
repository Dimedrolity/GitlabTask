using Newtonsoft.Json;

namespace GitlabTask
{
    public class Commit
    {
        [JsonProperty(PropertyName = "title")] public string Title { get; }

        [JsonProperty(PropertyName = "message")]
        public string Message { get; }

        [JsonProperty(PropertyName = "created_at")]
        public string CreatedAt { get; }

        [JsonProperty(PropertyName = "author_name")]
        public string AuthorName { get; }

        public Commit(string title, string message, string authorName, string createdAt)
        {
            Title = title;
            Message = message;
            AuthorName = authorName;
            CreatedAt = createdAt;
        }
    }
}