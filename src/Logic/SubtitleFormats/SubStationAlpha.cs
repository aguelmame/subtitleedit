﻿using System;
using System.Collections.Generic;
using System.Drawing;
using System.Text;
using System.Xml;

namespace Nikse.SubtitleEdit.Logic.SubtitleFormats
{
    public class SubStationAlpha : SubtitleFormat
    {

        public string Errors { get; private set; }

        public override string Extension
        {
            get { return ".ssa"; }
        }

        public override string Name
        {
            get { return "Sub Station Alpha"; }
        }

        public override bool IsTimeBased
        {
            get { return true; }
        }

        public override bool IsMine(List<string> lines, string fileName)
        {
            var subtitle = new Subtitle();
            LoadSubtitle(subtitle, lines, fileName);
            Errors = null;
            return subtitle.Paragraphs.Count > _errorCount;
        }

        public override string ToText(Subtitle subtitle, string title)
        {
            const string header =
@"[Script Info]
; This is a Sub Station Alpha v4 script.
Title: {0}
ScriptType: v4.00
Collisions: Normal
PlayDepth: 0

[V4 Styles]
Format: Name, Fontname, Fontsize, PrimaryColour, SecondaryColour, TertiaryColour, BackColour, Bold, Italic, BorderStyle, Outline, Shadow, Alignment, MarginL, MarginR, MarginV, AlphaLevel, Encoding
Style: Default,{1},{2},{3},65535,65535,-2147483640,-1,0,1,3,0,2,10,10,10,0,1

[Events]
Format: Marked, Start, End, Style, Name, MarginL, MarginR, MarginV, Effect, Text";

            const string headerNoStyles =
@"[Script Info]
; This is a Sub Station Alpha v4 script.
Title: {0}
ScriptType: v4.00
Collisions: Normal
PlayDepth: 0

[V4 Styles]
Format: Name, Fontname, Fontsize, PrimaryColour, SecondaryColour, TertiaryColour, BackColour, Bold, Italic, BorderStyle, Outline, Shadow, Alignment, MarginL, MarginR, MarginV, AlphaLevel, Encoding
{1}

[Events]
Format: Marked, Start, End, Style, Name, MarginL, MarginR, MarginV, Effect, Text";

            const string timeCodeFormat = "{0}:{1:00}:{2:00}.{3:00}"; // h:mm:ss.cc
            const string paragraphWriteFormat = "Dialogue: Marked={4},{0},{1},{3},{5},0000,0000,0000,{6},{2}";
            const string commentWriteFormat = "Comment: Marked={4},{0},{1},{3},{5},0000,0000,0000,{6},{2}";

            var sb = new StringBuilder();
            System.Drawing.Color fontColor = System.Drawing.Color.FromArgb(Configuration.Settings.SubtitleSettings.SsaFontColorArgb);
            bool isValidAssHeader =!string.IsNullOrEmpty(subtitle.Header) && subtitle.Header.Contains("[V4 Styles]");
            List<string> styles = new List<string>();
            if (isValidAssHeader)
            {
                sb.AppendLine(subtitle.Header.Trim());
                string formatLine = "Format: Marked, Start, End, Style, Name, MarginL, MarginR, MarginV, Effect, Text";
                if (!subtitle.Header.Contains(formatLine))
                    sb.AppendLine(formatLine);
                styles = AdvancedSubStationAlpha.GetStylesFromHeader(subtitle.Header);
            }
            else if (!string.IsNullOrEmpty(subtitle.Header) && subtitle.Header.Contains("[V4+ Styles]"))
            {
                LoadStylesFromAdvancedSubstationAlpha(subtitle, title, header, headerNoStyles, sb);
            }
            else if (subtitle.Header != null && subtitle.Header.Contains("http://www.w3.org/ns/ttml"))
            {
                LoadStylesFromTimedText10(subtitle, title, header, headerNoStyles, sb);
            }
            else
            {
                sb.AppendLine(string.Format(header,
                                            title,
                                            Configuration.Settings.SubtitleSettings.SsaFontName,
                                            (int)Configuration.Settings.SubtitleSettings.SsaFontSize,
                                            System.Drawing.ColorTranslator.ToWin32(fontColor)));
            }
            foreach (Paragraph p in subtitle.Paragraphs)
            {
                string start = string.Format(timeCodeFormat, p.StartTime.Hours, p.StartTime.Minutes, p.StartTime.Seconds, p.StartTime.Milliseconds / 10);
                string end = string.Format(timeCodeFormat, p.EndTime.Hours, p.EndTime.Minutes, p.EndTime.Seconds, p.EndTime.Milliseconds / 10);
                string style = "Default";
                string actor = "NTP";
                if (!string.IsNullOrEmpty(p.Actor))
                    actor = p.Actor;
                string effect = "!Effect";
                if (!string.IsNullOrEmpty(p.Effect))
                    effect = p.Effect;
                string layer = "0";
                if (!string.IsNullOrEmpty(p.Layer))
                {
                    if (Utilities.IsInteger(p.Layer))
                        layer = p.Layer;
                }
                if (!string.IsNullOrEmpty(p.Extra) && isValidAssHeader && styles.Contains(p.Extra))
                    style = p.Extra;
                if (p.IsComment)
                    sb.AppendLine(string.Format(commentWriteFormat, start, end, FormatText(p), style, layer, actor, effect));
                else
                    sb.AppendLine(string.Format(paragraphWriteFormat, start, end, FormatText(p), style, layer, actor, effect));
            }
            return sb.ToString().Trim();
        }

