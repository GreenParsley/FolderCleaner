using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FolderCleaner.Interfaces;

public interface IConsoleWrapper
{
    void WriteLine(string message);
    string ReadLine();
}
