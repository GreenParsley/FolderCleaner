using FolderCleaner.Enums;
using FolderCleaner.Services;
using FolderCleaner.Wrapper;
using Microsoft.Extensions.Logging;
using System.IO.Abstractions;
using System.Runtime.CompilerServices;

var loggerFactory = CreateLogger();
ILogger logger = loggerFactory.CreateLogger<Program>();

IFileSystem fileSystem = new FileSystem();
IConsoleWrapper consoleWrapper = new ConsoleWrapper();
IFileExtensionRepository fileExtensionRepository = new FileExtensionRepository(fileSystem);
//fileExtensionRepository.GetAll().ToList().ForEach(x => Console.WriteLine($"{x.TargetPath} {x.Extension}"));
//fileExtensionRepository.Add(new FileExtension("ccc", ".jpg"));
//fileExtensionRepository.GetAll().ToList().ForEach(x => Console.WriteLine($"{x.TargetPath} {x.Extension}"));
//fileExtensionRepository.Update(new FileExtension("ddd", ".jpg"));
//fileExtensionRepository.Update(new FileExtension("eee", ".csv"));
//fileExtensionRepository.GetAll().ToList().ForEach(x => Console.WriteLine($"{x.TargetPath} {x.Extension}"));

IWhiteListRepository whiteListRepository = new WhiteListRepository(fileSystem);
//whiteListRepository.GetAll().ToList().ForEach(x => Console.WriteLine(x));
//whiteListRepository.Add("kiwi");
//whiteListRepository.GetAll().ToList().ForEach(x => Console.WriteLine(x));

IUserInputValueService userInputValueService = new UserInputValueService(fileExtensionRepository, whiteListRepository, consoleWrapper);
//userInputValueService.GetFileExtensionAndPath();
//userInputValueService.GetFileName();
//userInputValueService.GetAllFileName().ToList().ForEach(x => Console.WriteLine(x));
//userInputValueService.StartTransferringFilesInfo();
//userInputValueService.EndTransferringFilesInfo();

IFileTransferService fileTransferService = new FileTransferService(fileExtensionRepository, whiteListRepository, fileSystem, consoleWrapper);
var boolValue = true;
while (true)
{
	if (boolValue == true)
	{
        userInputValueService.CommandList();
		boolValue = false;
    }

    var userCommend = Enum.Parse(typeof(CommendsType), Console.ReadLine().ToLower());
	switch (userCommend)
	{
		case CommendsType.help:
		case CommendsType.h:
			userInputValueService.CommandList();
			break;

		case CommendsType.addwhitelist:
		case CommendsType.awl:
            userInputValueService.AddFileNames();
            break;

		case CommendsType.addextension:
		case CommendsType.ae:
			userInputValueService.AddFileExtensionAndPath();
			break;

		case CommendsType.getwhitelist:
		case CommendsType.gwl:
			userInputValueService.GetAllFileName();
			break;

		case CommendsType.getextension:
		case CommendsType.ge:
            userInputValueService.GetAllExtension();
            break;

		case CommendsType.updateextension:
		case CommendsType.ue:
			userInputValueService.UpdateFileExtension();
			break;

		case CommendsType.run:
		case CommendsType.r:
            fileTransferService.Run();
			break;

        default:
			break;
	}
}

ILoggerFactory CreateLogger()
{
    return LoggerFactory.Create(builder =>
    {
        builder
            .AddFilter("Microsoft", LogLevel.Warning)
            .AddFilter("System", LogLevel.Warning)
            .AddFilter("NonHostConsoleApp.Program", LogLevel.Debug)
            .AddConsole();
    });
}
