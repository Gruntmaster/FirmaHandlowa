using FirmaHandlowaHome.Workers;
using Soneta.Business;
using Soneta.Handel;
using Soneta.Types;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

[assembly: Worker(typeof(DokumentHandlowyCustomInfoWorker),typeof(DokumentHandlowy))]

namespace FirmaHandlowaHome.Workers
{
    public class DokumentHandlowyCustomInfoWorker
    {
        [Context]
        public DokumentHandlowy DokumentHandlowy { get; set; }

        [Caption("Info o towarach")]
        public string TowaryStr
        {
            get
            {
                string result = string.Empty;
                foreach (PozycjaDokHandlowego pozycja in DokumentHandlowy.Pozycje)
                {
                    result += pozycja.Towar.Kod + "; ";
                }

                return result;
            }
        }

        [Caption("Rodzaj dostawy")]
        [ControlEdit(ControlEditKind.ComboBox)]
        public string RodzajDostawy
        {
            get => DokumentHandlowy.Features.GetString("RodzajDostawy");
            set
            {
                DokumentHandlowy.Features["RodzajDostawy"] = value;
            }
        }

        public string[] GetListRodzajDostawy()
        {
            return new string[]
            {
                "Kurier",
                "Odbiór osobisty",
                "Poczta"
            };
        }

        [Caption("Planowana data dostawy")]
        public Date PlanowanaDataDostawy
        {
            get
            {
               return DokumentHandlowy.Features.GetDate("PlanowanaDataDostawy");
            }
            set
            {
                if (value < Date.Today)
                {
                    throw new Exception("Nie można wprowadzić daty przeszłej!");
                }
                DokumentHandlowy.Features["PlanowanaDataDostawy"] = value;
            }
        }

        public bool IsReadOnlyPlanowanaDataDostawy()
        {
            if (RodzajDostawy == "Odbiór osobisty")
            {
                return true;
            }

            return false;
        }
    }
}
