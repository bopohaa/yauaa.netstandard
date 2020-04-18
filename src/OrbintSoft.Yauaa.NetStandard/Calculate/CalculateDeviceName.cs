﻿//-----------------------------------------------------------------------
// <copyright file="CalculateDeviceName.cs" company="OrbintSoft">
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
// <date>2020, 04, 16, 08:28</date>
namespace OrbintSoft.Yauaa.Calculate
{
    using System;
    using OrbintSoft.Yauaa.Analyzer;
    using OrbintSoft.Yauaa.Utils;

    /// <summary>
    /// CalculateDeviceName.
    /// </summary>
    [Serializable]
    public class CalculateDeviceName : IFieldCalculator
    {
        /// <inheritdoc/>
        public void Calculate(UserAgent userAgent)
        {
            // Make sure the DeviceName always starts with the DeviceBrand
            AgentField deviceName = userAgent.Get(UserAgent.DEVICE_NAME);
            if (deviceName.GetConfidence() >= 0)
            {
                AgentField deviceBrand = userAgent.Get(UserAgent.DEVICE_BRAND);
                string deviceNameValue = deviceName.GetValue();
                string deviceBrandValue = deviceBrand.GetValue();
                if (deviceName.GetConfidence() >= 0 &&
                    deviceBrand.GetConfidence() >= 0 &&
                    !deviceBrandValue.Equals("Unknown"))
                {
                    // In some cases it does start with the brand but without a separator following the brand
                    deviceNameValue = Normalize.CleanupDeviceBrandName(deviceBrandValue, deviceNameValue);
                }
                else
                {
                    deviceNameValue = Normalize.Brand(deviceNameValue);
                }

                userAgent.SetForced(
                    UserAgent.DEVICE_NAME,
                    deviceNameValue,
                    deviceName.GetConfidence());
            }
        }
    }
}
