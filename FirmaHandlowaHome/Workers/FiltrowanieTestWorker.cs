using FirmaHandlowaHome.Workers;
using Soneta.Business;
using Soneta.Handel;
using Soneta.Towary;
using Soneta.Types;
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
            // 1. Pobieranie wszystkich dokumentów handlowych
            List<DokumentHandlowy> doks1 = Session.GetHandel().DokHandlowe
                .Cast<DokumentHandlowy>()
                .ToList();

            // 2. Pobieranie dokumentów handlowych których symbol definicji to ZO
            // - Dokumenty mają nie być w buforze

            RowCondition rowCondition1 = new FieldCondition.Equal("Definicja.Symbol", "ZO");
            rowCondition1 &= new FieldCondition.NotEqual("Stan", StanDokumentuHandlowego.Bufor);

            // Zapis RowComditiom 1_2 jest tym samym co RowCondition 1_1
            RowCondition rowCondition1_2 = new RowCondition.And(
                new FieldCondition.Equal("Definicja.Symbol", "ZO"),
                new FieldCondition.NotEqual("Stan", StanDokumentuHandlowego.Bufor));

            List<DokumentHandlowy> doks2 = Session.GetHandel().DokHandlowe.PrimaryKey[rowCondition1]
                .Cast<DokumentHandlowy>()
                .ToList();

            // 3. Pobieranie dokumentów handlowych których data mieści się
            // w okresie miesiąca listopada

            Date dataOd = new Date(2024, 11, 1);
            Date dataDo = new Date(2024, 11, 30);
            RowCondition rowCondition2 = new FieldCondition.Contain("Data",dataOd,dataDo);

            List<DokumentHandlowy> doks3 = Session.GetHandel().DokHandlowe.PrimaryKey[rowCondition2]
               .Cast<DokumentHandlowy>()
               .ToList();

            // 4. Pobieranie wszystkich dokumentów handlowych które są w buforze
            // - Które są z listopada lub z września

            RowCondition rc4_1 = new FieldCondition.Equal("Stan", StanDokumentuHandlowego.Bufor);
            rc4_1 &= new RowCondition.Or(
                new FieldCondition.Contain("Data", FromTo.Month(2024, 11)),
                new FieldCondition.Contain("Data", FromTo.Month(2024, 9))
            );

            List<DokumentHandlowy> doks4 = Session.GetHandel().DokHandlowe.PrimaryKey[rc4_1]
                 .Cast<DokumentHandlowy>()
                 .ToList();

            // 5. Pobierz wszystkie towary które nie są usługami

            RowCondition rc5_1 = new FieldCondition.Equal("Typ", TypTowaru.Usługa);
            List<Towar> towary = Session.GetTowary().Towary.PrimaryKey[rc5_1]
                .Cast<Towar>()
                .ToList();

            // 6. Pobierz wszystkie towary z zamówień ZO
            // - 1 - Pobrać wszystkie dokumenty ZO
            // - 2 - Pobrać z pozycji tych dokumentów towary bez powtózeń

            RowCondition rc6_1 = new FieldCondition.Equal("Definicja.Symbol", "ZO");
            List<DokumentHandlowy> doks6 = Session.GetHandel().DokHandlowe.PrimaryKey[rc6_1]
               .Cast<DokumentHandlowy>()
               .ToList();

            List<Towar> towaryZO = new List<Towar>();
            foreach (DokumentHandlowy dokument in doks6)
            {
                foreach (PozycjaDokHandlowego pozycja in dokument.Pozycje)
                {
                    if (!towaryZO.Contains(pozycja.Towar))
                    {
                        towaryZO.Add(pozycja.Towar);
                    }
                }
            }

            List<Towar> towaryZO2 = doks6
                .SelectMany(x => x.Pozycje)
                .Select(x => x.Towar)
                .Distinct()
                .ToList();

        }
    }
}
