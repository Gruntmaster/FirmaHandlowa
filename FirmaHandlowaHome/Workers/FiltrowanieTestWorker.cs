using FirmaHandlowaHome.Workers;
using Soneta.Business;
using Soneta.Handel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

[assembly: Worker(typeof(FiltrowanieTestWorker), typeof(DokumentHandlowy))]

namespace FirmaHandlowaHome.Workers
{
    public class FiltrowanieTestWorker
    {
        [Context]
        public Session Session { get; set; }    

        [Action("Filtrowanie test",
           Icon = ActionIcon.Wizard,
           Target = ActionTarget.ToolbarWithText,
           Mode = ActionMode.SingleSession | ActionMode.ConfirmFinished)]
        public void DocumentAction()
        {

        }
    }
}
