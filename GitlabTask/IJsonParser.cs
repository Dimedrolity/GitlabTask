namespace GitlabTask
{
    public interface IJsonParser
    {
        public Commit[] ConvertJsonToCommits(string json);
    }
}