using Soneta.Business;
using Soneta.Types;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FirmaHandlowa.Workers
{
    public class DocumentZnizkaPositionParams : ContextBase
    {
        public DocumentZnizkaPositionParams(Context context) : base(context)
        {
        }

        [Caption("Rabat na pozycje")]
        public double Znizka { get; set; }
    }
}
