using FirmaHandlowaHome.Workers;
using Soneta.Business;
using Soneta.Handel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

[assembly: Worker(typeof(DocumentRabatPositionWorker), typeof(DokumentHandlowy))]
namespace FirmaHandlowaHome.Workers
{
    public class DocumentRabatPositionWorker
    {
        [Context]
        public Session Session { get; set; }

        [Context]
        public DokumentHandlowy DokumentHandlowy { get; set; }

        [Action("Ustaw rabat",
          CommandStyle = Soneta.Commands.CommandStyle.Green,
          Icon = ActionIcon.Book,
          Target = ActionTarget.ToolbarWithText,
          Mode = ActionMode.SingleSession | ActionMode.OnlyForm | ActionMode.ConfirmFinished)]

        public string DocumentRabatAction()
        {
            using (ITransaction transaction = Session.Logout(true))
            {
               foreach(PozycjaDokHandlowego pozycja in DokumentHandlowy.Pozycje) 
                {
                    pozycja.Cena = pozycja.Cena * 1.2;
                }
                transaction.CommitUI();
            }
            return "Zwiększono cenę wszystkich pozycji o 20%";
        }

        public bool IsEnabledDocumentRabatAction()
        {
            if (DokumentHandlowy.Bufor)
            {
                return true;
            }
            return false;
        }
    }
}
