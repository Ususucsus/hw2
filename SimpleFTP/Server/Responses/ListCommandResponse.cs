using System.Collections.Generic;

namespace SimpleFTP.Responses
{
    public class ListCommandResponse
    {
        public ListCommandResponse(List<FileResponse> files)
        {
            Size = files.Count;
            Files = files;
        }
        
        private int Size { get; }
        
        private List<FileResponse> Files { get; }

        public override string ToString() => $"{this.Size} ({string.Join(") (", this.Files)})";
    }
}