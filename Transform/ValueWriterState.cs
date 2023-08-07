using Newtonsoft.Json.Linq;
using System;

namespace XPath2Json.Transform
{
    internal abstract class ValueWriterState : JsonWriterState
    {
        internal JTokenType type = JTokenType.String;
        internal JsonAttribute _attribute;
        protected bool _isValueWriten;
        public ValueWriterState(JsonWriterState parent) : base(parent)
        {
            _isValueWriten = false;
            _attribute = JsonAttribute.None;
        }
        public override void WriteStartAttribute(string prefix, string localName, string ns)
        {
            _context.MoveToNextState(new AttributeWriterState(this))
                .WriteStartAttribute(prefix, localName, ns);
        }
        public override void WriteEndAttribute()
        {
            throw new InvalidOperationException();
        }
        public override void WriteEndElement()
        {
            if (!_isValueWriten) {
                WriteValue(string.Empty);
            }
            _context.MoveToPreviousState();
        }
        protected void WriteValue(string text)
        {
            if ((_attribute & JsonAttribute.Null) != 0) {
                _context.JsonTextWriter.WriteNull();
            } else if ((_attribute & JsonAttribute.Empty) != 0 && string.IsNullOrEmpty(text)) {
                _context.JsonTextWriter.WriteStartObject();
                _context.JsonTextWriter.WriteEndObject();
            } else {
                switch (type) {
                    case JTokenType.Float:
                        text = text.Replace(',', '.');
                        goto case JTokenType.Integer;
                    case JTokenType.Boolean:
                    case JTokenType.Integer:
                        _context.JsonTextWriter.WriteRawValue(text);
                        break;
                    default:
                        _context.JsonTextWriter.WriteRawValue($"\"{text}\"");
                        //_context.JsonTextWriter.WriteRawValue(text);
                        break;
                }
            }
        }
    }
}