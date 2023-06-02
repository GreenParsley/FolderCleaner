using FolderCleaner;
using FolderCleaner.Interfaces;
using FolderCleaner.Models;
using FolderCleaner.Repositories;
using FolderCleaner.Services;
using Moq;
using System.IO.Abstractions;
using System.Text.Json;

namespace TestFolderCleaner;

public class UserInputValueServiceTests
{
    private IUserInputValueService _userInputValueService;
    private Mock<IFileExtensionRepository> _fileExtensionRepositoryMock;
    private Mock<IWhiteListRepository> _whiteListRepositoryMock;
    private Mock<IConsoleWrapper> _consoleWrapperMock;

    [SetUp]
    public void Setup()
    {
        _fileExtensionRepositoryMock= new Mock<IFileExtensionRepository>();
        _whiteListRepositoryMock= new Mock<IWhiteListRepository>();
        _consoleWrapperMock= new Mock<IConsoleWrapper>();
        _userInputValueService = new UserInputValueService(_fileExtensionRepositoryMock.Object, _whiteListRepositoryMock.Object, _consoleWrapperMock.Object);
    }

    [TestCase("")]
    [TestCase("path")]
    [TestCase("path,extension")]
    [TestCase("path,extension,start")]
    [TestCase("path,extension,start,end")]
    [TestCase("path,extension,start,end,contain")]
    public void AddFileExtensionAndPath_SuccessfulAddFileExtensionAndPath(string value)
    {
        //Arrange - podaje siê wszystkie dane wejœciowe
        _consoleWrapperMock.Setup(x => x.ReadLine()).Returns(value);

        //Act - uruchamia siê funkcjê
        _userInputValueService.AddFileExtensionAndPath();

        //Assert - sprawdza siê wyniki
        _fileExtensionRepositoryMock.Verify(x => x.Add(It.IsAny<FileExtension>()));
        _consoleWrapperMock.Verify(x => x.WriteLine("Process completed"));
    }

    [Test]
    public void AddFileNames_SuccessfulAddFileNames()
    {
        //Arrange - podaje siê wszystkie dane wejœciowe
        var firstName = "name1";
        var secondName = "name2";
        _consoleWrapperMock.Setup(x => x.ReadLine()).Returns($"{firstName},{secondName}");

        //Act - uruchamia siê funkcjê
        _userInputValueService.AddFileNames();

        //Assert - sprawdza siê wyniki
        _whiteListRepositoryMock.Verify(x => x.Add(firstName));
        _whiteListRepositoryMock.Verify(x => x.Add(secondName));
        _consoleWrapperMock.Verify(x => x.WriteLine("Process completed"));
    }

    [Test]
    public void GetAllExtension_ShouldWriteAllExtensions()
    {
        //Arrange - podaje siê wszystkie dane wejœciowe
        var fileExtensionCollection = new List<FileExtension>();
        var fileExtension = new FileExtension("path", "extension", "start", "end", "contain");
        fileExtensionCollection.Add(fileExtension);
        var secondFileExtension = new FileExtension("path2", "extension2", "start", "end", "contain");
        fileExtensionCollection.Add(secondFileExtension);
        _fileExtensionRepositoryMock.Setup(x => x.GetAll()).Returns(fileExtensionCollection);

        //Act - uruchamia siê funkcjê
        _userInputValueService.GetAllExtension();

        //Assert - sprawdza siê wyniki
        _consoleWrapperMock.Verify(x => x.WriteLine(fileExtension.Extension));
        _consoleWrapperMock.Verify(x => x.WriteLine(secondFileExtension.Extension));
        _consoleWrapperMock.Verify(x => x.WriteLine("Process completed"));
    }

    [Test]
    public void GetAllFileNames_ShouldWriteAllNames()
    {
        //Arrange - podaje siê wszystkie dane wejœciowe
        var whiteListCollection = new List<string>();
        var fileName = "name";
        whiteListCollection.Add(fileName);
        var secondFileName = "name2";
        whiteListCollection.Add(secondFileName);
        _whiteListRepositoryMock.Setup(x => x.GetAll()).Returns(whiteListCollection);

        //Act - uruchamia siê funkcjê
        _userInputValueService.GetAllFileName();

        //Assert - sprawdza siê wyniki
        _consoleWrapperMock.Verify(x => x.WriteLine(whiteListCollection[0]));
        _consoleWrapperMock.Verify(x => x.WriteLine(whiteListCollection[1]));
        _consoleWrapperMock.Verify(x => x.WriteLine("Process completed"));
    }

    [Test]
    public void UpdateFileExtension_SuccessfulUpdateFileExtension()
    {
        //Arrange - podaje siê wszystkie dane wejœciowe
        _consoleWrapperMock.Setup(x => x.ReadLine()).Returns("newPath,extension,start,end,contain");

        //Act - uruchamia siê funkcjê
        _userInputValueService.UpdateFileExtension();

        //Assert - sprawdza siê wyniki
        _fileExtensionRepositoryMock.Verify(x => x.Update(It.IsAny<FileExtension>()));
        _consoleWrapperMock.Verify(x => x.WriteLine("Process completed"));
    }

    [Test]
    public void CommandList_ShouldWriteAllAvailableCommands()
    {
        //Arrange - podaje siê wszystkie dane wejœciowe


        //Act - uruchamia siê funkcjê
        _userInputValueService.CommandList();

        //Assert - sprawdza siê wyniki
        _consoleWrapperMock.Verify(x => x.WriteLine(It.IsAny<string>()), Times.Exactly(7));

        
    }
}