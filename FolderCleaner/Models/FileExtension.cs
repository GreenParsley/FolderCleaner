namespace FolderCleaner.Models;

public class FileExtension
{
    public string TargetPath { get; set; }
    public string Extension { get; set; }

    //konstruktory
    public FileExtension(string targetPath, string extension)
    {
        TargetPath = targetPath;
        Extension = extension;
    }
}
