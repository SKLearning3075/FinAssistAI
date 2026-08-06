namespace FinAssistAI.Api.Models
{
    public class UploadDocumentRequest
    {
        public string UserId { get; set; } = string.Empty;

        public IFormFile File { get; set; } = null!;
    }
}
