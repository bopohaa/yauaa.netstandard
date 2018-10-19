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

using System.Collections;
using System.Collections.Generic;

namespace OrbintSoft.Yauaa.Analyzer.Parse.UserAgentNS.Analyze
{
    public class NumberRangeList: IReadOnlyList<int>
    {
        private readonly int start;
        private readonly int end;

        public int Count => end - start + 1;

        public int this[int index] => start + index;

        public NumberRangeList(int start, int end)
        {
            this.start = start;
            this.end = end;
        }

        public int GetStart()
        {
            return start;
        }

        public int GetEnd()
        {
            return end;
        }

        public class NumberRangeEnumerator : IEnumerator<int>
        {
            readonly NumberRangeList list;
            int offset = -1;
            public int Current => list[offset];

            object IEnumerator.Current => list[offset];

            public NumberRangeEnumerator(NumberRangeList list)
            {
                this.list = list;
            }

            public void Dispose()
            {
                
            }

            public bool MoveNext()
            {
                if (offset < list.Count - 1)
                {
                    offset++;
                    return true;
                }
                else
                {
                    return false;
                }
            }

            public void Reset()
            {
                offset = -1;
            }
        }

        public IEnumerator<int> GetEnumerator()
        {
            return new NumberRangeEnumerator(this);
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }
    }
}