namespace GitlabTask
{
    public interface ICommitsGetter
    {
        public Commit[] GetCommitsOfProjectWithId(string id);
    }
}