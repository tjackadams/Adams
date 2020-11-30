using System.Collections.Generic;

namespace WebBlazor.Client.Models
{
    public record BadRequestResponse
    {
        public string Detail { get; init; }
        public Dictionary<string, List<string>> Errors { get; init; }
        public string Instance { get; init; }
        public int Status { get; init; }
        public string Title { get; init; }
    }
}