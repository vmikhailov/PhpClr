using System.Collections.Generic;
using System.Linq;
using PhpClr.Parsers.PhpParser.Grammar;
using PhpClr.Parsers.PhpParser.Toolbox;
using PhpClr.Parsers.PhpParser.Visitors;

namespace PhpClr.Parsers.PhpParser.Syntax
{
    public class ClassInitializerSyntax : MemberDeclarationSyntax
    {
        public ClassInitializerSyntax(MemberDeclarationSyntax heading = null)
            : base(heading)
        {
        }

        public override SyntaxType Kind => SyntaxType.ClassInitializer;

        public override void Accept(ApexSyntaxVisitor visitor) => visitor.VisitClassInitializer(this);

        public override IEnumerable<BaseSyntax> ChildNodes =>
            base.ChildNodes.Concat(GetNodes(Body));

        public BlockSyntax Body { get; set; }

        public bool IsStatic => Modifiers.EmptyIfNull().Any(m => m == PhpKeywords.Static);
    }
}
