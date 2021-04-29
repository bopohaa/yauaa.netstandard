﻿//-----------------------------------------------------------------------
// <copyright file="UserAgent.cs" company="OrbintSoft">
//   Yet Another User Agent Analyzer for .NET Standard
//   porting realized by Stefano Balzarotti, Copyright 2018-2020 (C) OrbintSoft
//
//   Original Author and License:
//
//   Yet Another UserAgent Analyzer
//   Copyright(C) 2013-2020 Niels Basjes
//
//   Licensed under the Apache License, Version 2.0 (the "License");
//   you may not use this file except in compliance with the License.
//   You may obtain a copy of the License at
//
//   https://www.apache.org/licenses/LICENSE-2.0
//
//   Unless required by applicable law or agreed to in writing, software
//   distributed under the License is distributed on an "AS IS" BASIS,
//   WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
//   See the License for the specific language governing permissions and
//   limitations under the License.
// </copyright>
// <author>Stefano Balzarotti, Niels Basjes</author>
// <date>2018, 11, 24, 12:51</date>
//-----------------------------------------------------------------------

namespace OrbintSoft.Yauaa.Analyzer
{
    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.Linq;
    using System.Security;
    using System.Text;
    using Antlr4.Runtime;
    //using Newtonsoft.Json;
    using OrbintSoft.Yauaa.Analyze;
    using OrbintSoft.Yauaa.Antlr4Source;
    using OrbintSoft.Yauaa.Logger;

    /// <summary>
    /// Defines the <see cref="UserAgent" /> class.
    /// This class contains all info about a parsed user agent and all utility to work with it.
    /// </summary>
    [Serializable]
    public class UserAgent : UserAgentBaseListener, IAntlrErrorListener<int>, IAntlrErrorListener<IToken>, IEquatable<UserAgent>, IUserAgent
    {
        /// <summary>
        /// Defines the AGENT_BUILD.
        /// </summary>
        [Obsolete("Use DefaultUserAgentFields.AGENT_BUILD")]
        public const string AGENT_BUILD = DefaultUserAgentFields.AGENT_BUILD;

        /// <summary>
        /// Defines the AGENT_CLASS.
        /// </summary>
        [Obsolete("Use DefaultUserAgentFields.AGENT_CLASS")]
        public const string AGENT_CLASS = DefaultUserAgentFields.AGENT_CLASS;

        /// <summary>
        /// Defines the AGENT_INFORMATION_EMAIL.
        /// </summary>
        [Obsolete("Use DefaultUserAgentFields.AGENT_INFORMATION_EMAIL")]
        public const string AGENT_INFORMATION_EMAIL = DefaultUserAgentFields.AGENT_INFORMATION_EMAIL;

        /// <summary>
        /// Defines the AGENT_INFORMATION_URL.
        /// </summary>
        [Obsolete("Use DefaultUserAgentFields.AGENT_INFORMATION_URL")]
        public const string AGENT_INFORMATION_URL = DefaultUserAgentFields.AGENT_INFORMATION_URL;

        /// <summary>
        /// Defines the AGENT_LANGUAGE.
        /// </summary>
        [Obsolete("Use DefaultUserAgentFields.AGENT_LANGUAGE")]
        public const string AGENT_LANGUAGE = DefaultUserAgentFields.AGENT_LANGUAGE;

        /// <summary>
        /// Defines the AGENT_LANGUAGE_CODE.
        /// </summary>
        [Obsolete("Use DefaultUserAgentFields.AGENT_LANGUAGE_CODE")]
        public const string AGENT_LANGUAGE_CODE = DefaultUserAgentFields.AGENT_LANGUAGE_CODE;

        /// <summary>
        /// Defines the AGENT_NAME.
        /// </summary>
        [Obsolete("Use DefaultUserAgentFields.AGENT_NAME")]
        public const string AGENT_NAME = DefaultUserAgentFields.AGENT_NAME;

        /// <summary>
        /// Defines the AGENT_NAME_VERSION.
        /// </summary>
        [Obsolete("Use DefaultUserAgentFields.AGENT_NAME_VERSION")]
        public const string AGENT_NAME_VERSION = DefaultUserAgentFields.AGENT_NAME_VERSION;

        /// <summary>
        /// Defines the AGENT_NAME_VERSION_MAJOR.
        /// </summary>
        [Obsolete("Use DefaultUserAgentFields.AGENT_NAME_VERSION_MAJOR")]
        public const string AGENT_NAME_VERSION_MAJOR = DefaultUserAgentFields.AGENT_NAME_VERSION_MAJOR;

        /// <summary>
        /// Defines the AGENT_SECURITY.
        /// </summary>
        [Obsolete("Use DefaultUserAgentFields.AGENT_SECURITY")]
        public const string AGENT_SECURITY = DefaultUserAgentFields.AGENT_SECURITY;

        /// <summary>
        /// Defines the AGENT_UUID.
        /// </summary>
        [Obsolete("Use DefaultUserAgentFields.AGENT_UUID")]
        public const string AGENT_UUID = DefaultUserAgentFields.AGENT_UUID;

        /// <summary>
        /// Defines the AGENT_VERSION.
        /// </summary>
        [Obsolete("Use DefaultUserAgentFields.AGENT_VERSION")]
        public const string AGENT_VERSION = DefaultUserAgentFields.AGENT_VERSION;

        /// <summary>
        /// Defines the AGENT_VERSION_MAJOR.
        /// </summary>
        [Obsolete("Use DefaultUserAgentFields.AGENT_VERSION_MAJOR")]
        public const string AGENT_VERSION_MAJOR = DefaultUserAgentFields.AGENT_VERSION_MAJOR;

        /// <summary>
        /// Defines the ANONYMIZED.
        /// </summary>
        [Obsolete("Use DefaultUserAgentFields.ANONYMIZED")]
        public const string ANONYMIZED = DefaultUserAgentFields.ANONYMIZED;

        /// <summary>
        /// Defines the DEVICE_BRAND.
        /// </summary>
        [Obsolete("Use DefaultUserAgentFields.DEVICE_BRAND")]
        public const string DEVICE_BRAND = DefaultUserAgentFields.DEVICE_BRAND;

        /// <summary>
        /// Defines the DEVICE_CLASS.
        /// </summary>
        [Obsolete("Use DefaultUserAgentFields.DEVICE_CLASS")]
        public const string DEVICE_CLASS = DefaultUserAgentFields.DEVICE_CLASS;

        /// <summary>
        /// Defines the DEVICE_CPU.
        /// </summary>
        [Obsolete("Use DefaultUserAgentFields.DEVICE_CPU")]
        public const string DEVICE_CPU = DefaultUserAgentFields.DEVICE_CPU;

        /// <summary>
        /// Defines the DEVICE_CPU_BITS.
        /// </summary>
        [Obsolete("Use DefaultUserAgentFields.DEVICE_CPU_BITS")]
        public const string DEVICE_CPU_BITS = DefaultUserAgentFields.DEVICE_CPU_BITS;

