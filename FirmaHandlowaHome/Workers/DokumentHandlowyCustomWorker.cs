using FirmaHandlowaHome.Workers;
using Soneta.Business;
using Soneta.Handel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

[assembly: Worker(typeof(DokumentHandlowyCustomWorker),typeof(DokumentHandlowy))]

namespace FirmaHandlowaHome.Workers
{
    public class DokumentHandlowyCustomWorker
    {
        [Context]
        public Session Session { get; set; }

        [Context]
        public DokumentHandlowy DokumentHandlowy { get; set; }

        [Action("Uzupełnij dane przesyłki",
            CommandStyle = Soneta.Commands.CommandStyle.Red,
            Icon = ActionIcon.Database,
            Target = ActionTarget.ToolbarWithText,
            Mode = ActionMode.SingleSession | ActionMode.OnlyForm | ActionMode.ConfirmFinished)]
        public void DocumentAction()
        {
            using (ITransaction transaction = Session.Logout(true))
            {
                DokumentHandlowy.Features["NazwaKuriera"] = "DPD";
                DokumentHandlowy.Features["RodzajKuriera"] = "Standardowo";

                DokumentHandlowy.Przesylka.NumerListu = "DP2131234453123";
                DokumentHandlowy.Przesylka.IdPrzesylki = "12345";

                transaction.CommitUI();
            }
        }

        [Action("Uzupełnij dane przesyłki 2",
           CommandStyle = Soneta.Commands.CommandStyle.Red,
           Icon = ActionIcon.Delete,
           Target = ActionTarget.ToolbarWithText,
           Mode = ActionMode.SingleSession | ActionMode.OnlyForm | ActionMode.ConfirmFinished)]
        public string DocumentAction2()
        {
            // Ustawianie numeru obcego dla dokuemntu handlowego
            using (ITransaction transaction = Session.Logout(true))
            {
                DokumentHandlowy.Features["NazwaKuriera"] = "DPD";
                DokumentHandlowy.Features["RodzajKuriera"] = "Standardowo";

                DokumentHandlowy.Przesylka.NumerListu = "DP2131234453123";
                DokumentHandlowy.Przesylka.IdPrzesylki = "12345";

                transaction.CommitUI();
            }
            return $"Dane przesyłki uzupełnione dla dokumentu {DokumentHandlowy.Numer.NumerPelny}";
        }

        public bool IsVisibleDocumentAction()
        {
            if(DokumentHandlowy.Definicja.Symbol == "ZO" || DokumentHandlowy.Definicja.Symbol == "OO")
            {
                return true;
            }
            return false;   
        }

        public bool IsEnabledDocumentAction()
        {
            if (DokumentHandlowy.Definicja.Symbol == "ZO")
            {
                return true;
            }
            return false;
        }
    }
}
