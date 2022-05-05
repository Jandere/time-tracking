using Application.Common.Models;
using Microsoft.AspNetCore.Http;

namespace Application.Common.Interfaces;

public interface IFileService
{
    Task<FileUploadResponse> UploadFile(FileUploadRequest request);
}