        /// <summary>
        /// Defines the DEVICE_FIRMWARE_VERSION.
        /// </summary>
        [Obsolete("Use DefaultUserAgentFields.DEVICE_FIRMWARE_VERSION")]
        public const string DEVICE_FIRMWARE_VERSION = DefaultUserAgentFields.DEVICE_FIRMWARE_VERSION;

        /// <summary>
        /// Defines the DEVICE_NAME.
        /// </summary>
        [Obsolete("Use DefaultUserAgentFields.DEVICE_NAME")]
        public const string DEVICE_NAME = DefaultUserAgentFields.DEVICE_NAME;

        /// <summary>
        /// Defines the DEVICE_VERSION.
        /// </summary>
        [Obsolete("Use DefaultUserAgentFields.DEVICE_VERSION")]
        public const string DEVICE_VERSION = DefaultUserAgentFields.DEVICE_VERSION;

        /// <summary>
        /// Defines the FACEBOOK_CARRIER.
        /// </summary>
        [Obsolete("Use DefaultUserAgentFields.FACEBOOK_CARRIER")]
        public const string FACEBOOK_CARRIER = DefaultUserAgentFields.FACEBOOK_CARRIER;

        /// <summary>
        /// Defines the FACEBOOK_DEVICE_CLASS.
        /// </summary>
        [Obsolete("Use DefaultUserAgentFields.FACEBOOK_DEVICE_CLASS")]
        public const string FACEBOOK_DEVICE_CLASS = DefaultUserAgentFields.FACEBOOK_DEVICE_CLASS;

        /// <summary>
        /// Defines the FACEBOOK_DEVICE_NAME.
        /// </summary>
        [Obsolete("Use DefaultUserAgentFields.FACEBOOK_DEVICE_NAME")]
        public const string FACEBOOK_DEVICE_NAME = DefaultUserAgentFields.FACEBOOK_DEVICE_NAME;

        /// <summary>
        /// Defines the FACEBOOK_DEVICE_VERSION.
        /// </summary>
        [Obsolete("Use DefaultUserAgentFields.FACEBOOK_DEVICE_VERSION")]
        public const string FACEBOOK_DEVICE_VERSION = DefaultUserAgentFields.FACEBOOK_DEVICE_VERSION;

        /// <summary>
        /// Defines the FACEBOOK_F_B_O_P.
        /// </summary>
        [Obsolete("Use DefaultUserAgentFields.FACEBOOK_F_B_O_P")]
        public const string FACEBOOK_F_B_O_P = DefaultUserAgentFields.FACEBOOK_F_B_O_P;

        /// <summary>
        /// Defines the FACEBOOK_F_B_S_S.
        /// </summary>
        [Obsolete("Use DefaultUserAgentFields.FACEBOOK_F_B_S_S")]
        public const string FACEBOOK_F_B_S_S = DefaultUserAgentFields.FACEBOOK_F_B_S_S;

        /// <summary>
        /// Defines the FACEBOOK_OPERATING_SYSTEM_NAME.
        /// </summary>
        [Obsolete("Use DefaultUserAgentFields.FACEBOOK_OPERATING_SYSTEM_NAME")]
        public const string FACEBOOK_OPERATING_SYSTEM_NAME = DefaultUserAgentFields.FACEBOOK_OPERATING_SYSTEM_NAME;

        /// <summary>
        /// Defines the FACEBOOK_OPERATING_SYSTEM_VERSION.
        /// </summary>
        [Obsolete("Use DefaultUserAgentFields.FACEBOOK_OPERATING_SYSTEM_VERSION")]
        public const string FACEBOOK_OPERATING_SYSTEM_VERSION = DefaultUserAgentFields.FACEBOOK_OPERATING_SYSTEM_VERSION;

        /// <summary>
        /// Defines the HACKER_ATTACK_VECTOR.
        /// </summary>
        [Obsolete("Use DefaultUserAgentFields.HACKER_ATTACK_VECTOR")]
        public const string HACKER_ATTACK_VECTOR = DefaultUserAgentFields.HACKER_ATTACK_VECTOR;

        /// <summary>
        /// Defines the HACKER_TOOLKIT.
        /// </summary>
        [Obsolete("Use DefaultUserAgentFields.HACKER_TOOLKIT")]
        public const string HACKER_TOOLKIT = DefaultUserAgentFields.HACKER_TOOLKIT;

        /// <summary>
        /// Defines the IE_COMPATIBILITY_NAME_VERSION.
        /// </summary>
        [Obsolete("Use DefaultUserAgentFields.IE_COMPATIBILITY_NAME_VERSION")]
        public const string IE_COMPATIBILITY_NAME_VERSION = DefaultUserAgentFields.IE_COMPATIBILITY_NAME_VERSION;

        /// <summary>
        /// Defines the IE_COMPATIBILITY_NAME_VERSION_MAJOR.
        /// </summary>
        [Obsolete("Use DefaultUserAgentFields.IE_COMPATIBILITY_NAME_VERSION_MAJOR")]
        public const string IE_COMPATIBILITY_NAME_VERSION_MAJOR = DefaultUserAgentFields.IE_COMPATIBILITY_NAME_VERSION_MAJOR;

        /// <summary>
        /// Defines the IE_COMPATIBILITY_VERSION.
        /// </summary>
        [Obsolete("Use DefaultUserAgentFields.IE_COMPATIBILITY_VERSION")]
        public const string IE_COMPATIBILITY_VERSION = DefaultUserAgentFields.IE_COMPATIBILITY_VERSION;

        /// <summary>
        /// Defines the IE_COMPATIBILITY_VERSION_MAJOR.
        /// </summary>
        [Obsolete("Use DefaultUserAgentFields.IE_COMPATIBILITY_VERSION_MAJOR")]
        public const string IE_COMPATIBILITY_VERSION_MAJOR = DefaultUserAgentFields.IE_COMPATIBILITY_VERSION_MAJOR;

        /// <summary>
        /// Defines the KOBO_AFFILIATE.
        /// </summary>
        [Obsolete("Use DefaultUserAgentFields.KOBO_AFFILIATE")]
        public const string KOBO_AFFILIATE = DefaultUserAgentFields.KOBO_AFFILIATE;

        /// <summary>
        /// Defines the KOBO_PLATFORM_ID.
        /// </summary>
        [Obsolete("Use DefaultUserAgentFields.KOBO_PLATFORM_ID")]
        public const string KOBO_PLATFORM_ID = DefaultUserAgentFields.KOBO_PLATFORM_ID;

        /// <summary>
        /// Defines the LAYOUT_ENGINE_BUILD.
        /// </summary>
        [Obsolete("Use DefaultUserAgentFields.LAYOUT_ENGINE_BUILD")]
        public const string LAYOUT_ENGINE_BUILD = DefaultUserAgentFields.LAYOUT_ENGINE_BUILD;

        /// <summary>
        /// Defines the LAYOUT_ENGINE_CLASS.
        /// </summary>
        [Obsolete("Use DefaultUserAgentFields.LAYOUT_ENGINE_CLASS")]
        public const string LAYOUT_ENGINE_CLASS = DefaultUserAgentFields.LAYOUT_ENGINE_CLASS;

