using System;
using System.Windows;
using System.Windows.Controls;
using Microsoft.Phone.Tasks;

namespace Riso7
{
  public partial class BattutaControl : UserControl
  {
    string _battuta;

    public BattutaControl(string battuta)
    {
      InitializeComponent();
      _battuta = battuta;

      txtBattuta.Text = _battuta;

    }

    private void MenuItem_Click(object sender, RoutedEventArgs e)
    {
      EmailComposeTask ect = new EmailComposeTask();
      ect.Body = _battuta + Environment.NewLine + Environment.NewLine + "--" + Environment.NewLine + "Shared via Riso7 for Windows Phone";
      ect.Subject = "Battuta";
      ect.Show();      
    }

    private void MenuItem_Click_1(object sender, RoutedEventArgs e)
    {
      ShareStatusTask sst = new ShareStatusTask();
      sst.Status = _battuta;
      sst.Show();
    }
  }
}
