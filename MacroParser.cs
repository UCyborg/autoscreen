﻿//-----------------------------------------------------------------------
// <copyright file="MacroParser.cs" company="Gavin Kendall">
//     Copyright (c) Gavin Kendall. All rights reserved.
// </copyright>
// <author>Gavin Kendall</author>
// <summary></summary>
//-----------------------------------------------------------------------
namespace AutoScreenCapture
{
    using System;

    public static class MacroParser
    {
        public static readonly string DateFormat = "yyyy-MM-dd";
        public static readonly string TimeFormat = "HH-mm-ss-fff";
        public static readonly string UserMacro = @"%year%-%month%-%day%\%screen%\%hour%-%minute%-%second%-%millisecond%.%format%";
        public static readonly string ScreenshotListMacro = @"%year%-%month%-%day%\%year%-%month%-%day%_%hour%-%minute%-%second%-%millisecond%.%format%";
        public static readonly string ApplicationMacro = @"%year%-%month%-%day%\%screen%\%year%-%month%-%day%_%hour%-%minute%-%second%-%millisecond%.%format%";

        public static string ParseTags(string path, string imageFormat, string screenName, DateTime dateTimeScreenshotTaken)
        {
            if (!string.IsNullOrEmpty(imageFormat))
            {
                path = path.Replace("%format%", ImageFormatCollection.GetByName(imageFormat).Extension.TrimStart('.'));
            }

            if (!string.IsNullOrEmpty(screenName))
            {
                path = path.Replace("%screen%", screenName);
            }

            path = path.Replace("%year%", dateTimeScreenshotTaken.ToString("yyyy"));
            path = path.Replace("%month%", dateTimeScreenshotTaken.ToString("MM"));
            path = path.Replace("%day%", dateTimeScreenshotTaken.ToString("dd"));
            path = path.Replace("%hour%", dateTimeScreenshotTaken.ToString("HH"));
            path = path.Replace("%minute%", dateTimeScreenshotTaken.ToString("mm"));
            path = path.Replace("%second%", dateTimeScreenshotTaken.ToString("ss"));
            path = path.Replace("%millisecond%", dateTimeScreenshotTaken.ToString("fff"));

            return path;
        }
    }
}