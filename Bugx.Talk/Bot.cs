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
using System.IO;
using System.Web;
using jabber.client;
using jabber.protocol.client;
using bedrock.net;
using System.Globalization;
using System.Text.RegularExpressions;
using Bugx.Web;

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

        /// <summary>
        /// Initializes a module and prepares it to handle requests.
        /// </summary>
        /// <param name="context">An <see cref="T:System.Web.HttpApplication"></see> that provides access to the methods, properties, and events common to all application objects within an ASP.NET application</param>
        public void Init(HttpApplication context)
        {
            if (_Bot != null)
            {
                _Bot.Dispose();
            }
            SubscriptionManager.LoadSettings();
            _Bot = new JabberClient();
            _Bot.OnMessage += new MessageHandler(Bot_OnMessage);
            _Bot.OnAuthenticate += new bedrock.ObjectHandler(Bot_OnAuthenticate);
            AsyncSocket.UntrustedRootOK = true;
            ErrorModule.ErrorComplete += new EventHandler<BugEventArgs>(ErrorModule_ErrorComplete);

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
        /// Handles the ErrorComplete event of the ErrorModule control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="Bugx.Web.BugEventArgs"/> instance containing the event data.</param>
        void ErrorModule_ErrorComplete(object sender, BugEventArgs e)
        {
            string message = string.Format(CultureInfo.InvariantCulture, Texts.WarningErrorInProduction, e.BugUri);
            foreach (string user in SubscriptionManager.Instance)
            {
                _Bot.Message(user, message);
            }
        }

        void Bot_OnAuthenticate(object sender)
        {
            string message = Texts.InfoApplicationRestart;
            foreach (string user in SubscriptionManager.Instance)
            {
                _Bot.Message(user, message);
            }
        }

        void Bot_OnMessage(object sender, Message msg)
        {
            if (msg.Type != MessageType.chat)
            {
                return;
            }
            string userAddress = string.Format(CultureInfo.InvariantCulture, "{0}@{1}", msg.From.User, msg.From.Server).ToLowerInvariant();
            switch(msg.Body.ToLowerInvariant())
            {
                case "subscribe":
                    SubscriptionManager.Instance.Add(userAddress);
                    _Bot.Message(userAddress, string.Format(CultureInfo.InvariantCulture, Texts.InfoSubscribeComplete, msg.From.User, msg.Body, BotVersion));
                    break;
                
                case "unsubscribe":
                    SubscriptionManager.Instance.Remove(userAddress);
                    _Bot.Message(userAddress, string.Format(CultureInfo.InvariantCulture, Texts.InfoUnsubscribeComplete, msg.From.User, msg.Body, BotVersion));
                    break;                

                case "help":
                case "?":
                    _Bot.Message(userAddress, string.Format(CultureInfo.InvariantCulture, Texts.InfoHelpComplete, msg.From.User, msg.Body, BotVersion));
                    break;

                default:
                    _Bot.Message(userAddress, string.Format(CultureInfo.InvariantCulture, Texts.ErrorUnknownCommand, msg.From.User, msg.Body, BotVersion));
                    break;
            }
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
