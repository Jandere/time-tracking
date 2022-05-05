using Microsoft.AspNetCore.Http;

namespace Application.Common.Models;

public class FileUploadRequest
{
    public IFormFile File { get; set; } = null!;
}