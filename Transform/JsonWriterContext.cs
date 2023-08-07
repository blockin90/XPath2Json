using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;

namespace XPath2Json.Transform
{
    class JsonWriterContext
    {
        internal JsonTextWriter JsonTextWriter
        { 
            get;
        }

        public JsonWriterState CurrentState { get; set; }
        public JsonWriterContext()
        {
            JsonTextWriter = new JsonTextWriter(Console.Out);
#if DEBUG
            JsonTextWriter.Formatting = Formatting.Indented;
#endif
            MoveToNextState(ObjectWriterState.CreateRoot(this));
        }
        public JsonWriterContext(TextWriter writer)
        {
            JsonTextWriter = new JsonTextWriter(writer);
#if DEBUG
            JsonTextWriter.Formatting = Formatting.Indented;
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
