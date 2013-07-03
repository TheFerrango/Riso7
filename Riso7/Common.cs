using System.IO;
using System.IO.IsolatedStorage;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using Newtonsoft.Json;

namespace Riso7
{

  public static class CommonTasks
  {
    static IsolatedStorageFile myStore;

    public static ListBox CaricaBattute()
    {
      ListBox coop = new ListBox(); 
      if (myStore.FileExists("battute.json"))
      {
        
         //coop.Children.Clear();
        string[] c = JsonConvert.DeserializeObject<string[]>(CommonTasks.ReadFile("battute.json"));
        foreach (string s in c)
        {
          Riso7.BattutaControl tb = new Riso7.BattutaControl(s);
          coop.Items.Add(tb);
        }
        
      }
      return coop;
    }

  
    static void tb_Tap(object sender, GestureEventArgs e)
    {
      MessageBox.Show(((TextBlock)sender).Tag.ToString(), "Battuta:", MessageBoxButton.OK);
    }

    public static void ResetStorage()
    {
      myStore.Remove();
      myStore = IsolatedStorageFile.GetUserStoreForApplication();
    }

    public static bool InitializeStorageAccess()
    {
      try
      {
        myStore = IsolatedStorageFile.GetUserStoreForApplication();
      }
      catch
      {
        return false;
      }
      return true;
    }

    public static bool WriteFile(string path, string content)
    {
      try
      {
        StreamWriter sw = new StreamWriter(new IsolatedStorageFileStream(path, FileMode.Create, myStore));
        sw.Write(content);
        sw.Close();
      }
      catch
      {
        return false;
      }
      return true;
    }

    public static string ReadFile(string path)
    {
      string coop = "";
      try
      {
        StreamReader sr = new StreamReader(new IsolatedStorageFileStream(path, FileMode.Open, myStore));
        while (!sr.EndOfStream)
          coop += sr.ReadLine() + "\n";
        sr.Close();
      }
      catch
      {
        return null;
      }
      return coop;
    }

  }
}
