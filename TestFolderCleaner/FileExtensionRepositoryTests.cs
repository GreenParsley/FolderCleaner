using FolderCleaner;
using FolderCleaner.Interfaces;
using FolderCleaner.Models;
using FolderCleaner.Repositories;
using Moq;
using System.IO.Abstractions;
using System.Text.Json;

namespace TestFolderCleaner;

public class FileExtensionRepositoryTests
{
    private IFileExtensionRepository _fileExtensionRepository;
    private Mock<IFileSystem> _fileSystemMock;

    [SetUp]
    public void Setup()
    {
        _fileSystemMock = new Mock<IFileSystem>();
        _fileExtensionRepository = new FileExtensionRepository(_fileSystemMock.Object);
    }

    [Test]
    public void Add_SuccessfulAddFileExtension()
    {
        //Arrange - podaje si� wszystkie dane wej�ciowe
        _fileSystemMock.Setup(x => x.File.ReadAllText(Const.FileExtensionName)).Returns("[]");
        var fileExtension = new FileExtension("path", "extension", "start", "end", "contain");

        //Act - uruchamia si� funkcj�
        _fileExtensionRepository.Add(fileExtension);

        //Assert - sprawdza si� wyniki
        var jsonConverted = JsonSerializer.Serialize(fileExtension);
        _fileSystemMock.Verify(x => x.File.WriteAllText(Const.FileExtensionName, It.Is<string>(y => y.Contains(jsonConverted))));
    }

    [Test]
    public void Update_SuccessfulUpdateFileExtension()
    {
        //Arrange - podaje si� wszystkie dane wej�ciowe
        var fileExtensionCollection = new List<FileExtension>();
        var fileExtension = new FileExtension("path", "extension", "start", "end", "contain");
        fileExtensionCollection.Add(fileExtension);
        var serializedCollection = JsonSerializer.Serialize(fileExtensionCollection);
        _fileSystemMock.Setup(x => x.File.ReadAllText(Const.FileExtensionName)).Returns(serializedCollection);
        var fileExtensionUpdate = new FileExtension("pathUpdate", "extension", "start", "end", "contain");


        //Act - uruchamia si� funkcj�
        _fileExtensionRepository.Update(fileExtensionUpdate);

        //Assert - sprawdza si� wyniki
        var serializedUpdate = JsonSerializer.Serialize(fileExtensionUpdate);
        _fileSystemMock.Verify(x => x.File.WriteAllText(Const.FileExtensionName, It.Is<string>(y => y.Contains(serializedUpdate))));
    }

    [TestCase("newExtension", "start", "end", "contain")]
    [TestCase("extension", "newStart", "end", "contain")]
    [TestCase("extension", "start", "newEnd", "contain")]
    [TestCase("extension", "start", "end", "newContain")]
    public void Update_UnsuccessfulUpdateFileExtension(string ext, string start, string end, string contain)
    {
        //Arrange - podaje si� wszystkie dane wej�ciowe
        var fileExtensionCollection = new List<FileExtension>();
        var fileExtension = new FileExtension("path", "extension", "start", "end", "contain");
        fileExtensionCollection.Add(fileExtension);
        var serializedCollection = JsonSerializer.Serialize(fileExtensionCollection);
        _fileSystemMock.Setup(x => x.File.ReadAllText(Const.FileExtensionName)).Returns(serializedCollection);
        var fileExtensionUpdate = new FileExtension("pathUpdate", ext, start, end, contain);


        //Act - uruchamia si� funkcj�
        _fileExtensionRepository.Update(fileExtensionUpdate);

        //Assert - sprawdza si� wyniki
        var serializedUpdate = JsonSerializer.Serialize(fileExtensionUpdate);
        _fileSystemMock.Verify(x => x.File.WriteAllText(Const.FileExtensionName, It.Is<string>(y => y.Contains(serializedUpdate))), Times.Never);
    }

    [Test]
    public void GetAll_SuccessfufGetAllFileExtension()
    {
        //Arrange - podaje si� wszystkie dane wej�ciowe
        var fileExtensionCollection = new List<FileExtension>();
        var fileExtension = new FileExtension("path", "extension", "start", "end", "contain");
        fileExtensionCollection.Add(fileExtension);
        var secondFileExtension = new FileExtension("path2", "extension2", "start", "end", "contain");
        fileExtensionCollection.Add(secondFileExtension);
        var serializedCollection = JsonSerializer.Serialize(fileExtensionCollection);
        _fileSystemMock.Setup(x => x.File.ReadAllText(Const.FileExtensionName)).Returns(serializedCollection);

        //Act - uruchamia si� funkcj�
        var fileCollection = _fileExtensionRepository.GetAll();

        //Assert - sprawdza si� wyniki
        var serializedFileCollection = JsonSerializer.Serialize(fileCollection);
        CollectionAssert.AreEqual(serializedCollection, serializedFileCollection);
    }
}