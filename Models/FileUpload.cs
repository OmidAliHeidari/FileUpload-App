namespace FileUploadApp.Models;

public class FileUpload
{
    public int FileUploadId { get; set; }
    public string UploaderName { get; set; }
    public string FileName { get; set; }
    public string FileType { get; set; }
    public byte[] FileData { get; set; }
    public string Description { get; set; } 
    public DateTime UploadTime { get; set; }

}