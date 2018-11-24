﻿//<copyright file="UserAgentGetChildrenVisitor.cs" company="OrbintSoft">
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
//<date>2018, 8, 15, 04:49</date>
//<summary></summary>

namespace OrbintSoft.Yauaa.Analyze.TreeWalker.Steps.Walk.StepDowns
{
    using Antlr4.Runtime;
    using Antlr4.Runtime.Misc;
    using Antlr4.Runtime.Tree;
    using OrbintSoft.Yauaa.Antlr4Source;
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Runtime.Serialization;

    /// <summary>
    /// Defines the <see cref="UserAgentGetChildrenVisitor" />
    /// </summary>
    [Serializable]
    public class UserAgentGetChildrenVisitor : UserAgentBaseVisitor<IEnumerator<IParseTree>>, ISerializable
    {
        /// <summary>
        /// Defines the Empty
        /// </summary>
        private static readonly IEnumerator<IParseTree> Empty = null;

        /// <summary>
        /// Defines the name
        /// </summary>
        private readonly string name;

        /// <summary>
        /// Defines the start
        /// </summary>
        private readonly int start;

        /// <summary>
        /// Defines the end
        /// </summary>
        private readonly int end;

        /// <summary>
        /// Defines the childIterable
        /// </summary>
        private ChildIterable childIterable;

