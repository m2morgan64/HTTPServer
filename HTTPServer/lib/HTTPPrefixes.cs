using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using static HTTPServer.lib.Utils.EventHelpers;

namespace HTTPServer.lib
{
    public class HttpPrefixes<TString> : List<TString>
    {
        public event EventHandler<SingleStringEventArgs> OnAdd;
        public event EventHandler<MultiStringEventArgs> OnAddRange;
        public event EventHandler<SingleStringEventArgs> OnRemove;
        public event EventHandler<MultiStringEventArgs> OnRemoveRange;
        
        public new void Add(TString item)
        {
            RaiseSingleStringEvent(this, OnAdd, item as string);
            base.Add(item);
        }

        public new void AddRange(IEnumerable<TString> items)
        {
            RaiseMultiStringEvent(this, OnAddRange, items as List<string>);
            base.AddRange(items);
        }

        public new void Remove(TString item)
        {
            RaiseSingleStringEvent(this, OnRemove, item as string);
            base.Remove(item);
        }

        [SuppressMessage("ReSharper", "UnusedMember.Global")]
        public new void RemoveRange(Int32 index, Int32 count)
        {
            RaiseMultiStringEvent(this, OnRemoveRange, GetRange(index, count) as List<string>);
            base.RemoveRange(index, count);
        }
    }
}
