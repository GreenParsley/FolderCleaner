using FolderCleaner.Enums;

namespace FolderCleaner.Services;

public class UserInputValueService : IUserInputValueService
{
    //property
    private IFileExtensionRepository _fileExtensionRepository;
    private IWhiteListRepository _whiteListRepository;

    //konstruktor
    public UserInputValueService(IFileExtensionRepository fileExtensionRepository, IWhiteListRepository whiteListRepository)
    {
        _fileExtensionRepository = fileExtensionRepository;
        _whiteListRepository = whiteListRepository;
    }

    public void AddFileExtensionAndPath()
    {
        var fileExtensionAndPath = GetFileExtensionFromUser();
        _fileExtensionRepository.Add(fileExtensionAndPath);
        EndProcessInfo();
    }

    public void AddFileNames()
    {
        Console.WriteLine("Enter the file names.");
        var fileNames = Console.ReadLine();
        var fileNamesSplitted = fileNames.Split(",").ToList();
        foreach (var name in fileNamesSplitted)
        {
            _whiteListRepository.Add(name);
        }
        EndProcessInfo();
    }

    public void GetAllExtension()
    {
        Console.WriteLine("All extensions:");
        var extensions = _fileExtensionRepository.GetAll();
        foreach ( var extension in extensions ) 
        {
            Console.WriteLine(extension.Extension);
        }
        EndProcessInfo();
    }

    public void GetAllFileName()
    {
        Console.WriteLine("All file names:");
        var fileNames = _whiteListRepository.GetAll();
        foreach (var name in fileNames)
        {
            Console.WriteLine(name);
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
        Console.WriteLine($"{CommendsType.help}, {CommendsType.h} - Return commands list");
        Console.WriteLine($"{CommendsType.addwhitelist}, {CommendsType.awl} - Add elements to white list");
        Console.WriteLine($"{CommendsType.addextension}, {CommendsType.ae} - Add extension to extensions list");
        Console.WriteLine($"{CommendsType.getwhitelist}, {CommendsType.gwl} - Get all elements from white list");
        Console.WriteLine($"{CommendsType.getextension}, {CommendsType.ge} - Get all extensions from extensions list");
        Console.WriteLine($"{CommendsType.updateextension}, {CommendsType.ue} - Update extension in extensions list");
        Console.WriteLine($"{CommendsType.run}, {CommendsType.r} - Run program");
    }

    private void EndProcessInfo()
    {
        Console.WriteLine("Process completed");
    }

    private FileExtension GetFileExtensionFromUser()
    {
        //pobrać dodatkowe rzeczy od użytkownika
        // niech użytkownik wpisuje wszystkie wartości po przecinku, podzielić i przekazać dalej
        Console.WriteLine("Enter the file extension, target path, start with, end with and contain.");
        var fileData = Console.ReadLine();
        var fileDataSplitted = fileData.Split(",").ToList();
        return new FileExtension(fileDataSplitted[0], fileDataSplitted[1], fileDataSplitted[2], fileDataSplitted[3], fileDataSplitted[4]);
    }
}
