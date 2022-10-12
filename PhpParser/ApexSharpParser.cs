using PhpClr.Parsers.PhpParser.Grammar;
using PhpClr.Parsers.PhpParser.Visitors;
using MemberDeclarationSyntax = PhpClr.Parsers.PhpParser.Syntax.MemberDeclarationSyntax;

namespace PhpClr.Parsers.PhpParser
{
    public class ApexSharpParser
    {
        private static PhpGrammar PhpGrammar { get; } = new PhpGrammar();

        // Get the AST for a given APEX File
        public static MemberDeclarationSyntax GetApexAst(string apexCode)
        {
            // return ApexGrammar.CompilationUnit.ParseEx(apexCode);
            return null;
        }

        // Format APEX Code so each statement is in its own line
        public static string FormatApex(string apexCode)
        {
            return GetApexAst(apexCode).ToApex(tabSize: 0);
        }

        // Indent APEX code, Pass the Tab Size. If Tab size is set to 0, no indentions
        public static string IndentApex(string apexCode, int tabSize = 4)
        {
            return GetApexAst(apexCode).ToApex(tabSize);
        }
    }
}
