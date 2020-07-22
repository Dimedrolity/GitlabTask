using Newtonsoft.Json;

namespace GitlabTask
{
    public class JsonParser : IJsonParser
    {
        public Commit[] ConvertJsonToCommits(string json)
        {
            return JsonConvert.DeserializeObject<Commit[]>(json);
        }
    }
}