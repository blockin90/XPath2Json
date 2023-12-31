﻿using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Xml.XPath;

namespace XPath2Json.XPath
{
    class JsonRootTreeItem : JsonTreeItem
    {
        public JsonRootTreeItem(JToken token, XPathItem parent = null) : base(token, parent)
        {
        }

        protected override void InitChildren()
        {
            if (_wrappedChildren == null) {
                var children = GetJObjectChildrens((JObject)_token);
                InitWrappedChildren(children);
            }
            _childIndex = 0;
        }

        public override string Name
        {
            get { return string.Empty; }
        }

        public override XPathNodeType NodeType
        {
            get { return XPathNodeType.Root; }
        }
    }
}
