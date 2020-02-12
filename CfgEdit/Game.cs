using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text;

namespace CfgEdit
{
    public class Game
    {
        public string m_name { get; private set; }
        public string m_directory { get; private set; }
        private ObservableCollection<Module> m_moduleList;

        public Game(string name, string directory)
        {
            m_name = name;
            m_directory = directory;
            m_moduleList = new ObservableCollection<Module>();
        }

        public void AddModule(Module module)
        {
            m_moduleList.Add(module);
        }

        public void RemoveModule(Module module)
        {
            m_moduleList.Remove(module);
        }

        public void RemoveModuleAt(int index)
        {
            m_moduleList.RemoveAt(index);
        }

        public ObservableCollection<Module> GetModules()
        {
            return m_moduleList;
        }

    }
}
