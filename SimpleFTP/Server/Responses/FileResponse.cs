namespace SimpleFTP.Responses
{
    public class FileResponse
    {
        public FileResponse(string name, bool isDir)
        {
            Name = name;
            IsDir = isDir;
        }
        
        private string Name { get; set; }
        
        private bool IsDir { get; set; }

        public override string ToString() => $"{this.Name} {this.IsDir}";
    }
}