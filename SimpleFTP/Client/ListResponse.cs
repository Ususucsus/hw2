using System.Collections.Generic;

namespace Client
{
    public class ListResponse
    {
        public ListResponse(List<FileResponse> files, int numberOfFiles)
        {
            Files = files;
            NumberOfFiles = numberOfFiles;
        }

        public List<FileResponse> Files { get; }
        
        public int NumberOfFiles { get; }
    }
}