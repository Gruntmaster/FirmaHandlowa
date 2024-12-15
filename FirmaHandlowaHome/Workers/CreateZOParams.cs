using Soneta.Business;
using Soneta.CRM;
using Soneta.Magazyny;
using Soneta.Towary;
using Soneta.Types;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FirmaHandlowaHome.Workers
{
    public class CreateZOParams : ContextBase
    {
        private Kontrahent _kontrahent;
        private Magazyn _magazyn;
        private Towar _towar;
        public CreateZOParams(Context context) : base(context) 
        { 
        
        }

        [Caption("Kontrahent")]
        [Required]
        public Kontrahent Kontrahent
        {
            get => _kontrahent;
            set
            {
                _kontrahent = value;
                OnChanged();
            }
        }

        [Caption("Magazyn")]
        [Required]
        public Magazyn Magazyn
        {
            get => _magazyn;
            set
            {
                _magazyn = value;
                OnChanged();
            }
        }

        [Caption("Towar")]
        [Required]
        public Towar Towar
        {
            get => _towar;
            set
            {
                _towar = value;
                OnChanged();
            }
        }
    }
}
