using AppCustom.Controller.Base;
using Autodesk.Revit.DB.Mechanical;
using Autodesk.Revit.DB;
using Autodesk.Revit.UI;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AppCustom.Views;
using AppCustom.Models;
using System.Diagnostics;
using System.Windows.Threading;
using System.Windows.Media.Media3D;
using HelixToolkit.Wpf;
using System.Windows;
using System.Windows.Media;

namespace AppCustom.Controller
{
    public class ControlViewSlitDucts : ViewModelBase
    {
        UIDocument uidoc { get; }
        Document doc { get; }
        private ObservableCollection<Duct> _ductsCollection;
        public ObservableCollection<Duct> DuctsCollection
        {
            get { return _ductsCollection; }
            set { _ductsCollection = value; }
        }
        public ObservableCollection<DuctTypeModel> ListductTypes { get; set; }
        #region Main View
        private SlitDuctsView mainview;
        public SlitDuctsView MainView
        {
            get
            {
                if (mainview == null)
                {
                    mainview = new SlitDuctsView() { DataContext = this };
                    _ductsCollection = new ObservableCollection<Duct>();
                    LoadDucts();
                }
                return mainview;
            }
            set
            {
                mainview = value;
                OnPropertyChanged(nameof(MainView));
            }
        }
        #endregion

        public RelayCommand<object> ButtonRun { get; set; }
        public RelayCommand<object> ButtonClose { get; set; }

        public ControlViewSlitDucts(UIApplication uiapp)
        {
            uidoc = uiapp.ActiveUIDocument;
            doc = uidoc.Document;

            List<DuctType> clection = new FilteredElementCollector(doc)
                .OfClass(typeof(DuctType))
                .OfCategory(BuiltInCategory.OST_DuctCurves)
                .Cast<DuctType>()
                .ToList();
            List<Duct> duct = new FilteredElementCollector(doc, doc.ActiveView.Id)
                .OfClass(typeof(Duct))
                .OfCategory(BuiltInCategory.OST_DuctCurves)
                .WhereElementIsNotElementType()
                .Cast<Duct>()
                .ToList();

            ListductTypes = new ObservableCollection<DuctTypeModel>();
            ListductTypes.Add(new DuctTypeModel(duct));
            foreach (DuctType c in clection)
            {
                ListductTypes.Add(new DuctTypeModel(c, duct));
            }
            ButtonRun = new RelayCommand<object>(p => ConditionRun(), p => EventButtonRun());
            ButtonClose = new RelayCommand<object>(p => true, p => { this.mainview.Close(); });
           
            
        }
        public void EventButtonRun()
        {
            double result = double.Parse(mainview.tbSement.Text);
            DuctTypeModel ductype = mainview.cbbDuctType.SelectedItem as DuctTypeModel;

            using (TransactionGroup group = new TransactionGroup(doc, "Split Ducts"))
            {
                group.Start();

                this.mainview.ProgressWindow.Maximum = ductype.ListDuct.Count;
                double value = 0;

                using (Transaction transaction = new Transaction(doc, "Process Ducts"))
                {
                    transaction.Start();

                    ductype.ListDuct.ForEach(d =>
                    {
                        this.mainview.btnOk.IsEnabled = false;
                        this.mainview.btnCanncel.IsEnabled = false;

                        CalculateRevit.SetAllDuct(doc, d, result);

                        this.mainview.ProgressWindow.Dispatcher.Invoke(() =>
                        {
                            this.mainview.ProgressWindow.Value = ++value;
                        }, DispatcherPriority.Background);
                    });

                    transaction.Commit();
                }

                group.Assimilate();
            }

            this.mainview.btnOk.IsEnabled = true;
            this.mainview.btnCanncel.IsEnabled = true;

            TaskDialog.Show("YS", "Success!");
            _ductsCollection.Clear();
            LoadDucts();
        }
       
        public bool ConditionRun()
        {
            if (!string.IsNullOrEmpty(mainview.tbSement.Text))
            {
                return true;
            }
            return false;
        }

