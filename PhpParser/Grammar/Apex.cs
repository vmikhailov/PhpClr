using HackCLR.Parsers.PhpParser.Syntax;
using HackCLR.Parsers.PhpParser.Toolbox;

namespace HackCLR.Parsers.PhpParser.Parser
{
    internal static class Apex
    {
        private static ApexGrammar ApexGrammar { get; } = new ApexGrammar();

        public static MemberDeclarationSyntax ParseFile(string text) =>
            ApexGrammar.CompilationUnit.ParseEx(text);

        public static ClassDeclarationSyntax ParseClass(string text) =>
            ApexGrammar.ClassDeclaration.ParseEx(text);

        public static EnumDeclarationSyntax ParseEnum(string text) =>
            ApexGrammar.EnumDeclaration.ParseEx(text);
    }
}
