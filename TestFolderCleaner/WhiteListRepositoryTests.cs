using FolderCleaner;
using FolderCleaner.Interfaces;
using FolderCleaner.Models;
using FolderCleaner.Repositories;
using Moq;
using System.IO.Abstractions;
using System.Text.Json;

namespace TestFolderCleaner;

public class WhiteListRepositoryTests
{
    private IWhiteListRepository _whiteListRepository;
    private Mock<IFileSystem> _fileSystemMock;

    [SetUp]
    public void Setup()
    {
        _fileSystemMock = new Mock<IFileSystem>();
        _whiteListRepository = new WhiteListRepository(_fileSystemMock.Object);
    }

    [Test]
    public void Add_SuccessfulAddToWhiteList()
    {
        //Arrange - podaje si� wszystkie dane wej�ciowe
        _fileSystemMock.Setup(x => x.File.ReadAllText(Const.FileWhiteListName)).Returns("[]");
        var fileName = "name";

        //Act - uruchamia si� funkcj�
        _whiteListRepository.Add(fileName);

        //Assert - sprawdza si� wyniki
        _fileSystemMock.Verify(x => x.File.WriteAllText(Const.FileWhiteListName, It.Is<string>(y => y.Contains(fileName))));
    }

    [Test]
    public void GetAll_SuccessfufGetAllFromWhiteList()
    {
        //Arrange - podaje si� wszystkie dane wej�ciowe
        var whiteListCollection = new List<string>();
        var fileName = "name";
        whiteListCollection.Add(fileName);
        var secondFileName = "name2";
        whiteListCollection.Add(secondFileName);
        var serializedCollection = JsonSerializer.Serialize(whiteListCollection);
        _fileSystemMock.Setup(x => x.File.ReadAllText(Const.FileWhiteListName)).Returns(serializedCollection);

        //Act - uruchamia si� funkcj�
        var fileNameCollection = _whiteListRepository.GetAll();

        //Assert - sprawdza si� wyniki
        var serializedFileNameCollection = JsonSerializer.Serialize(whiteListCollection);
        CollectionAssert.AreEqual(serializedCollection, serializedFileNameCollection);
    }
}