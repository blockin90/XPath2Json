using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Reflection;

namespace XPath2Json.XPath
{
    class JsonTreeItem : JsonItem
    {
        protected readonly JToken _token;
        protected XPathItem[] _wrappedChildren = null;
        protected int _childIndex;

        public Dictionary<string, int> _childrenFirstIndex = new Dictionary<string, int>(); //name -> index


        public JsonTreeItem(JToken token, XPathItem parent = null) : base(parent)
        {
            _token = token;
        }

        protected internal override JToken JToken
        {
            get { return _token; }
        }

        public override XPathItem MoveToFirstAttribute()
        {
            return null;
        }

        public override XPathItem MoveToFirstChild()
        {
            InitChildren();
            if (_wrappedChildren.Length > 0) {
                return _wrappedChildren[_childIndex];
            }
            return null;
        }

        public override XPathItem MoveToAttribute(string name)
        {
            return null;
        }

        static protected List<JToken> GetJObjectChildrens(JObject obj)
        {
            List<JToken> children = new List<JToken>();
            foreach (var child in obj.Children()) {
                if (child.First is JArray) {
                    children.AddRange(child.First.Children());
                } else {
                    children.Add(child);
                }
            }
            return children;
        }

        protected virtual void InitChildren()
        {
            if (_wrappedChildren == null) {
                
                List<JToken> children;
                if (_token is JObject) { //для массивов
                    children = GetJObjectChildrens((JObject)_token);
                } else if (_token is JProperty && _token.First is JObject) {
                    children = new List<JToken>();
                    foreach (var child in _token.First.Children()) {
                        if (child is JArray) {
                            children.AddRange(child.Children());
                        } else {
                            children.Add(child);
                        }
                    }
                } else if (_token is JProperty && _token.First is JArray) {
                    children = _token.First.Children().ToList();
                } else {
                    children = _token.Children().ToList();
                }
                InitWrappedChildren(children);
            }
            _childIndex = 0;
        }

        protected void InitWrappedChildren(List<JToken> children)
        {
            _wrappedChildren = new JsonItem[children.Count];
            for (int i = 0; i < children.Count; ++i) {
                var wrapped = CreateWrappedItem(children[i],this);
                _wrappedChildren[i] = wrapped;

                if (!_childrenFirstIndex.ContainsKey(wrapped.Name)) {
                    _childrenFirstIndex[wrapped.Name] = i;
                }
            }

        }

        public XPathItem MoveToChild(string localName, string namespaceURI)
        {
            if(_wrappedChildren == null) {
                InitChildren();
                _childIndex = 0;
            }
            if( _childrenFirstIndex.TryGetValue(localName, out _childIndex)) {
                return _wrappedChildren[_childIndex];
            }
            return null;
        }

        public static XPathItem CreateWrappedItem(JToken child, JsonItem parent)
        {
            if (child is JValue) { //array
                return new JsonArrayTextItem((JValue)child, parent);
            } else {
                if (child is JProperty) {
                    var childProperty = (JProperty)child;
                    if (childProperty.Count == 1 && childProperty.First is JValue) {
                        return new JsonElementtem(childProperty, parent);
                    } else {
                        return new JsonTreeItem(childProperty, parent);
                    }
                } else if(child is JObject) {
                    return new JsonTreeItem(child, parent);
                } else {
                    throw new NotImplementedException();
                }
            }
            throw new NotImplementedException();
        }

        public override bool IsEmptyElement
        {
            get
            {
                InitChildren();
                return _wrappedChildren.Length == 0;
            }
        }

        public XPathItem GetNext()
        {
            if (_childIndex >= _wrappedChildren.Length - 1)
                return null;
            return _wrappedChildren[++_childIndex];
            
        }

        internal XPathItem GetPrevious()
        {
            if (_childIndex <= 0)
                return null;
            return _wrappedChildren[--_childIndex];
        }
    }
}