        /// <summary>
        /// Initializes a new instance of the <see cref="UserAgentGetChildrenVisitor"/> class.
        /// </summary>
        /// <param name="info">The info<see cref="SerializationInfo"/></param>
        /// <param name="context">The context<see cref="StreamingContext"/></param>
        public UserAgentGetChildrenVisitor(SerializationInfo info, StreamingContext context)
        {
            name = (string)info.GetValue("name", typeof(string));
            start = (int)info.GetValue("start", typeof(int));
            end = (int)info.GetValue("end", typeof(int));
            Init(name, start, end);
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="UserAgentGetChildrenVisitor"/> class.
        /// </summary>
        /// <param name="name">The name<see cref="string"/></param>
        /// <param name="start">The start<see cref="int"/></param>
        /// <param name="end">The end<see cref="int"/></param>
        public UserAgentGetChildrenVisitor(string name, int start, int end)
        {
            this.name = name;
            this.start = start;
            this.end = end;
            Init(name, start, end);
        }

        /// <summary>
        /// The Init
        /// </summary>
        /// <param name="name">The name<see cref="string"/></param>
        /// <param name="start">The start<see cref="int"/></param>
        /// <param name="end">The end<see cref="int"/></param>
        private void Init(string name, int start, int end)
        {
            switch (name)
            {
                case "keyvalue":
                    childIterable = new ChildIterable(false, start, end, clazz => (
                        clazz is UserAgentParser.KeyValueContext ||
                        clazz is UserAgentParser.KeyWithoutValueContext ||
                        clazz is UserAgentParser.ProductNameKeyValueContext));
                    break;

                case "product":
                    childIterable = new ChildIterable(false, start, end, clazz => (
                        clazz is UserAgentParser.ProductContext ||
                        clazz is UserAgentParser.CommentProductContext ||
                        clazz is UserAgentParser.ProductNameNoVersionContext));
                    break;

                case "uuid":
                    childIterable = new ChildIterable(false, start, end, clazz => (
                        clazz is UserAgentParser.UuIdContext ||
                        clazz is UserAgentParser.ProductNameUuidContext));
                    break;

                case "base64":
                    childIterable = new ChildIterable(false, start, end, clazz => (
                        clazz is UserAgentParser.Base64Context));
                    break;

                case "url":
                    childIterable = new ChildIterable(false, start, end, clazz => (
                        clazz is UserAgentParser.SiteUrlContext ||
                        clazz is UserAgentParser.ProductNameUrlContext));
                    break;

                case "email":
                    childIterable = new ChildIterable(false, start, end, clazz => (
                        clazz is UserAgentParser.EmailAddressContext ||
                        clazz is UserAgentParser.ProductNameEmailContext));
                    break;

                case "text":
                    childIterable = new ChildIterable(false, start, end, clazz => (
                        clazz is UserAgentParser.MultipleWordsContext ||
                        clazz is UserAgentParser.VersionWordsContext ||
                        clazz is UserAgentParser.EmptyWordContext ||
                        clazz is UserAgentParser.RootTextContext ||
                        clazz is UserAgentParser.KeyValueVersionNameContext));
                    break;

                case "name":
                    childIterable = new ChildIterable(false, start, end, clazz => (
                        clazz is UserAgentParser.ProductNameContext));
                    break;

                case "version":
                    childIterable = new ChildIterable(true, start, end, clazz => (
                        clazz is UserAgentParser.ProductVersionContext ||
                        clazz is UserAgentParser.ProductVersionWithCommasContext ||
                        clazz is UserAgentParser.ProductVersionWordsContext ||
                        clazz is UserAgentParser.ProductVersionSingleWordContext));
                    break;

                case "comments":
                    childIterable = new ChildIterable(true, start, end, clazz => (
                        clazz is UserAgentParser.CommentBlockContext));
                    break;

                case "key":
                    childIterable = new ChildIterable(false, start, end, clazz => (
                        clazz is UserAgentParser.KeyNameContext));
                    break;

                case "value":
                    childIterable = new ChildIterable(false, start, end, clazz => (
                        clazz is UserAgentParser.UuIdContext ||
                        clazz is UserAgentParser.MultipleWordsContext ||
                        clazz is UserAgentParser.SiteUrlContext ||
                        clazz is UserAgentParser.EmailAddressContext ||
                        clazz is UserAgentParser.KeyValueVersionNameContext ||
                        clazz is UserAgentParser.KeyValueProductVersionNameContext));
                    break;

                case "entry":
                    childIterable = new ChildIterable(false, start, end, clazz => (
                        clazz is UserAgentParser.CommentEntryContext));
                    break;

                default:
                    childIterable = new ChildIterable(false, start, end, clazz => (false));
                    break;
            }
        }

        /// <summary>
        /// Gets the DefaultResult
        /// </summary>
        protected override IEnumerator<IParseTree> DefaultResult
        {
            get
            {
                return Empty;
            }
        }

        /// <summary>
        /// The GetChildrenByName
        /// </summary>
        /// <param name="ctx">The ctx<see cref="ParserRuleContext"/></param>
        /// <returns>The <see cref="IEnumerator{IParseTree}"/></returns>
        internal IEnumerator<IParseTree> GetChildrenByName(ParserRuleContext ctx)
        {
            return childIterable.Iterator(ctx);
        }

        /// <summary>
        /// The VisitUserAgent
        /// </summary>
        /// <param name="context">The context<see cref="UserAgentParser.UserAgentContext"/></param>
        /// <returns>The <see cref="IEnumerator{IParseTree}"/></returns>
        public override IEnumerator<IParseTree> VisitUserAgent([NotNull] UserAgentParser.UserAgentContext context)
        {
            IEnumerator<IParseTree> children = GetChildrenByName(context);
            if (!children.MoveNext() && children.Current == null)
            {
                return VisitChildren(context);
            }
            return children;
        }

        /// <summary>
        /// The VisitRootElements
        /// </summary>
        /// <param name="context">The context<see cref="UserAgentParser.RootElementsContext"/></param>
        /// <returns>The <see cref="IEnumerator{IParseTree}"/></returns>
        public override IEnumerator<IParseTree> VisitRootElements([NotNull] UserAgentParser.RootElementsContext context)
        {
            return GetChildrenByName(context);
        }

        /// <summary>
        /// The VisitProduct
        /// </summary>
        /// <param name="context">The context<see cref="UserAgentParser.ProductContext"/></param>
        /// <returns>The <see cref="IEnumerator{IParseTree}"/></returns>
        public override IEnumerator<IParseTree> VisitProduct([NotNull] UserAgentParser.ProductContext context)
        {
            return GetChildrenByName(context);
        }

        /// <summary>
        /// The VisitProductNameNoVersion
        /// </summary>
        /// <param name="context">The context<see cref="UserAgentParser.ProductNameNoVersionContext"/></param>
        /// <returns>The <see cref="IEnumerator{IParseTree}"/></returns>
        public override IEnumerator<IParseTree> VisitProductNameNoVersion([NotNull] UserAgentParser.ProductNameNoVersionContext context)
        {
            return GetChildrenByName(context);
        }

        /// <summary>
        /// The VisitCommentProduct
        /// </summary>
        /// <param name="context">The context<see cref="UserAgentParser.CommentProductContext"/></param>
        /// <returns>The <see cref="IEnumerator{IParseTree}"/></returns>
        public override IEnumerator<IParseTree> VisitCommentProduct([NotNull] UserAgentParser.CommentProductContext context)
        {
            return GetChildrenByName(context);
        }

        /// <summary>
        /// The VisitProductName
        /// </summary>
        /// <param name="context">The context<see cref="UserAgentParser.ProductNameContext"/></param>
        /// <returns>The <see cref="IEnumerator{IParseTree}"/></returns>
        public override IEnumerator<IParseTree> VisitProductName([NotNull] UserAgentParser.ProductNameContext context)
        {
            return GetChildrenByName(context);
        }

        /// <summary>
        /// The VisitProductNameKeyValue
        /// </summary>
        /// <param name="context">The context<see cref="UserAgentParser.ProductNameKeyValueContext"/></param>
        /// <returns>The <see cref="IEnumerator{IParseTree}"/></returns>
        public override IEnumerator<IParseTree> VisitProductNameKeyValue([NotNull] UserAgentParser.ProductNameKeyValueContext context)
        {
            switch (name)
            {
                case "key":
                    var list = new List<ParserRuleContext>() { context.key };
                    return list.GetEnumerator();
                case "value":
                    var children = context.multipleWords().Select(s => s as IParseTree).ToList();
                    if (children.Count != 0)
                    {
                        return children.GetEnumerator();
                    }

                    children = context.keyValueProductVersionName().Select(s => s as IParseTree).ToList();
                    if (children.Count != 0)
                    {
                        return children.GetEnumerator();
                    }

                    children = context.siteUrl().Select(s => s as IParseTree).ToList();
                    if (children.Count != 0)
                    {
                        return children.GetEnumerator();
                    }

                    children = context.emailAddress().Select(s => s as IParseTree).ToList();
                    if (children.Count != 0)
                    {
                        return children.GetEnumerator();
                    }

                    children = context.uuId().Select(s => s as IParseTree).ToList();
                    return children.GetEnumerator();
                default:
                    return GetChildrenByName(context);
            }
        }

        /// <summary>
        /// The VisitProductVersion
        /// </summary>
        /// <param name="context">The context<see cref="UserAgentParser.ProductVersionContext"/></param>
        /// <returns>The <see cref="IEnumerator{IParseTree}"/></returns>
        public override IEnumerator<IParseTree> VisitProductVersion([NotNull] UserAgentParser.ProductVersionContext context)
        {
            return GetChildrenByName(context);
        }

        /// <summary>
        /// The VisitProductVersionWithCommas
        /// </summary>
        /// <param name="context">The context<see cref="UserAgentParser.ProductVersionWithCommasContext"/></param>
        /// <returns>The <see cref="IEnumerator{IParseTree}"/></returns>
        public override IEnumerator<IParseTree> VisitProductVersionWithCommas([NotNull] UserAgentParser.ProductVersionWithCommasContext context)
        {
            return GetChildrenByName(context);
        }

        /// <summary>
        /// The VisitKeyValue
        /// </summary>
        /// <param name="context">The context<see cref="UserAgentParser.KeyValueContext"/></param>
        /// <returns>The <see cref="IEnumerator{IParseTree}"/></returns>
        public override IEnumerator<IParseTree> VisitKeyValue([NotNull] UserAgentParser.KeyValueContext context)
        {
            return GetChildrenByName(context);
        }

        /// <summary>
        /// The VisitKeyWithoutValue
        /// </summary>
        /// <param name="context">The context<see cref="UserAgentParser.KeyWithoutValueContext"/></param>
        /// <returns>The <see cref="IEnumerator{IParseTree}"/></returns>
        public override IEnumerator<IParseTree> VisitKeyWithoutValue([NotNull] UserAgentParser.KeyWithoutValueContext context)
        {
            return GetChildrenByName(context);
        }

        /// <summary>
        /// The VisitCommentBlock
        /// </summary>
        /// <param name="context">The context<see cref="UserAgentParser.CommentBlockContext"/></param>
        /// <returns>The <see cref="IEnumerator{IParseTree}"/></returns>
        public override IEnumerator<IParseTree> VisitCommentBlock([NotNull] UserAgentParser.CommentBlockContext context)
        {
            return GetChildrenByName(context);
        }

        /// <summary>
        /// The VisitCommentEntry
        /// </summary>
        /// <param name="context">The context<see cref="UserAgentParser.CommentEntryContext"/></param>
        /// <returns>The <see cref="IEnumerator{IParseTree}"/></returns>
        public override IEnumerator<IParseTree> VisitCommentEntry([NotNull] UserAgentParser.CommentEntryContext context)
        {
            return GetChildrenByName(context);
        }

        /// <summary>
        /// The GetObjectData
        /// </summary>
        /// <param name="info">The info<see cref="SerializationInfo"/></param>
        /// <param name="context">The context<see cref="StreamingContext"/></param>
        public void GetObjectData(SerializationInfo info, StreamingContext context)
        {
            info.AddValue("name", name, typeof(string));
            info.AddValue("start", start, typeof(int));
            info.AddValue("end", end, typeof(int));
        }
    }
}