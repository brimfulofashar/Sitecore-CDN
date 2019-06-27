using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Web;
using HtmlAgilityPack;

namespace Feature.CDN.Filters
{
    public class RemoveStaticMarkupFilter : MemoryStream
    {
        private readonly Stream _responseStream;
        private readonly Encoding _encoding;
        private readonly MemoryStream _buffer;

        public RemoveStaticMarkupFilter(Stream stream, Encoding encoding)
        {
            _buffer = new MemoryStream();
            _responseStream = stream;
            _encoding = encoding;
        }

        public virtual string ProcessHtml(string html)
        {
            var htmlDoc = new HtmlDocument();
            htmlDoc.LoadHtml(html);
            StringBuilder output = new StringBuilder();
            foreach (HtmlNode node in htmlDoc.DocumentNode.DescendantsAndSelf().Where(x => x.Attributes.Any(y => y.Name == "data-rs" && y.Value== "1")).ToList())
            {
                output.AppendLine(node.OuterHtml);
            }

            return "<html>" + output.ToString() + "</html>";
        }

        public override void Flush()
        {
            var html = _encoding.GetString(_buffer.ToArray());

            html = ProcessHtml(html);

            var outBuffer = _encoding.GetBytes(html);

            _responseStream.Write(outBuffer, 0, outBuffer.Length);

            base.Flush();
        }

        public override void Write(byte[] buffer, int offset, int count)
        {
            _buffer.Write(buffer, offset, count);
        }
    }
}