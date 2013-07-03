using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Shapes;
using Microsoft.Phone.Controls;
using Zel10Support;

namespace Riso7
{
  public partial class ReadPage : PhoneApplicationPage
  {
    public ReadPage()
    {
      InitializeComponent();
      App.NetManager.BatchOperationCompleted += new Zel10Net.BatchOperationCompletedHandler(NetManager_BatchOperationCompleted);
    }

    void NetManager_BatchOperationCompleted(object sender, System.Collections.Generic.Dictionary<string, string> results)
    {
      barra.IsIndeterminate = false;
      if (results.ContainsKey("battute"))
      {
        if (results["battute"] != "FAIL")
        {
          CommonTasks.WriteFile("battute.json", results["battute"].Remove(0, 5));
          quiBattute.ItemsSource = CommonTasks.CaricaBattute().Items;
        }
        else
          MessageBox.Show("Si è verificato un errore.");
      }
      results.Clear();
    }

    private void PhoneApplicationPage_Loaded(object sender, RoutedEventArgs e)
    {
      quiBattute.ItemsSource = CommonTasks.CaricaBattute().Items;

      if (App.NetManager.IsNetworkAvaible)
      {
        barra.IsIndeterminate = true;
        App.NetManager.AddNetJob(new GetBattute());
        if (!App.NetManager.OperationInProgress)
          App.NetManager.Execute();
      }
      else
      {
        quiBattute = CommonTasks.CaricaBattute();
        if (quiBattute.Items.Count == 0)
          MessageBox.Show("Non sono presenti battute.\nAttivare la connessione dati e riprovare");
      }
    }

    private void PhoneApplicationPage_Unloaded(object sender, RoutedEventArgs e)
    {
      App.NetManager.BatchOperationCompleted -= new Zel10Net.BatchOperationCompletedHandler(NetManager_BatchOperationCompleted);
    }
  }
}