        //helix
        private void UpdateHelixViewport()
        {
          




            if (mainview != null && mainview.helixViewport != null)
            {
                mainview.helixViewport.Children.Clear();
                var meshBuilder = new MeshBuilder();

                foreach (var duct in _ductsCollection)
                {
                    GeometryElement geomElement = duct.get_Geometry(new Options());
                    foreach (GeometryObject geomObj in geomElement)
                    {
                        if (geomObj is Solid solid)
                        {
                            foreach (Face face in solid.Faces)
                            {
                                Mesh mesh = face.Triangulate();
                                for (int i = 0; i < mesh.NumTriangles; i++)
                                {
                                    MeshTriangle triangle = mesh.get_Triangle(i);
                                    meshBuilder.AddTriangle(
                                        triangle.get_Vertex(0).ToPoint3D(),
                                        triangle.get_Vertex(1).ToPoint3D(),
                                        triangle.get_Vertex(2).ToPoint3D());
                                }
                            }
                        }
                    }
                }
                // Create a single material for all ducts
                System.Windows.Media.Color color = Colors.Red; // Example: Blue color
                System.Windows.Media.Media3D.Material material = new DiffuseMaterial(new SolidColorBrush(color));

                var meshGeometry = meshBuilder.ToMesh();
                var model = new GeometryModel3D { Geometry = meshGeometry, Material = material };
                mainview.helixViewport.Children.Add(new ModelVisual3D { Content = model });







                //FilteredElementCollector collector = new FilteredElementCollector(doc);
                //collector.OfCategory(BuiltInCategory.OST_DuctFitting);
                //var meshBuilderffitng = new MeshBuilder();
                //// List to store all faces of fittings
                //List<Face> allFaces = new List<Face>();
                //foreach (Element fittingElement in collector)
                //{
                //    // Get geometry of the fitting element
                //    GeometryElement geometryElement = fittingElement.get_Geometry(new Options());

                //    // Iterate through geometry elements
                //    foreach (GeometryObject geomObj in geometryElement)
                //    {
                //        // Check if it's a solid
                //        if (geomObj is Solid solid)
                //        {
                //            // Iterate through faces of the solid
                //            foreach (Face face in solid.Faces)
                //            {
                //                Mesh mesh = face.Triangulate();
                //                for (int i = 0; i < mesh.NumTriangles; i++)
                //                {
                //                    MeshTriangle triangle = mesh.get_Triangle(i);
                //                    meshBuilderffitng.AddTriangle(
                //                        triangle.get_Vertex(0).ToPoint3D(),
                //                        triangle.get_Vertex(1).ToPoint3D(),
                //                        triangle.get_Vertex(2).ToPoint3D());
                //                }
                //            }
                //        }
                //        // If it's not a solid, check if it's a geometry instance
                //        else if (geomObj is GeometryInstance geomInst)
                //        {
                //            // Get the geometry of the instance
                //            GeometryElement instGeometry = geomInst.GetInstanceGeometry();

                //            // Iterate through geometry of the instance
                //            foreach (GeometryObject instGeomObj in instGeometry)
                //            {
                //                if (instGeomObj is Solid instSolid)
                //                {
                //                    // Iterate through faces of the instance solid
                //                    foreach (Face face in instSolid.Faces)
                //                    {
                //                        Mesh mesh = face.Triangulate();
                //                        for (int i = 0; i < mesh.NumTriangles; i++)
                //                        {
                //                            MeshTriangle triangle = mesh.get_Triangle(i);
                //                            meshBuilderffitng.AddTriangle(
                //                                triangle.get_Vertex(0).ToPoint3D(),
                //                                triangle.get_Vertex(1).ToPoint3D(),
                //                                triangle.get_Vertex(2).ToPoint3D());
                //                        }
                //                    }
                //                }
                //            }
                //        }
                //    }

                //}



                //// Create a single material for all ducts
                //System.Windows.Media.Color color2 = Colors.Red; // Example: Blue color
                //System.Windows.Media.Media3D.Material material2 = new DiffuseMaterial(new SolidColorBrush(color2));

                //var meshGeometry2 = meshBuilderffitng.ToMesh();
                //var model2 = new GeometryModel3D { Geometry = meshGeometry2, Material = material2 };
                //mainview.helixViewport.Children.Add(new ModelVisual3D { Content = model2 });
            }
            else
            {
                MessageBox.Show("sadasd");
            }
           

           
        }
        private void LoadDucts()
        {
            var ducts = new FilteredElementCollector(doc)
                            .OfCategory(BuiltInCategory.OST_DuctCurves)
                            .WhereElementIsNotElementType()
                            .ToElements()
                            .Cast<Duct>();

            foreach (var duct in ducts)
            {
                _ductsCollection.Add(duct);
            }

           UpdateHelixViewport();
        }
    }
    public static class RevitExtensions
    {
        public static Point3D ToPoint3D(this XYZ point)
        {
            return new Point3D(point.X, point.Y, point.Z);
        }
    }
}
