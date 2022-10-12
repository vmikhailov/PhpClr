using PhpClr.Parsers.PhpParser.Visitors;

namespace PhpClr.Parsers.PhpParser.Syntax
{
    public class InterfaceDeclarationSyntax : ClassDeclarationSyntax
    {
        public InterfaceDeclarationSyntax(MemberDeclarationSyntax heading = null)
            : base(heading)
        {
        }

        public InterfaceDeclarationSyntax(MemberDeclarationSyntax heading, ClassDeclarationSyntax classBody)
            : base(heading, classBody)
        {
        }

        public override SyntaxType Kind => SyntaxType.Interface;

        public override void Accept(ApexSyntaxVisitor visitor) => visitor.VisitInterfaceDeclaration(this);

        public override bool IsInterface => true;
    }
}
