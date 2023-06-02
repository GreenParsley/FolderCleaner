using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FolderCleaner.Wrapper;

[ExcludeFromCodeCoverage]
public class ConsoleWrapper : IConsoleWrapper
{
    public string ReadLine()
    {
        var output = Console.ReadLine();
        if (output == null)
        {
            throw new ArgumentNullException();
        }
        else
        {
            return output;
        }
        
    }

    public void WriteLine(string message)
    {
        Console.WriteLine(message);
    }
}
