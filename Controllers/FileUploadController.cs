using FileUploadApp.Data;
using FileUploadApp.Models;
using Microsoft.AspNetCore.Mvc;

namespace FileUploadApp.Controllers;

public class FileUploadController : Controller
{
    private readonly FileUploadContext _context;
    public FileUploadController(FileUploadContext context)
    {
        _context = context;
    }
    public IActionResult Index()
    {
        return View();
    }

    [HttpGet]
    public IActionResult UploadFile()
    {
        return View();
    }

    [HttpPost]
    public async Task<IActionResult> UploadFile(FileUpload model, IFormFile file)
    {
        if (file != null)
        {
            using (var memoryStream = new MemoryStream())
            {
                await file.CopyToAsync(memoryStream);
                model.FileName = file.FileName;
                model.FileType = file.ContentType;
                model.FileData = memoryStream.ToArray();
                model.UploadTime = DateTime.Now;

                _context.FileUploads.Add(model);
                await _context.SaveChangesAsync();
            }
            return RedirectToAction("Index");
        }
        return RedirectToAction("UploadFile");
    }

    [HttpGet]
    public IActionResult UploadedFiles()
    {
        var files = _context.FileUploads.ToList();

        return View(files);
    }

    [HttpGet]
    public IActionResult DownloadFile(int id)
    {
        var file = _context.FileUploads.FirstOrDefault(f => f.FileUploadId == id);

        if (file == null)
        {
            return NotFound();
        }

        return File(file.FileData, file.FileType, file.FileName);
    }

    [HttpGet]
    public IActionResult UpdateFile(int id)
    {
        var file = _context.FileUploads.FirstOrDefault(f => f.FileUploadId == id);

        if (file == null)
        {
            return NotFound();
        }

        return View(file);
    }

    [HttpPost]
    public async Task<IActionResult> UpdateFile(int id, FileUpload model, IFormFile file)
    {
        var getFile = _context.FileUploads.FirstOrDefault(f => f.FileUploadId == id);

        if (getFile == null)
        {
            return NotFound();
        }

        getFile.UploaderName = model.UploaderName;
        getFile.Description = model.Description;

        if (file != null && file.Length > 0)
        {
            using (var memoryStream = new MemoryStream())
            {
                await file.CopyToAsync(memoryStream);
                getFile.FileData = memoryStream.ToArray();
                getFile.FileName = file.FileName;
                getFile.FileType = file.ContentType;
            }
        }

        await _context.SaveChangesAsync();
        return RedirectToAction("UploadedFiles");
    }

    [HttpPost]
    public async Task<IActionResult> DeleteFile(int id)
    {
        var file = _context.FileUploads.FirstOrDefault(f => f.FileUploadId == id);

        if (file == null)
        {
            return NotFound();
        }

        _context.FileUploads.Remove(file);
        await _context.SaveChangesAsync();

        return RedirectToAction("UploadedFiles");
    }
}