using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.IO;
using Microsoft.Win32;

namespace CfgEdit
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        ObservableCollection<Game> gameList = new ObservableCollection<Game>();
        string baseDirectory = $@"{ Directory.GetCurrentDirectory() }/Modules";
        
        public MainWindow()
        {
            InitializeComponent();

            gameComboBox.ItemsSource = gameList;

            InitGameList();
        }

        private void InitGameList()
        {

            if (Directory.Exists(baseDirectory))
            {
                string[] directories = Directory.GetDirectories(baseDirectory, "*", SearchOption.TopDirectoryOnly);

                foreach (string s in directories)
                {
                    Game game = new Game(System.IO.Path.GetFileName(s), s);
                    gameList.Add(game);

                    string[] moduleFiles = Directory.GetFiles(s, "*.cfgm", SearchOption.TopDirectoryOnly);

                    foreach (string m in moduleFiles)
                    {
                        Module module = new Module(System.IO.Path.GetFileNameWithoutExtension(m), m);
                        game.AddModule(module);
                    }
                }
            }
        }

        private void ClearAll()
        {
            parameterComboBox.SelectedIndex = 0;
            valueTextBox.Clear();
            rawTextBox.Clear();
        }

        private void LoadFile()
        {
            OpenFileDialog openFileDialog = new OpenFileDialog();
            openFileDialog.Filter = "Config Files (*.cfg)|*.cfg|All Files (*.*)|*.*";
            if (openFileDialog.ShowDialog() == true)
            {
                ClearAll();
                rawTextBox.Text = File.ReadAllText(openFileDialog.FileName);
            }
        }

        private void SaveFile()
        {
            SaveFileDialog saveFileDialog = new SaveFileDialog();
            saveFileDialog.Filter = "Config Files (*.cfg)|*.cfg|All Files (*.*)|*.*";
            if (saveFileDialog.ShowDialog() == true)
            {
                File.WriteAllText(saveFileDialog.FileName, rawTextBox.Text);
            }
        }

        private void NewGameButton_Click(object sender, RoutedEventArgs e)
        {
            string directory = $@"{ baseDirectory }/New Game";

            if (Directory.Exists(directory))
            {
                int fileCount = 1;
                string newDirectory;

                do
                {
                    fileCount++;
                    newDirectory = $@"{ directory }({ fileCount.ToString() })";

                } while (Directory.Exists(newDirectory));

                Game newGame = new Game($@"New Game({ fileCount.ToString() })", newDirectory);
                gameList.Add(newGame);
                Directory.CreateDirectory(newDirectory);
            }
            else
            {
                Game newGame = new Game("New Game", directory);
                gameList.Add(newGame);
                Directory.CreateDirectory(directory);
             }
        }

        private void DeleteGameButton_Click(object sender, RoutedEventArgs e)
        {
            if (gameComboBox.SelectedIndex == -1)
            {
                return;
            }
            //Might be a copy instead of a reference, exercise caution
            Game gameToDelete = gameList.ElementAt(gameComboBox.SelectedIndex);
            Directory.Delete(gameToDelete.m_directory, true);
            gameList.Remove(gameToDelete); 
        }

        private void GameComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (gameComboBox.SelectedIndex == -1)
            {
                return;
            }

            moduleComboBox.ItemsSource = gameList.ElementAt(gameComboBox.SelectedIndex).GetModules();
        }

        private void ModuleComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (moduleComboBox.SelectedIndex == -1)
            {
                parameterComboBox.ItemsSource = null;
                return;
            }

            parameterComboBox.ItemsSource = gameList.ElementAt(gameComboBox.SelectedIndex).GetModules().ElementAt(moduleComboBox.SelectedIndex).GetParameters();

            if (parameterComboBox.HasItems)
            {
                parameterComboBox.SelectedIndex = 0;
            }
        }

        private void ValueAddButton_Click(object sender, RoutedEventArgs e)
        {
            if (valueTextBox.Text.Length == 0)
            {
                return;
            }

            rawTextBox.AppendText($"{ valueTextBox.Text }\n");
            valueTextBox.Clear();
        }

        private void ValueClearButton_Click(object sender, RoutedEventArgs e)
        {
            valueTextBox.Clear();
        }

        private void ParameterComboBoxItem_PreviewMouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            if (parameterComboBox.SelectedIndex == -1)
            {
                return;
            }

            Parameter parameter = (Parameter)((TextBlock)sender).DataContext;
            string text = parameter.m_syntax;
            valueTextBox.AppendText(text);
        }

        private void parameterComboBoxItem_PreviewMouseRightButtonUp(object sender, MouseButtonEventArgs e)
        {
            if (parameterComboBox.SelectedIndex == -1)
            {
                return;
            }

            Parameter parameter = (Parameter)((TextBlock)sender).DataContext;
            string text = parameter.m_name;
            valueTextBox.AppendText(text);
        }

        private void MenuNewButton_Click(object sender, RoutedEventArgs e)
        {
            MessageBoxResult confirmed = MessageBox.Show("Unsaved work will be lost, continue?", "CFGEdit - New", MessageBoxButton.YesNo, MessageBoxImage.Exclamation, MessageBoxResult.No);

            if (confirmed == MessageBoxResult.Yes)
            {
                ClearAll();
            }
            if (confirmed == MessageBoxResult.No)
            {
                //The user clicked no
            }
        }

        private void MenuOpenButton_Click(object sender, RoutedEventArgs e)
        {
            MessageBoxResult confirmed = MessageBox.Show("Unsaved work will be lost, continue?", "CFGEdit - Open", MessageBoxButton.YesNo, MessageBoxImage.Exclamation, MessageBoxResult.No);

            if (confirmed == MessageBoxResult.Yes)
            {
                LoadFile();
            }
            if (confirmed == MessageBoxResult.No)
            {
                //The user clicked no
            }
        }

        private void MenuSaveButton_Click(object sender, RoutedEventArgs e)
        {
            SaveFile();
        }

        private void MenuQuitButton_Click(object sender, RoutedEventArgs e)
        {
            MessageBoxResult confirmed = MessageBox.Show("Unsaved work will be lost, continue?", "CFGEdit - Quit", MessageBoxButton.YesNo, MessageBoxImage.Exclamation, MessageBoxResult.No);

            if (confirmed == MessageBoxResult.Yes)
            {
                Application.Current.Shutdown();
            }
            if (confirmed == MessageBoxResult.No)
            {
                //The user clicked no
            }
        }

        private void MenuAboutButton_Click(object sender, RoutedEventArgs e)
        {
            string aboutMessage = 
                "CFGEdit\n" +
                "v1.0";

            MessageBox.Show(aboutMessage, "CFGEdit - About", MessageBoxButton.OK, MessageBoxImage.Information);
        }
    }
}
