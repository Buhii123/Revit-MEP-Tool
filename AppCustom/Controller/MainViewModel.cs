using AppCustom.Models;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AppCustom.Utils;
using Autodesk.Revit.UI;
using System.Windows;

namespace AppCustom.ViewModels
{
    public class MainViewModel : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;

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
                PathNodes.Add(CreateNode(path, Path.GetFileName(path), ExplorerType.Directory));
            }
        }

        private void AddPathNodes(IEnumerable<string> paths)
        {
            foreach (var path in paths)
            {
                AddPathNode(path);
            }
        }

        // Constructor accepting a single path
        public MainViewModel(string path) : this(new List<string> { path })
        {

        }

        // Constructor accepting multiple paths
        public MainViewModel(IEnumerable<string> paths)
        {
            PathNodes = new ObservableCollection<LazyTreeNode>();
            AddPathNodes(paths);
        }

      
        public ObservableCollection<LazyTreeNode> PathNodes { get; set; }
    }
}
