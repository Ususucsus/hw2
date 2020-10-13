namespace Client
{
    public class FileResponse
    {
        public FileResponse(string fileName, bool isDirectory)
        {
            FileName = fileName;
            IsDirectory = isDirectory;
        }

        public string FileName { get; }
        
        public bool IsDirectory { get; }
    }
}