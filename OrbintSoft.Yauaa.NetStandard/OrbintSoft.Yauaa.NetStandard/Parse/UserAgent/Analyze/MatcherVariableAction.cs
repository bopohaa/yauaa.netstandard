﻿/*
 * Yet Another UserAgent Analyzer .NET Standard
 * Porting realized by Balzarotti Stefano, Copyright (C) OrbintSoft
 * 
 * Original Author and License:
 * 
 * Yet Another UserAgent Analyzer
 * Copyright (C) 2013-2018 Niels Basjes
 *
 * Licensed under the Apache License, Version 2.0 (the "License");
 * you may not use this file except in compliance with the License.
 * You may obtain a copy of the License at
 *
 * http://www.apache.org/licenses/LICENSE-2.0
 *
 * Unless required by applicable law or agreed to in writing, software
 * distributed under the License is distributed on an "AS IS" BASIS,
 * WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
 * See the License for the specific language governing permissions and
 * limitations under the License.
 * 
 * All rights should be reserved to the original author Niels Basjes
 */

using Antlr4.Runtime;
using log4net;
using OrbintSoft.Yauaa.Analyzer.Parse.UserAgent.Analyze.TreeWalker.Steps;
using OrbintSoft.Yauaa.Analyzer.Parse.UserAgent.Antlr4Source;
using System.Collections.Generic;

namespace OrbintSoft.Yauaa.Analyzer.Parse.UserAgent.Analyze
{
    public class MatcherVariableAction :MatcherAction
    {
        private static readonly ILog LOG = LogManager.GetLogger(typeof(MatcherVariableAction));

        private readonly string variableName;
        private WalkList.WalkResult foundValue = null;
        private readonly string expression;
        private HashSet<MatcherAction> interestedActions;

        public MatcherVariableAction(string variableName, string config, Matcher matcher)
        {
            this.variableName = variableName;
            expression = config;
            Init(config, matcher);
        }

        protected override ParserRuleContext ParseWalkerExpression(UserAgentTreeWalkerParser parser)
        {
            return parser.matcher();
        }

        protected override void SetFixedValue(string fixedValue)
        {
            throw new InvalidParserConfigurationException(
                "It is useless to put a fixed value \"" + fixedValue + "\" in the variable section.");
        }

        public string GetVariableName()
        {
            return variableName;
        }

        public override void Inform(string key, WalkList.WalkResult newlyFoundValue)
        {
            if (verbose)
            {
                LOG.Info(string.Format("INFO  : VARIABLE ({0}): {1}", variableName, key));
                LOG.Info(string.Format("NEED  : VARIABLE ({0}): {1}", variableName, GetMatchExpression()));
            }
            /*
             * We know the tree is parsed from left to right.
             * This is also the priority in the fields.
             * So we always use the first value we find.
             */
            if (this.foundValue == null)
            {
                this.foundValue = newlyFoundValue;
                if (verbose)
                {
                    LOG.Info(string.Format("KEPT  : VARIABLE ({0}): {1}", variableName, key));
                }

                if (interestedActions != null && interestedActions.Count != 0)
                {
                    foreach (MatcherAction action in interestedActions)
                    {
                        action.Inform(variableName, newlyFoundValue.GetValue(), newlyFoundValue.GetTree());
                    }
                }
            }
            else
            {
                if (verbose)
                {
                    LOG.Info((string.Format("IGNORE: VARIABLE ({0}): {1}", variableName, key)));
                }
            }
        }

        public override bool ObtainResult()
        {
            ProcessInformedMatches();
            return foundValue != null;
        }

        public override void Reset()
        {
            base.Reset();
            foundValue = null;
        }

        
        public override string ToString()
        {
            return "VARIABLE: (" + variableName + "): " + expression;
        }

        public void SetInterestedActions(HashSet<MatcherAction> newInterestedActions)
        {
            interestedActions = newInterestedActions;
        }
    }
}