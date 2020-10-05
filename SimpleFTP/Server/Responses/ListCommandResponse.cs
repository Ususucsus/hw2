using System.Collections.Generic;

namespace SimpleFTP.Responses
{
    /// <summary>
    /// Represents class for list command response model.
    /// </summary>
    public class ListCommandResponse
    {
        /// <summary>
        /// Initalizes a new instance of the <see cref="ListCommandResponse"/> class.
        /// </summary
        /// <param name="files">List of file response models.</param>
        public ListCommandResponse(List<FileResponse> files)
        {
            this.size = files.Count;
            this.files = files;
        }

        /// <summary>
        /// Number of files.
        /// </summary>
        private readonly int size;

        /// <summary>
        /// List of files.
        /// </summary>
        private readonly List<FileResponse> files;

        /// <summary>
        /// Returns list command response string representation.
        /// </summary>
        /// <returns>String representation.</returns>
        public override string ToString() => $"{this.size} {string.Join(" ", this.files)}";
    }
}