using AppCustom.Controller.Base;
using AppCustom.Views;
using Autodesk.Revit.UI;
using Autodesk.Revit.DB;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Autodesk.Revit.DB.Plumbing;
using System.Collections.ObjectModel;
using static Autodesk.Revit.DB.SpecTypeId;
using System.Windows;

namespace AppCustom.Controller
{
    public class ViewSetPipeInsution : ViewModelBase
    {
        private UIDocument uidoc;
        private Document doc;
        private MainWindows _mainview;
        private ObservableCollection<string> _comboBoxItems;

        public ObservableCollection<string> ComboBoxItems
        {
            get { return _comboBoxItems; }
            set
            {
                _comboBoxItems = value;
                OnPropertyChanged(nameof(ComboBoxItems));
            }
        }

        public MainWindows Mainview
        {
            get
            {
                if (_mainview == null)
                {
                    _mainview = new MainWindows() { DataContext = this };
                }
                return _mainview;
            }
            set
            {
                _mainview = value;
                OnPropertyChanged(nameof(Mainview));
            }
        }
        public string GetNameInsu { get;private set;}
        public double GetThin{ get; private set; }


        public RelayCommand<object> RunCommand { get; set; }


        public ViewSetPipeInsution (Document _doc, UIDocument _uidoc) 
        {
            this.doc = _doc;
            this.uidoc = _uidoc;
            var collector = new FilteredElementCollector(doc)
                                                .OfClass(typeof(PipeInsulationType))
                                                .Cast<PipeInsulationType>();
            ComboBoxItems = new ObservableCollection<string>();


            foreach (PipeInsulationType item in collector) 
            {
                ComboBoxItems.Add(item.Name);
            }

            InitializeRelayCommand();
        }
        private void InitializeRelayCommand()
        {
            RunCommand = new RelayCommand<object>(p => true, p => RunSetup());
           
        }

        private void RunSetup()
        {
            GetNameInsu = this._mainview.comboBox.Text;
            string input = this._mainview.textBox.Text;
            if (double.TryParse(input, out double result))
            {
                GetThin=result / 304.8;
            }
            else
            {
                MessageBox.Show("Chuyển đổi không thành công. Vui lòng nhập một số hợp lệ.");
            }
            this._mainview.Close();
        }
    }
}