        private static string FormatText(Paragraph p)
        {
            string text = p.Text.Replace(Environment.NewLine, "\\N");
            text = text.Replace("<i>", @"{\i1}");
            text = text.Replace("</i>", @"{\i0}");
            text = text.Replace("<u>", @"{\u1}");
            text = text.Replace("</u>", @"{\u0}");
            text = text.Replace("<b>", @"{\b1}");
            text = text.Replace("</b>", @"{\b0}");
            int count = 0;
            while (text.Contains("<font ") && count < 10)
            {
                int start = text.IndexOf(@"<font ");
                int end = text.IndexOf('>', start);
                if (end > 0)
                {
                    string fontTag = text.Substring(start + 4, end - (start + 4));
                    text = text.Remove(start, end - start + 1);
                    text = text.Replace("</font>", string.Empty);

                    fontTag = FormatTag(ref text, start, fontTag, "face=\"", "\"", "fn", "}");
                    fontTag = FormatTag(ref text, start, fontTag, "face='", "'", "fn", "}");

                    fontTag = FormatTag(ref text, start, fontTag, "size=\"", "\"", "fs", "}");
                    fontTag = FormatTag(ref text, start, fontTag, "size='", "'", "fs", "}");

                    fontTag = FormatTag(ref text, start, fontTag, "color=\"", "\"", "c&H", "&}");
                    fontTag = FormatTag(ref text, start, fontTag, "color='", "'", "c&H", "&}");
                }
                count++;
            }
            return text;
        }

        private void LoadStylesFromAdvancedSubstationAlpha(Subtitle subtitle, string title, string header, string headerNoStyles, StringBuilder sb)
        {
            try
            {
                bool styleFound = false;
                var ttStyles = new StringBuilder();
                foreach (string styleName in AdvancedSubStationAlpha.GetStylesFromHeader(subtitle.Header))
                {
                    try
                    {
                        var ssaStyle = AdvancedSubStationAlpha.GetSsaStyle(styleName, subtitle.Header);
                        if (ssaStyle != null)
                        {
                            string bold = "-1";
                            if (ssaStyle.Bold)
                                bold = "1";
                            string italic = "0";
                            if (ssaStyle.Italic)
                                italic = "1";

                            string newAlignment = "2";
                            switch (ssaStyle.Alignment)
                            {
                                case "1":
                                    newAlignment = "1";
                                    break;
                                case "3":
                                    newAlignment = "3";
                                    break;
                                case "4":
                                    newAlignment = "9";
                                    break;
                                case "5":
                                    newAlignment = "10";
                                    break;
                                case "6":
                                    newAlignment = "11";
                                    break;
                                case "7":
                                    newAlignment = "5";
                                    break;
                                case "8":
                                    newAlignment = "6";
                                    break;
                                case "9":
                                    newAlignment = "7";
                                    break;
                            }

                            //Format: Name, Fontname, Fontsize, PrimaryColour, SecondaryColour, TertiaryColour, BackColour, Bold, Italic, BorderStyle, Outline, Shadow, Alignment, MarginL, MarginR, MarginV, AlphaLevel, Encoding
                            string styleFormat = "Style: {0},{1},{2},{3},{4},{5},{6},{7},{8},{9},{10},{11},{12},{13},{14},{15},0,1";
                            //                           N   FN  FS  PC  SC  TC  BC  Bo  It  BS  O    Sh   Ali  ML   MR   MV   A Encoding

                            ttStyles.AppendLine(string.Format(styleFormat, ssaStyle.Name, ssaStyle.FontName, ssaStyle.FontSize, ssaStyle.Primary.ToArgb(), ssaStyle.Secondary.ToArgb(),
                                                ssaStyle.Outline.ToArgb(), ssaStyle.Background.ToArgb(), bold, italic, ssaStyle.BorderStyle, ssaStyle.OutlineWidth, ssaStyle.ShadowWidth,
                                                newAlignment, ssaStyle.MarginLeft, ssaStyle.MarginRight, ssaStyle.MarginVertical));
                            styleFound = true;
                        }
                    }
                    catch
                    {
                    }
                }

                if (styleFound)
                {
                    sb.AppendLine(string.Format(headerNoStyles, title, ttStyles.ToString()));
                    subtitle.Header = sb.ToString();
                }
                else
                {
                    sb.AppendLine(string.Format(header, title));
                }
            }
            catch
            {
                sb.AppendLine(string.Format(header, title));
            }
        }

