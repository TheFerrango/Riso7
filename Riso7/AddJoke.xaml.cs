using System;
using System.Windows;
using Microsoft.Phone.Controls;
using Zel10Support;

namespace Riso7
{
  public partial class AddJoke : PhoneApplicationPage
  {
    public AddJoke()
    {
      InitializeComponent();
    }

    private void btnAdd_Click(object sender, RoutedEventArgs e)
    {
      if (App.NetManager.IsNetworkAvaible)
      {
        App.NetManager.AddNetJob(new ContentUpload(txtBattuta.Text));
        NavigationService.Navigate(new Uri("/MainPage.xaml", UriKind.Relative));
      }
      else
        MessageBox.Show("Rete non disponibile, per favore riprova in un secondo momento");
    }
  }
}