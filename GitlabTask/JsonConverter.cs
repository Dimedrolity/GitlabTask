using System.Collections.Generic;
using GitlabTask.Interfaces;
using Newtonsoft.Json;

namespace GitlabTask
{
    public class JsonConverter : IJsonConverter
    {
        public IEnumerable<GitlabCommit> ConvertJsonToCommits(string json)
        {
            return JsonConvert.DeserializeObject<GitlabCommit[]>(json);
        }
    }
}