using static System.Net.Mime.MediaTypeNames;
using System;
using System.Text.Json;
using FolderCleaner.Models;
using System.IO;
using System.IO.Abstractions;

namespace FolderCleaner.Repositories;

public class FileExtensionRepository : IFileExtensionRepository
{
    //property
    private string _fileName;
    private IFileSystem _fileSystem;


    //konstruktor
    public FileExtensionRepository(IFileSystem fileSystem)
    {
        _fileName = Const.FileExtensionName;
        _fileSystem = fileSystem;
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
        if (fileExtensionList.Any(x => x.Extension.Equals(fileExtension.Extension) && 
        x.StartWith.Equals(fileExtension.StartWith) &&
        x.EndWith.Equals(fileExtension.EndWith) &&
        x.Contain.Equals(fileExtension.Contain)))
        {
            var updatedFileExtension = fileExtensionList.First(x => x.Extension.Equals(fileExtension.Extension));
            updatedFileExtension.TargetPath = fileExtension.TargetPath;
            SaveFileExtensionCollection(fileExtensionList);
        }
    }

    private List<FileExtension> GetFileExtensionCollection() 
    {
        string jsonString = _fileSystem.File.ReadAllText(_fileName);
        var fileExtensionList = JsonSerializer.Deserialize<List<FileExtension>>(jsonString)!;
        return fileExtensionList;
    }

    private void SaveFileExtensionCollection(List<FileExtension> fileExtensionList)
    {
        var jsonConverted = JsonSerializer.Serialize(fileExtensionList);
        _fileSystem.File.WriteAllText(_fileName, jsonConverted);
    }
}
