using Newtonsoft.Json.Linq;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace XPath2Json.Transform
{
    internal class ArrayWriterState : ValueWriterState
    {
        string _localName;

        bool _isCurrentElementClosed; //для отлавливания "закрытий" родительского элемента

        public ArrayWriterState(JsonWriterState parent, string localName, JTokenType type) : base(parent)
        {
            _localName = localName;
            this.type = type;
            _isCurrentElementClosed = false;
            _context.JsonTextWriter.WriteStartArray();
        }
        public ArrayWriterState(JsonWriterState parent, string localName) : base(parent)
        {
            _localName = localName;
            _isCurrentElementClosed = false;
            _context.JsonTextWriter.WriteStartArray();
        }

        public override void WriteStartElement(string localName)
        {
            _attribute = JsonAttribute.None;
            if (_isCurrentElementClosed == true) {
                if (localName != _localName) {
                    _context.JsonTextWriter.WriteEndArray();
                    _context.MoveToPreviousState()
                        .WriteStartElement(localName);
                }
                _isCurrentElementClosed = false;
            } else {
                _isCurrentElementClosed = true; //управление передается в другое состояние, которое будет отвечать за закрытие элемента
                _context.MoveToNextState(new ObjectWriterState(this))
                    .WriteStartElement (localName);
            }
        }

        public override void WriteStartAttribute(string prefix, string localName, string ns)
        {
            _context.MoveToNextState(new AttributeWriterState(this))
                .WriteStartAttribute(prefix,localName,ns);
        }

        public override void WriteEndElement()
        {
            if (_isCurrentElementClosed) {
                //закрыватеся родительский для текущего элемента тег
                _context.JsonTextWriter.WriteEndArray();
                _context.MoveToPreviousState()
                    .WriteEndElement();
            } else {
                if ((_attribute & JsonAttribute.Empty) != 0) {
                    _context.JsonTextWriter.WriteStartObject();
                    _context.JsonTextWriter.WriteEndObject();
                }
                _isCurrentElementClosed = true;
            }
        }

        public override void WriteEndAttribute()
        {
            throw new InvalidOperationException();
        }

        public override void WriteString(string text)
        {
            WriteValue(text);
        }
    }
}
