using FolderCleaner;
using FolderCleaner.Interfaces;
using FolderCleaner.Models;
using FolderCleaner.Repositories;
using FolderCleaner.Services;
using FolderCleaner.Wrapper;
using Moq;
using System.IO.Abstractions;
using System.Text.Json;

namespace TestFolderCleaner;

public class FileTransferServiceTests
{
    private IFileTransferService _fileTransferService;
    private Mock<IFileExtensionRepository> _fileExtensionRepositoryMock;
    private Mock<IWhiteListRepository> _whiteListRepositoryMock;
    private Mock<IFileSystem> _fileSystemMock;
    private Mock<IConsoleWrapper> _consoleWrapperMock;

    [SetUp]
    public void Setup()
    {
        _fileExtensionRepositoryMock= new Mock<IFileExtensionRepository>();
        _whiteListRepositoryMock= new Mock<IWhiteListRepository>();
        _fileSystemMock = new Mock<IFileSystem>();
        _consoleWrapperMock = new Mock<IConsoleWrapper>();
        _fileTransferService = new FileTransferService(_fileExtensionRepositoryMock.Object, _whiteListRepositoryMock.Object, _fileSystemMock.Object,  _consoleWrapperMock.Object);
    }

    [Test]
    public void Run_ShouldTransferFiles()
    {
        //Arrange - podaje siê wszystkie dane wejœciowe
        var fileExtensionCollection = new List<FileExtension>();
        var whiteListCollection = new List<string>();
        var fileFromDesktop = new string[] { @"fakePath\startcontainend.extension" };
        var fileExtension = new FileExtension("path", "extension", "start", "end", "contain");
        var fileName = "name";
        fileExtensionCollection.Add(fileExtension);
        whiteListCollection.Add(fileName);
        var serializedFileExtensionCollection = JsonSerializer.Serialize(fileExtensionCollection);
        var serializedFileNameCollection = JsonSerializer.Serialize(whiteListCollection);
        _fileExtensionRepositoryMock.Setup(x => x.GetAll()).Returns(fileExtensionCollection);
        _whiteListRepositoryMock.Setup(x => x.GetAll()).Returns(whiteListCollection);
        _fileSystemMock.Setup(x => x.Directory.GetFiles(Const.DesktopPath)).Returns(fileFromDesktop);
        _fileSystemMock.Setup(x => x.Path.GetExtension(fileFromDesktop[0])).Returns("extension");
        _fileSystemMock.Setup(x => x.Path.GetFileNameWithoutExtension(fileFromDesktop[0])).Returns("startcontainend");
        _fileSystemMock.Setup(x => x.Path.GetFileName(fileFromDesktop[0])).Returns("startcontainend.extension");
        _fileSystemMock.Setup(x => x.Path.Combine(fileExtension.TargetPath, "startcontainend.extension")).Returns(@"path\startcontainend.extension");
        _fileSystemMock.Setup(x => x.File.Move(fileFromDesktop[0], @"path\startcontainend.extension"));
        _fileSystemMock.Setup(x => x.Directory.Exists(fileExtension.TargetPath)).Returns(false);
        _fileSystemMock.Setup(x => x.Directory.CreateDirectory(fileExtension.TargetPath));

        //Act - uruchamia siê funkcjê
        _fileTransferService.Run();

        //Assert - sprawdza siê wyniki
        _fileSystemMock.Verify(x => x.File.Move(fileFromDesktop[0], @"path\startcontainend.extension"));
        _fileSystemMock.Verify(x => x.Directory.CreateDirectory(fileExtension.TargetPath));
        _consoleWrapperMock.Verify(x => x.WriteLine("Transfer complited"));
    }

    [Test]
    public void Run_ShouldNotTransferFiles_WhenDontHaveDefinedExtension()
    {
        //Arrange - podaje siê wszystkie dane wejœciowe
        var fileExtensionCollection = new List<FileExtension>();
        var whiteListCollection = new List<string>();
        var fileFromDesktop = new string[] { @"fakePath\filestartcontainend.extension2" };
        var fileExtension = new FileExtension("path", "extension", "start", "end", "contain");
        var fileName = "name";
        fileExtensionCollection.Add(fileExtension);
        whiteListCollection.Add(fileName);
        var serializedFileExtensionCollection = JsonSerializer.Serialize(fileExtensionCollection);
        var serializedFileNameCollection = JsonSerializer.Serialize(whiteListCollection);
        _fileExtensionRepositoryMock.Setup(x => x.GetAll()).Returns(fileExtensionCollection);
        _whiteListRepositoryMock.Setup(x => x.GetAll()).Returns(whiteListCollection);
        _fileSystemMock.Setup(x => x.Directory.GetFiles(Const.DesktopPath)).Returns(fileFromDesktop);
        _fileSystemMock.Setup(x => x.Path.GetExtension(fileFromDesktop[0])).Returns("extension2");
        _fileSystemMock.Setup(x => x.Path.GetFileNameWithoutExtension(fileFromDesktop[0])).Returns("filestartcontainend");

        //Act - uruchamia siê funkcjê
        _fileTransferService.Run();

        //Assert - sprawdza siê wyniki
        _consoleWrapperMock.Verify(x => x.WriteLine("There is no such extension in the list."));
    }

    [Test]
    public void Run_ShouldNotTransferFiles_WhenFilesIsInWhiteList()
    {
        //Arrange - podaje siê wszystkie dane wejœciowe
        var fileExtensionCollection = new List<FileExtension>();
        var whiteListCollection = new List<string>();
        var fileFromDesktop = new string[] { @"fakePath\startcontainend.extension" };
        var fileExtension = new FileExtension("path", "extension", "start", "end", "contain");
        var fileName = "startcontainend.extension";
        fileExtensionCollection.Add(fileExtension);
        whiteListCollection.Add(fileName);
        var serializedFileExtensionCollection = JsonSerializer.Serialize(fileExtensionCollection);
        var serializedFileNameCollection = JsonSerializer.Serialize(whiteListCollection);
        _fileExtensionRepositoryMock.Setup(x => x.GetAll()).Returns(fileExtensionCollection);
        _whiteListRepositoryMock.Setup(x => x.GetAll()).Returns(whiteListCollection);
        _fileSystemMock.Setup(x => x.Directory.GetFiles(Const.DesktopPath)).Returns(fileFromDesktop);

        //Act - uruchamia siê funkcjê
        _fileTransferService.Run();

        //Assert - sprawdza siê wyniki
        _consoleWrapperMock.Verify(x => x.WriteLine("There is no such extension in the list."), Times.Never);
        _consoleWrapperMock.Verify(x => x.WriteLine("Transfer complited"), Times.Never);
    }
}