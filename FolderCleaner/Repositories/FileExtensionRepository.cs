using static System.Net.Mime.MediaTypeNames;
using System;
using System.Text.Json;
using FolderCleaner.Models;
using System.IO;

namespace FolderCleaner.Repositories;

public class FileExtensionRepository : IFileExtensionRepository
{
    //property
    private string _fileName;


    //konstruktor
    public FileExtensionRepository()
    {
        _fileName = Const.FileExtensionName;
    }

    public void Add(FileExtension fileExtension)
    {
        var fileExtensionList = GetFileExtensionCollection();
        fileExtensionList.Add(fileExtension);
        SaveFileExtensionCollection(fileExtensionList);
    }

    public List<FileExtension> GetAll()
    {
        return GetFileExtensionCollection();
    }

    public void Update(FileExtension fileExtension)
    {
        var fileExtensionList = GetFileExtensionCollection();
        //warunki że takie samo fileExtension i startWith itd.
        if (fileExtensionList.Any(x => x.Extension.Equals(fileExtension.Extension) && 
        x.StartWith.Equals(fileExtension.StartWith) &&
        x.EndWith.Equals(fileExtension.EndWith) &&
        x.Constain.Equals(fileExtension.Constain)))
        {
            var updatedFileExtension = fileExtensionList.First(x => x.Extension.Equals(fileExtension.Extension));
            updatedFileExtension.TargetPath = fileExtension.TargetPath;
            SaveFileExtensionCollection(fileExtensionList);
        }
    }

    private List<FileExtension> GetFileExtensionCollection() 
    {
        string jsonString = File.ReadAllText(_fileName);
        var fileExtensionList = JsonSerializer.Deserialize<List<FileExtension>>(jsonString)!;
        return fileExtensionList;
    }

    private void SaveFileExtensionCollection(List<FileExtension> fileExtensionList)
    {
        var jsonConverted = JsonSerializer.Serialize(fileExtensionList);
        File.WriteAllText(_fileName, jsonConverted);
    }
}