        private static void LoadStylesFromTimedText10(Subtitle subtitle, string title, string header, string headerNoStyles, StringBuilder sb)
        {
            try
            {
                var lines = new List<string>();
                foreach (string s in subtitle.Header.Replace(Environment.NewLine, "\n").Split('\n'))
                    lines.Add(s);
                var tt = new TimedText10();
                var sub = new Subtitle();
                tt.LoadSubtitle(sub, lines, string.Empty);


                var xml = new XmlDocument();
                xml.LoadXml(subtitle.Header);
                var nsmgr = new XmlNamespaceManager(xml.NameTable);
                nsmgr.AddNamespace("ttml", "http://www.w3.org/ns/ttml");
                XmlNode head = xml.DocumentElement.SelectSingleNode("ttml:head", nsmgr);
                int stylexmlCount = 0;
                var ttStyles = new StringBuilder();
                foreach (XmlNode node in head.SelectNodes("//ttml:style", nsmgr))
                {
                    string name = null;
                    if (node.Attributes["xml:id"] != null)
                        name = node.Attributes["xml:id"].Value;
                    else if (node.Attributes["id"] != null)
                        name = node.Attributes["id"].Value;
                    if (name != null)
                    {
                        stylexmlCount++;

                        string fontFamily = "Arial";
                        if (node.Attributes["tts:fontFamily"] != null)
                            fontFamily = node.Attributes["tts:fontFamily"].Value;

                        string fontWeight = "normal";
                        if (node.Attributes["tts:fontWeight"] != null)
                            fontWeight = node.Attributes["tts:fontWeight"].Value;

                        string fontStyle = "normal";
                        if (node.Attributes["tts:fontStyle"] != null)
                            fontStyle = node.Attributes["tts:fontStyle"].Value;

                        string color = "#ffffff";
                        if (node.Attributes["tts:color"] != null)
                            color = node.Attributes["tts:color"].Value.Trim();
                        Color c = Color.White;
                        try
                        {
                            if (color.StartsWith("rgb("))
                            {
                                string[] arr = color.Remove(0, 4).TrimEnd(')').Split(",".ToCharArray(), StringSplitOptions.RemoveEmptyEntries);
                                c = Color.FromArgb(int.Parse(arr[0]), int.Parse(arr[1]), int.Parse(arr[2]));
                            }
                            else
                            {
                                c = System.Drawing.ColorTranslator.FromHtml(color);
                            }
                        }
                        catch
                        {
                        }

                        string fontSize = "20";
                        if (node.Attributes["tts:fontSize"] != null)
                            fontSize = node.Attributes["tts:fontSize"].Value.Replace("px", string.Empty).Replace("em", string.Empty);
                        int fSize;
                        if (!int.TryParse(fontSize, out fSize))
                            fSize = 20;

                        string styleFormat = "Style: {0},{1},{2},{3},65535,65535,-2147483640,-1,0,1,3,0,2,10,10,10,0,1";

                        ttStyles.AppendLine(string.Format(styleFormat, name, fontFamily, fSize.ToString(), c.ToArgb()));
                    }
                }

                if (stylexmlCount > 0)
                {
                    sb.AppendLine(string.Format(headerNoStyles, title, ttStyles.ToString()));
                    subtitle.Header = sb.ToString();
                }
                else
                {
                    sb.AppendLine(string.Format(header, title));
                }
            }
            catch
            {
                sb.AppendLine(string.Format(header, title));
            }
        }

