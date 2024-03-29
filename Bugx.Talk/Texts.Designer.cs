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
        ///   Looks up a localized string similar to *Application is shutting down*.
        ///Reason: _{0}_..
        /// </summary>
        internal static string ApplicationIsShuttingDown {
            get {
                return ResourceManager.GetString("ApplicationIsShuttingDown", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Command complete..
        /// </summary>
        internal static string CommandComplete {
            get {
                return ResourceManager.GetString("CommandComplete", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to If you want to send a message to all subscribers, you must /*subscribe* to bugx list.
        ///_Tell me /*help* for more information_.
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
        /// - /*Announcement* : Show a short message in bot status
        /// - /*Recycle* : Enable you to restart application pool
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
        ///I will give you all information on bad things happening here.
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
        ///   Looks up a localized string similar to The application shut down because of a change to the Bin folder or files contained in it..
        /// </summary>
        internal static string ShutdownBinDirChangeOrDirectoryRename {
            get {
                return ResourceManager.GetString("ShutdownBinDirChangeOrDirectoryRename", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to The application shut down because of a change to the App_Browsers folder or files contained in it..
        /// </summary>
        internal static string ShutdownBrowsersDirChangeOrDirectoryRename {
            get {
                return ResourceManager.GetString("ShutdownBrowsersDirChangeOrDirectoryRename", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to The application shut down because of a change to Global.asax..
        /// </summary>
        internal static string ShutdownChangeInGlobalAsax {
            get {
                return ResourceManager.GetString("ShutdownChangeInGlobalAsax", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to The application shut down because of a change in the code access security policy file..
        /// </summary>
        internal static string ShutdownChangeInSecurityPolicyFile {
            get {
                return ResourceManager.GetString("ShutdownChangeInSecurityPolicyFile", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to The application shut down because of a change to the App_Code folder or files contained in it..
        /// </summary>
        internal static string ShutdownCodeDirChangeOrDirectoryRename {
            get {
                return ResourceManager.GetString("ShutdownCodeDirChangeOrDirectoryRename", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to The application shut down because of a change to the application level configuration..
        /// </summary>
        internal static string ShutdownConfigurationChange {
            get {
                return ResourceManager.GetString("ShutdownConfigurationChange", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to The application shut down because of the hosting environment..
        /// </summary>
        internal static string ShutdownHostingEnvironment {
            get {
                return ResourceManager.GetString("ShutdownHostingEnvironment", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to The application shut down because of a call to System.Web.HttpRuntime.Close()..
        /// </summary>
        internal static string ShutdownHttpRuntimeClose {
            get {
                return ResourceManager.GetString("ShutdownHttpRuntimeClose", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to The application shut down because of an application initialization error..
        /// </summary>
        internal static string ShutdownInitializationError {
            get {
                return ResourceManager.GetString("ShutdownInitializationError", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to The application shut down because of the maximum number of dynamic recompiles of resources limit..
        /// </summary>
        internal static string ShutdownMaxRecompilationsReached {
            get {
                return ResourceManager.GetString("ShutdownMaxRecompilationsReached", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to No Shutdown reason provided..
        /// </summary>
        internal static string ShutdownNone {
            get {
                return ResourceManager.GetString("ShutdownNone", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to The application shut down because of a change to the physical path for the application..
        /// </summary>
        internal static string ShutdownPhysicalApplicationPathChanged {
            get {
                return ResourceManager.GetString("ShutdownPhysicalApplicationPathChanged", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to The application shut down because of a change to the App_GlobalResources folder or files contained in it..
        /// </summary>
        internal static string ShutdownResourcesDirChangeOrDirectoryRename {
            get {
                return ResourceManager.GetString("ShutdownResourcesDirChangeOrDirectoryRename", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to The application shut down because of a call to System.Web.HttpRuntime.UnloadAppDomain()..
        /// </summary>
        internal static string ShutdownUnloadAppDomainCalled {
            get {
                return ResourceManager.GetString("ShutdownUnloadAppDomainCalled", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to 
        ///      *Error from*: {0}
        ///*From BOT*: {5}      
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
