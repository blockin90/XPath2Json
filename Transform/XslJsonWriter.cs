using Newtonsoft.Json.Linq;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using System.IO;

namespace XPath2Json.Transform
{
    public class XslJsonWriter : XmlWriter
    {
        JsonWriterContext context;
        public XslJsonWriter()
        {
            context = new JsonWriterContext();
        }
        public XslJsonWriter(StreamWriter writer)
        {
            context = new JsonWriterContext(writer);
        }

        public override string LookupPrefix(string ns)
        {
            return string.Empty;
        }

        public override void Flush()
        {
            while (context.CurrentState != null) {
                if (!(context.CurrentState is PropertyWriterState)) {
                    context.CurrentState.WriteEndElement();

                    //context.JsonTextWriter.WriteEnd();
                    //context.MoveToPreviousState();
                }
            }
            context.JsonTextWriter.Flush();
        }

        public override void WriteStartElement(string prefix, string localName, string ns)
        {
            context.CurrentState.WriteStartElement(localName);
        }
        public override void WriteStartAttribute(string prefix, string localName, string ns)
        {
            context.CurrentState.WriteStartAttribute(prefix, localName, ns);
        }
        public override void WriteEndElement()
        {
            context.CurrentState.WriteEndElement();
        }
        public override void WriteEndAttribute()
        {
            context.CurrentState.WriteEndAttribute();
        }
        public override void WriteString(string text)
        {
            context.CurrentState.WriteString(text);
        }

        #region Not implemented

        public override void WriteFullEndElement()
        {
            throw new NotImplementedException();
        }

        public override void WriteSurrogateCharEntity(char lowChar, char highChar)
        {
            throw new NotImplementedException();
        }

        public override void WriteWhitespace(string ws)
        {
            throw new NotImplementedException();
        }

        public override void WriteBase64(byte[] buffer, int index, int count)
        {
            throw new NotImplementedException();
        }

        public override void WriteCData(string text)
        {
            throw new NotImplementedException();
        }

        public override void WriteCharEntity(char ch)
        {
            throw new NotImplementedException();
        }

        public override void WriteChars(char[] buffer, int index, int count)
        {
            throw new NotImplementedException();
        }

        public override void WriteComment(string text)
        {
            throw new NotImplementedException();
        }

        public override void WriteDocType(string name, string pubid, string sysid, string subset)
        {
            throw new NotImplementedException();
        }


        public override void WriteEndDocument()
        {
            throw new NotImplementedException();
        }

        public override void WriteEntityRef(string name)
        {
            throw new NotImplementedException();
        }


        public override void WriteProcessingInstruction(string name, string text)
        {
            throw new NotImplementedException();
        }

        public override void WriteRaw(char[] buffer, int index, int count)
        {
            throw new NotImplementedException();
        }

        public override System.Xml.WriteState WriteState
        {
            get
            {
                throw new NotImplementedException();
            }
        }

        public override void WriteStartDocument()
        {
            throw new NotImplementedException();
        }

        public override void WriteStartDocument(bool standalone)
        {
            throw new NotImplementedException();
        }

        public override void WriteRaw(string data)
        {
            throw new NotImplementedException();
        }
        #endregion
    }
}
