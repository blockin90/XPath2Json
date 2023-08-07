using Newtonsoft.Json;
using System;
using System.Collections.Generic;

namespace XPath2Json.Transform
{
    class JsonWriterContext
    {
        internal JsonTextWriter JsonTextWriter
        { 
            get;
        }
        //internal Stack<JsonWriterState> JsonWriterStates { get; }

        public JsonWriterState CurrentState { get; set; }
        public JsonWriterContext()
        {
            JsonTextWriter = new JsonTextWriter(Console.Out);
#if DEBUG
            JsonTextWriter.Formatting = Formatting.Indented;
#endif
            //JsonWriterStates = new Stack<JsonWriterState>();
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
