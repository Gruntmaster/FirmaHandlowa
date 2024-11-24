﻿using FirmaHandlowaHome.Workers;
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

        [Action("Testowy prycisk",
            CommandStyle = Soneta.Commands.CommandStyle.Red,
            Icon = ActionIcon.Database,
            Target = ActionTarget.ToolbarWithText,
            Mode = ActionMode.SingleSession | ActionMode.OnlyForm | ActionMode.ConfirmFinished)]
        public void DocumentAction()
        {
            using (ITransaction transaction = Session.Logout(true))
            {
                DokumentHandlowy.Obcy.Numer = "1234";

                transaction.CommitUI();
            }
        }
    }
}
