using AppCustom.Controller.Base;
using AppCustom.Models;
using AppCustom.Views;
using Autodesk.Revit.DB;
using Autodesk.Revit.UI;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Threading;

namespace AppCustom.Controller
{
    public class ControllerViewColor : ViewModelBase
    {
        private UIDocument _uidoc;
        private Document _doc;
        private ViewSetColerFillter _mainview;
        private List<Parameter> _parameter;
        private List<GruopElement> _gruopElements;
        private List<ElementId> _elementIds;
        private FilteredElementCollector _collector;

        public ViewSetColerFillter Mainview
        {
            get
            {
                if (_mainview == null)
                {
                    _mainview = new ViewSetColerFillter() { DataContext = this };
                }
                return _mainview;
            }
            set
            {
                _mainview = value;
                OnPropertyChanged(nameof(Mainview));
            }
        }
        public List<Parameter> Parameters
        {
            get => _parameter;
            set
            {
                _parameter = value;
                OnPropertyChanged(nameof(Parameters));
            }
        }
        public List<GruopElement> GruopsElement
        {
            get => _gruopElements;
            set
            {
                _gruopElements = value;
                OnPropertyChanged(nameof(GruopsElement));
            }
        }
        public List<ElementId> ElementIds
        {
            get => _elementIds;
            set
            {
                _elementIds = value;
                OnPropertyChanged(nameof(ElementIds));
            }
        }
        public List<GetCategoryAndCount> Categories { get; set; } = new List<GetCategoryAndCount>();

        public ControllerViewColor(Document doc, UIDocument uidoc)
        {
            this._doc = doc;
            this._uidoc = uidoc;
            TransactionMethod.TranTransactionRun(ResetTemporary, _doc, "UnHide");
            _collector = new FilteredElementCollector(doc, doc.ActiveView.Id);
            Categories = GetCategories();
            InitializeRelayCommand();
        }

        public RelayCommand<object> SelectionChangedCommand { get; set; }
        public RelayCommand<object> SelectionChangedParameterCommand { get; set; }
        public RelayCommand<object> SelectionListElementsCommand { get; set; }
        public RelayCommand<object> ApplyCommand { get; set; }
        public RelayCommand<object> HideElementCommand { get; set; }
        public RelayCommand<object> UnHideCommand { get; set; }

        private void InitializeRelayCommand()
        {
            SelectionChangedCommand = new RelayCommand<object>(p => true, p => SelectChangeListView());
            SelectionChangedParameterCommand = new RelayCommand<object>(p => true, p => SelectChangeListViewParamter());
            ApplyCommand = new RelayCommand<object>(p => true, p => Apply());
            SelectionListElementsCommand = new RelayCommand<object>(p => true, p => SelectionListElements());
            HideElementCommand = new RelayCommand<object>(p => true, p => HideElement());
            UnHideCommand = new RelayCommand<object>(p => true, p => UnHideAll());
        }

        private List<GetCategoryAndCount> GetCategories()
        {
            CategoryFilter categoriesFilter = new CategoryFilter(_collector);
            ElementFillter elementFilter = new ElementFillter(_collector);

            return categoriesFilter.ListItems()
                .Select(c => new GetCategoryAndCount(c, elementFilter.ListItems()))
                .ToList();
        }

        private System.Windows.Media.Color GetRandomColor(Random random)
        {
            byte[] colorBytes = new byte[3];
            random.NextBytes(colorBytes);
            return System.Windows.Media.Color.FromRgb(colorBytes[0], colorBytes[1], colorBytes[2]);
        }

        private void SelectChangeListView()
        {
            this._mainview.lvParameters.SelectedItem = null;
            var getcategory = this._mainview.lvCategory?.SelectedItem as GetCategoryAndCount;
            this._mainview.lvParameters.SelectedIndex = 0;
            Parameters = getcategory?.ElementAll.GetParameters();
        }

