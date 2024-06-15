using AppCustom.Commands;
using Autodesk.Revit.DB;
using Autodesk.Revit.DB.Plumbing;
using Autodesk.Revit.UI;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
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
using Excel = Microsoft.Office.Interop.Excel;
using System.Runtime.InteropServices;

namespace AppCustom.Views
{
    
    public partial class ViewSettingInsulation : Window
    {
        private Document doc;
        string fileNameProject;
        private List<string> PipeTypes= new List<string>();
        private List<string> SytemTypes = new List<string>();
        private List<string> InsulationTypes = new List<string>();
        private ObservableCollection<GetInfoCheckInsulationPipe> InfoItems { get; set; }
        public ObservableCollection<GetInfoCheckInsulationPipe> infoItems { get; private set; }

        


        public ViewSettingInsulation(Document doc)
        {
            this.doc = doc;
            this.fileNameProject = doc.Title;
            InitializeComponent();

            InfoItems = new ObservableCollection<GetInfoCheckInsulationPipe>();
            PipeTypes = CalculateRevit.GetAllPipeTypeNames(doc);
            SytemTypes= CalculateRevit.GetAllPipingSystems(doc);
            InsulationTypes= CalculateRevit.GetAllInsulationPipeTypes(doc);

            ComboBox1.ItemsSource= PipeTypes; 
            ComboBox2.ItemsSource= SytemTypes;
            ComboBox3.ItemsSource = InsulationTypes;

            DataGridItems.ItemsSource = InfoItems;
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            // Kiểm tra các ComboBox đã chọn giá trị chưa
            if (ComboBox1.SelectedItem != null &&
                ComboBox2.SelectedItem != null &&
                ComboBox3.SelectedItem != null &&
                !string.IsNullOrEmpty(txtbox1.Text) &&
                !string.IsNullOrEmpty(txtbox2.Text) &&
                !string.IsNullOrEmpty(txtbox3.Text))
            {
                var selectedItem1 = ComboBox1.SelectedItem.ToString();
                var selectedItem2 = ComboBox2.SelectedItem.ToString();
                var selectedItem3 = ComboBox3.SelectedItem.ToString();
                var newItem = new GetInfoCheckInsulationPipe
                {
                    PipeType = selectedItem1,
                    SytemPipe = selectedItem2,
                    InsulationType = selectedItem3,
                    From = txtbox1.Text,
                    To = txtbox2.Text,
                    thickness = txtbox3.Text 
                };

                bool itemExists = InfoItems.Any(item =>
                        item.PipeType == newItem.PipeType &&
                        item.SytemPipe == newItem.SytemPipe &&
                        item.InsulationType == newItem.InsulationType &&
                        item.From == newItem.From &&
                        item.To == newItem.To)
                       ;
                if (!itemExists)
                {
                    InfoItems.Add(newItem);

                    // Đặt lại các ComboBox về trạng thái ban đầu
                    ComboBox1.SelectedIndex = 1;
                    ComboBox2.SelectedIndex = 1;
                    ComboBox3.SelectedIndex = 1;
                    txtbox1.Text = "";
                    txtbox2.Text = "";
                    txtbox3.Text = "";
                }
                else
                {
                    MessageBox.Show("The item already exists in the grid.", "Duplicate Item", MessageBoxButton.OK, MessageBoxImage.Information);
                }
            }
            else
            {
                MessageBox.Show("Please select an item from each ComboBox.", "Input Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        // Xử lý sự kiện Click của Button
        // Xử lý sự kiện Click của Button để xóa mục được chọn
        private void DeleteButton_Click(object sender, RoutedEventArgs e)
        {
            // Lấy đối tượng ListItem từ thuộc tính Tag của nút
            if (sender is Button button && button.Tag is GetInfoCheckInsulationPipe selectedItem)
            {
                InfoItems.Remove(selectedItem);
            }
            else
            {
                MessageBox.Show("Unable to delete the item.", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        // Xử lý sự kiện Click của Button để xóa tất cả các mục
        private void DeleteAllButton_Click(object sender, RoutedEventArgs e)
        {
            if (InfoItems.Any())
            {
                var result = MessageBox.Show("Are you sure you want to delete all items?", "Confirmation", MessageBoxButton.YesNo, MessageBoxImage.Warning);
                if (result == MessageBoxResult.Yes)
                {
                    InfoItems.Clear();
                }
            }
            else
            {
                MessageBox.Show("There are no items to delete.", "Information", MessageBoxButton.OK, MessageBoxImage.Information);
            }
        }

        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
           
           
            this.infoItems = this.InfoItems;

            this.Close();
        }
        private void TextBox_PreviewTextInput(object sender, TextCompositionEventArgs e)
        {
            // Chỉ cho phép nhập số
            e.Handled = !IsTextAllowed(e.Text);
        }

        private static readonly Regex _regex = new Regex("[^0-9]+");

        private static bool IsTextAllowed(string text)
        {
            return !_regex.IsMatch(text);
        }
        // Lớp để biểu diễn một mục trong ListView



        private void LoadButton_Click(object sender, RoutedEventArgs e)
        {
            var openFileDialog = new Microsoft.Win32.OpenFileDialog
            {
                Filter = "Excel Files|*.xlsx",
                Title = "Select an Excel File"
            };

            if (openFileDialog.ShowDialog() == true)
            {
                string filePath = openFileDialog.FileName;
                LoadDataFromExcel(filePath);
            }
        }

        private void SaveButton_Click(object sender, RoutedEventArgs e)
        {
            var saveFileDialog = new Microsoft.Win32.SaveFileDialog
            {
                Filter = "Excel Files|*.xlsx",
                Title = "Save an Excel File",
                FileName = fileNameProject+".xlsx"
            };

            if (saveFileDialog.ShowDialog() == true)
            {
                string filePath = saveFileDialog.FileName;
                SaveDataToExcel(filePath);
            }
        }

        private void LoadDataFromExcel(string filePath)
        {
            Excel.Application excelApp = new Excel.Application();
            Excel.Workbook workbook = excelApp.Workbooks.Open(filePath);
            Excel.Worksheet worksheet = workbook.Worksheets[1]; // Chọn worksheet đầu tiên

            int rowCount = worksheet.UsedRange.Rows.Count;

            InfoItems.Clear();
            for (int row = 2; row <= rowCount; row++)
            {
                var item = new GetInfoCheckInsulationPipe
                {
                    PipeType = worksheet.Cells[row, 1].Value.ToString(),
                    SytemPipe = worksheet.Cells[row, 2].Value.ToString(),
                    From = worksheet.Cells[row, 3].Value.ToString(),
                    To = worksheet.Cells[row, 4].Value.ToString(),
                    InsulationType = worksheet.Cells[row, 5].Value.ToString(),
                    thickness = worksheet.Cells[row, 6].Value.ToString()
                };
                InfoItems.Add(item);
            }

            workbook.Close();
            excelApp.Quit();

            Marshal.ReleaseComObject(worksheet);
            Marshal.ReleaseComObject(workbook);
            Marshal.ReleaseComObject(excelApp);

            
        }


        private void SaveDataToExcel(string filePath)
        {
            Excel.Application excelApp = new Excel.Application();
            Excel.Workbook workbook = excelApp.Workbooks.Add();
            Excel.Worksheet worksheet = workbook.Worksheets[1]; // Chọn worksheet đầu tiên

            worksheet.Cells[1, 1].Value = "Pipe Type";
            worksheet.Cells[1, 2].Value = "System Type";
            worksheet.Cells[1, 3].Value = "From";
            worksheet.Cells[1, 4].Value = "To";
            worksheet.Cells[1, 5].Value = "Insulation Type";
            worksheet.Cells[1, 6].Value = "Thickness";

            int rowIndex = 2;
            foreach (var item in InfoItems)
            {
                worksheet.Cells[rowIndex, 1].Value = item.PipeType;
                worksheet.Cells[rowIndex, 2].Value = item.SytemPipe;
                worksheet.Cells[rowIndex, 3].Value = item.From;
                worksheet.Cells[rowIndex, 4].Value = item.To;
                worksheet.Cells[rowIndex, 5].Value = item.InsulationType;
                worksheet.Cells[rowIndex, 6].Value = item.thickness;

                rowIndex++;
            }

            workbook.SaveAs(filePath);
            workbook.Close();
            excelApp.Quit();

            Marshal.ReleaseComObject(worksheet);
            Marshal.ReleaseComObject(workbook);
            Marshal.ReleaseComObject(excelApp);

            MessageBox.Show("Data saved Thành Công.", "Thành Công", MessageBoxButton.OK, MessageBoxImage.Information);
        }



    }

}
