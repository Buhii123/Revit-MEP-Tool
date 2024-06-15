using Autodesk.Revit.UI;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AppCustom.Commands
{
    public class MyExternalEventHandler : IExternalEventHandler
    {
        private Queue<string> _messages = new Queue<string>();

        public void AddMessage(string message)
        {
            _messages.Enqueue(message);
        }

        public void Execute(UIApplication app)
        {
            while (_messages.Count > 0)
            {
                string message = _messages.Dequeue();
                TaskDialog.Show("External Event", message);
            }
        }

        public string GetName()
        {
            return "My External Event Handler";
        }
    }
}
