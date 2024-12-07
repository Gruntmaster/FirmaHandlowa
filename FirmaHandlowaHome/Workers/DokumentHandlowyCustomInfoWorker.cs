﻿using FirmaHandlowaHome.Workers;
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
                foreach(PozycjaDokHandlowego pozycja in DokumentHandlowy.Pozycje)
                {
                    result += pozycja.Towar.Kod + "; ";
                }

                return result;
            }
        }
    }
}
