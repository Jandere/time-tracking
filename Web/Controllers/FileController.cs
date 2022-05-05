using Application.Common.Interfaces;
using Application.Common.Models;
using Microsoft.AspNetCore.Mvc;

namespace Web.Controllers;

public class FileController : BaseApiController
{
    private readonly IFileService _fileService;

    public FileController(IFileService fileService)
    {
        _fileService = fileService;
    }

    [HttpPost]
    public async Task<ActionResult<FileUploadResponse>> UploadFile([FromForm] FileUploadRequest request)
    {
        return await _fileService.UploadFile(request);
    }
}