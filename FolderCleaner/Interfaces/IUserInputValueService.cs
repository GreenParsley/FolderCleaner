namespace FolderCleaner.Interfaces;

public interface IUserInputValueService
{
    void AddFileExtensionAndPath();
    void AddFileNames();
    void GetAllExtension();
    void GetAllFileName();
    void UpdateFileExtension();
    void CommandList();
}
