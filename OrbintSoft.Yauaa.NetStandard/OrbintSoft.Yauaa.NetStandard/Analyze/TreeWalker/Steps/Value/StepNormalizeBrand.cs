﻿//<copyright file="StepNormalizeBrand.cs" company="OrbintSoft">
//	Yet Another UserAgent Analyzer.NET Standard
//	Porting realized by Stefano Balzarotti, Copyright (C) OrbintSoft
//
//	Original Author and License:
//
//	Yet Another UserAgent Analyzer
//	Copyright(C) 2013-2018 Niels Basjes
//
//	Licensed under the Apache License, Version 2.0 (the "License");
//	you may not use this file except in compliance with the License.
//	You may obtain a copy of the License at
//
//	https://www.apache.org/licenses/LICENSE-2.0
//
//	Unless required by applicable law or agreed to in writing, software
//	distributed under the License is distributed on an "AS IS" BASIS,
//	WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
//	See the License for the specific language governing permissions and
//	limitations under the License.
//
//</copyright>
//<author>Stefano Balzarotti, Niels Basjes</author>
//<date>2018, 8, 14, 13:03</date>
//<summary></summary>

namespace OrbintSoft.Yauaa.Analyze.TreeWalker.Steps.Value
{
    using Antlr4.Runtime.Tree;
    using OrbintSoft.Yauaa.Utils;
    using System;

    /// <summary>
    /// Defines the <see cref="StepNormalizeBrand" />
    /// </summary>
    [Serializable]
    public class StepNormalizeBrand : Step
    {
        /// <summary>
        /// The Walk
        /// </summary>
        /// <param name="tree">The tree<see cref="IParseTree"/></param>
        /// <param name="value">The value<see cref="string"/></param>
        /// <returns>The <see cref="WalkList.WalkResult"/></returns>
        public override WalkList.WalkResult Walk(IParseTree tree, string value)
        {
            string actualValue = GetActualValue(tree, value);
            string filteredValue = Normalize.Brand(actualValue);
            return WalkNextStep(tree, filteredValue);
        }

        public override bool CanFail()
        {
            return false;
        }

        /// <summary>
        /// The ToString
        /// </summary>
        /// <returns>The <see cref="string"/></returns>
        public override string ToString()
        {
            return "StepNormalizeBrand()";
        }
    }
}