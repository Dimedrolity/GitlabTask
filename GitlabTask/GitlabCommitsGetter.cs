namespace GitlabTask
{
    public class GitlabCommitsGetter : ICommitsGetter
    {
        private readonly IJsonParser _jsonParser;

        public GitlabCommitsGetter(IJsonParser jsonParser)
        {
            _jsonParser = jsonParser;
        }

        public Commit[] GetCommitsOfProjectWithId(string id)
        {
            //send http-get request to Gitlab api
            //GET /projects/:id/repository/commits
            var json = "httpclient.getAsync('/projects/:id/repository/commits)'";
            return _jsonParser.ConvertJsonToCommits(json);
        }
    }
}