        /// <summary>
        /// Defines the LAYOUT_ENGINE_NAME.
        /// </summary>
        [Obsolete("Use DefaultUserAgentFields.LAYOUT_ENGINE_NAME")]
        public const string LAYOUT_ENGINE_NAME = DefaultUserAgentFields.LAYOUT_ENGINE_NAME;

        /// <summary>
        /// Defines the LAYOUT_ENGINE_NAME_VERSION.
        /// </summary>
        [Obsolete("Use DefaultUserAgentFields.LAYOUT_ENGINE_NAME_VERSION")]
        public const string LAYOUT_ENGINE_NAME_VERSION = DefaultUserAgentFields.LAYOUT_ENGINE_NAME_VERSION;

        /// <summary>
        /// Defines the LAYOUT_ENGINE_NAME_VERSION_MAJOR.
        /// </summary>
        [Obsolete("Use DefaultUserAgentFields.LAYOUT_ENGINE_NAME_VERSION_MAJOR")]
        public const string LAYOUT_ENGINE_NAME_VERSION_MAJOR = DefaultUserAgentFields.LAYOUT_ENGINE_NAME_VERSION_MAJOR;

        /// <summary>
        /// Defines the LAYOUT_ENGINE_VERSION.
        /// </summary>
        [Obsolete("Use DefaultUserAgentFields.LAYOUT_ENGINE_VERSION")]
        public const string LAYOUT_ENGINE_VERSION = DefaultUserAgentFields.LAYOUT_ENGINE_VERSION;

        /// <summary>
        /// Defines the LAYOUT_ENGINE_VERSION_MAJOR.
        /// </summary>
        [Obsolete("Use DefaultUserAgentFields.LAYOUT_ENGINE_VERSION_MAJOR")]
        public const string LAYOUT_ENGINE_VERSION_MAJOR = DefaultUserAgentFields.LAYOUT_ENGINE_VERSION_MAJOR;

        /// <summary>
        /// Defines the NULL_VALUE.
        /// </summary>
        [Obsolete("Use DefaultUserAgentFields.NULL_VALUE")]
        public const string NULL_VALUE = DefaultUserAgentFields.NULL_VALUE;

        /// <summary>
        /// Defines the OPERATING_SYSTEM_CLASS.
        /// </summary>
        [Obsolete("Use DefaultUserAgentFields.OPERATING_SYSTEM_CLASS")]
        public const string OPERATING_SYSTEM_CLASS = DefaultUserAgentFields.OPERATING_SYSTEM_CLASS;

        /// <summary>
        /// Defines the OPERATING_SYSTEM_NAME.
        /// </summary>
        [Obsolete("Use DefaultUserAgentFields.OPERATING_SYSTEM_NAME")]
        public const string OPERATING_SYSTEM_NAME = DefaultUserAgentFields.OPERATING_SYSTEM_NAME;

        /// <summary>
        /// Defines the OPERATING_SYSTEM_NAME_VERSION.
        /// </summary>
        [Obsolete("Use DefaultUserAgentFields.OPERATING_SYSTEM_NAME_VERSION")]
        public const string OPERATING_SYSTEM_NAME_VERSION = DefaultUserAgentFields.OPERATING_SYSTEM_NAME_VERSION;

        /// <summary>
        /// Defines the OPERATING_SYSTEM_NAME_VERSION_MAJOR.
        /// </summary>
        [Obsolete("Use DefaultUserAgentFields.OPERATING_SYSTEM_NAME_VERSION_MAJOR")]
        public const string OPERATING_SYSTEM_NAME_VERSION_MAJOR = DefaultUserAgentFields.OPERATING_SYSTEM_NAME_VERSION_MAJOR;

        /// <summary>
        /// Defines the OPERATING_SYSTEM_VERSION.
        /// </summary>
        [Obsolete("Use DefaultUserAgentFields.OPERATING_SYSTEM_VERSION")]
        public const string OPERATING_SYSTEM_VERSION = DefaultUserAgentFields.OPERATING_SYSTEM_VERSION;

        /// <summary>
        /// Defines the OPERATING_SYSTEM_VERSION_BUILD.
        /// </summary>
        [Obsolete("Use DefaultUserAgentFields.OPERATING_SYSTEM_VERSION_BUILD")]
        public const string OPERATING_SYSTEM_VERSION_BUILD = DefaultUserAgentFields.OPERATING_SYSTEM_VERSION_BUILD;

        /// <summary>
        /// Defines the OPERATING_SYSTEM_VERSION_MAJOR.
        /// </summary>
        [Obsolete("Use DefaultUserAgentFields.OPERATING_SYSTEM_VERSION_MAJOR")]
        public const string OPERATING_SYSTEM_VERSION_MAJOR = DefaultUserAgentFields.OPERATING_SYSTEM_VERSION_MAJOR;

        /// <summary>
        /// Defines the SET_ALL_FIELDS.
        /// </summary>
        [Obsolete("Use DefaultUserAgentFields.SET_ALL_FIELDS")]
        public const string SET_ALL_FIELDS = DefaultUserAgentFields.SET_ALL_FIELDS;

        /// <summary>
        /// Defines the SYNTAX_ERROR.
        /// </summary>
        [Obsolete("Use DefaultUserAgentFields.SYNTAX_ERROR")]
        public const string SYNTAX_ERROR = DefaultUserAgentFields.SYNTAX_ERROR;

        /// <summary>
        /// Defines the UNKNOWN_VALUE.
        /// </summary>
        [Obsolete("Use DefaultUserAgentFields.UNKNOWN_VALUE")]
        public const string UNKNOWN_VALUE = DefaultUserAgentFields.UNKNOWN_VALUE;

        /// <summary>
        /// Defines the UNKNOWN_VERSION.
        /// </summary>
        [Obsolete("Use DefaultUserAgentFields.UNKNOWN_VERSION")]
        public const string UNKNOWN_VERSION = DefaultUserAgentFields.UNKNOWN_VERSION;

        /// <summary>
        /// Defines the UNKNOWN_NAME_VERSION.
        /// </summary>
        [Obsolete("Use DefaultUserAgentFields.UNKNOWN_NAME_VERSION")]
        public const string UNKNOWN_NAME_VERSION = DefaultUserAgentFields.UNKNOWN_NAME_VERSION;

        /// <summary>
        /// Defines the USERAGENT_FIELDNAME.
        /// </summary>
        [Obsolete("Use DefaultUserAgentFields.USERAGENT_FIELDNAME")]
        public const string USERAGENT_FIELDNAME = DefaultUserAgentFields.USERAGENT_FIELDNAME;

        /// <summary>
        /// Defines the WEBVIEW_APP_NAME.
        /// </summary>
        [Obsolete("Use DefaultUserAgentFields.WEBVIEW_APP_NAME")]
        public const string WEBVIEW_APP_NAME = DefaultUserAgentFields.WEBVIEW_APP_NAME;

