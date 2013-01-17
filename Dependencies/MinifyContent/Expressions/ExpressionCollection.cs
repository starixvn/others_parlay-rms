using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text.RegularExpressions;
using System.Web;
using System.Web.Configuration;

namespace MinifyContent.Expressions
{
    class ExpressionCollection
        : List<Expression>
    {
        private ExpressionCollection()
        { }


        public ExpressionCollection Collections()
        {
            //Condition to check that collection items are already present
            if (this.Count == 0)
            {
                //Read configuration from web.config
                Configuration configuration = WebConfigurationManager.OpenWebConfiguration(HttpContext.Current.Request.ApplicationPath);
                List<ResponseStreamConfiguration> sections = configuration.Sections.OfType<ResponseStreamConfiguration>().ToList();
                ResponseStreamConfiguration conf = sections != null && sections.Count > 0 ? sections[0] : new ResponseStreamConfiguration();

                #region Regular Expressions
                //Remove tabs
                Expression exp = new Expression(new Regex("\t", RegexOptions.Compiled | RegexOptions.Multiline));
                this.Add(exp);

                //Remove space after and before tag
                Regex spaceBeforeTag = new Regex(@"\s+<", RegexOptions.Multiline | RegexOptions.Compiled);
                Regex spaceAfterTag = new Regex(@">\s+", RegexOptions.Compiled | RegexOptions.Multiline);
                List<Regex> lstExprs = new List<Regex>();
                lstExprs.Add(spaceBeforeTag);
                lstExprs.Add(spaceAfterTag);
                exp = new Expression(lstExprs);
                exp.EscapeInCss = true;
                exp.EscapeInJs = true;
                exp.ReplaceHandler = (expr, input) =>
                {
                    input = expr.RegularExpressions[0].Replace(input, conf.SupressWithWhiteSpace ? " <" : "<");
                    input = expr.RegularExpressions[1].Replace(input, conf.SupressWithWhiteSpace ? "> " : ">");
                    return input;
                };
                this.Add(exp);


                //Remove new line character
                exp = new Expression(new Regex(@"\r\n\s*|\n\s*", RegexOptions.Multiline | RegexOptions.Compiled));
                this.Add(exp);

                //Remove html comments '<!-- -->' from javascript
                exp = new Expression(new Regex(@"<script[\s\=\""\'\/\\\w]+>(<!--)?[\w;\(\):\s{}\',.\?]+(//\s*-->)?</script>", RegexOptions.Compiled | RegexOptions.Multiline));
                exp.EscapeInCss = true;
                exp.EscapeInJs = true;
                exp.ReplaceHandler = (expr, input) =>
                {
                    if (conf.IsHtmlCommentTagInJS)
                    {
                        string str = string.Empty;
                        string mainString = string.Empty;
                        if (expr.RegularExpression.IsMatch(input))
                        {
                            MatchCollection matchColl = expr.RegularExpression.Matches(input);
                            foreach (Match match in matchColl)
                            {
                                GroupCollection gc = match.Groups;
                                mainString = gc[0].Value;
                                if (gc[1].Value != string.Empty && gc[2].Value != string.Empty)
                                    input = input.Replace(mainString, mainString.Replace(gc[1].Value, string.Empty).Replace(gc[2].Value, string.Empty));
                            }

                        }
                    }
                    return input;
                };
                this.Add(exp);

                
                #endregion
            }
            return this;

        }

        static ExpressionCollection _instance;

        private static object synclock = new object();

        public static ExpressionCollection Instance
        {
            get
            {
                if (_instance == null)
                {
                    lock (synclock)
                    {
                        if (_instance == null)
                            _instance = new ExpressionCollection();
                    }
                }
                return _instance;
            }
        }
    }
}
