using FolderCleaner.Models;
using System.Text.Json;

namespace FolderCleaner.Repositories;

public class WhiteListRepository : IWhiteListRepository
{
    private string _fileName;

    public WhiteListRepository()
    {
        _fileName = Const.FileWhiteListName;
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
        string jsonString = File.ReadAllText(_fileName);
        var whiteList = JsonSerializer.Deserialize<List<string>>(jsonString)!;
        return whiteList;
    }

    private void SaveWhiteListCollection(List<string> whiteList)
    {
        var jsonConverted = JsonSerializer.Serialize(whiteList);
        File.WriteAllText(_fileName, jsonConverted);
    }
}
