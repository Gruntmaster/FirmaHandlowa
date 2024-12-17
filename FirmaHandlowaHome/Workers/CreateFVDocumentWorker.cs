using FirmaHandlowa.Workers;
using Microsoft.Extensions.DependencyInjection;
using Soneta.Business;
using Soneta.Handel;
using Soneta.Handel.RelacjeDokumentow.Api;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

[assembly: Worker(typeof(CreateFVDocumentWorker), typeof(DokumentHandlowy))]

namespace FirmaHandlowa.Workers
{
    public class CreateFVDocumentWorker
    {
        [Context]
        public Session Session { get; set; }

        [Context]
        public DokumentHandlowy DokumentHandlowy { get; set; }

        [Action("Zatwierdź i utwórz WZ",
           Icon = ActionIcon.Start,
           Target = ActionTarget.ToolbarWithText,
           Mode = ActionMode.SingleSession | ActionMode.OnlyForm | ActionMode.ConfirmFinished)]

        public void CreateFV()
        {
            using (ITransaction transaction = Session.Logout(true))
            {
                DokumentHandlowy.Stan = StanDokumentuHandlowego.Zatwierdzony;

                transaction.CommitUI();
            }

            IRelacjeService relacjeService = Session.GetRequiredService<IRelacjeService>();

            using (ITransaction transaction1 = Session.Logout(true))
            {
                DokumentHandlowy dokumentWZ = relacjeService
                    .NowyPodrzednyIndywidualny(
                    new DokumentHandlowy[] { DokumentHandlowy }, "WZ")
                    .FirstOrDefault();

                transaction1.CommitUI();
            }

        }

        public bool IsEnabledCreateFV()
        {
            return DokumentHandlowy.Stan == StanDokumentuHandlowego.Bufor;
        }

        public bool IsVisibleCreateFV()
        {
            return DokumentHandlowy.Definicja.Symbol == "FV";
        }

    }
}
