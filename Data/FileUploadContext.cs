using Microsoft.EntityFrameworkCore;
using FileUploadApp.Models;

namespace FileUploadApp.Data;

public class FileUploadContext : DbContext
{
    public FileUploadContext(DbContextOptions<FileUploadContext> options) : base(options)
    {
    }

    public DbSet<FileUpload> FileUploads { get; set; }
}