/*
BUGx Talk: An Asp.Net Bug Tracking tool.
Copyright (C) 2007 Olivier Bossaer

This library is free software; you can redistribute it and/or
modify it under the terms of the GNU Lesser General Public
License as published by the Free Software Foundation; either
version 2.1 of the License, or (at your option) any later version.

This library is distributed in the hope that it will be useful,
but WITHOUT ANY WARRANTY; without even the implied warranty of
MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the GNU
Lesser General Public License for more details.

You should have received a copy of the GNU Lesser General Public
License along with this library; if not, write to the Free Software
Foundation, Inc., 51 Franklin Street, Fifth Floor, Boston, MA  02110-1301  USA

Wavenet, hereby disclaims all copyright interest in
the library `BUGx' (An Asp.Net Bug Tracking tool) written
by Olivier Bossaer. (olivier.bossaer@gmail.com)
*/

using System;
using System.Web;
using jabber.client;
using jabber.protocol.client;
using bedrock.net;
using System.Globalization;
using System.Text.RegularExpressions;
using Bugx.Web;
using System.Text;
using System.Collections.Generic;
using System.Reflection;
using System.Web.Hosting;

namespace Bugx.Talk
{
    public class Bot: IHttpModule
    {
        JabberClient _Bot;
        
        /// <summary>
        /// Releases unmanaged resources and performs other cleanup operations before the
        /// <see cref="Bugx.Talk.Bot"/> is reclaimed by garbage collection.
        /// </summary>
        ~Bot()
        {
            Dispose(false);
        }

        /// <summary>
        /// Disposes of the resources (other than memory) used by the module that implements <see cref="T:System.Web.IHttpModule"></see>.
        /// </summary>
        public void Dispose()
        {
            Dispose(true);
        }

        /// <summary>
        /// Disposes of the resources (other than memory) used by the module that implements <see cref="T:System.Web.IHttpModule"></see>.
        /// </summary>
        /// <param name="disposing">if set to <c>true</c> dispose; Otherwise destroying.</param>
        protected virtual void Dispose(bool disposing)
        {
            if (disposing)
            {
                _Bot.Dispose();
            }
        }

        static bool _Initialized;
        /// <summary>
        /// Initializes a module and prepares it to handle requests.
        /// </summary>
        /// <param name="context">An <see cref="T:System.Web.HttpApplication"></see> that provides access to the methods, properties, and events common to all application objects within an ASP.NET application</param>
        public void Init(HttpApplication context)
        {
            if (ErrorModule.IsReBug || _Initialized)
            {//Disable bot when it's a ReBug or if module is already initialized
                return;
            }
            _Initialized = true;
            SubscriptionManager.LoadSettings();
            _Bot = new JabberClient();
            _Bot.AutoPresence = false;
            _Bot.OnMessage += new MessageHandler(Bot_OnMessage);
            _Bot.OnAuthenticate += new bedrock.ObjectHandler(Bot_OnAuthenticate);
            _Bot.OnPresence += new PresenceHandler(_Bot_OnPresence);

            AsyncSocket.UntrustedRootOK = true;
            ErrorModule.ErrorComplete += new EventHandler<BugEventArgs>(ErrorModule_ErrorComplete);
            ErrorModule.ApplicationUnload += new EventHandler<ApplicationUnloadEventArgs>(ErrorModule_ApplicationUnload);

            SubscriptionManager settings = SubscriptionManager.Instance;
            _Bot.User         = settings.User;
            _Bot.Password     = settings.Password;
            _Bot.Server       = settings.Server;
            _Bot.NetworkHost  = settings.NetworkHost;
            _Bot.Resource     = "Bugx Talk (bot)";
            _Bot.AutoStartTLS = true;

            _Bot.Connect();
        }

        /// <summary>
        /// Handles the ApplicationUnload event of the ErrorModule control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="Bugx.Web.ApplicationUnloadEventArgs"/> instance containing the event data.</param>
        void ErrorModule_ApplicationUnload(object sender, ApplicationUnloadEventArgs e)
        {
            string reason = e.Reason.ToString();
            Message(string.Format(CultureInfo.InvariantCulture,
                                  Texts.ApplicationIsShuttingDown,
                                  Texts.ResourceManager.GetString("Shutdown" + reason, Texts.Culture) ?? reason));
        }

