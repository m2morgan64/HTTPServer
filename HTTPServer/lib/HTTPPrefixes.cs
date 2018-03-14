using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static HTTPServer.lib.Utils.EventHelpers;

namespace HTTPServer.lib
{
    public class HTTPPrefixes<String> : List<String>
    {
        public event EventHandler<SingleStringEventArgs> OnAdd;
        public event EventHandler<MultiStringEventArgs> OnAddRange;
        public event EventHandler<SingleStringEventArgs> OnRemove;
        public event EventHandler<MultiStringEventArgs> OnRemoveRange;
        
        public new void Add(String item)
        {
            RaiseSingleStringEvent(this, OnAdd, item as string);
            base.Add(item);
        }

        public new void AddRange(IEnumerable<String> items)
        {
            RaiseMultiStringEvent(this, OnAddRange, items as List<string>);
            base.AddRange(items);
        }

        public new void Remove(String item)
        {
            RaiseSingleStringEvent(this, OnRemove, item as string);
            base.Remove(item);
        }

        public new void RemoveRange(Int32 index, Int32 count)
        {
            RaiseMultiStringEvent(this, OnRemoveRange, this.GetRange(index, count) as List<string>);
            base.RemoveRange(index, count);
        }
    }
}
