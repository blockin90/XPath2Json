using Newtonsoft.Json.Linq;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace XPath2Json.Transform
{
    internal class AttributeWriterState : JsonWriterState
    {
        ValueWriterState _parent;
        string _attributeName;
        string _attributeNs;
        public AttributeWriterState( ValueWriterState parent) : base(parent)
        {
            _parent = parent;
        }

        public override void WriteStartElement(string localName)
        {
            throw new InvalidOperationException();
        }
        public override void WriteStartAttribute(string prefix, string localName, string ns)
        {
            _attributeName = localName;
            _attributeNs = ns;
        }

        public override void WriteEndElement()
        {
            throw new InvalidOperationException();
        }
        public override void WriteEndAttribute()
        {
            _context.MoveToPreviousState();
        }

        public override void WriteString(string text)
        {
            //todo: добавить проверку ns + конвертацию текста в bool
            if(_attributeName == "array" && text=="true") {
                _parent._attribute |= JsonAttribute.Array;
            } else if(_attributeName == "nil" && text == "true") {
                _parent._attribute |= JsonAttribute.Null;
            } else if(_attributeName == "type" ) {
                _parent._attribute |= JsonAttribute.Type;
                Enum.TryParse(text, true, out _parent.type);
            } else if(_attributeName == "empty" && text == "true") {
                _parent._attribute |= JsonAttribute.Empty;
            } else if(_attributeName == "emptyArray" && text == "true") {
                _parent._attribute |= JsonAttribute.EmptyArray;
            }
        }
    }
}