        private void SelectChangeListViewParamter()
        {
            var parameter = this._mainview.lvParameters?.SelectedItem as Parameter;
            if (parameter != null)
            {
                var getcategory = this._mainview.lvCategory?.SelectedItem as GetCategoryAndCount;
                var NewEleme = new List<GruopElement>();
                var parameters = getcategory.ElementAll
                    .Select(el => el?.LookupParameter(parameter.Definition.Name)?.AsValueString())
                    .Where(p => p != null)
                    .Distinct()
                    .ToList();

                var random = new Random();
                NewEleme.AddRange(parameters
                    .Select(p => new GroupElementBuilder().SetValueParameter(p).SetBackground(GetRandomColor(random)).SetElements(getcategory.ElementAll, parameter).Build()));

                GruopsElement = NewEleme;
            }
        }
        //private async void SelectChangeListViewParamter()
        //{
        //    var parameter = this._mainview.lvParameters?.SelectedItem as Parameter;
        //    if (parameter != null)
        //    {
        //        var getcategory = this._mainview.lvCategory?.SelectedItem as GetCategoryAndCount;

        //        // Hiển thị thể hiện của ProgressBar
        //        this._mainview.ProgressWindow.IsIndeterminate = true;

        //        try
        //        {
        //            // Tải dữ liệu một cách bất đồng bộ
        //            var parameters = await Task.Run(() =>
        //            {
        //                return getcategory.ElementAll
        //                    .Select(el => el?.LookupParameter(parameter.Definition.Name)?.AsValueString())
        //                    .Where(p => p != null)
        //                    .Distinct()
        //                    .ToList();
        //            });

        //            // Tắt ProgressBar sau khi dữ liệu đã được tải xong
        //            this._mainview.ProgressWindow.IsIndeterminate = false;

        //            var NewEleme = new List<GruopElement>();
        //            var random = new Random();
        //            NewEleme.AddRange(parameters
        //                .Select(p => new GroupElementBuilder().SetValueParameter(p).SetBackground(GetRandomColor(random)).SetElements(getcategory.ElementAll, parameter).Build()));

        //            GruopsElement = NewEleme;
        //        }
        //        catch (Exception ex)
        //        {
        //            MessageBox.Show("Error occurred: " + ex.Message);
        //        }
        //    }
        //}

        private void Apply()
        {
            using (TransactionGroup gr = new TransactionGroup(_doc, "Group Filter"))
            {
                this._mainview.btnApply.IsEnabled = false;
                gr.Start();
                TransactionMethod.TranTransactionRun(() =>
                {
                    double value = 0;
                    this._mainview.ProgressWindow.Maximum = GruopsElement.Sum(g => g.Elements.Count);
                    foreach (GruopElement grelement in GruopsElement)
                    {
                        foreach (ElementByFilter el in grelement.Elements)
                        {
                            if (el != null)
                            {
                                SetColorElement(el);
                                value++;
                                this._mainview.ProgressWindow.Dispatcher?.Invoke(() => { this._mainview.ProgressWindow.Value = value; }, DispatcherPriority.Background);
                            }
                        }
                    }
                }, _doc, "Set Color");

                MessageBox.Show("Done!");
                this._mainview.btnApply.IsEnabled = true;
                gr.Assimilate();
            }
        }

        private void SelectionListElements()
        {
            var selected = this._mainview.lvElements?.SelectedItem as GruopElement;
            if (selected != null)
            {
                var elements = selected?.Elements?.Select(x => x?.Id).ToList();
                ElementIds = elements;
                using (Transaction tr = new Transaction(_doc, "Set Element ID"))
                {
                    tr.Start();
                    _uidoc.Selection.SetElementIds(elements);
                    _uidoc.ShowElements(elements);
                    tr.Commit();
                }
            }
        }

        private void HideElement()
        {
            if (ElementIds != null)
                TransactionMethod.TranTransactionRun(HideElementsTemporary, ElementIds, _doc, "Hide Element");
        }

        private void UnHideAll()
        {
            TransactionMethod.TranTransactionRun(ResetTemporary, _doc, "UnHide");
        }

        private void ResetTemporary()
        {
            Autodesk.Revit.DB.View view = _doc.ActiveView;
            if (view.IsTemporaryHideIsolateActive())
            {
                view.DisableTemporaryViewMode(TemporaryViewMode.TemporaryHideIsolate);
            }
        }

        private void HideElementsTemporary(List<ElementId> elementIds)
        {
            _doc.ActiveView.IsolateElementsTemporary(elementIds);
        }

        private void SetColorElement(ElementByFilter e)
        {
            _doc.ActiveView.SetElementOverrides(e.Id, new SetOptionColor(_doc, e.Color));
        }
    }
}