        private static string FormatTag(ref string text, int start, string fontTag, string tag, string endSign, string ssaTagName, string endSsaTag)
        {
            if (fontTag.Contains(tag))
            {
                int fontStart = fontTag.IndexOf(tag);
                int fontEnd = fontTag.IndexOf(endSign, fontStart + tag.Length);
                if (fontEnd > 0)
                {
                    string subTag = fontTag.Substring(fontStart + tag.Length, fontEnd - (fontStart + tag.Length));
                    if (tag.Contains("color"))
                    {
                        subTag = subTag.Replace("#", string.Empty);

                        // switch from rrggbb to bbggrr
                        if (subTag.Length >= 6)
                            subTag = subTag.Remove(subTag.Length - 6) + subTag.Substring(subTag.Length - 2, 2) + subTag.Substring(subTag.Length - 4, 2) + subTag.Substring(subTag.Length - 6, 2);
                    }
                    fontTag = fontTag.Remove(fontStart, fontEnd - fontStart + 1);
                    if (start < text.Length)
                        text = text.Insert(start, @"{\" + ssaTagName + subTag + endSsaTag);
                }
            }
            return fontTag;
        }

        public override void LoadSubtitle(Subtitle subtitle, List<string> lines, string fileName)
        {
            _errorCount = 0;
            Errors = null;
            bool eventsStarted = false;
            subtitle.Paragraphs.Clear();
            string[] format = "Marked, Start, End, Style, Name, MarginL, MarginR, MarginV, Effect, Text".Split(',');
            int indexLayer = 0;
            int indexStart = 1;
            int indexEnd = 2;
            int indexStyle = 3;
            int indexName = 4;
            int indexEffect = 8;
            int indexText = 9;
            var errors = new StringBuilder();
            int lineNumber = 0;

            var header = new StringBuilder();
            foreach (string line in lines)
            {
                lineNumber++;
                if (!eventsStarted)
                    header.AppendLine(line);

                if (line.Trim().ToLower() == "[events]")
                {
                    eventsStarted = true;
                }
                else if (!string.IsNullOrEmpty(line) && line.Trim().StartsWith(";"))
                {
                    // skip comment lines
                }
                else if (eventsStarted && line.Trim().Length > 0)
                {
                    string s = line.Trim().ToLower();
                    if (s.StartsWith("format:"))
                    {
                        if (line.Length > 10)
                        {
                            format = line.ToLower().Substring(8).Split(',');
                            for (int i = 0; i < format.Length; i++)
                            {
                                if (format[i].Trim().ToLower() == "layer")
                                    indexLayer = i;
                                else if (format[i].Trim().ToLower() == "start")
                                    indexStart = i;
                                else if (format[i].Trim().ToLower() == "end")
                                    indexEnd = i;
                                else if (format[i].Trim().ToLower() == "text")
                                    indexText = i;
                                else if (format[i].Trim().ToLower() == "effect")
                                    indexEffect = i;
                                else if (format[i].Trim().ToLower() == "style")
                                    indexStyle = i;
                            }
                        }
                    }
                    else if (!string.IsNullOrEmpty(s))
                    {
                        string text = string.Empty;
                        string start = string.Empty;
                        string end = string.Empty;
                        string style = string.Empty;
                        string layer = string.Empty;
                        string effect = string.Empty;
                        string name = string.Empty;

                        string[] splittedLine;

                        if (s.StartsWith("dialogue:"))
                            splittedLine = line.Substring(10).Split(',');
                        else
                            splittedLine = line.Split(',');

                        for (int i = 0; i < splittedLine.Length; i++)
                        {
                            if (i == indexStart)
                                start = splittedLine[i].Trim();
                            else if (i == indexEnd)
                                end = splittedLine[i].Trim();
                            else if (i == indexLayer)
                                layer = splittedLine[i];
                            else if (i == indexEffect)
                                effect = splittedLine[i];
                            else if (i == indexText)
                                text = splittedLine[i];
                            else if (i == indexStyle)
                                style = splittedLine[i];
                            else if (i == indexName)
                                name = splittedLine[i];
                            else if (i > indexText)
                                text += "," + splittedLine[i];
                        }

                        try
                        {
                            var p = new Paragraph();

                            p.StartTime = GetTimeCodeFromString(start);
                            p.EndTime = GetTimeCodeFromString(end);
                            p.Text = AdvancedSubStationAlpha.GetFormattedText(text);
                            if (!string.IsNullOrEmpty(style))
                                p.Extra = style;
                            if (!string.IsNullOrEmpty(effect))
                                p.Effect = effect;
                            if (!string.IsNullOrEmpty(layer))
                                p.Layer = layer;
                            if (!string.IsNullOrEmpty(name))
                                p.Actor = name;
                            p.IsComment = s.StartsWith("comment:");
                            subtitle.Paragraphs.Add(p);
                        }
                        catch
                        {
                            _errorCount++;
                            if (errors.Length < 2000)
                                errors.AppendLine(string.Format(Configuration.Settings.Language.Main.LineNumberXErrorReadingTimeCodeFromSourceLineY, lineNumber, line));
                        }
                    }
                }
            }
            if (header.Length > 0)
                subtitle.Header = header.ToString();
            subtitle.Renumber(1);
            Errors = errors.ToString();
        }

        private static TimeCode GetTimeCodeFromString(string time)
        {
            // h:mm:ss.cc
            string[] timeCode = time.Split(':', '.');
            return new TimeCode(int.Parse(timeCode[0]),
                                int.Parse(timeCode[1]),
                                int.Parse(timeCode[2]),
                                int.Parse(timeCode[3]) * 10);
        }

        public override void RemoveNativeFormatting(Subtitle subtitle, SubtitleFormat newFormat)
        {
            if (newFormat != null && newFormat.Name == new AdvancedSubStationAlpha().Name)
            {
                // do we need any conversion?
            }
            else
            {
                foreach (Paragraph p in subtitle.Paragraphs)
                {
                    int indexOfBegin = p.Text.IndexOf("{");
                    string pre = string.Empty;
                    while (indexOfBegin >= 0 && p.Text.IndexOf("}") > indexOfBegin)
                    {
                        string s = p.Text.Substring(indexOfBegin);
                        if (s.StartsWith("{\\an1}") ||
                            s.StartsWith("{\\an2}") ||
                            s.StartsWith("{\\an3}") ||
                            s.StartsWith("{\\an4}") ||
                            s.StartsWith("{\\an5}") ||
                            s.StartsWith("{\\an6}") ||
                            s.StartsWith("{\\an7}") ||
                            s.StartsWith("{\\an8}") ||
                            s.StartsWith("{\\an9}"))
                        {
                            pre = s.Substring(0, 6);
                        }
                        else if (s.StartsWith("{\\an1\\") ||
                            s.StartsWith("{\\an2\\") ||
                            s.StartsWith("{\\an3\\") ||
                            s.StartsWith("{\\an4\\") ||
                            s.StartsWith("{\\an5\\") ||
                            s.StartsWith("{\\an6\\") ||
                            s.StartsWith("{\\an7\\") ||
                            s.StartsWith("{\\an8\\") ||
                            s.StartsWith("{\\an9\\"))
                        {
                            pre = s.Substring(0, 5) + "}";
                        }
                        else if (s.StartsWith("{\\a1}") || s.StartsWith("{\\a1\\") ||
                                 s.StartsWith("{\\a3}") || s.StartsWith("{\\a3\\"))
                        {
                            pre = s.Substring(0, 4) + "}";
                        }
                        else if (s.StartsWith("{\\a9}") || s.StartsWith("{\\a9\\"))
                        {
                            pre = "{\\an4}";
                        }
                        else if (s.StartsWith("{\\a10}") || s.StartsWith("{\\a10\\"))
                        {
                            pre = "{\\an5}";
                        }
                        else if (s.StartsWith("{\\a11}") || s.StartsWith("{\\a11\\"))
                        {
                            pre = "{\\an6}";
                        }
                        else if (s.StartsWith("{\\a5}") || s.StartsWith("{\\a5\\"))
                        {
                            pre = "{\\an7}";
                        }
                        else if (s.StartsWith("{\\a6}") || s.StartsWith("{\\a6\\"))
                        {
                            pre = "{\\an8}";
                        }
                        else if (s.StartsWith("{\\a7}") || s.StartsWith("{\\a7\\"))
                        {
                            pre = "{\\an9}";
                        }
                        int indexOfEnd = p.Text.IndexOf("}");
                        p.Text = p.Text.Remove(indexOfBegin, (indexOfEnd - indexOfBegin) + 1);

                        indexOfBegin = p.Text.IndexOf("{");
                    }
                    p.Text = pre + p.Text;
                }
            }
        }

        public override bool HasStyleSupport
        {
            get
            {
                return true;
            }
        }

    }
}
