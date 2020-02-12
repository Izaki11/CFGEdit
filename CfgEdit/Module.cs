using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text;
using System.IO;

namespace CfgEdit
{
    public class Module
    {
        public string m_name { get; private set; }
        public string m_directory { get; private set; }
        private ObservableCollection<Parameter> m_parameterList;

        public Module(string name, string directory)
        {
            m_name = name;
            m_directory = directory;
            m_parameterList = new ObservableCollection<Parameter>();
            InitModule();
        }

        public void InitModule()
        {
            string[] fileLines = File.ReadAllLines(m_directory);

            foreach (string line in fileLines)
            {
                line.Trim();

                if (line.StartsWith("//") || String.IsNullOrWhiteSpace(line))
                {
                    continue;
                }

                string[] splitLines = line.Split(";");
                Parameter parameter = new Parameter(splitLines[0], splitLines[1], splitLines[2]);
                m_parameterList.Add(parameter);
            }
        }

        public ObservableCollection<Parameter> GetParameters()
        {
            return m_parameterList;
        }
    }
}
