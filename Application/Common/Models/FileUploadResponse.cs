namespace Application.Common.Models;

public class FileUploadResponse
{
    public FileUploadResponse(string path)
    {
        Path = path;
    }

    public string Path { get; set; }
}