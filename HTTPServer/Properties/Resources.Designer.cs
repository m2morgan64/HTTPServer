﻿//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a tool.
//     Runtime Version:4.0.30319.42000
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace HTTPServer.Properties {
    using System;
    
    
    /// <summary>
    ///   A strongly-typed resource class, for looking up localized strings, etc.
    /// </summary>
    // This class was auto-generated by the StronglyTypedResourceBuilder
    // class via a tool like ResGen or Visual Studio.
    // To add or remove a member, edit your .ResX file then rerun ResGen
    // with the /str option, or rebuild your VS project.
    [global::System.CodeDom.Compiler.GeneratedCodeAttribute("System.Resources.Tools.StronglyTypedResourceBuilder", "15.0.0.0")]
    [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
    [global::System.Runtime.CompilerServices.CompilerGeneratedAttribute()]
    internal class Resources {
        
        private static global::System.Resources.ResourceManager resourceMan;
        
        private static global::System.Globalization.CultureInfo resourceCulture;
        
        [global::System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1811:AvoidUncalledPrivateCode")]
        internal Resources() {
        }
        
        /// <summary>
        ///   Returns the cached ResourceManager instance used by this class.
        /// </summary>
        [global::System.ComponentModel.EditorBrowsableAttribute(global::System.ComponentModel.EditorBrowsableState.Advanced)]
        internal static global::System.Resources.ResourceManager ResourceManager {
            get {
                if (object.ReferenceEquals(resourceMan, null)) {
                    global::System.Resources.ResourceManager temp = new global::System.Resources.ResourceManager("HTTPServer.Properties.Resources", typeof(Resources).Assembly);
                    resourceMan = temp;
                }
                return resourceMan;
            }
        }
        
        /// <summary>
        ///   Overrides the current thread's CurrentUICulture property for all
        ///   resource lookups using this strongly typed resource class.
        /// </summary>
        [global::System.ComponentModel.EditorBrowsableAttribute(global::System.ComponentModel.EditorBrowsableState.Advanced)]
        internal static global::System.Globalization.CultureInfo Culture {
            get {
                return resourceCulture;
            }
            set {
                resourceCulture = value;
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to &lt;HTML&gt;
        ///&lt;Head&gt;
        ///&lt;Title&gt;%CODE% - %REASON%&lt;/Title&gt;
        ///&lt;/Head&gt;
        ///&lt;Body&gt;
        ///&lt;H1&gt;%CODE%&lt;/H1&gt;
        ///&lt;H2&gt;%REASON%&lt;/H2&gt;
        ///&lt;center&gt;
        ///&lt;svg id=&quot;svg&quot; version=&quot;1.1&quot; width=&quot;400&quot; height=&quot;308.48329048843186&quot; xmlns=&quot;http://www.w3.org/2000/svg&quot; xmlns:xlink=&quot;http://www.w3.org/1999/xlink&quot; style=&quot;display: block;&quot;&gt;&lt;g id=&quot;svgg&quot;&gt;&lt;path id=&quot;path0&quot; d=&quot;M4.398 164.583 L 4.475 297.068 193.364 297.068 L 382.253 297.068 382.331 164.583 L 382.408 32.099 193.364 32.099 L 4.320 32.099 4.398 164.583 M211.623 34.493 C 213.942 34.642,215.875 34.913,216.13 [rest of string was truncated]&quot;;.
        /// </summary>
        internal static string ErrorResponseContentTemplate {
            get {
                return ResourceManager.GetString("ErrorResponseContentTemplate", resourceCulture);
            }
        }
    }
}
