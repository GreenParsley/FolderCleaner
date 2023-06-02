namespace FolderCleaner.Models;

public class FileExtension
{
    //property
    public string TargetPath { get; set; }
    public string Extension { get; set; }
    public string StartWith { get; set; }
    public string EndWith { get; set; }
    public string Contain { get; set; }

    //konstruktor
    public FileExtension(string targetPath, string extension, string startWith, string endWith, string contain)
    {
        //dodać wartości do konstruktora
        TargetPath = targetPath;
        Extension = extension;
        StartWith = startWith;
        EndWith = endWith;
        Contain = contain;
    }
}
