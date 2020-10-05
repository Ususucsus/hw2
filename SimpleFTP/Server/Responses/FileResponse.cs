namespace SimpleFTP.Responses
{
    /// <summary>
    /// Represents file response model.
    /// </summary>
    public class FileResponse
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="FileResponse"/> class.
        /// </summary>
        /// <param name="name">File name.</param>
        /// <param name="isDir">Is file directory.</param>
        public FileResponse(string name, bool isDir)
        {
            this.name = name;
            this.isDir = isDir;
        }

        /// <summary>
        /// File name.
        /// </summary>
        private readonly string name;

        /// <summary>
        /// Is file directory.
        /// </summary>
        private readonly bool isDir;

        /// <summary>
        /// Returns file response string representation.
        /// </summary>
        /// <returns>String representation.</returns>
        public override string ToString() => $"{this.name} {this.isDir}";
    }
}