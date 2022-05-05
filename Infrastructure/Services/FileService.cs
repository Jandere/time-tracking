using Application.Common.Exceptions;
using Application.Common.Interfaces;
using Application.Common.Models;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;

namespace Infrastructure.Services;

public class FileService : IFileService
{
    private readonly IWebHostEnvironment _webHostEnvironment;

    public FileService(IWebHostEnvironment webHostEnvironment)
    {
        _webHostEnvironment = webHostEnvironment;
    }
    
    public async Task<FileUploadResponse> UploadFile(FileUploadRequest request)
    {
        var file = request?.File;

        if (file == null)
            return new FileUploadResponse("");
        
        if (file.Length <= 0)
            throw new BusinessLogicException("File is empty");

        var path = $"Files{Path.DirectorySeparatorChar}{file.FileName}";

        var absolutePath = Path.Combine(_webHostEnvironment.WebRootPath, path);

        await using var fileStream = new FileStream(absolutePath, FileMode.Create);
        
        await file.CopyToAsync(fileStream);

        return new FileUploadResponse($"{Path.DirectorySeparatorChar}{path}");
    }
}