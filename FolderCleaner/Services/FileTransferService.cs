using System.Diagnostics;
using System.IO.Abstractions;

namespace FolderCleaner.Services;

public class FileTransferService : IFileTransferService
{
    //property
    private IFileExtensionRepository _fileExtensionRepository;
    private IWhiteListRepository _whiteListRepository;
    private string _desktopPath;
    private IFileSystem _fileSystem;
    private IConsoleWrapper _consoleWrapper;

    //konstruktor
    public FileTransferService(IFileExtensionRepository fileExtensionRepository, IWhiteListRepository whiteListRepository, IFileSystem fileSystem, IConsoleWrapper consoleWrapper)
    {
        _fileExtensionRepository = fileExtensionRepository;
        _whiteListRepository = whiteListRepository;
        _desktopPath = Const.DesktopPath;
        _fileSystem = fileSystem;
        _consoleWrapper = consoleWrapper;
    }

    public void Run()
    {
        var allFileExtensionAndPath = _fileExtensionRepository.GetAll();
        var allNamesFromWhiteList = _whiteListRepository.GetAll();
        var filesToTransfer = FileNamesToTransfer(allNamesFromWhiteList);
        TransferFiles(allFileExtensionAndPath, filesToTransfer);
    }

    private string[] GetFilesFromDesktop()
    {
        var filesFromDesktop = _fileSystem.Directory.GetFiles(_desktopPath);
        return filesFromDesktop;
    }

    private List<string> FileNamesToTransfer(List<string> whiteListRepository)
    {
        var filesFromDesktop = GetFilesFromDesktop();
        var filesToTransfer = filesFromDesktop.Where(x => !whiteListRepository.Any(y => x.Contains(y))).ToList();
        return filesToTransfer;
    }

    private void TransferFiles(List<FileExtension> fileExtensionRepository, List<string> filesToTransfer)
    {
        foreach (var file in filesToTransfer)
        {
            var extension = _fileSystem.Path.GetExtension(file);
            var fileName = _fileSystem.Path.GetFileNameWithoutExtension(file);
            var fileExtension = fileExtensionRepository.FirstOrDefault(x => x.Extension.Equals(extension) && 
            fileName.StartsWith(x.StartWith) &&
            fileName.EndsWith(x.EndWith) &&
            fileName.Contains(x.Contain));
            if (fileExtension != null)
            {
                if (_fileSystem.Directory.Exists(fileExtension.TargetPath) == false)
                {
                    _fileSystem.Directory.CreateDirectory(fileExtension.TargetPath);
                }
                var test = _fileSystem.Path.GetFileName(file);
                var test2 = _fileSystem.Path.Combine(fileExtension.TargetPath, _fileSystem.Path.GetFileName(file));
                _fileSystem.File.Move(file, _fileSystem.Path.Combine(fileExtension.TargetPath, _fileSystem.Path.GetFileName(file)));
                _consoleWrapper.WriteLine("Transfer complited");
            }
            else
            {
                _consoleWrapper.WriteLine("There is no such extension in the list.");
            }
        }
    }
}
