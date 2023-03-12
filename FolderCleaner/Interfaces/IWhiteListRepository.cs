namespace FolderCleaner.Interfaces;

public interface IWhiteListRepository
{
    void Add(string fileName);
    List<string> GetAll();
}
