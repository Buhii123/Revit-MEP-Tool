using AppCustom.Controller.Base;
using AppCustom.Views;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using static Autodesk.Revit.DB.SpecTypeId;

namespace AppCustom.Controller
{
    public class ViewSetting : ViewModelBase
    {
        private ViewSettingDownUp _mainview;
        public ViewSettingDownUp Mainview
        {
            get
            {
                if (_mainview == null)
                {
                    _mainview = new ViewSettingDownUp { DataContext = this };
                }
                return _mainview;
            }
            set
            {
                _mainview = value;
                OnPropertyChanged(nameof(Mainview));
            }
        }
    
        public int OffsetValue { get; private set; }
        public RelayCommand<object> AppLyCommand { get; set; }

        public ViewSetting()
        {
            OffsetValue = 500;
            InitializeRelayCommand();
        }
       private void InitializeRelayCommand()
        {
            AppLyCommand = new RelayCommand<object>(p => true, p => RunSetup());

        }

       
        private void RunSetup()
        {
            string input = this._mainview.OffsetTextBox.Text;
            if (_mainview != null) {
                if (int.TryParse(input, out int result))
                {
                    OffsetValue = result;
                }
                else
                {
                    MessageBox.Show("Chuyển đổi không thành công. Vui lòng nhập một số hợp lệ.");
                }
            }
          
            this._mainview.Close();
        }
    }
}
