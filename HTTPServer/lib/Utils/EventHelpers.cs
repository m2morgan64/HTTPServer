using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;

namespace HTTPServer.lib.Utils
{
    public static class EventHelpers
    {
        public static void RaiseSimpleEvent(object obj, EventHandler<EventArgs> evt)
        {
            evt?.Invoke(obj, new EventArgs());
        }

        public static void RaiseSingleStringEvent(object obj, EventHandler<SingleStringEventArgs> evt, string val)
        {
            evt?.Invoke(obj, new SingleStringEventArgs(val));
        }

        public static void RaiseMultiStringEvent(object obj, EventHandler<MultiStringEventArgs> evt, List<string> vals)
        {
            evt?.Invoke(obj, new MultiStringEventArgs(vals));
        }

        [SuppressMessage("ReSharper", "UnusedMember.Global")]
        public static void RaiseMultiStringEvent(object obj, EventHandler<MultiStringEventArgs> evt, params string[] vals)
        {
            evt?.Invoke(obj, new MultiStringEventArgs(vals));
        }

        public class SingleStringEventArgs : EventArgs
        {
            public string Value { get; protected set; }
            public SingleStringEventArgs(string val)
            {
                Value = val;
            }
        }

        public class MultiStringEventArgs : EventArgs
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
