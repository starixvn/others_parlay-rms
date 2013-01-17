using System.Configuration;
using System.IO;
using System.Text;
using System.Web;
using System.Web.Configuration;

namespace MinifyContent
{

    class ResponseStream : MemoryStream
    {

        Stream outputStream;
        string outputContentType;

        public ResponseStream(HttpResponse response)
            : this(response.Filter, response.ContentType)
        { }

        public ResponseStream(Stream stream)
            : this(stream, "text/html")
        { }

        public ResponseStream(Stream stream, string contentType)
        {
            outputStream = stream;
            outputContentType = contentType;
        }

        public override void Write(byte[] buffer, int offset, int count)
        {
            //convert byte array to string
            string bufferString = UTF8Encoding.UTF8.GetString(buffer);
            
            //loop all the expressions to minify content
            foreach (Expressions.Expression expr in Expressions.ExpressionCollection.Instance.Collections())
            {
                //Check if expression can be ignored in CSS case
                if (outputContentType == "text/css" && expr.EscapeInCss)
                    continue;

                //TODO: Javascript stuff!!!
                
              
                //if custom handler is define then execute it.
                if (expr.ReplaceHandler == null)
                    bufferString = expr.RegularExpression.Replace(bufferString, expr.ReplaceWith);
                else
                    bufferString = expr.ReplaceHandler(expr, bufferString);

            }

            //write the content with converting in bytes
            outputStream.Write(UTF8Encoding.UTF8.GetBytes(bufferString), offset, UTF8Encoding.UTF8.GetByteCount(bufferString));
        }

    }
}
