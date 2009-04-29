// <copyright file="ConfigurationSettings.cs" company="Wavenet">
// Copyright © Wavenet 2009
// </copyright>
// <author>Olivier Bossaer</author>
// <email>olivier.bossaer@gmail.com</email>
// <date>2006-04-29</date>

namespace Bugx.Watermark
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Configuration;
    using System.Collections.Specialized;
    using System.Globalization;

    /// <summary>
    /// Handle all configuration for <see cref="EnvironmentWatermark"/> module.
    /// </summary>
    public static class ConfigurationSettings
    {
        /// <summary>
        /// Default text for watermark.
        /// </summary>
        public const string DefaultText = @"Pre-production Environment";

        /// <summary>
        /// Gets a value indicating whether: <c>True</c> if <see cref="EnvironmentWatermark"/> is <c>enable</c>; Otherwise <c>False</c>
        /// </summary>
        public static bool Enable
        {
            get { return Settings == null || Settings["Enable"] == null || Convert.ToBoolean(Settings["Enable"], CultureInfo.InvariantCulture); }
        }

        /// <summary>
        /// Gets the Watermark text.
        /// </summary>
        public static string Text
        {
            get { return Settings == null ? DefaultText : Settings["Text"] ?? DefaultText; }
        }

        /// <summary>
        /// Gets a <see cref="NameValueCollection"/> fill with all settings.
        /// </summary>
        private static NameValueCollection Settings
        {
            get { return (NameValueCollection)ConfigurationManager.GetSection("bugx/watermark"); }
        }
    }
}
