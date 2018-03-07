using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TCPServer.lib
{
    public static class EventHelpers
    {
        public static void RaiseSingleStringEvent(object obj, EventHandler<SingleStringEventArgs> evt, string val)
        {
            if (evt != null)
            {
                evt(obj, new SingleStringEventArgs(val));
            }
        }

        public static void RaiseMultiStringEvent(object obj, EventHandler<MultiStringEventArgs> evt, List<string> vals)
        {
            if (evt != null)
            {
                evt(obj, new MultiStringEventArgs(vals));
            }
        }

        public static void RaiseMultiStringEvent(object obj, EventHandler<MultiStringEventArgs> evt, params string[] vals)
        {
            if (evt != null)
            {
                evt(obj, new MultiStringEventArgs(vals));
            }
        }

        public class SingleStringEventArgs : System.EventArgs
        {
            public string Value { get; protected set; }
            public SingleStringEventArgs(string val)
            {
                Value = val;
            }
        }

        public class MultiStringEventArgs : System.EventArgs
        {
            public List<string> Values { get; protected set; }
            public MultiStringEventArgs(List<string> vals)
            {
                Values = vals;
            }

            public MultiStringEventArgs(params string[] vals)
            {
                Values.AddRange(vals);
            }
        }
    }
}
