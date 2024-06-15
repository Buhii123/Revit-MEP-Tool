
using AppCustom.Asset;
using AppCustom.Commands;
using AppCustom.Models;
using AppCustom.Utils;
using Autodesk.Revit.UI;
using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.IO;
using System.Reflection;
using System.Windows;
using System.Windows.Forms;
using System.Windows.Input;
using System.Windows.Threading;

namespace AppCustom.Views
{
    /// <summary>
    /// Interaction logic for ViewLoadFamily.xaml
    /// </summary>
    public partial class ViewLoadFamily : System.Windows.Controls.UserControl, IDockablePaneProvider, INotifyPropertyChanged

    {
        private const string ResourceName = "AppCustom.data.txt"; // Adjust the namespace and resource name accordingly
        private string PathFileName = CoreAssembly.GetDataFileName(); // Keeping this in case of fallback
        public event PropertyChangedEventHandler PropertyChanged;
        public string paths;
        private ObservableCollection<LazyTreeNode> _pathNodes;
        public ObservableCollection<LazyTreeNode> PathNodes
        {
            get { return _pathNodes; }
            set
            {
                if (_pathNodes != value)
                {
                    _pathNodes = value;
                    OnPropertyChanged(nameof(PathNodes));
                }
            }
        }
        private LoadFamilyExternalCommand loadfamily;
        private ExternalEvent externalEvent;
        private DispatcherTimer timer;

        public ViewLoadFamily(LoadFamilyExternalCommand loadfamily, ExternalEvent externalEvent)
        {
            this.loadfamily = loadfamily;
            this.externalEvent = externalEvent;
            InitializeComponent();
         
            PathNodes = new ObservableCollection<LazyTreeNode>();
            DataContext = this;
            LoadLastPath();
            StartTimer();
        }

        #region Event
        private void LoadButton_Click(object sender, RoutedEventArgs e)
        {
            using (FolderBrowserDialog fbd = new FolderBrowserDialog() { Description = "Select Folder" })
            {
                if (fbd.ShowDialog() == System.Windows.Forms.DialogResult.OK)
                {
                    txtPath.Text = fbd.SelectedPath;
                }
            }
        }

        private void ButtonLoad_Click(object sender, RoutedEventArgs e)
        {
            PathNodes.Clear();
            paths = txtPath.Text;
            SaveLastPath(paths);
            AddPathNode(paths);
        }

        private void FolderTreeView_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            if (FolderTreeView.SelectedItem is LazyTreeNode selectedNode)
            {
                string filePath = selectedNode.Key;
                if (File.Exists(filePath) && System.IO.Path.GetExtension(filePath).Equals(".rfa", StringComparison.OrdinalIgnoreCase))
                {
                    try
                    {
                        string directoryPath = System.IO.Path.GetDirectoryName(filePath);
                        string fileName = System.IO.Path.GetFileName(filePath);
                        loadfamily.familyPath = directoryPath;
                        loadfamily.familyName = fileName;
                        externalEvent.Raise();
                    }
                    catch (Exception ex)
                    {
                        System.Windows.MessageBox.Show($"Error {ex}");
                    }
                }
            }
        }
        #endregion

        private void StartTimer()
        {
            timer = new DispatcherTimer();
            timer.Interval = TimeSpan.FromSeconds(1);
            timer.Tick += Timer_Tick;
            timer.Start();
        }

        private void Timer_Tick(object sender, EventArgs e)
        {
            lbTextNgay.Content = $"{DateTime.Now:dd/MM/yyyy}";
            lbTextGio.Content = $"{DateTime.Now:HH} Giờ {DateTime.Now:mm} Phút {DateTime.Now:ss} Giây";
        }

        private void SaveLastPath(string path)
        {
            // Save to a user-specific location, like AppData
            string userPath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), "data.txt");
            File.WriteAllText(userPath, path); // This will overwrite the file
        }

        private void LoadLastPath()
        {
            string userPath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), "data.txt");
            string lastPath = null;

            // Check if the user-specific file exists
            if (File.Exists(userPath))
            {
                lastPath = File.ReadAllText(userPath);
            }
            else
            {
                // If user-specific file does not exist, read from the embedded resource
                var assembly = Assembly.GetExecutingAssembly();
                using (Stream stream = assembly.GetManifestResourceStream(ResourceName))
                using (StreamReader reader = new StreamReader(stream))
                {
                    lastPath = reader.ReadToEnd();
                }
            }

            if (!string.IsNullOrEmpty(lastPath))
            {
                txtPath.Text = lastPath;
                AddPathNode(lastPath);
            }
        }

        #region CodeMain
        public LazyTreeNode CreateNode(string key, string text, ExplorerType explorerType)
        {
            var images = ToggleImageUtils.GetExplorers(explorerType);
            var node = new LazyTreeNode { Key = key, Text = text };
            node.OnExpanded += Node_OnExpanded;
            node.OpenedImage = images.opendedImage;
            node.ClosedImage = images.closedImage;
            if (DirectoryUtils.IsDirectoryOrFileExists(key))
            {
                node.AddDummyNode();
            }

            return node;
        }

        private void Node_OnExpanded(LazyTreeNode node)
        {
            // Subdirectories
            foreach (var di in DirectoryUtils.GetDirectories(node.Key))
            {
                node.Children.Add(CreateNode(di.FullName, di.Name, ExplorerType.Directory));
            }

            // Files
            foreach (var fi in DirectoryUtils.GetFiles(node.Key))
            {
                node.Children.Add(CreateNode(fi.FullName, fi.Name, ExplorerType.File));
            }
        }

        private void AddPathNode(string path)
        {
            if (Directory.Exists(path) || File.Exists(path))
            {
                PathNodes.Add(CreateNode(path, System.IO.Path.GetFileName(path), ExplorerType.Directory));
            }
        }
        #endregion

        #region
        protected void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        public void SetupDockablePane(DockablePaneProviderData data)
        {
            data.FrameworkElement = this as FrameworkElement;
            data.InitialState = new DockablePaneState()
            {
                DockPosition = DockPosition.Right,
            };
        }
        #endregion
    }

}
