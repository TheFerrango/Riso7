using System;
using System.IO.IsolatedStorage;
using System.Windows;
using Microsoft.Phone.Controls;
using Microsoft.Phone.Tasks;

namespace Riso7
{
  public partial class Settings : PhoneApplicationPage
  {
    IsolatedStorageSettings iss;

    public Settings()
    {
      InitializeComponent();
      iss = IsolatedStorageSettings.ApplicationSettings;
    }

    private void btnCreate_Click(object sender, RoutedEventArgs e)
    {
      if (App.NetManager.IsNetworkAvaible)
      {
        WebBrowserTask wbt = new WebBrowserTask();
        wbt.Uri = new Uri("http://mattiabeta.xoom.it/beta/reg.php", UriKind.Absolute);
        wbt.Show();
      }
      else
      {
        MessageBox.Show("Connessione non disponibile.\nAbilitare la connessione dati o collegarsi ad una rete wifi");
      }
    }

    private void btnLogin_Click(object sender, RoutedEventArgs e)
    {
      if (pwdUsr.Password != "" && txtUsr.Text != "")
        if (App.NetManager.IsNetworkAvaible)
        {
          iss["usrName"] = txtUsr.Text;
          iss["usrPass"] = pwdUsr.Password;
          iss.Save();
          NavigationService.Navigate(new Uri("/MainPage.xaml", UriKind.Relative));
        }
        else
        {
          MessageBox.Show("Connessione non disponibile.\nAbilitare la connessione dati o collegarsi ad una rete wifi");
        }
    }

    private void PhoneApplicationPage_Loaded(object sender, RoutedEventArgs e)
    {
      txtUsr.Text = iss["usrName"] as string;
      pwdUsr.Password = iss["usrPass"] as string;
    }
  }
}