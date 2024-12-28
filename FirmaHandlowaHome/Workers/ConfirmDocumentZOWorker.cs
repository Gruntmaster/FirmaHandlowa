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

[assembly: Worker(typeof(ConfirmDocumentZOWorker), typeof(DokumentHandlowy))]

namespace FirmaHandlowa.Workers
{
    public class ConfirmDocumentZOWorker
    {
        [Context]
        public Session Session { get; set; }

        [Context]
        public DokumentHandlowy DokumentHandlowy { get; set; }

        [Action("Zatwierdź i utwórz FV",
            Icon = ActionIcon.Start,
            Target = ActionTarget.ToolbarWithText,
            Mode = ActionMode.SingleSession | ActionMode.OnlyForm | ActionMode.ConfirmFinished)]

        public void ConfirmDocument()
        {
            using (ITransaction transaction = Session.Logout(true))
            {
                DokumentHandlowy.Stan = StanDokumentuHandlowego.Zatwierdzony;

                transaction.CommitUI();
            }


            IRelacjeService relacjeService = Session.GetRequiredService<IRelacjeService>();

            using (ITransaction transaction = Session.Logout(true))
            {
                DokumentHandlowy dokumentFaktura = relacjeService.NowyPodrzednyIndywidualny(
                    new DokumentHandlowy[] { DokumentHandlowy },
                    "FV"
                ).FirstOrDefault();


                transaction.CommitUI();
            }
        }

        public bool IsEnabledConfirmDocument()
        {
            return DokumentHandlowy.Stan == StanDokumentuHandlowego.Bufor;
        }

        public bool IsVisibleConfirmDocument()
        {
            return DokumentHandlowy.Definicja.Symbol == "ZO";
        }
    }
}
