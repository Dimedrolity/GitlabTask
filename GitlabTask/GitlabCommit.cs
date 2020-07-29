using System;
using Newtonsoft.Json;

namespace GitlabTask
{
    public class GitlabCommit : IComparable<GitlabCommit>
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
        
        public int CompareTo(GitlabCommit other)
        {
            return string.Compare(CreatedAt, other.CreatedAt, StringComparison.Ordinal);
        }

        public override string ToString()
        {
            return Title;
        }
    }
}