namespace GitlabTask
{
    public interface IJsonConverter
    {
        public Commit[] ConvertJsonToCommits(string json);
    }
}