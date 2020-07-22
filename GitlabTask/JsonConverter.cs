using Newtonsoft.Json;

namespace GitlabTask
{
    public class JsonConverter : IJsonConverter
    {
        public Commit[] ConvertJsonToCommits(string json)
        {
            return JsonConvert.DeserializeObject<Commit[]>(json);
        }
    }
}