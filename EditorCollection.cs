﻿//-----------------------------------------------------------------------
// <copyright file="EditorCollection.cs" company="Gavin Kendall">
//     Copyright (c) Gavin Kendall. All rights reserved.
// </copyright>
// <author>Gavin Kendall</author>
// <summary></summary>
//-----------------------------------------------------------------------
namespace AutoScreenCapture
{
    using System;
    using System.Collections;
    using System.Text;
    using System.Xml;

    public static class EditorCollection
    {
        private static ArrayList _editorList = new ArrayList();

        private const string XML_FILE_INDENT_CHARS = "   ";
        private const string XML_FILE_EDITOR_NODE = "editor";
        private const string XML_FILE_EDITORS_NODE = "editors";
        private const string XML_FILE_ROOT_NODE = "autoscreen";

        private const string EDITOR_NAME = "name";
        private const string EDITOR_ARGUMENTS = "arguments";
        private const string EDITOR_APPLICATION = "application";
        private const string EDITOR_XPATH = "/" + XML_FILE_ROOT_NODE + "/" + XML_FILE_EDITORS_NODE + "/" + XML_FILE_EDITOR_NODE;

        public static void Add(Editor editor)
        {
            _editorList.Add(editor);

            Log.Write("Added " + editor.Name + " (" + editor.Application + " " + editor.Arguments + ")");
        }

        public static void Remove(Editor editor)
        {
            _editorList.Remove(editor);

            Log.Write("Removed " + editor.Name + " (" + editor.Application + " " + editor.Arguments + ")");
        }

        public static int Count
        {
            get { return _editorList.Count; }
        }

        public static Editor Get(Editor editorToFind)
        {
            for (int i = 0; i < _editorList.Count; i++)
            {
                Editor editor = GetByIndex(i);

                if (editor.Equals(editorToFind))
                {
                    return GetByIndex(i);
                }
            }

            return null;
        }

        public static Editor GetByIndex(int index)
        {
            return (Editor)_editorList[index];
        }

        public static Editor GetByName(string name)
        {
            for (int i = 0; i < _editorList.Count; i++)
            {
                Editor editor = GetByIndex(i);

                if (editor.Name.Equals(name))
                {
                    return GetByIndex(i);
                }
            }

            return null;
        }

        /// <summary>
        /// Loads the image editors.
        /// </summary>
        public static void Load()
        {
            XmlDocument xdoc = new XmlDocument();
            xdoc.LoadXml(Properties.Settings.Default.Editors);

            XmlNodeList xeditors = xdoc.SelectNodes(EDITOR_XPATH);

            foreach (XmlNode xeditor in xeditors)
            {
                Editor editor = new Editor();
                XmlNodeReader xreader = new XmlNodeReader(xeditor);

                while (xreader.Read())
                {
                    if (xreader.IsStartElement())
                    {
                        switch (xreader.Name)
                        {
                            case EDITOR_NAME:
                                xreader.Read();
                                editor.Name = xreader.Value;
                                break;

                            case EDITOR_APPLICATION:
                                xreader.Read();
                                editor.Application = xreader.Value;
                                break;

                            case EDITOR_ARGUMENTS:
                                xreader.Read();
                                editor.Arguments = xreader.Value;
                                break;
                        }
                    }
                }

                xreader.Close();

                if (!string.IsNullOrEmpty(editor.Name) &&
                    !string.IsNullOrEmpty(editor.Application) &&
                    !string.IsNullOrEmpty(editor.Arguments))
                {
                    Add(editor);
                }
            }
        }

        /// <summary>
        /// Saves the image editors.
        /// </summary>
        public static void Save()
        {
            XmlWriterSettings xsettings = new XmlWriterSettings
            {
                Indent = true,
                CloseOutput = true,
                CheckCharacters = true,
                Encoding = Encoding.UTF8,
                NewLineChars = Environment.NewLine,
                IndentChars = XML_FILE_INDENT_CHARS,
                NewLineHandling = NewLineHandling.Entitize,
                ConformanceLevel = ConformanceLevel.Document
            };

            StringBuilder editors = new StringBuilder();

            using (XmlWriter xwriter = XmlWriter.Create(editors))
            {
                xwriter.WriteStartDocument();
                xwriter.WriteStartElement(XML_FILE_ROOT_NODE);
                xwriter.WriteStartElement(XML_FILE_EDITORS_NODE);

                foreach (object obj in _editorList)
                {
                    Editor editor = (Editor)obj;

                    xwriter.WriteStartElement(XML_FILE_EDITOR_NODE);
                    xwriter.WriteElementString(EDITOR_NAME, editor.Name);
                    xwriter.WriteElementString(EDITOR_APPLICATION, editor.Application);
                    xwriter.WriteElementString(EDITOR_ARGUMENTS, editor.Arguments);

                    xwriter.WriteEndElement();
                }

                xwriter.WriteEndElement();
                xwriter.WriteEndElement();
                xwriter.WriteEndDocument();

                xwriter.Flush();
                xwriter.Close();
            }

            Properties.Settings.Default.Editors = editors.ToString();
        }
    }
}