        /// <summary>
        /// Defines the WEBVIEW_APP_NAME_VERSION_MAJOR.
        /// </summary>
        [Obsolete("Use DefaultUserAgentFields.WEBVIEW_APP_NAME_VERSION_MAJOR")]
        public const string WEBVIEW_APP_NAME_VERSION_MAJOR = DefaultUserAgentFields.WEBVIEW_APP_NAME_VERSION_MAJOR;

        /// <summary>
        /// Defines the WEBVIEW_APP_VERSION.
        /// </summary>
        [Obsolete("Use DefaultUserAgentFields.WEBVIEW_APP_VERSION")]
        public const string WEBVIEW_APP_VERSION = DefaultUserAgentFields.WEBVIEW_APP_VERSION;

        /// <summary>
        /// Defines the WEBVIEW_APP_VERSION_MAJOR.
        /// </summary>
        [Obsolete("Use DefaultUserAgentFields.WEBVIEW_APP_VERSION_MAJOR")]
        public const string WEBVIEW_APP_VERSION_MAJOR = DefaultUserAgentFields.WEBVIEW_APP_VERSION_MAJOR;

        /// <summary>
        /// Standard fields used during parsing.
        /// </summary>
        public static readonly string[] StandardFields =
        {
            DefaultUserAgentFields.DEVICE_CLASS,
            DefaultUserAgentFields.DEVICE_BRAND,
            DefaultUserAgentFields.DEVICE_NAME,
            DefaultUserAgentFields.OPERATING_SYSTEM_CLASS,
            DefaultUserAgentFields.OPERATING_SYSTEM_NAME,
            DefaultUserAgentFields.OPERATING_SYSTEM_VERSION,
            DefaultUserAgentFields.OPERATING_SYSTEM_VERSION_MAJOR,
            DefaultUserAgentFields.OPERATING_SYSTEM_NAME_VERSION,
            DefaultUserAgentFields.OPERATING_SYSTEM_NAME_VERSION_MAJOR,
            DefaultUserAgentFields.LAYOUT_ENGINE_CLASS,
            DefaultUserAgentFields.LAYOUT_ENGINE_NAME,
            DefaultUserAgentFields.LAYOUT_ENGINE_VERSION,
            DefaultUserAgentFields.LAYOUT_ENGINE_VERSION_MAJOR,
            DefaultUserAgentFields.LAYOUT_ENGINE_NAME_VERSION,
            DefaultUserAgentFields.LAYOUT_ENGINE_NAME_VERSION_MAJOR,
            DefaultUserAgentFields.AGENT_CLASS,
            DefaultUserAgentFields.AGENT_NAME,
            DefaultUserAgentFields.AGENT_VERSION,
            DefaultUserAgentFields.AGENT_VERSION_MAJOR,
            DefaultUserAgentFields.AGENT_NAME_VERSION,
            DefaultUserAgentFields.AGENT_NAME_VERSION_MAJOR,
        };

        /// <summary>
        /// We manually sort the list of fields to ensure the output is consistent.
        /// Any unspecified fieldnames will be appended to the end.
        /// </summary>
        protected internal static readonly IList<string> PreSortedFieldList = new List<string>(32);

        /// <summary>
        /// Default fields dictionary.
        /// </summary>
        private static readonly IDictionary<string, IAgentField> DefaultsForKnownFields = new SortedDictionary<string, IAgentField>();

        /// <summary>
        /// Defines the logger.
        /// </summary>
        private static readonly ILogger Logger = new Logger<UserAgent>();

        /// <summary>
        /// All fields extracted by the user agent.
        /// </summary>
        private readonly IDictionary<string, AgentField> allFields = new Dictionary<string, AgentField>();

        /// <summary>
        /// The full user agent string.
        /// </summary>
        private string userAgentString = null;

        /// <summary>
        /// The field names to be extracted.
        /// </summary>
        private HashSet<string> wantedFieldNames = null;

