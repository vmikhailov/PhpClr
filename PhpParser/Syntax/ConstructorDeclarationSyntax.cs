using PhpClr.Parsers.PhpParser.Visitors;

namespace PhpClr.Parsers.PhpParser.Syntax
{
    public class ConstructorDeclarationSyntax : MethodDeclarationSyntax
    {
        public ConstructorDeclarationSyntax(MethodDeclarationSyntax method = null)
            : base(method)
        {
            if (method != null)
            {
                Body = method.Body;
                ReturnType = method.ReturnType;
                Identifier = method.Identifier;
                Parameters = method.Parameters;
            }
        }

        public override SyntaxType Kind => SyntaxType.Constructor;

        public override void Accept(ApexSyntaxVisitor visitor) => visitor.VisitConstructorDeclaration(this);

        public override MemberDeclarationSyntax WithTypeAndName(ParameterSyntax typeAndName)
        {
            Identifier = typeAndName.Identifier ?? typeAndName.Type.Identifier;
            return this;
        }

        public ExpressionSyntax ChainedConstructorCall { get; set; }
    }
}
