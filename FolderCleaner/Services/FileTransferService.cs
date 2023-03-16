using System.Diagnostics;

namespace FolderCleaner.Services;

public class FileTransferService : IFileTransferService
{
    //property
    private IFileExtensionRepository _fileExtensionRepository;
    private IWhiteListRepository _whiteListRepository;
    private string _desktopPath;

    //konstruktor
    public FileTransferService(IFileExtensionRepository fileExtensionRepository, IWhiteListRepository whiteListRepository)
    {
        _fileExtensionRepository = fileExtensionRepository;
        _whiteListRepository = whiteListRepository;
        _desktopPath = Const.DesktopPath;
    }

    public void Run()
    {
        var allFileExtensionAndPath = _fileExtensionRepository.GetAll();
        var allNamesFromWhiteList = _whiteListRepository.GetAll();
        var filesToTransfer = FileNamesToTransfer(allNamesFromWhiteList);
        TransferFiles(allFileExtensionAndPath, filesToTransfer);
    }

    private String[] GetFilesFromDesktop()
    {
        var filesFromDesktop = Directory.GetFiles(_desktopPath);
        return filesFromDesktop;
    }

    private List<string> FileNamesToTransfer(List<string> whiteListRepository)
    {
        var filesFromDesktop = GetFilesFromDesktop();
        var filesToTransfer = filesFromDesktop.Except(whiteListRepository).ToList();
        return filesToTransfer;
    }

    private void TransferFiles(List<FileExtension> fileExtensionRepository, List<string> filesToTransfer)
    {
        foreach (var file in filesToTransfer)
        {
            var extension = Path.GetExtension(file);
            //dodać trzy kolejne properties dodane do warunków poniże
            var fileExtension = _fileExtensionRepository.GetAll().FirstOrDefault(x => x.Extension.Equals(extension));
            if (fileExtension != null)
            {
                if (Directory.Exists(fileExtension.TargetPath) == false)
                {
                    Directory.CreateDirectory(fileExtension.TargetPath);
                }
                File.Move(file, Path.Combine(fileExtension.TargetPath, Path.GetFileName(file)));
                Console.WriteLine("Transfer complited");
            }
            else
            {
                Console.WriteLine("There is no such extension in the list.");
            }
        }
    }
}