        /// <summary>
        /// Initializes static members of the <see cref="UserAgent"/> class.
        /// </summary>
        static UserAgent()
        {
            // Device : Family - Brand - Model
            DefaultsForKnownFields[DefaultUserAgentFields.DEVICE_CLASS] = new AgentField(DefaultUserAgentFields.UNKNOWN_VALUE);
            DefaultsForKnownFields[DefaultUserAgentFields.DEVICE_BRAND] = new AgentField(DefaultUserAgentFields.UNKNOWN_VALUE);
            DefaultsForKnownFields[DefaultUserAgentFields.DEVICE_NAME] = new AgentField(DefaultUserAgentFields.UNKNOWN_VALUE);

            // Operating system
            DefaultsForKnownFields[DefaultUserAgentFields.OPERATING_SYSTEM_CLASS] = new AgentField(DefaultUserAgentFields.UNKNOWN_VALUE);
            DefaultsForKnownFields[DefaultUserAgentFields.OPERATING_SYSTEM_NAME] = new AgentField(DefaultUserAgentFields.UNKNOWN_VALUE);
            DefaultsForKnownFields[DefaultUserAgentFields.OPERATING_SYSTEM_VERSION] = new AgentField(DefaultUserAgentFields.UNKNOWN_VERSION);
            DefaultsForKnownFields[DefaultUserAgentFields.OPERATING_SYSTEM_VERSION_MAJOR] = new AgentField(DefaultUserAgentFields.UNKNOWN_VERSION);
            DefaultsForKnownFields[DefaultUserAgentFields.OPERATING_SYSTEM_NAME_VERSION] = new AgentField(DefaultUserAgentFields.UNKNOWN_NAME_VERSION);
            DefaultsForKnownFields[DefaultUserAgentFields.OPERATING_SYSTEM_NAME_VERSION_MAJOR] = new AgentField(DefaultUserAgentFields.UNKNOWN_NAME_VERSION);

            // Engine : Class (=None/Hacker/Robot/Browser) - Name - Version
            DefaultsForKnownFields[DefaultUserAgentFields.LAYOUT_ENGINE_CLASS] = new AgentField(DefaultUserAgentFields.UNKNOWN_VALUE);
            DefaultsForKnownFields[DefaultUserAgentFields.LAYOUT_ENGINE_NAME] = new AgentField(DefaultUserAgentFields.UNKNOWN_VALUE);
            DefaultsForKnownFields[DefaultUserAgentFields.LAYOUT_ENGINE_VERSION] = new AgentField(DefaultUserAgentFields.UNKNOWN_VERSION);
            DefaultsForKnownFields[DefaultUserAgentFields.LAYOUT_ENGINE_VERSION_MAJOR] = new AgentField(DefaultUserAgentFields.UNKNOWN_VERSION);
            DefaultsForKnownFields[DefaultUserAgentFields.LAYOUT_ENGINE_NAME_VERSION] = new AgentField(DefaultUserAgentFields.UNKNOWN_NAME_VERSION);
            DefaultsForKnownFields[DefaultUserAgentFields.LAYOUT_ENGINE_NAME_VERSION_MAJOR] = new AgentField(DefaultUserAgentFields.UNKNOWN_NAME_VERSION);

            // Agent: Class (=Hacker/Robot/Browser) - Name - Version
            DefaultsForKnownFields[DefaultUserAgentFields.AGENT_CLASS] = new AgentField(DefaultUserAgentFields.UNKNOWN_VALUE);
            DefaultsForKnownFields[DefaultUserAgentFields.AGENT_NAME] = new AgentField(DefaultUserAgentFields.UNKNOWN_VALUE);
            DefaultsForKnownFields[DefaultUserAgentFields.AGENT_VERSION] = new AgentField(DefaultUserAgentFields.UNKNOWN_VERSION);
            DefaultsForKnownFields[DefaultUserAgentFields.AGENT_VERSION_MAJOR] = new AgentField(DefaultUserAgentFields.UNKNOWN_VERSION);
            DefaultsForKnownFields[DefaultUserAgentFields.AGENT_NAME_VERSION] = new AgentField(DefaultUserAgentFields.UNKNOWN_NAME_VERSION);
            DefaultsForKnownFields[DefaultUserAgentFields.AGENT_NAME_VERSION_MAJOR] = new AgentField(DefaultUserAgentFields.UNKNOWN_NAME_VERSION);

            PreSortedFieldList.Add(DefaultUserAgentFields.DEVICE_CLASS);
            PreSortedFieldList.Add(DefaultUserAgentFields.DEVICE_NAME);
            PreSortedFieldList.Add(DefaultUserAgentFields.DEVICE_BRAND);
            PreSortedFieldList.Add(DefaultUserAgentFields.DEVICE_CPU);
            PreSortedFieldList.Add(DefaultUserAgentFields.DEVICE_CPU_BITS);
            PreSortedFieldList.Add(DefaultUserAgentFields.DEVICE_FIRMWARE_VERSION);
            PreSortedFieldList.Add(DefaultUserAgentFields.DEVICE_VERSION);

            PreSortedFieldList.Add(DefaultUserAgentFields.OPERATING_SYSTEM_CLASS);
            PreSortedFieldList.Add(DefaultUserAgentFields.OPERATING_SYSTEM_NAME);
            PreSortedFieldList.Add(DefaultUserAgentFields.OPERATING_SYSTEM_VERSION);
            PreSortedFieldList.Add(DefaultUserAgentFields.OPERATING_SYSTEM_VERSION_MAJOR);
            PreSortedFieldList.Add(DefaultUserAgentFields.OPERATING_SYSTEM_NAME_VERSION);
            PreSortedFieldList.Add(DefaultUserAgentFields.OPERATING_SYSTEM_NAME_VERSION_MAJOR);
            PreSortedFieldList.Add(DefaultUserAgentFields.OPERATING_SYSTEM_VERSION_BUILD);

            PreSortedFieldList.Add(DefaultUserAgentFields.LAYOUT_ENGINE_CLASS);
            PreSortedFieldList.Add(DefaultUserAgentFields.LAYOUT_ENGINE_NAME);
            PreSortedFieldList.Add(DefaultUserAgentFields.LAYOUT_ENGINE_VERSION);
            PreSortedFieldList.Add(DefaultUserAgentFields.LAYOUT_ENGINE_VERSION_MAJOR);
            PreSortedFieldList.Add(DefaultUserAgentFields.LAYOUT_ENGINE_NAME_VERSION);
            PreSortedFieldList.Add(DefaultUserAgentFields.LAYOUT_ENGINE_NAME_VERSION_MAJOR);
            PreSortedFieldList.Add(DefaultUserAgentFields.LAYOUT_ENGINE_BUILD);

            PreSortedFieldList.Add(DefaultUserAgentFields.AGENT_CLASS);
            PreSortedFieldList.Add(DefaultUserAgentFields.AGENT_NAME);
            PreSortedFieldList.Add(DefaultUserAgentFields.AGENT_VERSION);
            PreSortedFieldList.Add(DefaultUserAgentFields.AGENT_VERSION_MAJOR);
            PreSortedFieldList.Add(DefaultUserAgentFields.AGENT_NAME_VERSION);
            PreSortedFieldList.Add(DefaultUserAgentFields.AGENT_NAME_VERSION_MAJOR);
            PreSortedFieldList.Add(DefaultUserAgentFields.AGENT_BUILD);
            PreSortedFieldList.Add(DefaultUserAgentFields.AGENT_LANGUAGE);
            PreSortedFieldList.Add(DefaultUserAgentFields.AGENT_LANGUAGE_CODE);
            PreSortedFieldList.Add(DefaultUserAgentFields.AGENT_INFORMATION_EMAIL);
            PreSortedFieldList.Add(DefaultUserAgentFields.AGENT_INFORMATION_URL);
            PreSortedFieldList.Add(DefaultUserAgentFields.AGENT_SECURITY);
            PreSortedFieldList.Add(DefaultUserAgentFields.AGENT_UUID);

            PreSortedFieldList.Add(DefaultUserAgentFields.WEBVIEW_APP_NAME);
            PreSortedFieldList.Add(DefaultUserAgentFields.WEBVIEW_APP_VERSION);
            PreSortedFieldList.Add(DefaultUserAgentFields.WEBVIEW_APP_VERSION_MAJOR);
            PreSortedFieldList.Add(DefaultUserAgentFields.WEBVIEW_APP_NAME_VERSION_MAJOR);

            PreSortedFieldList.Add(DefaultUserAgentFields.FACEBOOK_CARRIER);
            PreSortedFieldList.Add(DefaultUserAgentFields.FACEBOOK_DEVICE_CLASS);
            PreSortedFieldList.Add(DefaultUserAgentFields.FACEBOOK_DEVICE_NAME);
            PreSortedFieldList.Add(DefaultUserAgentFields.FACEBOOK_DEVICE_VERSION);
            PreSortedFieldList.Add(DefaultUserAgentFields.FACEBOOK_F_B_O_P);
            PreSortedFieldList.Add(DefaultUserAgentFields.FACEBOOK_F_B_S_S);
            PreSortedFieldList.Add(DefaultUserAgentFields.FACEBOOK_OPERATING_SYSTEM_NAME);
            PreSortedFieldList.Add(DefaultUserAgentFields.FACEBOOK_OPERATING_SYSTEM_VERSION);

            PreSortedFieldList.Add(DefaultUserAgentFields.ANONYMIZED);

            PreSortedFieldList.Add(DefaultUserAgentFields.HACKER_ATTACK_VECTOR);
            PreSortedFieldList.Add(DefaultUserAgentFields.HACKER_TOOLKIT);

            PreSortedFieldList.Add(DefaultUserAgentFields.KOBO_AFFILIATE);
            PreSortedFieldList.Add(DefaultUserAgentFields.KOBO_PLATFORM_ID);

            PreSortedFieldList.Add(DefaultUserAgentFields.IE_COMPATIBILITY_VERSION);
            PreSortedFieldList.Add(DefaultUserAgentFields.IE_COMPATIBILITY_VERSION_MAJOR);
            PreSortedFieldList.Add(DefaultUserAgentFields.IE_COMPATIBILITY_NAME_VERSION);
            PreSortedFieldList.Add(DefaultUserAgentFields.IE_COMPATIBILITY_NAME_VERSION_MAJOR);

            PreSortedFieldList.Add(DefaultUserAgentFields.SYNTAX_ERROR);
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="UserAgent"/> class.
        /// </summary>
        public UserAgent()
        {
            this.Init();
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="UserAgent"/> class.
        /// </summary>
        /// <param name="wantedFieldNames">The wanted field names.</param>
        public UserAgent(ICollection<string> wantedFieldNames)
        {
            this.SetWantedFieldNames(wantedFieldNames);
            this.Init();
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="UserAgent"/> class.
        /// </summary>
        /// <param name="userAgentString">The user agent strung to be parsed.</param>
        public UserAgent(string userAgentString)
        {
            // wantedFieldNames == null; --> Assume we want all fields.
            this.Init();
            this.UserAgentString = userAgentString;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="UserAgent"/> class.
        /// </summary>
        /// <param name="userAgentString">The user agent strung to be parsed.</param>
        /// <param name="wantedFieldNames">The wanted field names.</param>
        public UserAgent(string userAgentString, ICollection<string> wantedFieldNames)
        {
            this.SetWantedFieldNames(wantedFieldNames);
            this.Init();
            this.UserAgentString = userAgentString;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="UserAgent"/> class.
        /// </summary>
        /// <param name="userAgent">The userAgent<see cref="UserAgent"/>.</param>
        public UserAgent(UserAgent userAgent)
        {
            this.Clone(userAgent);
        }

        /// <summary>
        /// Gets the numer of ambiguities found.
        /// </summary>
        public int AmbiguityCount { get; private set; }

        /// <summary>
        /// Gets a value indicating whether some fields are ambiguos.
        /// </summary>
        public bool HasAmbiguity { get; private set; }

        /// <summary>
        /// Gets a value indicating whether the user agent contains syntax errors.
        /// </summary>
        public bool HasSyntaxError { get; private set; }

#if !VERBOSE
        /// <summary>
        /// Gets or sets a value indicating whether debug logging is enabled.
        /// </summary>
        public bool IsDebug { get; set; } = false;
#else
        public bool IsDebug { get; set; } = true;
#endif

        /// <inheritdoc/>
        public string UserAgentString
        {
            get
            {
                return this.userAgentString;
            }

            set
            {
                this.userAgentString = value;
                this.Reset();
            }
        }

        /// <summary>
        /// Used to know if the requested field is only for internal use (It is used by the parser, but doesn't contain meaningful for the user).
        /// </summary>
        /// <param name="fieldname">The name of the field.</param>
        /// <returns>True if it's for internal use.</returns>
        public static bool IsSystemField(string fieldname)
        {
            return DefaultUserAgentFields.SET_ALL_FIELDS.Equals(fieldname) ||
                   DefaultUserAgentFields.SYNTAX_ERROR.Equals(fieldname) ||
                   DefaultUserAgentFields.USERAGENT_FIELDNAME.Equals(fieldname);
        }

        /// <summary>
        /// Used to clone the <see cref="UserAgent"/> object.
        /// </summary>
        /// <param name="userAgent">The object to be cloned.</param>
        public void Clone(UserAgent userAgent)
        {
            this.wantedFieldNames = userAgent.wantedFieldNames;
            this.Init();
            this.IsDebug = userAgent.IsDebug;
            this.UserAgentString = userAgent.userAgentString;

            foreach (var entry in userAgent.allFields)
            {
                this.Set(entry.Key, entry.Value.GetValue(), entry.Value.Confidence);
            }

            this.HasSyntaxError = userAgent.HasSyntaxError;
            this.HasAmbiguity = userAgent.HasAmbiguity;
            this.AmbiguityCount = userAgent.AmbiguityCount;
        }

        /// <summary>
        /// Check if the two user agents are equals.
        /// </summary>
        /// <param name="other">The other useragent.</param>
        /// <returns>True if equals.</returns>
        public bool Equals(UserAgent other)
        {
            if (ReferenceEquals(this, other))
            {
                return true;
            }

            if (other is null)
            {
                return false;
            }

            return Equals(this.userAgentString, other.userAgentString) &&
                   (this.allFields == other.allFields || (this.allFields != null && this.allFields.SequenceEqual(other.allFields)));
        }

        /// <inheritdoc/>
        public override bool Equals(object obj)
        {
            if (!(obj is UserAgent))
            {
                return false;
            }

            return this.Equals((UserAgent)obj);
        }

        /// <summary>
        /// Extract the requested field by name.
        /// </summary>
        /// <param name="fieldName">The field name.</param>
        /// <returns>The extracted field.</returns>
        public AgentField Get(string fieldName)
        {
            if (DefaultUserAgentFields.USERAGENT_FIELDNAME.Equals(fieldName))
            {
                var agentField = new AgentField(this.userAgentString);
                agentField.SetValue(this.userAgentString, 0L);
                return agentField;
            }
            else if (fieldName != null)
            {
                return this.allFields.ContainsKey(fieldName) ? this.allFields[fieldName] : null;
            }
            else
            {
                return null;
            }
        }

        /// <inheritdoc/>
        IAgentField IUserAgent.Get(string fieldName)
        {
            return this.Get(fieldName);
        }

        /// <inheritdoc/>
        public IList<string> GetAvailableFieldNames()
        {
            var resultSet = new List<string>(this.allFields.Count + 10);
            foreach (var fieldName in this.allFields.Keys)
            {
                if (!resultSet.Contains(fieldName))
                {
                    var field = this.allFields[fieldName];
                    if (field?.GetValue() != null)
                    {
                        if (this.wantedFieldNames is null || this.wantedFieldNames.Contains(fieldName))
                        {
                            resultSet.Add(fieldName);
                        }
                        else
                        {
                            if (field.Confidence >= 0)
                            {
                                resultSet.Add(fieldName);
                            }
                        }
                    }
                }
            }

            // This is not a field; this is a special operator.
            resultSet.Remove(DefaultUserAgentFields.SET_ALL_FIELDS);
            return resultSet;
        }

        /// <summary>
        /// Retrieves all available field names for the user agent sorted.
        /// </summary>
        /// <returns>The List of available field names sorted. </returns>
        public List<string> GetAvailableFieldNamesSorted()
        {
            var fieldNames = new List<string>(this.GetAvailableFieldNames());

            var result = new List<string>();
            foreach (var fieldName in PreSortedFieldList)
            {
                if (fieldNames.Remove(fieldName))
                {
                    result.Add(fieldName);
                }
            }

            fieldNames.Sort();
            result.AddRange(fieldNames);
            return result;
        }

        /// <inheritdoc/>
        public long GetConfidence(string fieldName)
        {
            if (DefaultUserAgentFields.USERAGENT_FIELDNAME.Equals(fieldName))
            {
                return 0L;
            }

            if(this.allFields.TryGetValue(fieldName, out var field))
            {
                return field.GetConfidence();
            }
            else
            {
                return -1L;
            }
        }

        /// <inheritdoc/>
        public override int GetHashCode()
        {
            var hash = 3060293; // A random number
            foreach (var item in this.allFields.Keys)
            {
                hash = (hash, this.allFields[item], item).GetHashCode();
            }

            return (this.userAgentString, hash).GetHashCode();
        }

        /// <inheritdoc/>
        public string GetValue(string fieldName)
        {
            if (DefaultUserAgentFields.USERAGENT_FIELDNAME.Equals(fieldName))
            {
                return this.userAgentString;
            }

            if(this.allFields.TryGetValue(fieldName, out var field))
            {
                return field.GetValue();
            }

            return DefaultUserAgentFields.UNKNOWN_VALUE;
        }

        /// <summary>
        /// Process and sets a value for all fields.
        /// </summary>
        public void ProcessSetAll()
        {
            if (this.allFields.ContainsKey(DefaultUserAgentFields.SET_ALL_FIELDS))
            {
                var setAllField = this.allFields[DefaultUserAgentFields.SET_ALL_FIELDS];
                var value = setAllField.GetValue();
                var confidence = setAllField.Confidence;
                foreach (var fieldEntry in this.allFields)
                {
                    if (!IsSystemField(fieldEntry.Key))
                    {
                        fieldEntry.Value.SetValue(value, confidence);
                    }
                }
            }
        }

        /// <inheritdoc/>
        public virtual void Reset()
        {
            this.HasSyntaxError = false;
            this.HasAmbiguity = false;
            this.AmbiguityCount = 0;

            foreach (var field in this.allFields.Values)
            {
                field.Reset();
            }
        }

        /// <inheritdoc/>
        public virtual void Set(string attribute, string value, long confidence)
        {
            if(!this.allFields.TryGetValue(attribute, out var field))
            {
                field = new AgentField(null); // The fields we do not know get a 'null' default
                this.allFields[attribute] = field;
            }
            var updated = field.SetValue(value, confidence);

            var wasEmpty = confidence == -1;
            if (this.IsDebug && !wasEmpty)
            {
                if (updated)
                {
                    Logger.Info($"USE  {attribute} ({confidence}) = {value ?? "null"}");
                }
                else
                {
                    Logger.Info($"SKIP {attribute} ({confidence}) = {value ?? "null"}");
                }
            }
        }

        /// <summary>
        /// Sets values for user agent with another <see cref="UserAgent"/> and the applied matcher.
        /// </summary>
        /// <param name="newValuesUserAgent">The new user agent to be used.</param>
        /// <param name="appliedMatcher">The applied matcher.</param>
        public virtual void Set(UserAgent newValuesUserAgent, Matcher appliedMatcher)
        {
            foreach (var fieldName in newValuesUserAgent.allFields.Keys)
            {
                var field = newValuesUserAgent.allFields[fieldName];
                this.Set(fieldName, field.Value, field.Confidence);
            }
        }

        /// <inheritdoc/>
        public void SetForced(string attribute, string value, long confidence)
        {
            if (!this.allFields.TryGetValue(attribute, out var field))
            {
                field = new AgentField(null); // The fields we do not know get a 'null' default
                this.allFields[attribute] = field;
            }
            field.SetValueForced(value, confidence);

            var wasEmpty = confidence == -1;
            if (this.IsDebug && !wasEmpty)
            {
                Logger.Info($"USE  {attribute} ({confidence}) = {value}");
            }
        }

        /// <summary>
        /// Fired when a syntax error in the user agent occurs.
        /// </summary>
        /// <param name="output">The output.</param>
        /// <param name="recognizer">The recognizer used for parsing.</param>
        /// <param name="offendingSymbol">The offenting symbol tha caused the syntax error.</param>
        /// <param name="line">The line number where the syntax error occured.</param>
        /// <param name="charPositionInLine">The char position in line where the syntax error occured.</param>
        /// <param name="msg">Tge error message.</param>
        /// <param name="e">The exception.</param>
        public void SyntaxError(TextWriter output, IRecognizer recognizer, int offendingSymbol, int line, int charPositionInLine, string msg, RecognitionException e)
        {
            this.SyntaxError(output, recognizer, null, line, charPositionInLine, msg, e);
        }

        /// <summary>
        /// Fired when a syntax error in the user agent occurs.
        /// </summary>
        /// <param name="output">The output.</param>
        /// <param name="recognizer">The recognizer used for parsing.</param>
        /// <param name="offendingSymbol">The offenting symbol tha caused the syntax error.</param>
        /// <param name="line">The line number where the syntax error occured.</param>
        /// <param name="charPositionInLine">The char position in line where the syntax error occured.</param>
        /// <param name="msg">Tge error message.</param>
        /// <param name="e">The exception.</param>
        public void SyntaxError(TextWriter output, IRecognizer recognizer, IToken offendingSymbol, int line, int charPositionInLine, string msg, RecognitionException e)
        {
            if (this.IsDebug)
            {
                Logger.Error($"Syntax error");
                Logger.Error($"Source : {this.userAgentString}");
                Logger.Error($"Message: {msg}");
            }

            this.HasSyntaxError = true;
            var syntaxError = new AgentField("false");
            syntaxError.SetValue("true", 1);
            this.allFields[DefaultUserAgentFields.SYNTAX_ERROR] = syntaxError;
        }

        /*
        /// <summary>
        /// Gives a JSON representation of the user agent.
        /// </summary>
        /// <returns>The json.</returns>
        public string ToJson()
        {
            var fields = new List<string>
            {
                DefaultUserAgentFields.USERAGENT_FIELDNAME,
            };
            fields.AddRange(this.GetAvailableFieldNamesSorted());
            return this.ToJson(fields);
        }
        */

        /// <summary>
        /// Gives an XML representation of the user agent.
        /// </summary>
        /// <returns>The XML.</returns>
        public string ToXML()
        {
            List<string> fields = new List<string>
            {
                DefaultUserAgentFields.USERAGENT_FIELDNAME,
            };
            fields.AddRange(this.GetAvailableFieldNamesSorted());
            return this.ToXML(fields);
        }

        /// <summary>
        /// Gives an XML representation of the user agent.
        /// </summary>
        /// <param name="fieldNames">The field names to export.</param>
        /// <returns>The XML.</returns>
        public string ToXML(IList<string> fieldNames)
        {
            StringBuilder sb =
                new StringBuilder(10240)
                .Append("<?xml version=\"1.0\" encoding=\"UTF-8\"?>")
                .Append("<Yauaa>");

            foreach (string fieldName in fieldNames)
            {
                if (DefaultUserAgentFields.USERAGENT_FIELDNAME.Equals(fieldName))
                {
                    sb
                        .Append("<Useragent>")
                        .Append(SecurityElement.Escape(this.UserAgentString))
                        .Append("</Useragent>");
                }
                else
                {
                    sb
                        .Append('<').Append(SecurityElement.Escape(fieldName)).Append('>')
                        .Append(SecurityElement.Escape(this.GetValue(fieldName)))
                        .Append("</").Append(SecurityElement.Escape(fieldName)).Append('>');
                }
            }

            sb.Append("</Yauaa>");

            return sb.ToString();
        }

        /*
        /// <summary>
        /// Gives an JSON representation of the user agent.
        /// </summary>
        /// <param name="fieldNames">The list of fieldNames.</param>
        /// <returns>The JSON.</returns>
        public string ToJson(IList<string> fieldNames)
        {
            var sb = new StringBuilder(10240);
            sb.Append("{");
            var addSeparator = false;
            foreach (var fieldName in fieldNames)
            {
                if (addSeparator)
                {
                    sb.Append(',');
                }
                else
                {
                    addSeparator = true;
                }

                if (DefaultUserAgentFields.USERAGENT_FIELDNAME.Equals(fieldName))
                {
                    sb
                        .Append("\"Useragent\"")
                        .Append(':')
                        .Append(JsonConvert.ToString(this.UserAgentString));
                }
                else
                {
                    sb
                        .Append(JsonConvert.ToString(fieldName))
                        .Append(':')
                        .Append(JsonConvert.ToString(this.GetValue(fieldName)));
                }
            }

            sb.Append("}");
            return sb.ToString();
        }
        */

        /// <inheritdoc/>
        public override string ToString()
        {
            return this.ToString(this.GetAvailableFieldNamesSorted());
        }

        /// <summary>
        /// Returns a string representation of the user agent with the specified fields.
        /// </summary>
        /// <param name="fieldNames">The list of fieldNames.</param>
        /// <returns>The string.</returns>
        public string ToString(IList<string> fieldNames)
        {
            var sb = new StringBuilder($"  - user_agent_string: '\"{this.userAgentString}\"'\n");
            var maxLength = 0;
            foreach (var fieldName in fieldNames)
            {
                maxLength = Math.Max(maxLength, fieldName.Length);
            }

            foreach (var fieldName in fieldNames)
            {
                if (!DefaultUserAgentFields.USERAGENT_FIELDNAME.Equals(fieldName) && this.allFields.ContainsKey(fieldName))
                {
                    var field = this.allFields[fieldName];
                    if (field.GetValue() != null)
                    {
                        sb.Append("    ").Append(fieldName);
                        for (var l = fieldName.Length; l < maxLength + 2; l++)
                        {
                            sb.Append(' ');
                        }

                        sb.Append(": '").Append(field.GetValue()).Append('\'');
                        sb.Append('\n');
                    }
                }
            }

            return sb.ToString();
        }

        /// <summary>
        /// Returns a string representation of the user agent with the specified fields.
        /// </summary>
        /// <param name="fieldName1">The field.</param>
        /// <param name="otherFieldNames">The other fields.</param>
        /// <returns>The string.</returns>
        public string ToString(string fieldName1, params string[] otherFieldNames)
        {
            var l = new List<string> { fieldName1 };
            l.AddRange(otherFieldNames);
            return this.ToString(l);
        }

        /// <summary>
        /// Returns a yaml representation of the user agent, this can be also used as yaml test case to be loaded in yaml resources.
        /// </summary>
        /// <returns>The yaml result.</returns>
        public string ToYamlTestCase()
        {
            return this.ToYamlTestCase(false, null);
        }

        /// <summary>
        /// Returns a yaml representation of the user agent, this can be also used as yaml test case to be loaded in yaml resources.
        /// </summary>
        /// <param name="showConfidence">True if you want export the confidence.</param>
        /// <returns>The yaml result.</returns>
        public string ToYamlTestCase(bool showConfidence)
        {
            return this.ToYamlTestCase(showConfidence, null);
        }

        /// <summary>
        /// Returns a yaml representation of the user agent, this can be also used as yaml test case to be loaded in yaml resources.
        /// </summary>
        /// <param name="showConfidence">>True if you want export the confidence.</param>
        /// <param name="comments">A dictionary of comments that you can add.</param>
        /// <returns>The yaml result.</returns>
        public string ToYamlTestCase(bool showConfidence, IDictionary<string, string> comments)
        {
            var sb = new StringBuilder(10240);
            sb.Append("\n");
            sb.Append("- test:\n");
            sb.Append("#    options:\n");
            sb.Append("#    - 'verbose'\n");
            sb.Append("#    - 'init'\n");
            sb.Append("#    - 'only'\n");
            sb.Append("    input:\n");
            sb.Append("      user_agent_string: '").Append(this.userAgentString).Append("'\n");
            sb.Append("    expected:\n");

            var fieldNames = this.GetAvailableFieldNamesSorted();

            var maxNameLength = 30;
            var maxValueLength = 0;
            foreach (var fieldName in this.allFields.Keys)
            {
                maxNameLength = Math.Max(maxNameLength, fieldName.Length);
            }

            foreach (var fieldName in fieldNames)
            {
                string value = this.GetValue(fieldName);
                if (value != null)
                {
                    maxValueLength = Math.Max(maxValueLength, value.Length);
                }
            }

            foreach (var fieldName in fieldNames)
            {
                sb.Append("      ").Append(fieldName);
                for (var l = fieldName.Length; l < maxNameLength + 7; l++)
                {
                    sb.Append(' ');
                }

                var value = this.GetValue(fieldName);
                sb.Append(": '").Append(value).Append('\'');
                if (showConfidence)
                {
                    int l = value is null ? 0 : value.Length;
                    for (; l < maxValueLength + 5; l++)
                    {
                        sb.Append(' ');
                    }

                    sb.Append("# ").Append($"{this.GetConfidence(fieldName):8}");
                }

                if (comments != null && comments.ContainsKey(fieldName))
                {
                    sb.Append(" | ").Append(comments[fieldName]);
                }

                sb.Append('\n');
            }

            sb.Append("\n\n");

            return sb.ToString();
        }

        /// <summary>
        /// Used to associate without cheking an agent field to a field name.
        /// </summary>
        /// <param name="fieldName">The field name.</param>
        /// <param name="agentField">The agent field.</param>
        internal void SetImmediateForTesting(string fieldName, AgentField agentField)
        {
            this.allFields[fieldName] = agentField;
        }

        /// <summary>
        /// Used for initialization.
        /// </summary>
        private void Init()
        {
            if (this.wantedFieldNames is null)
            {
                foreach (var kv in DefaultsForKnownFields)
                {
                    this.allFields[kv.Key] = new AgentField(kv.Value.DefaultValue);
                }
            }
            else
            {
                foreach (var wantedFieldName in this.wantedFieldNames)
                {
                    if (DefaultsForKnownFields.ContainsKey(wantedFieldName))
                    {
                        var agentField = DefaultsForKnownFields[wantedFieldName];
                        this.allFields[wantedFieldName] = new AgentField(agentField.DefaultValue);
                    }
                }
            }
        }

        /// <summary>
        /// Used to set the wanted field names.
        /// </summary>
        /// <param name="newWantedFieldNames">The new wanted field names.</param>
        private void SetWantedFieldNames(ICollection<string> newWantedFieldNames)
        {
            if (newWantedFieldNames != null && newWantedFieldNames.Any())
            {
                this.wantedFieldNames = new HashSet<string>(newWantedFieldNames);
            }
        }
    }
}
