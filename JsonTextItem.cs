using Newtonsoft.Json.Linq;
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Xml;
using System.Xml.XPath;

namespace XPath2Json
{
	class JsonTextItem : JsonItem
	{
		private readonly JValue _value;

		public JsonTextItem(JValue value, XPathItem parent = null) : base(parent)
		{
			_value = value;
		}

		protected internal override JToken JToken
        {
			get { return _value; }
		}

        public override XPathNodeType NodeType
		{
			get 
			{
				return XPathNodeType.Text; 
			}
		}

        public override XPathItem MoveToFirstAttribute()
		{
			return null;
		}

		public override XPathItem MoveToFirstChild()
		{
			return null;
		}

        public override string Value
		{
			get 
			{
				return _value.ToString(); 
			}
		}

        public override bool IsEmptyElement
		{
			get { return _value.Value == null; }
		}

		public override XPathItem MoveToAttribute(string name)
		{
			return null;			
		}
	}
}
