using FolderCleaner.Enums;

namespace FolderCleaner.Services;

public class UserInputValueService : IUserInputValueService
{
    //property
    private IFileExtensionRepository _fileExtensionRepository;
    private IWhiteListRepository _whiteListRepository;
    private IConsoleWrapper _consoleWrapper;

    //konstruktor
    public UserInputValueService(IFileExtensionRepository fileExtensionRepository, IWhiteListRepository whiteListRepository, IConsoleWrapper consoleWrapper)
    {
        _fileExtensionRepository = fileExtensionRepository;
        _whiteListRepository = whiteListRepository;
        _consoleWrapper = consoleWrapper;
    }

    public void AddFileExtensionAndPath()
    {
        var fileExtensionAndPath = GetFileExtensionFromUser();
        _fileExtensionRepository.Add(fileExtensionAndPath);
        EndProcessInfo();
    }

    public void AddFileNames()
    {
        _consoleWrapper.WriteLine("Enter the file names.");
        var fileNames = _consoleWrapper.ReadLine();
        var fileNamesSplitted = fileNames.Split(",").ToList();
        foreach (var name in fileNamesSplitted)
        {
            _whiteListRepository.Add(name);
        }
        EndProcessInfo();
    }

    public void GetAllExtension()
    {
        _consoleWrapper.WriteLine("All extensions:");
        var extensions = _fileExtensionRepository.GetAll();
        foreach ( var extension in extensions ) 
        {
            _consoleWrapper.WriteLine(extension.Extension);
        }
        EndProcessInfo();
    }

    public void GetAllFileName()
    {
        _consoleWrapper.WriteLine("All file names:");
        var fileNames = _whiteListRepository.GetAll();
        foreach (var name in fileNames)
        {
            _consoleWrapper.WriteLine(name);
        }
        EndProcessInfo();
    }

    public void UpdateFileExtension()
    {
        var fileExtensionAndPath = GetFileExtensionFromUser();
        _fileExtensionRepository.Update(fileExtensionAndPath);
        EndProcessInfo();
    }

    public void CommandList()
    {
        _consoleWrapper.WriteLine($"{CommendsType.help}, {CommendsType.h} - Return commands list");
        _consoleWrapper.WriteLine($"{CommendsType.addwhitelist}, {CommendsType.awl} - Add elements to white list");
        _consoleWrapper.WriteLine($"{CommendsType.addextension}, {CommendsType.ae} - Add extension to extensions list");
        _consoleWrapper.WriteLine($"{CommendsType.getwhitelist}, {CommendsType.gwl} - Get all elements from white list");
        _consoleWrapper.WriteLine($"{CommendsType.getextension}, {CommendsType.ge} - Get all extensions from extensions list");
        _consoleWrapper.WriteLine($"{CommendsType.updateextension}, {CommendsType.ue} - Update extension in extensions list");
        _consoleWrapper.WriteLine($"{CommendsType.run}, {CommendsType.r} - Run program");
    }

    private void EndProcessInfo()
    {
        _consoleWrapper.WriteLine("Process completed");
    }

    private FileExtension GetFileExtensionFromUser()
    {
        _consoleWrapper.WriteLine("Enter the target path, file extension, start with, end with and contain.");
        var fileData = _consoleWrapper.ReadLine();
        var fileDataSplitted = fileData.Split(",").ToList();
        var count = fileDataSplitted.Count;
        return new FileExtension(count > 0 ? fileDataSplitted[0] : String.Empty, 
            count > 1 ? fileDataSplitted[1] : String.Empty, 
            count > 2 ? fileDataSplitted[2] : String.Empty, 
            count > 3 ? fileDataSplitted[3] : String.Empty,
            count > 4 ? fileDataSplitted[4] : String.Empty);
    }
}
