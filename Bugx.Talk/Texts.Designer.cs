﻿//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a tool.
//     Runtime Version:2.0.50727.42
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace Bugx.Talk {
    using System;
    
    
    /// <summary>
    ///   A strongly-typed resource class, for looking up localized strings, etc.
    /// </summary>
    // This class was auto-generated by the StronglyTypedResourceBuilder
    // class via a tool like ResGen or Visual Studio.
    // To add or remove a member, edit your .ResX file then rerun ResGen
    // with the /str option, or rebuild your VS project.
    [global::System.CodeDom.Compiler.GeneratedCodeAttribute("System.Resources.Tools.StronglyTypedResourceBuilder", "2.0.0.0")]
    [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
    [global::System.Runtime.CompilerServices.CompilerGeneratedAttribute()]
    internal class Texts {
        
        private static global::System.Resources.ResourceManager resourceMan;
        
        private static global::System.Globalization.CultureInfo resourceCulture;
        
        [global::System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1811:AvoidUncalledPrivateCode")]
        internal Texts() {
        }
        
        /// <summary>
        ///   Returns the cached ResourceManager instance used by this class.
        /// </summary>
        [global::System.ComponentModel.EditorBrowsableAttribute(global::System.ComponentModel.EditorBrowsableState.Advanced)]
        internal static global::System.Resources.ResourceManager ResourceManager {
            get {
                if (object.ReferenceEquals(resourceMan, null)) {
                    global::System.Resources.ResourceManager temp = new global::System.Resources.ResourceManager("Bugx.Talk.Texts", typeof(Texts).Assembly);
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
        ///   Looks up a localized string similar to If you want to send a message to all subscribers, you must /*subscribe* to bugx list..
        /// </summary>
        internal static string ErrorChatWhenNotSubscribed {
            get {
                return ResourceManager.GetString("ErrorChatWhenNotSubscribed", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to _I think you&apos;re talking alone_ ^^;.
        ///_Perhaps you should ask for /help_.
        /// </summary>
        internal static string ErrorNoChatAvailable {
            get {
                return ResourceManager.GetString("ErrorNoChatAvailable", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Hello {0},
        ///I&apos;m sorry but i don&apos;t recognize your command [&quot;{1}&quot;] :&apos;(
        ///
        ///Send /? _or_ /help to have a complete set of available commands.
        /// </summary>
        internal static string ErrorUnknownCommand {
            get {
                return ResourceManager.GetString("ErrorUnknownCommand", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Application has restarted..
        /// </summary>
        internal static string InfoApplicationRestart {
            get {
                return ResourceManager.GetString("InfoApplicationRestart", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Welcome {0},
        ///I&apos;m Bugx Talk version {2}.
        ///I can help you for following commands:
        /// - /*Subscribe* : Subscribe to Bugx list
        /// - /*Unsubscribe* : Stop receiving Bugx information
        /// - /*Subscribers* : List all subscribers
        /// - /? _or_ /*help*: Give you these information
        ///What can I do for you? :-D.
        /// </summary>
        internal static string InfoHelpComplete {
            get {
                return ResourceManager.GetString("InfoHelpComplete", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Welcome {0},
        ///I will give you all information on bad things appening here.
        ///_Note: If you want to stop receiving these messages, simply tell me /*Unsubscribe*_.
        /// </summary>
        internal static string InfoSubscribeComplete {
            get {
                return ResourceManager.GetString("InfoSubscribeComplete", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Subscribers:{0}.
        /// </summary>
        internal static string InfoSubscribers {
            get {
                return ResourceManager.GetString("InfoSubscribers", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to You are now unsubscribed for this Bugx list..
        /// </summary>
        internal static string InfoUnsubscribeComplete {
            get {
                return ResourceManager.GetString("InfoUnsubscribeComplete", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to There is no subscriber.
        /// </summary>
        internal static string NoSubscribers {
            get {
                return ResourceManager.GetString("NoSubscribers", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to 
        ///      *Error from*: {0}
        ///*Message*: {1}
        ///*Type*: {2}
        ///*Assembly*: {3}
        ///*Report*: {4}
        ///    .
        /// </summary>
        internal static string WarningErrorInProduction {
            get {
                return ResourceManager.GetString("WarningErrorInProduction", resourceCulture);
            }
        }
    }
}
