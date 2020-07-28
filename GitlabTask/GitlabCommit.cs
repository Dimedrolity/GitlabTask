using Newtonsoft.Json;

namespace GitlabTask
{
    public class GitlabCommit
    {
        [JsonProperty(PropertyName = "title")] 
        public string Title { get; }

        [JsonProperty(PropertyName = "created_at")]
        public string CreatedAt { get; }

        public GitlabCommit(string title, string createdAt)
        {
            Title = title;
            CreatedAt = createdAt;
        }

        public override string ToString()
        {
            return Title;
        }
    }
}