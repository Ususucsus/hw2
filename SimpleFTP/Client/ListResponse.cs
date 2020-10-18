using System.Collections.Generic;

namespace Client
{
    /// <summary>
    /// Represents list response class.
    /// </summary>
    public class ListResponse
    {
        /// <summary>
        /// Initalizes a new instance of the <see cref="ListResponse"/> class.
        /// </summary>
        /// <param name="files">List of files responses</param>
        /// <param name="numberOfFiles">Number of files.</param>
        public ListResponse(List<FileResponse> files, int numberOfFiles)
        {
            Files = files;
            NumberOfFiles = numberOfFiles;
        }

        /// <summary>
        /// List of files responses.
        /// </summary>
        public List<FileResponse> Files { get; }
        
        /// <summary>
        /// Number of files.
        /// </summary>
        public int NumberOfFiles { get; }

        public override string ToString()
        {
            return $"count: {NumberOfFiles}\n{string.Join("\n", Files)}";
        }
    }
}