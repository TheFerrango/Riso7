using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Zel10Support
{
  public interface INetWork
  {
    void ExecuteJob(CookieAwareWebClient cawc, string address);
    string JobName();
  }
  
  public static class NetworkAddresses
  {
    //static string _ServerAddress = "http://panizza1.dyndns.org:2401/";
    static string _ServerAddress = "http://mattiabeta.xoom.it/beta/";//"http://transit.zel10.com/";

    public static string ServerAddress
    {
      get { return NetworkAddresses._ServerAddress; }
      set { NetworkAddresses._ServerAddress = value; }
    }

    static string _UploadPath = "addBattuta.php";
    static string _DownloadPath = "json.php";
    static string _LoginPath = "checklogin.php";

    public static string ObtainLoginAddress()
    {
      return _ServerAddress + _LoginPath;
    }
    
    public static string ObtainBattuteDownloadAddress()
    {
      return _ServerAddress + _DownloadPath;
    }

    public static string ObtainBattuteUploadAddress()
    {
      return _ServerAddress + _UploadPath;
    }
  }
}
