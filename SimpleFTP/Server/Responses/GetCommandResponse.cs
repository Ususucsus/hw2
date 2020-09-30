using System;
using System.Runtime.CompilerServices;

namespace SimpleFTP.Responses
{
    public class GetCommandResponse
    {
        public GetCommandResponse(byte[] content)
        {
            Size = content.Length;
            Content = content;
        }

        private long Size { get; }
        
        private byte[] Content { get; }

        public override string ToString() => $"{this.Size} {Convert.ToBase64String(this.Content)}";
    }
}