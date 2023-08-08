using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Xml.XPath;
using System.Xml;
using Newtonsoft.Json.Linq;
using System.Diagnostics;
using System.Reflection.Emit;

namespace XPath2Json.XPath
{
	/// <summary>
	/// Base class for XPath items, representing file system items: directories and files
	/// </summary>
	internal abstract class JsonItem : XPathItem
	{
		private XPathItem _parent;

		protected JsonItem(XPathItem parent = null)
		{
			_parent = parent;
		}

		protected internal abstract JToken JToken { get; }

		/// <summary>
		/// A json item path
		/// </summary>
		public virtual string Path
		{
			get { return JToken.Path; }
		}
		string name = null;

		/// <summary>
		/// node's local name
		/// </summary>
		public override string Name
		{
			get 
			{
				if (name == null) {
					if (JToken.Parent is JArray) {
						name = ((JProperty)JToken.Parent.Parent).Name;
					} else if (JToken is JObject) {
						name = ((JProperty)JToken.Parent).Name;
					} else if (JToken is JProperty) {
						name = ((JProperty)JToken).Name;
					} else {
						Console.WriteLine("exception");
						throw new NotImplementedException();
					}

//					Debug.WriteLine(name);
				}
				return name;
			}
		}

		public override XPathItem MoveToNext()
		{
			var dir = _parent as JsonTreeItem;
			return dir == null ? null : dir.GetNext();
		}

		public override XPathItem MoveToParent()
		{
			return _parent;
		}

		public override XPathItem MoveToPrevious()
		{
			var dir = _parent as JsonTreeItem;
			return dir == null ? null : dir.GetPrevious();
		}

		public override XPathNodeType NodeType
		{
			get { return XPathNodeType.Element; }
		}

		public override bool IsSamePosition(XPathItem _item)
		{
			var fsi = _item as JsonItem;
			return fsi != null && Path == fsi.Path;
		}
	}

}
