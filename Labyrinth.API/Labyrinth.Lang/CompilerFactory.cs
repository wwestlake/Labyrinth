using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Labyrinth.Lang
{
    public class CompilerFactory : ICompilerFactory
    {
        private readonly IEnumerable<ILanguageCompiler> _compilers;

        public CompilerFactory(IEnumerable<ILanguageCompiler> compilers)
        {
            _compilers = compilers;
        }

        public ILanguageCompiler GetCompiler(SupportedLanguages language)
        {
            return _compilers.FirstOrDefault(compiler => compiler.GetLanguageName() == language.ToString())
                   ?? throw new NotSupportedException($"Language {language} is not supported.");
        }
    }
}
