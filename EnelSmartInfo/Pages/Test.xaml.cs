using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Navigation;
using Microsoft.Phone.Controls;
using Microsoft.Phone.Shell;
using System.Collections.ObjectModel;
using EnelSmartInfo.Classes;

namespace EnelSmartInfo.Pages
{
    public partial class Test : PhoneApplicationPage
    {
        ObservableCollection<Consiglio> rigaConsiglio = new ObservableCollection<Consiglio>();

        public Test()
        {
            InitializeComponent();
            
        }

        private void Button_Tap(object sender, System.Windows.Input.GestureEventArgs e)
        {
            int sum = 0;
            int cont = 0;
            int[] arr = new int[10];

            if ((Rad1.IsChecked == true || Rad2.IsChecked == true || Rad3.IsChecked == true) && (Rad4.IsChecked == true || Rad5.IsChecked == true || Rad6.IsChecked == true) && (Rad7.IsChecked == true || Rad8.IsChecked == true || Rad9.IsChecked == true) && (Rad10.IsChecked == true || Rad11.IsChecked == true || Rad12.IsChecked == true) && (Rad13.IsChecked == true || Rad14.IsChecked == true || Rad15.IsChecked == true) && (Rad16.IsChecked == true || Rad17.IsChecked == true || Rad18.IsChecked == true) && (Rad19.IsChecked == true || Rad20.IsChecked == true || Rad21.IsChecked == true) && (Rad22.IsChecked == true || Rad23.IsChecked == true || Rad24.IsChecked == true) && (Rad25.IsChecked == true || Rad26.IsChecked == true || Rad27.IsChecked == true) && (Rad28.IsChecked == true || Rad29.IsChecked == true || Rad30.IsChecked == true))
            {
                if (Rad1.IsChecked == true || Rad2.IsChecked == true || Rad3.IsChecked == true)
                {
                    if (Rad1.IsChecked == true)
                    {
                        arr[0] = 1;
                    }
                    else if (Rad2.IsChecked == true)
                    {
                        sum = sum + 10;
                        arr[0] = 0;
                    }
                    else if (Rad3.IsChecked == true)
                    {
                        cont++;
                        sum = sum + 4;
                        arr[0] = 1;
                    }
                }

                if (Rad4.IsChecked == true || Rad5.IsChecked == true || Rad6.IsChecked == true)
                {
                    if (Rad4.IsChecked == true)
                    {
                        sum = sum + 10;
                        arr[1] = 0;
                    }
                    else if (Rad5.IsChecked == true)
                    {
                        arr[1] = 1;
                    }
                    else if (Rad6.IsChecked == true)
                    {
                        cont++;
                        sum = sum + 4;
                        arr[1] = 1;
                    }
                }

                if (Rad7.IsChecked == true || Rad8.IsChecked == true || Rad9.IsChecked == true)
                {
                    if (Rad7.IsChecked == true)
                    {
                        sum = sum + 10;
                        arr[2] = 0;
                    }
                    else if (Rad8.IsChecked == true)
                    {
                        arr[2] = 1;
                    }
                    else if (Rad9.IsChecked == true)
                    {
                        cont++;
                        sum = sum + 4;
                        arr[2] = 1;
                    }
                }

                if (Rad10.IsChecked == true || Rad11.IsChecked == true || Rad12.IsChecked == true)
                {
                    if (Rad10.IsChecked == true)
                    {
                        sum = sum + 10;
                        arr[3] = 0;
                    }
                    else if (Rad11.IsChecked == true)
                    {
                        arr[3] = 1;
                    }
                    else if (Rad12.IsChecked == true)
                    {
                        cont++;
                        sum = sum + 4;
                        arr[3] = 1;
                    }
                }

                if (Rad13.IsChecked == true || Rad14.IsChecked == true || Rad15.IsChecked == true)
                {
                    if (Rad13.IsChecked == true)
                    {
                        sum = sum + 10;
                        arr[4] = 0;
                    }
                    else if (Rad14.IsChecked == true)
                    {
                        arr[4] = 1;
                    }
                    else if (Rad15.IsChecked == true)
                    {
                        cont++;
                        sum = sum + 4;
                        arr[4] = 1;
                    }
                }

                if (Rad16.IsChecked == true || Rad17.IsChecked == true || Rad18.IsChecked == true)
                {
                    if (Rad16.IsChecked == true)
                    {
                        sum = sum + 10;
                        arr[5] = 0;
                    }
                    else if (Rad17.IsChecked == true)
                    {
                        arr[5] = 1;
                    }
                    else if (Rad18.IsChecked == true)
                    {
                        cont++;
                        sum = sum + 4;
                        arr[5] = 1;
                    }
                }

                if (Rad19.IsChecked == true || Rad20.IsChecked == true || Rad21.IsChecked == true)
                {
                    if (Rad19.IsChecked == true)
                    {
                        sum = sum + 10;
                        arr[6] = 0;
                    }
                    else if (Rad20.IsChecked == true)
                    {
                        arr[6] = 1;
                    }
                    else if (Rad21.IsChecked == true)
                    {
                        cont++;
                        sum = sum + 4;
                        arr[6] = 1;
                    }
                }

                if (Rad22.IsChecked == true || Rad23.IsChecked == true || Rad24.IsChecked == true)
                {
                    if (Rad22.IsChecked == true)
                    {
                        arr[7] = 1;
                    }
                    else if (Rad23.IsChecked == true)
                    {
                        sum = sum + 10;
                        arr[7] = 0;
                    }
                    else if (Rad24.IsChecked == true)
                    {
                        cont++;
                        sum = sum + 4;
                        arr[7] = 1;
                    }
                }

                if (Rad25.IsChecked == true || Rad26.IsChecked == true || Rad27.IsChecked == true)
                {
                    if (Rad25.IsChecked == true)
                    {
                        arr[8] = 1;
                    }
                    else if (Rad26.IsChecked == true)
                    {
                        sum = sum + 10;
                        arr[8] = 0;
                    }
                    else if (Rad27.IsChecked == true)
                    {
                        cont++;
                        sum = sum + 4;
                        arr[8] = 1;
                    }
                }

                if (Rad28.IsChecked == true || Rad29.IsChecked == true || Rad30.IsChecked == true)
                {
                    if (Rad28.IsChecked == true)
                    {
                        sum = sum + 10;
                        arr[9] = 0;
                    }
                    else if (Rad29.IsChecked == true)
                    {
                        arr[9] = 1;
                    }
                    else if (Rad30.IsChecked == true)
                    {
                        cont++;
                        sum = sum + 4;
                        arr[9] = 1;
                    }
                }
                
                Voto.Text = sum + "/100";
                if (sum < 60)
                {
                    Valutazione.Text = "Sei un artista dello spreco elettrico! Il tuo portafoglio e l’ambiente potrebbero certamente beneficiare dei seguenti consigli:";
                }
                else if (sum >= 60 && sum < 85)
                {
                    Valutazione.Text = "Sei nella norma. Tieni al tuo portafoglio ma potresti risparmiare ulteriormente se seguissi qualcuno dei consigli che abbiamo per te:";
                }
                else if (sum >= 85)
                {
                    Valutazione.Text = "Complimenti! Se tutti fossero come te, i problemi energetici globali sarebbero solo un lontano ricordo! Ad ogni modo qui sotto troverai qualche consiglio che ti potrà aiutare a migliorare ulteriormente la tua efficienza:";
                }
                else if (sum == 100)
                {
                    Valutazione.Text = "Complimenti! Se tutti fossero come te, i problemi energetici globali sarebbero solo un lontano ricordo! Non abbiamo consigli da fornirti, probabilmente dovresti essere te a suggerircene!";
                }
                if (cont >= 6)
                {
                    Ignavo.Visibility = Visibility.Visible;
                    Ignavo.Text = "Hai vinto il premio di “Correntista Ignavo”. Non hai saputo rispondere alla maggior parte delle domande di questo test. Non sarebbe forse il caso di acquisire più consapevolezza del mondo che ti circonda? Ne potrebbero beneficiare sia il tuo portafoglio che l’ambiente stesso!";
                }

                if (arr[0] == 1)
                {
                    rigaConsiglio.Add(new Consiglio(Name = "Sostituisci le lampadine con alogene o a led."));
                }
                if (arr[1] == 1)
                {
                    rigaConsiglio.Add(new Consiglio("Quando una lampadina non serve, spegnila."));
                }
                if (arr[2] == 1)
                {
                    rigaConsiglio.Add(new Consiglio("Imposta il frigorifero ad una temperatura maggiore di 3°."));
                }
                if (arr[3] == 1)
                {
                    rigaConsiglio.Add(new Consiglio("Sostituisci le guarnizioni del frigorifero."));
                }
                if (arr[4] == 1)
                {
                    rigaConsiglio.Add(new Consiglio("Avvia lavastoviglie e/o lavatrice solo a pieno carico, possibilmente lavando a bassa temperatura. Spegni la lavastoviglie quando parte il programma di asciugatura, è sufficiente aprire lo sportello."));
                }
                if (arr[5] == 1)
                {
                    rigaConsiglio.Add(new Consiglio("Fai partire lavastoviglie e/o lavatrice nelle ore serali / notturne o nei giorni festivi."));
                }
                if (arr[6] == 1)
                {
                    rigaConsiglio.Add(new Consiglio("Preferisci un forno a microonde a quello tradizionale. Consuma circa la metà e non necessita di preriscaldamento."));
                }
                if (arr[7] == 1)
                {
                    rigaConsiglio.Add(new Consiglio("Non usare il grill nel forno elettrico, se non strettamente necessario."));
                }
                if (arr[8] == 1)
                {
                    rigaConsiglio.Add(new Consiglio("Collega Televisori, PC e altri dispositivi ad una multipresa con pulsante di spegnimento, così da spegnerli totalmente quando non in uso."));
                }
                if (arr[9] == 1)
                {
                    rigaConsiglio.Add(new Consiglio("Regola il condizionatore ad una temperatura non inferiore ai 24-25 gradi. Tieni sempre porte e finestre chiuse quando l’impianto è in funzione. Se non fa molto caldo, prova ad usare solo la funzione di deumidificazione. Raffredda solo le stanze nelle quali ti trattieni, anziché l’intera casa. Controlla che il condizionatore sia sufficientemente carico di gas, altrimenti perderà in efficienza."));
                }

                Consigli.ItemsSource = rigaConsiglio;

                try
                {
                    TestScrollViewer.Visibility = Visibility.Collapsed;
                    this.box_popup.IsPopupOpen = true;
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message + "\n\n" + ex.InnerException);
                }

            }
            else
            {
                MessageBox.Show("Compila tutti i campi del test prima di procedere!");
            }
        }

        private void Button_Tap_1(object sender, System.Windows.Input.GestureEventArgs e)
        {
            NavigationService.Navigate(new Uri("/HomePage.xaml?id=1", UriKind.Relative));
        }

        protected override void OnBackKeyPress(System.ComponentModel.CancelEventArgs e)
        {
            //quando premo indietro, vado dal browser alla schermata principale
            if (box_popup.IsPopupOpen == true)
            {
                NavigationService.Navigate(new Uri("/HomePage.xaml?id=1", UriKind.Relative));
                e.Cancel = true;
            }
        }
    }
}