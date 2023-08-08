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
        protected JToken[] _children = null;
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
            if (_children.Length > 0) {
                return GetWrappedChild(_childIndex);
            }
            return null;
        }

        public override XPathItem MoveToAttribute(string name)
        {
            return null;
        }

        static protected JToken[] GetJObjectChildrens(JObject obj)
        {
            List<JToken> children = new List<JToken>();
            foreach (var child in obj.Children().Cast<JProperty>()) {
                if (child.First is JArray) {
                    children.AddRange(child.First.Children());
                } else {
                    children.Add(child);
                }
            }
            return children.ToArray();
        }

        protected virtual void InitChildren()
        {
            if (_children == null) {
                if (_token is JObject) { //для массивов
                    _children = GetJObjectChildrens((JObject)_token);
                } else if (_token is JProperty && _token.First is JObject) {
                    List<JToken> children = new List<JToken>();
                    foreach (var child in _token.First.Children()) {
                        if (child is JArray) {
                            children.AddRange(child.Children());
                        } else {
                            children.Add(child);
                        }
                    }
                    _children = children.ToArray();
                } else if (_token is JProperty && _token.First is JArray) {
                    _children = _token.First.Children().ToArray();
                } else {
                    _children = _token.Children().Cast<JProperty>().ToArray();
                }
                InitWrappedChildren();
            }
            _childIndex = 0;
        }

        protected void InitWrappedChildren()
        {
            _wrappedChildren = new JsonItem[_children.Length];
            for (int i = 0; i < _children.Length; ++i) {
                var wrapped = CreateWrappedItem(_children[i]);
                _wrappedChildren[i] = wrapped;

                if (!_childrenFirstIndex.ContainsKey(wrapped.Name)) {
                    _childrenFirstIndex[wrapped.Name] = i;
                }
            }

        }

        public XPathItem MoveToChild(string localName, string namespaceURI)
        {
            if(_children == null) {
                InitChildren();
                _childIndex = 0;
            }
            if( _childrenFirstIndex.TryGetValue(localName, out _childIndex)) {
                return _wrappedChildren[_childIndex];
            }
            return null;
        }
        protected XPathItem CreateWrappedItem(JToken child)
        {
            return CreateItem(child, this);
        }

        public static XPathItem CreateItem(JToken child, JsonItem parent)
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
                return _children.Length == 0;
            }
        }

        XPathItem GetWrappedChild(int index)
        {
            return _wrappedChildren[index];
            //if (wrappedChild == null) {
            //    wrappedChild = CreateWrappedItem(_children[index]);
            //    _wrappedChildren[index] = wrappedChild;
            //}
            //return wrappedChild;
        }

        public XPathItem GetNext()
        {
            if (_childIndex >= _children.Length - 1)
                return null;
            return GetWrappedChild(++_childIndex);
            
        }

        internal XPathItem GetPrevious()
        {
            if (_childIndex <= 0)
                return null;
            return CreateWrappedItem(_children[--_childIndex]);
        }
    }
}
