using System;
using System.Linq;

namespace Labyrinth.Lang
{
    public interface ICompilerFactory
    {
        ILanguageCompiler GetCompiler(SupportedLanguages language);
    }
}
