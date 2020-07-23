namespace GitlabTask.Interfaces
{
    public interface IJsonConverter
    {
        public GitlabCommit[] ConvertJsonToCommits(string json);
    }
}