        /// <summary>
        /// Send a message to all subscribers.
        /// </summary>
        /// <param name="text">The text.</param>
        void Message(string text)
        {
            foreach (string user in SubscriptionManager.Instance)
            {
                _Bot.Message(user, text);
            }
        }

        Dictionary<string, PresenceType> _Presence = new Dictionary<string, PresenceType>();

        void _Bot_OnPresence(object sender, Presence pres)
        {
            string user = pres.From.ToString();
            if (pres.Type == PresenceType.unavailable || pres.Show == "away")
            {
                _Presence.Remove(user);
            }
            else if (user.Contains("/"))
            {
                _Presence[user] = pres.Type;
            }
        }

        /// <summary>
        /// Handles the ErrorComplete event of the ErrorModule control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="Bugx.Web.BugEventArgs"/> instance containing the event data.</param>
        void ErrorModule_ErrorComplete(object sender, BugEventArgs e)
        {
            HttpContext context = HttpContext.Current;
            Exception exception = context.Error.GetBaseException();
            Message(string.Format(CultureInfo.InvariantCulture,
                                  Texts.WarningErrorInProduction,
                                  context.Request.Url,
                                  exception.Message,
                                  exception.GetType().FullName,
                                  GetRelevantSource(context.Error) ?? exception.Source,
                                  e.BugUri));
        }

        /// <summary>
        /// Gets the relevent source.
        /// </summary>
        /// <param name="exception">The exception.</param>
        /// <returns></returns>
        static string  GetRelevantSource(Exception exception)
        {
            if (exception == null)
            {
                return null;
            }
            string result = GetRelevantSource(exception.InnerException);
            if (string.IsNullOrEmpty(result))
            {
                if (!exception.Source.StartsWith("mscorlib", StringComparison.InvariantCultureIgnoreCase) &&
                    !exception.Source.StartsWith("System", StringComparison.InvariantCultureIgnoreCase))
                {//If exception source is relevant then simply return it.
                    return exception.Source;
                }
                //Search relevant information.
                Match firstReleventLine = Regex.Match(exception.StackTrace, @"\sat (?!System)(?<Type>.+)\.[^(\s]+\(");
                if (!firstReleventLine.Success)
                {
                    return null;
                }
                Type type = FindType(firstReleventLine.Groups["Type"].Value);
                if (type == null)
                {
                    return null;
                }
                return type.Assembly.FullName.Split(',')[0];
            }
            return  result;
        }

        /// <summary>
        /// Finds the type.
        /// </summary>
        /// <param name="type">The type.</param>
        /// <returns></returns>
        static Type FindType(string type)
        {
            foreach (Assembly assembly in AppDomain.CurrentDomain.GetAssemblies())
            {
                Type result = assembly.GetType(type);
                if (result != null)
                {
                    return result;
                }
            }
            return null;
        }

        /// <summary>
        /// Bot_s the on authenticate.
        /// </summary>
        /// <param name="sender">The sender.</param>
        void Bot_OnAuthenticate(object sender)
        {
            if (!string.IsNullOrEmpty(SubscriptionManager.Instance.Announcement))
            {
                _Bot.Presence(PresenceType.available, SubscriptionManager.Instance.Announcement, null, 24);
            }
            else
            {
                _Bot.Presence(PresenceType.available, BotVersion, null, 24);
            }
            Message(Texts.InfoApplicationRestart);
        }

