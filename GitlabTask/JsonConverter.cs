using GitlabTask.Interfaces;
using Newtonsoft.Json;

namespace GitlabTask
{
    public class JsonConverter : IJsonConverter
    {
        public GitlabCommit[] ConvertJsonToCommits(string json)
        {
            return JsonConvert.DeserializeObject<GitlabCommit[]>(json);
        }
    }
}