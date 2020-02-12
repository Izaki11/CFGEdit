using System;
using System.Collections.Generic;
using System.Text;

namespace CfgEdit
{
    public class Parameter
    {
        public string m_name { get; private set; }
        public string m_syntax { get; private set; }
        public string m_description { get; private set; }
        public string m_toolTipFormatted { get; private set; }

        public Parameter(string name, string syntax, string description)
        {
            m_name = name;
            m_syntax = syntax;
            m_description = description;
            m_toolTipFormatted = $"Syntax: {m_syntax}\n\nDescription: {m_description}";
        }
    }
}