        void Bot_OnMessage(object sender, Message msg)
        {
            if (msg.Type != MessageType.chat || string.IsNullOrEmpty(msg.Body))
            {
                return;
            }
            string userAddress = string.Format(CultureInfo.InvariantCulture, "{0}@{1}", msg.From.User, msg.From.Server).ToLowerInvariant();
            if (!ProcessCommand(msg))
            {
                if (!SubscriptionManager.Instance.Contains(userAddress))
                {
                    _Bot.Message(msg.From.ToString(), Texts.ErrorChatWhenNotSubscribed);
                    return;
                }
                List<string> users = GetBroadcastList();
                if (users.Count == 1)
                {
                    _Bot.Message(msg.From.ToString(), Texts.ErrorNoChatAvailable);
                    return;
                }
                foreach (string user in users)
                {
                    if (string.Compare(user, userAddress, true) != 0)
                    {
                        _Bot.Message(user, string.Format(CultureInfo.InvariantCulture, "*{0}*: {1}", msg.From.User, msg.Body));
                    }
                }
            }
        }

        /// <summary>
        /// Gets the broadcast list.
        /// </summary>
        /// <returns></returns>
        List<string> GetBroadcastList()
        {
            List<string> result = new List<string>();
            foreach (string user in SubscriptionManager.Instance)
            {
                foreach (KeyValuePair<string, PresenceType> availableUser in _Presence)
                {
                    if (availableUser.Key.StartsWith(user, StringComparison.InvariantCultureIgnoreCase))
                    {
                        result.Add(user);
                        break;
                    }
                }
            }
            return result;
        }

        /// <summary>
        /// Processes the command.
        /// </summary>
        /// <param name="msg">The MSG.</param>
        /// <returns></returns>
        bool ProcessCommand(Message msg)
        {
            string userAddress = string.Format(CultureInfo.InvariantCulture, "{0}@{1}", msg.From.User, msg.From.Server).ToLowerInvariant();
            string command = msg.Body.ToLowerInvariant().Trim();
            if (!command.StartsWith("/"))
            {
                return false;
            }
            switch (command.Substring(1))
            {
                case "subscribe":
                    SubscriptionManager.Instance.Add(userAddress);
                    _Bot.Message(msg.From.ToString(), string.Format(CultureInfo.InvariantCulture, Texts.InfoSubscribeComplete, msg.From.User, msg.Body, BotVersion));
                    break;

                case "unsubscribe":
                    SubscriptionManager.Instance.Remove(userAddress);
                    _Bot.Message(msg.From.ToString(), string.Format(CultureInfo.InvariantCulture, Texts.InfoUnsubscribeComplete, msg.From.User, msg.Body, BotVersion));
                    break;

                case "recycle":
                    HostingEnvironment.InitiateShutdown();
                    break;

                case "subscribers":
                    string list;
                    if (SubscriptionManager.Instance.Count > 0)
                    {
                        StringBuilder listDetail = new StringBuilder();
                        foreach (string user in SubscriptionManager.Instance)
                        {
                            listDetail.AppendFormat("\r\n- {0}", user);
                        }
                        list = listDetail.ToString();
                    }
                    else
                    {
                        list = Texts.NoSubscribers;
                    }
                    _Bot.Message(msg.From.ToString(), string.Format(CultureInfo.InvariantCulture, Texts.InfoSubscribers, list));
                    break;

                case "help":
                case "?":
                    _Bot.Message(msg.From.ToString(), string.Format(CultureInfo.InvariantCulture, Texts.InfoHelpComplete, msg.From.User, msg.Body, BotVersion));
                    break;

                default:
                    if (command.StartsWith("/announcement"))
                    {
                        SubscriptionManager.Instance.Announcement = command.Substring(13).Trim();
                        _Bot.Presence(PresenceType.available, SubscriptionManager.Instance.Announcement, null, 24);
                        _Bot.Message(msg.From.ToString(), Texts.CommandComplete);
                    }
                    else
                    {
                        _Bot.Message(msg.From.ToString(), string.Format(CultureInfo.InvariantCulture, Texts.ErrorUnknownCommand, msg.From.User, msg.Body, BotVersion));
                    }
                    break;
            }
            return true;
        }

        /// <summary>
        /// Gets the bot version.
        /// </summary>
        /// <value>The bot version.</value>
        static string BotVersion
        {
            get{ return Regex.Match(typeof(Bot).Assembly.FullName, @"Version=([\d\.]+)", RegexOptions.IgnoreCase).Groups[1].Value; }
        }
    }
}
