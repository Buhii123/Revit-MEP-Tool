using AppCustom.Asset;
using Autodesk.Revit.Attributes;
using Autodesk.Revit.DB;
using Autodesk.Revit.UI;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AppCustom.Commands
{
    [Transaction(TransactionMode.Manual)]
    public  class DaluongCommand : IExternalCommand
    {
        private ExternalEvent _exEvent;
        private MyExternalEventHandler _handler;
        public  Result Execute(ExternalCommandData commandData, ref string message, ElementSet elements)
        {
            TaskDialog.Show("External Event", ResourceAssembly.GetNameNames());
            // Tạo và đăng ký External Event nếu chưa tạo
            if (_handler == null)
            {
                _handler = new MyExternalEventHandler();
                _exEvent = ExternalEvent.Create(_handler);
            }

            // Khởi động các Task để thực hiện tính toán phức tạp
            Task[] tasks = new Task[3];
            tasks[0] = Task.Run(() => DoSomeWork(1));
            tasks[1] = Task.Run(() => DoSomeWork(2));
            tasks[2] = Task.Run(() => DoSomeWork(3));

            // Khi tất cả các Task hoàn thành, kích hoạt External Event
            Task.WhenAll(tasks).ContinueWith(t =>
            {
                // Kích hoạt External Event từ luồng chính của Revit
                _handler.AddMessage("All tasks completed.");
                _exEvent.Raise();
            });

            return Result.Succeeded;
        }
        private async Task DoSomeWork(int taskNumber)
        {
            // Một số tính toán hoặc xử lý dữ liệu
            await Task.Delay(3000 * taskNumber); // Giả lập tính toán phức tạp

            // Gửi thông báo sau khi hoàn thành
            _handler.AddMessage($"Task {taskNumber} completed.");
            _exEvent.Raise();
        }

    }
}
