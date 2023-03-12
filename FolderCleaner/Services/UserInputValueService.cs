using FolderCleaner.Enums;

namespace FolderCleaner.Services;

public class UserInputValueService : IUserInputValueService
{
    //property
    private IFileExtensionRepository _fileExtensionRepository;
    private IWhiteListRepository _whiteListRepository;

    //konstruktor - musi przyjmować IFileExtensionRepository i IWhiteListRepository
    public UserInputValueService(IFileExtensionRepository fileExtensionRepository, IWhiteListRepository whiteListRepository)
    {
        _fileExtensionRepository = fileExtensionRepository;
        _whiteListRepository = whiteListRepository;
    }

    // 1 metoda = pobierze od użytkownika fileExtension i targetPath - console.ReadLine
    public void AddFileExtensionAndPath()
    {
        var fileExtensionAndPath = GetFileExtensionFromUser();
        _fileExtensionRepository.Add(fileExtensionAndPath);
        EndProcessInfo();
    }

    // 1.5 metoda = pobierze od użytkownika nazwe pliku z WhiteList - console.ReadLine
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

    // 2 metoda = wypisz wszystkie rozszerzenia jakie aktualnie są
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

    //2.5 metoda = wypisze wszystkie nazwy plików z WhiteList
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

    // 4 metoda = zwróci użytkownikowi informacje o możliwych komendach (tylko szkielet metody)
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
        Console.WriteLine("Enter the file extension.");
        var fileExtension = Console.ReadLine();
        Console.WriteLine("Enter the target path.");
        var targetPath = Console.ReadLine();
        return new FileExtension(fileExtension, targetPath);
    }
}
