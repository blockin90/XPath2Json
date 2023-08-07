using Newtonsoft.Json.Linq;
using System.Xml.XPath;

namespace XPath2Json.XPath
{
	class JsonElementtem : JsonItem
	{
		private readonly JProperty _property;
        JsonTextItem child = null;

        public JsonElementtem(JProperty property, XPathItem parent = null) : base(parent)
		{
			_property = property;
		}

		protected internal override JToken JToken
        {
			get { return _property; }
		}

		public override XPathItem MoveToFirstChild()
		{
			if (!IsEmptyElement) {
				if (child == null) {
					child = new JsonTextItem(_property.First as JValue, this);
				}
			} 
			return child;
		}

        public override string Value
		{
			get 
			{
				return (_property.First as JValue)?.Value.ToString(); 
			}
		}

        public override bool IsEmptyElement
		{
			get { return _property.First == null; }
		}
	}
}
