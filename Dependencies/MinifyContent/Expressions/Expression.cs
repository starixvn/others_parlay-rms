using System.Collections.Generic;
using System.Text.RegularExpressions;


namespace MinifyContent.Expressions
{
    /// <summary>
    /// Delegate allows to define method for minify content
    /// </summary>
    /// <param name="sender">Expression object</param>
    /// <param name="input">Content string</param>
    /// <returns></returns>
    internal delegate string Replace(Expression sender, string input);

    /// <summary>
    /// A class to define expressions to minify content
    /// </summary>
    class Expression
    {
        public Expression()
        { }

        public Expression(Regex expr, string replaceWith = "")
        { RegularExpression = expr; ReplaceWith = replaceWith; }

        public Expression(List<Regex> exprs, string replaceWith = "")
        { RegularExpressions = exprs; ReplaceWith = replaceWith; }

        public bool EscapeInCss { get; set; }

        public bool EscapeInJs { get; set; }

        public Regex RegularExpression { get; private set; }

        public List<Regex> RegularExpressions { get; private set; }

        public string ReplaceWith { get; private set; }

        public Replace ReplaceHandler { get; set; }

    }
}
