using Newtonsoft.Json.Linq;
using System;
using System.IO;
using System.Xml;
using System.Xml.XPath;

namespace XPath2Json.XPath
{
	public class JsonXPathNavigator : XPathNavigator
	{
		private XPathItem _rootItem;
		private JObject _json;

		NameTable nameTable = new NameTable();

        public JsonXPathNavigator(JObject json)
		{
			_rootItem = new JsonRootTreeItem(json.Root, null);
            _json= json;

        }

		private JsonXPathNavigator(JsonXPathNavigator navigator)
		{
			_rootItem = navigator._rootItem;
            _json = navigator._json;
			nameTable = navigator.nameTable;
        }

		public override string BaseURI
		{
			get { return string.Empty; }
		}

		public override XPathNavigator Clone()
		{
			return new JsonXPathNavigator(this);
		}

		public override bool IsEmptyElement
		{
			get { return _rootItem.IsEmptyElement ; }
		}

		public override bool IsSamePosition(XPathNavigator other)
		{
			var o = other as JsonXPathNavigator;
			return o != null && o._rootItem.IsSamePosition(_rootItem);
		}

		public override string LocalName
		{
			get { return _rootItem.Name; }
		}

		public override bool MoveTo(XPathNavigator other)
		{
			var o = other as JsonXPathNavigator;
			if (o != null)
			{
				_rootItem = o._rootItem;
				return true;
			}
			return false;
		}

		public override bool MoveToFirstAttribute()
		{
			return MoveToItem(_rootItem.MoveToFirstAttribute());
		}

		private bool MoveToItem(XPathItem newItem)
		{
			if (newItem == null) return false;
			_rootItem = newItem;
			return true;
		}

		public override bool MoveToFirstChild()
		{
			return MoveToItem(_rootItem.MoveToFirstChild());
		}

		public override bool MoveToFirstNamespace(XPathNamespaceScope namespaceScope)
		{
			return false;
		}

		public override bool MoveToId(string id)
		{
			return false;
		}

		public override bool MoveToNext()
		{
			return MoveToItem(_rootItem.MoveToNext());
		}

		public override bool MoveToNextAttribute()
		{
			return MoveToItem(_rootItem.MoveToNextAttribute());
		}

		public override bool MoveToNextNamespace(XPathNamespaceScope namespaceScope)
		{
			return false;
		}

		public override bool MoveToParent()
		{
			return MoveToItem(_rootItem.MoveToParent());
		}

		public override bool MoveToPrevious()
		{
			return MoveToItem(_rootItem.MoveToPrevious());
		}

		public override string Name
		{
			get { return LocalName; }
		}

		public override System.Xml.XmlNameTable NameTable
		{
			get { return nameTable; }
		}

		public override string NamespaceURI
		{
			get { return string.Empty; }
		}

		public override XPathNodeType NodeType
		{
			get { return _rootItem.NodeType; }
		}

		public override string Prefix
		{
			get { return string.Empty; }
		}

		public override string Value
		{
			get { return _rootItem.Value; }
		}

		public override bool MoveToAttribute(string localName, string namespaceURI)
		{
			if (namespaceURI != string.Empty) return false;
			return MoveToItem(_rootItem.MoveToAttribute(localName));
		}
        public override string OuterXml 
		{
			get
			{
				return _json.ToString();
			}
			set
			{
				throw new NotImplementedException();
			}
		}

    }
}
