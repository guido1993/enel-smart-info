using System.Diagnostics;
using System.Windows;
using Microsoft.Phone.Scheduler;
using Microsoft.Phone.Shell;
using System;
using System.IO;
using System.IO.IsolatedStorage;
using System.Xml;
using System.Xml.Linq;
using System.Linq;
using System.Net;
using System.Net.NetworkInformation;

namespace BackgroundAgents
{
    public class ScheduledAgent : ScheduledTaskAgent
    {
        /// <remarks>
        /// Costruttore ScheduledAgent, inizializza il gestore UnhandledException
        /// </remarks>
        static ScheduledAgent()
        {
            // La sottoscrizione del gestore eccezioni gestite
            Deployment.Current.Dispatcher.BeginInvoke(delegate
            {
                Application.Current.UnhandledException += UnhandledException;
            });
        }

        /// Codice da eseguire in caso di eccezioni non gestite
        private static void UnhandledException(object sender, ApplicationUnhandledExceptionEventArgs e)
        {
            if (Debugger.IsAttached)
            {
                // Si è verificata un'eccezione non gestita; inserire un'interruzione nel debugger
                Debugger.Break();
            }
        }

        /// <summary>
        /// Agente che esegue un'attività pianificata
        /// </summary>
        /// <param name="task">
        /// L'attività richiamata
        /// </param>
        /// <remarks>
        /// Questo metodo viene chiamato quando viene richiamata un'attività a elevato utilizzo di risorse o periodica
        /// </remarks>
        protected override void OnInvoke(ScheduledTask task)
        {
            ShellToast toast = new ShellToast();
            toast.Title = "ENEL smart info";
            toast.Content = "hai superato la tua soglia mensile!";
            toast.Show();


            FlipTileData TileData = new FlipTileData()
            {
                Count = int.Parse("1"),
                BackContent = "E' giunta una notifica",
                BackTitle = "Enel InfoPlus"
            };
            ShellTile.ActiveTiles.FirstOrDefault().Update(TileData);

            NotifyComplete();
        }
    }
}