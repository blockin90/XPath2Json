using Newtonsoft.Json.Linq;

namespace XPath2Json.XPath
{
	class JsonArrayTextItem : JsonItem
	{
		private readonly JValue _value;

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
				return new JsonTextItem(_value, this);
			} 
			return null;
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
