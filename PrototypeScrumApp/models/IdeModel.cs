namespace PrototypeScrumApp.models
{
    public class IdeModel
    {
        public string Ide { get; set; }
        public string FilePath { get; set; }

        public IdeModel() {}

        public IdeModel(string selectedIde, string filePath)
        {
            FilePath = filePath;
            Ide = selectedIde;
        }
    }
}
