using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.XPath;

namespace XPath2Json
{
	/// <summary>
	/// Abstract class that defines behaviour of the XPath navigator positioned on a specific item
	/// </summary>
	internal abstract class XPathItem
	{
		public abstract string Name { get; }
		public virtual XPathItem MoveToFirstAttribute()
		{ return null; }
		public virtual XPathItem MoveToNextAttribute()
		{ return null; }
        public virtual XPathItem MoveToAttribute(string name)
        { return null; }
        public abstract XPathItem MoveToFirstChild();
		public abstract XPathItem MoveToNext();
		public abstract XPathItem MoveToPrevious();
		public abstract XPathNodeType NodeType { get; }
		public virtual string Value { get { return string.Empty; } }
		public abstract bool IsEmptyElement { get; }
		public abstract XPathItem MoveToParent();
        public abstract bool IsSamePosition(XPathItem item);
	}
}
