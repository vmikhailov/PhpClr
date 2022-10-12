using Sprache;

namespace PhpClr.Parsers.PhpParser.Toolbox
{
    public interface ICommentParserProvider
    {
        IComment CommentParser { get; }
    }
}
