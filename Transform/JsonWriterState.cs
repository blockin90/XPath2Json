namespace XPath2Json.Transform
{
    abstract class JsonWriterState
    {
        protected internal JsonWriterContext _context;
        public JsonWriterState(JsonWriterState parentState)
        {
            ParentState= parentState;
            _context = parentState._context;
        }
        public JsonWriterState(JsonWriterContext context)
        {
            ParentState = null;
            _context = context;
        }

        public JsonWriterState ParentState { get; }

        public abstract void WriteStartElement(string localName);
        public abstract void WriteStartAttribute(string prefix, string localName, string ns);
        public abstract void WriteEndElement();
        public abstract void WriteEndAttribute();
        public abstract void WriteString(string text);
    }
}
