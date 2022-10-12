using System.Collections.Generic;

namespace PhpClr.Parsers.PhpParser.Syntax
{
    public interface IAnnotatedSyntax
    {
        List<AnnotationSyntax> Annotations { get; }

        List<string> Modifiers { get; }
    }
}
