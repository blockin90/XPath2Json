using Newtonsoft.Json.Linq;
using System.Runtime.Remoting.Contexts;
using static System.Net.Mime.MediaTypeNames;

namespace XPath2Json.Transform
{
    internal class PropertyWriterState : ValueWriterState
    {
        private string _localName;
        public PropertyWriterState(string localName, JsonWriterState parent) : base(parent)
        {
            _localName = localName;
            _context.JsonTextWriter.WritePropertyName(localName);
        }

        public override void WriteStartElement(string localName)
        {
            if ((_attribute & JsonAttribute.Array) != 0) {
                _context.MoveToNextState(new ArrayWriterState(ParentState, _localName))
                    .WriteStartElement(localName);
            } else {
                _isValueWriten = true;
                _context.MoveToNextState(new ObjectWriterState(ParentState))
                    .WriteStartElement(localName);
            }
        }

        public override void WriteEndElement()
        {
            if ((_attribute & JsonAttribute.EmptyArray) != 0) {
                _context.JsonTextWriter.WriteStartArray();
                _context.JsonTextWriter.WriteEndArray();
                _context.MoveToPreviousState();
            } else {
                base.WriteEndElement();
            }
        }

        public override void WriteString(string text)
        {
            if ((_attribute & JsonAttribute.Array) != 0) {
                _context.MoveToNextState(new ArrayWriterState(ParentState, _localName, type))
                    .WriteString(text);
            } else {
                if (type != JTokenType.String && string.IsNullOrEmpty(text)) {
                    _context.JsonTextWriter.WriteNull();
                } else {
                    WriteValue(text);
                }
                _isValueWriten = true;
            }
        }
    }
}