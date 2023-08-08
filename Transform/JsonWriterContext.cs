using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Xml;

namespace XPath2Json.Transform
{
    class JsonWriterContext
    {
        internal JsonTextWriter JsonTextWriter
        { 
            get;
        }

        public readonly NameTable nameTable = new NameTable();

        public JsonWriterState CurrentState { get; set; }
        public JsonWriterContext()
        {
            JsonTextWriter = new JsonTextWriter(Console.Out);
#if DEBUG
            JsonTextWriter.Formatting = Newtonsoft.Json.Formatting.Indented;
#endif
            MoveToNextState(ObjectWriterState.CreateRoot(this));

            nameTable.Add(AttributeWriterState.arrayAttributeName);
            nameTable.Add(AttributeWriterState.nilAttributeName);
            nameTable.Add(AttributeWriterState.emptyAttributeName);
            nameTable.Add(AttributeWriterState.typeAttributeName);
            nameTable.Add(AttributeWriterState.emptyArrayAttributeName);
        }
        public JsonWriterContext(TextWriter writer)
        {
            JsonTextWriter = new JsonTextWriter(writer);
#if DEBUG
            //JsonTextWriter.Formatting = Formatting.Indented;
#endif
            MoveToNextState(ObjectWriterState.CreateRoot(this));
        }
        public JsonWriterState MoveToNextState(JsonWriterState newState)
        {
            return (CurrentState = newState);
        }

        public JsonWriterState MoveToPreviousState()
        {
            return (CurrentState = CurrentState.ParentState);
        }
    }
}
