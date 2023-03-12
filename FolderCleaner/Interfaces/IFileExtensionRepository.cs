namespace FolderCleaner.Interfaces;

public interface IFileExtensionRepository
{
    void Add(FileExtension fileExtension);
    void Update(FileExtension fileExtension);
    List<FileExtension> GetAll();
}
