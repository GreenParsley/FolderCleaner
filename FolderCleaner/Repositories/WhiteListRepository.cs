using FolderCleaner.Models;
using System.IO.Abstractions;
using System.Text.Json;

namespace FolderCleaner.Repositories;

public class WhiteListRepository : IWhiteListRepository
{
    private string _fileName;
    private IFileSystem _fileSystem;

    public WhiteListRepository(IFileSystem fileSystem)
    {
        _fileName = Const.FileWhiteListName;
        _fileSystem = fileSystem;
    }

    public void Add(string newFileName)
    {
        var whiteList = GetWhiteListCollection();
        whiteList.Add(newFileName);
        SaveWhiteListCollection(whiteList);
    }

    public List<string> GetAll()
    {
        return GetWhiteListCollection();
    }

    private List<string> GetWhiteListCollection()
    {
        string jsonString = _fileSystem.File.ReadAllText(_fileName);
        var whiteList = JsonSerializer.Deserialize<List<string>>(jsonString)!;
        return whiteList;
    }

    private void SaveWhiteListCollection(List<string> whiteList)
    {
        var jsonConverted = JsonSerializer.Serialize(whiteList);
        _fileSystem.File.WriteAllText(_fileName, jsonConverted);
    }
}
