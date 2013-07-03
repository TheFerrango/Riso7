using System;
using System.IO.IsolatedStorage;
using System.Windows;
using Microsoft.Phone.Controls;
using Zel10Support;

namespace Riso7
{
  public partial class MainPage : PhoneApplicationPage
  {
    // Constructor
    public MainPage()
    {
      InitializeComponent();
      IsolatedStorageSettings iss = IsolatedStorageSettings.ApplicationSettings;
      CommonTasks.InitializeStorageAccess();

      if (new Microsoft.Phone.Marketplace.LicenseInformation().IsTrial() == false)
        adControl.Visibility = System.Windows.Visibility.Collapsed;

      if (iss["usrPass"] as string != "" && !App.IsLogged)
      {
        barra.IsIndeterminate = true;
        App.NetManager.BatchOperationCompleted += new Zel10Net.BatchOperationCompletedHandler(NetManager_BatchOperationCompleted);

        App.NetManager.AddNetJob(new Login(iss["usrName"] as string, iss["usrPass"] as string));
        if (!App.NetManager.OperationInProgress)
        {
          if (App.NetManager.IsNetworkAvaible)
            App.NetManager.Execute();
          else
            MessageBox.Show("Connessione non disponibile.\nAbilitare la connessione dati o collegarsi ad una rete wifi");
        }
        else
          MessageBox.Show("E' in corso un'altra operazione. Riprovare più tardi");
      }
    }

    void NetManager_BatchOperationCompleted(object sender, System.Collections.Generic.Dictionary<string, string> results)
    {
      if (results.ContainsKey("login"))
      {
        if (results["login"].Contains("errati") == false)
        {
          App.IsLogged = true;
        }
        else
          MessageBox.Show("Impossibile effettuare il login. Controllare i dati inseriti e lo stato della rete.");
      }
      barra.IsIndeterminate = false;
      results.Clear();
    }

    private void info_Click(object sender, EventArgs e)
    {
      MessageBox.Show("Server a cura di Mattia Beta.\nClient mobile per Windows Phone 7.1 a cura di TheFerrango.\nInclude il deserializzatore JSON per Windows Phone 7 JSON.NET prodotto da Newtonsoft.\nVisitaci sul sito http://mattiabeta.xoom.it/beta/ e unisciti a noi, contribuendo ad espandere il nostro database sempre di più! =) \nVersione 2.3.0", "Informazioni su", MessageBoxButton.OK);
    }

    private void btnRead_Tap(object sender, System.Windows.Input.GestureEventArgs e)
    {
      NavigationService.Navigate(new Uri("/ReadPage.xaml", UriKind.Relative));
    }

    private void btnAdd_Tap(object sender, System.Windows.Input.GestureEventArgs e)
    {
      if (!App.IsLogged)
      {
        MessageBox.Show("Attenzione, login necessario per aggiungere battute");
      }
      else
      {
        NavigationService.Navigate(new Uri("/AddJoke.xaml", UriKind.Relative));
      }
    }

    private void btnOpt_Tap(object sender, System.Windows.Input.GestureEventArgs e)
    {
      NavigationService.Navigate(new Uri("/Settings.xaml", UriKind.Relative));
    }

    private void PhoneApplicationPage_Loaded(object sender, RoutedEventArgs e)
    {
      while (NavigationService.CanGoBack)
        NavigationService.RemoveBackEntry();
    }

    private void PhoneApplicationPage_Unloaded(object sender, RoutedEventArgs e)
    {
      App.NetManager.BatchOperationCompleted -= new Zel10Net.BatchOperationCompletedHandler(NetManager_BatchOperationCompleted);
    }
  }
}