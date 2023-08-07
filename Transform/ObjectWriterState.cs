using Newtonsoft.Json.Linq;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace XPath2Json.Transform
{
    internal class ObjectWriterState : JsonWriterState
    {

        private ObjectWriterState(JsonWriterContext context) : base(context)
        {
            _context.JsonTextWriter.WriteStartObject();
        }

        public ObjectWriterState(JsonWriterState parent) : base(parent)
        {
            _context.JsonTextWriter.WriteStartObject();
        }

        public static ObjectWriterState CreateRoot(JsonWriterContext context)
        {
            return new ObjectWriterState(context);
        }

        public override void WriteStartElement(string localName)
        {
            _context.MoveToNextState(new PropertyWriterState(localName, this));
        }

        public override void WriteEndElement()
        {
            _context.JsonTextWriter.WriteEndObject();
            _context.MoveToPreviousState();
        }

        public override void WriteStartAttribute(string prefix, string localName, string ns)
        {
            throw new InvalidOperationException();
        }

        public override void WriteEndAttribute()
        {
            throw new InvalidOperationException();
        }

        public override void WriteString(string text)
        {
            throw new InvalidOperationException();
        }
    }
}
