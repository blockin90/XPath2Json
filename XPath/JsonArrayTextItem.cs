using Newtonsoft.Json.Linq;

namespace XPath2Json.XPath
{
	class JsonArrayTextItem : JsonItem
	{
		private readonly JValue _value;
		JsonTextItem child = null;

        public JsonArrayTextItem(JValue property, XPathItem parent = null) : base(parent)
		{
			_value = property;
		}

		protected internal override JToken JToken
        {
			get { return _value; }
		}

		public override XPathItem MoveToFirstChild()
		{
			if (!IsEmptyElement) {
				if(child == null) {
                    return new JsonTextItem(_value, this);
                }
			} 
			return child;
		}

        public override string Value
		{
			get { return _value?.Value.ToString(); }
		}

        public override bool IsEmptyElement
		{
			get { return _value == null; }
		}
	}
}
