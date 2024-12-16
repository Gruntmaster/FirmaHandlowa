using Soneta.Business;
using Soneta.CRM;
using Soneta.Handel;
using Soneta.Magazyny;
using Soneta.Tools;
using Soneta.Types;

namespace FirmaHandlowa.Workers
{
    public class TowarListParams : ContextBase
    {
        private DefDokHandlowego _definicja;
        private Magazyn _magazyn;
        private Kontrahent _kontrahent;

        public TowarListParams(Context context) : base(context)
        {
            _definicja = Session.GetHandel().DefDokHandlowych.WgSymbolu["ZO"];
        }

        [Caption("Definicja dokumentu")]
        [Required]
        [Priority(10)]
        [DefaultWidth(20)]
        public DefDokHandlowego Definicja
        {
            get => _definicja;
            set
            {
                _definicja = value;
                OnChanged();
            }
        }


        [Caption("Kontrahent")]
        [Required]
        [Priority(20)]
        [DefaultWidth(20)]
        public Kontrahent Kontrahent
        {
            get => _kontrahent;
            set
            {
                _kontrahent = value;
                OnChanged();

            }
        }

        public bool IsVisibleKontrahent()
        {
            return _definicja != null;
        }


        [Caption("Magazyn")]
        [Required]
        [Priority(30)]
        [DefaultWidth(20)]
        public Magazyn Magazyn
        {
            get => _magazyn;
            set
            {
                _magazyn = value;
                OnChanged();
            }
        }

        public bool IsReadOnlyMagazyn()
        {
            return _definicja == null;
        }
    }
}
