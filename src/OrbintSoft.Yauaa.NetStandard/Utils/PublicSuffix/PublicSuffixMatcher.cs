//-----------------------------------------------------------------------
// <copyright file="PublicSuffixMatcher.cs" company="OrbintSoft">
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
// <date>2018, 11, 24, 12:49</date>
//-----------------------------------------------------------------------

using Nager.PublicSuffix;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;

namespace OrbintSoft.Yauaa.Utils.PublicSuffix
{
    /// <summary>
    ///  Utility class that can test if DNS names match the content of the Public Suffix List.
    /// </summary>
    public class PublicSuffixMatcher
    {
        private class Provider : Nager.PublicSuffix.ITldRuleProvider
        {
            private const string TOPDOMAIN_PSL_RESOURCE_NAME = "OrbintSoft.Yauaa.td_public_suffix_list.txt";
            public Task<IEnumerable<Nager.PublicSuffix.TldRule>> BuildAsync()
            {
                var assembly = typeof(PublicSuffixMatcher).Assembly;
                using (var resource = assembly.GetManifestResourceStream(TOPDOMAIN_PSL_RESOURCE_NAME))
                using (var reader = new StreamReader(resource))
                {
                    var ruleData = reader.ReadToEnd();
                    var ruleParser = new TldRuleParser();
                    var rules = ruleParser.ParseRules(ruleData);
                    return Task.FromResult(rules);
                }
            }
        }

        private readonly Nager.PublicSuffix.DomainParser _domainParser;

        public PublicSuffixMatcher()
        {
            _domainParser = new Nager.PublicSuffix.DomainParser(new Provider());
        }

        public static PublicSuffixMatcher DefaultInstance { get; } = new PublicSuffixMatcher();

        public bool TryParse(string hostname, out DomainInfo rval)
        {
            try
            {
                rval = _domainParser.Parse(hostname);
                return true;
            }
            catch
            {
                rval = default;
                return false;
            }
        }
    }
}

namespace DomainParser.Library
{
    public static class DomainName
    {
        public static bool TryParse(string hostname, out DomainInfo rval)
            => OrbintSoft.Yauaa.Utils.PublicSuffix.PublicSuffixMatcher.DefaultInstance.TryParse(hostname, out rval);
    }
}