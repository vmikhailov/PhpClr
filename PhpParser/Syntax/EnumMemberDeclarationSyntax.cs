using PhpClr.Parsers.PhpParser.Visitors;

namespace PhpClr.Parsers.PhpParser.Syntax
{
    public class EnumMemberDeclarationSyntax : MemberDeclarationSyntax
    {
        public EnumMemberDeclarationSyntax(MemberDeclarationSyntax heading = null)
            : base(heading)
        {
        }

        public override SyntaxType Kind => SyntaxType.EnumMember;

        public override void Accept(ApexSyntaxVisitor visitor) => visitor.VisitEnumMember(this);

        public string Identifier { get; set; }
    }
}
