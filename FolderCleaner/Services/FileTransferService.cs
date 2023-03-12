namespace FolderCleaner.Services;

public class FileTransferService : IFileTransferService
{
    //property
    private IFileExtensionRepository _fileExtensionRepository;
    private IWhiteListRepository _whiteListRepository;

    //konstruktor
    public FileTransferService(IFileExtensionRepository fileExtensionRepository, IWhiteListRepository whiteListRepository)
    {
        _fileExtensionRepository = fileExtensionRepository;
        _whiteListRepository = whiteListRepository;
    }

    //przenoszenie plików z fileextension ale zostawiać pliki z whitelist
    public void Run()
    {
        var allFileExtensionAndPath = _fileExtensionRepository.GetAll();
        var allNamesFromWhiteList =  _whiteListRepository.GetAll();
        //pobrać wszystkie pliki z zdefiniowanej ścieżki pulpitu (osobna metoda private)
        //wyrzucić wszystkie pliki z białej listy z listy plików z pulpitu (osobna metoda private)
        //pętla która przejdzie po wszystkich plikach i będzie sprawdzać jakie rozszerzenia i przenosić do targetPath (osobna metoda private), trzeba się upewnić że folder z targetPath isnieje, jak nie to stworzyć.
    }
}
