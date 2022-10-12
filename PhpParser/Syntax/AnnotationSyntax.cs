using System.Collections.Generic;
using PhpClr.Parsers.PhpParser.Grammar;
using PhpClr.Parsers.PhpParser.Visitors;

namespace PhpClr.Parsers.PhpParser.Syntax
{
    public class AnnotationSyntax : BaseSyntax
    {
        public AnnotationSyntax(string identifier = null, string parameters = null)
        {
            Identifier = identifier;
            Parameters = parameters;
        }

        public override SyntaxType Kind => SyntaxType.Annotation;

        public string Identifier { get; set; }

        public string Parameters { get; set; }

        public override void Accept(ApexSyntaxVisitor visitor) => visitor.VisitAnnotation(this);

        public override IEnumerable<BaseSyntax> ChildNodes => NoChildren;

        public bool IsTest => PhpKeywords.UnitTestKeywords.Contains(Identifier);
    }
}
