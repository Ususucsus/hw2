namespace Client
{
    /// <summary>
    /// Represents file response class.
    /// </summary>
    public class FileResponse
    {
        /// <summary>
        /// Initalizes a new instance of the <see cref="FileResponse"/> class.
        /// </summary>
        /// <param name="fileName">File name.</param>
        /// <param name="isDirectory">Is file is directory.</param>
        public FileResponse(string fileName, bool isDirectory)
        {
            FileName = fileName;
            IsDirectory = isDirectory;
        }
        
        /// <summary>
        /// File name.
        /// </summary>
        public string FileName { get; }
        
        /// <summary>
        /// Is file is directory.
        /// </summary>
        public bool IsDirectory { get; }

        public override string ToString()
        {
            return $"name: {FileName}, is directory: {IsDirectory}";
        }
    }
}