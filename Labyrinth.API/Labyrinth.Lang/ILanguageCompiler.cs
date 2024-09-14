using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Labyrinth.Lang
{
    public interface ILanguageCompiler
    {
        (byte[] compiledAssembly, List<string> errors) Compile(List<string> filePaths);
        string GetLanguageName();
    }
}
