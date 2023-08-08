using Newtonsoft.Json.Linq;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static System.Net.Mime.MediaTypeNames;
using XPath2Json.Transform;
using System.Runtime.InteropServices;

namespace XPath2Json.Transform
{
    internal class AttributeWriterState : JsonWriterState
    {
        readonly ValueWriterState _parent;
        //string _attributeName;
        //string _attributeNs;

        JsonAttribute _attribute = JsonAttribute.None;

        public const string arrayAttributeName = "array";
        public const string nilAttributeName = "nil";
        public const string typeAttributeName = "type";
        public const string emptyAttributeName = "empty";
        public const string emptyArrayAttributeName = "emptyArray";

        public AttributeWriterState(ValueWriterState parent) : base(parent)
        {
            _parent = parent;
        }

        public override void WriteStartElement(string localName)
        {
            throw new InvalidOperationException();
        }
        public override void WriteStartAttribute(string prefix, string localName, string ns)
        {
            //_attributeName = _context.nameTable.Get(localName);
            //_attributeNs = ns;

            var attributeName = _context.nameTable.Add(localName);
            if (ReferenceEquals(attributeName, arrayAttributeName)) {
                _attribute = JsonAttribute.Array;
            } else if (ReferenceEquals(attributeName, nilAttributeName)) {
                _attribute = JsonAttribute.Null;
            } else if (ReferenceEquals(attributeName, typeAttributeName)) {
                _attribute = JsonAttribute.Type;
            } else if (ReferenceEquals(attributeName, emptyAttributeName)) {
                _attribute = JsonAttribute.Empty;
            } else if (ReferenceEquals(attributeName, emptyArrayAttributeName)) {
                _attribute = JsonAttribute.EmptyArray;
            }
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
            if( _attribute == JsonAttribute.Type) {
                _parent._attribute |= JsonAttribute.Type;
                _parent.type = JTokenType.String;
                switch (text) {
                    case "integer": 
                        _parent.type = JTokenType.Integer; break;
                    case "float":
                        _parent.type = JTokenType.Float; break;
                    case "boolean":
                        _parent.type = JTokenType.Boolean;
                        break;
                    default:
                        _parent.type = JTokenType.String;
                        break;
                }
            } else if(text == "true"){
                _parent._attribute |= _attribute;
            }

            //if (ReferenceEquals(_attributeName, arrayAttributeName)) {
            //    if (text == "true") {
            //        _parent._attribute |= JsonAttribute.Array;
            //    }
            //} else if (ReferenceEquals(_attributeName, nilAttributeName)) {
            //    if (text == "true") {
            //        _parent._attribute |= JsonAttribute.Null;
            //    }
            //} else if (ReferenceEquals(_attributeName, typeAttributeName)) {
            //    _parent._attribute |= JsonAttribute.Type;
            //    _parent.type = JTokenType.String;
            //    //Enum.TryParse(text, true, out _parent.type);
            //} else if (ReferenceEquals(_attributeName, emptyAttributeName)) {
            //    if (text == "true") {
            //        _parent._attribute |= JsonAttribute.Empty;
            //    }
            //} else if (ReferenceEquals(_attributeName, emptyArrayAttributeName)) {
            //    if (text == "true") {
            //        _parent._attribute |= JsonAttribute.EmptyArray;
            //    }
            //}
        }
    }
}
