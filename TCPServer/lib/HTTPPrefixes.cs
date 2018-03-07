using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static TCPServer.lib.EventHelpers;

namespace TCPServer.lib
{
    public class HTTPPrefixes<T> : List<T>
    {
        public event EventHandler<SingleStringEventArgs> OnAdd;
        public event EventHandler<MultiStringEventArgs> OnAddRange;
        public event EventHandler<SingleStringEventArgs> OnRemove;
        public event EventHandler<MultiStringEventArgs> OnRemoveRange;
        
        public new void Add(T item)
        {
            RaiseSingleStringEvent(this, OnAdd, item as string);
            base.Add(item);
        }

        public new void AddRange(IEnumerable<T> items)
        {
            RaiseMultiStringEvent(this, OnAddRange, items as List<string>);
            base.AddRange(items);
        }

        public new void Remove(T item)
        {
            RaiseSingleStringEvent(this, OnRemove, item as string);
            base.Remove(item);
        }

        public new void RemoveRange(Int32 index, Int32 count)
        {
            RaiseSingleStringEvent(this, OnRemove, item as string);
            base.RemoveRange(index, count);
        }
    }
}
