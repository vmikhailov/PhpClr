using HackCLR.Parsers.PhpParser.Visitors;
using PhpClr.Parsers.PhpParser.Syntax;

namespace PhpClr.Parsers.PhpParser.Visitors
{
    public static class Extensions
    {
        public static string ToApex(this BaseSyntax node, int tabSize = 4) =>
            ApexCodeGenerator.GenerateApex(node, tabSize);

        public static string GetCodeInsideMethod(this MethodDeclarationSyntax node, int tabSize = 4) =>
            ApexMethodBodyGenerator.GenerateApex(node, tabSize);
    }
}
