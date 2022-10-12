using PhpClr.Parsers.PhpParser.Visitors;

namespace PhpClr.Parsers.PhpParser.Syntax
{
    public class BaseSyntaxDebuggerProxy
    {
        public BaseSyntaxDebuggerProxy(BaseSyntax content) => Content = content;

        private BaseSyntax Content { get; }

        public string NodeType => Content.GetType().Name;

        public string ApexCode => Content.ToApex();
    }
}
