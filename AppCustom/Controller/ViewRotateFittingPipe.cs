using AppCustom.Controller.Base;
using Autodesk.Revit.UI;
using AppCustom.Views;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AppCustom.ExternelEventHandler;
using Autodesk.Revit.DB;
using System.Windows;
using System.Globalization;
using System.Windows.Controls;

namespace AppCustom.Controller
{
    public class ViewRotateFittingPipe : ViewModelBase
    {
        private RoateView _mainview;
        private string input;
        public string Input {
            get
            {
                return input;
            }
            set
            {
                input = value;
                OnPropertyChanged(nameof(Input));
            }
        }
        public RoateView Mainview
        {
            get
            {
                if (_mainview == null)
                {
                    _mainview = new RoateView { DataContext = this };
                }
                return _mainview;
            }
            set
            {
                _mainview = value;
                OnPropertyChanged(nameof(Mainview));
            }
        }
        private ICollection<ElementId> fittingId { get; }
        private Line direc { get; }
        private Document doc { get; }
        public ViewRotateFittingPipe(Document doc, ICollection<ElementId> fittingId, Line direc)
        {
      
            this.direc= direc;
            this.fittingId= fittingId;
            this.doc = doc;
            InitializeRelayCommand();
        }
        //command
        public RelayCommand<object> RightRotate { get; set; }
        public RelayCommand<object> LeftRotate { get; set; }

        private void InitializeRelayCommand()
        {
            RightRotate = new RelayCommand<object>(p => true, p => ButtonRotateRight());
            LeftRotate = new RelayCommand<object>(p => true, p => ButtonRotateLeft());


        }
        private void ButtonRotateRight()
        {
           
            double degree = 0;

            this.Input = this._mainview.textBox.Text;
            if (_mainview != null)
            {
                if (double.TryParse(this.Input, out double result))
                {
                    degree = result;
                }
                else
                {
                    MessageBox.Show("Chuyển đổi không thành công. Vui lòng nhập một số hợp lệ.");
                }
            }
            double radian = degree * Math.PI / 180; // Chuyển đổi sang radian

            using (Transaction tran = new Transaction(doc, "Rotate fitting Right"))
            {
                tran.Start();
                ElementTransformUtils.RotateElements(doc,this.fittingId, this.direc, radian);
                // doc.GetElement(fittingId).Location.Rotate(direc, -radian);
                tran.Commit();
            }
        }
        private void ButtonRotateLeft()
        {
            double degree = 0;

            this.Input = this._mainview.textBox.Text;
            if (_mainview != null)
            {
                if (double.TryParse(this.Input, out double result))
                {
                    degree = result;
                }
                else
                {
                    MessageBox.Show("Chuyển đổi không thành công. Vui lòng nhập một số hợp lệ.");
                }
            }
            double radian = degree * Math.PI / 180; // Chuyển đổi sang radian

            using (Transaction tran = new Transaction(doc, "Rotate fitting Right"))
            {
                tran.Start();
                ElementTransformUtils.RotateElements(doc,this.fittingId, this.direc, -radian);
               // doc.GetElement(fittingId).Location.Rotate(direc, radian);
                tran.Commit();
            }
        }
    }
}
