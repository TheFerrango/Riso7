using System;
using System.Collections.Generic;
using System.Net;
using Microsoft.Phone.Net.NetworkInformation;

namespace Zel10Support
{
  /*
   * Zel10Net networking class works in this awesome way:
   * Every networking-needing task is divided into smaller tasks, such as login, logout,
   * file upload, file download and so on.
   * 
   * A Zel10Net type object exposes a method named AddNetJob, which requires an object that implements i INetWork
   * interface. this way, the class iterates through all the 'jobs' added to the list, waiting for each job to finish
   * before starting the next one. This way, a fake syncronous-like experience can be provided, and when all jobs
   * in the queue have been completed, a 'BatchOperationCompleted' event is thrown.
   * 
   * This way, to add functionalities to the class, you just have to implement your class using the INetWork interface
   * and implementing the ExecuteNetworkOperation() method.
   * Yay!
   */
  public class Zel10Net
  {
    public delegate void BatchOperationCompletedHandler(object sender, Dictionary<string, string> results);
    public event BatchOperationCompletedHandler BatchOperationCompleted;
    Dictionary<string, string> _results;
    Queue<INetWork> _jobs;
    INetWork currJob;
    CookieAwareWebClient _zel10Client;

    public bool OperationInProgress
    {
      get
      {
        return _zel10Client.IsBusy;
      }
    }

    public bool IsNetworkAvaible
    {
      get
      {
        return NetworkInterface.GetIsNetworkAvailable();
      }
    }

    public Zel10Net()
    {
      _zel10Client = new CookieAwareWebClient();
      _jobs = new Queue<INetWork>();
      _results = new Dictionary<string, string>();
      _zel10Client.UploadStringCompleted += new UploadStringCompletedEventHandler(_zel10Client_UploadStringCompleted);
      _zel10Client.DownloadStringCompleted += new DownloadStringCompletedEventHandler(_zel10Client_DownloadStringCompleted);
    }

    void _zel10Client_DownloadStringCompleted(object sender, DownloadStringCompletedEventArgs e)
    {
      try
      {
        _results[currJob.JobName()] = e.Result;
        if (_jobs.Count > 0)
        {
          currJob = _jobs.Dequeue();
          currJob.ExecuteJob(_zel10Client, NetworkAddresses.ServerAddress);
        }
        else
        {
          BatchOperationCompleted(sender, _results);
        }
      }
      catch
      {
        _results[currJob.JobName()] = "FAIL";
        BatchOperationCompleted(sender, _results);
      }
    }

    void _zel10Client_UploadStringCompleted(object sender, UploadStringCompletedEventArgs e)
    {
      try
      {
        _results[currJob.JobName()] = e.Result;
      }
      catch
      {
        _results[currJob.JobName()] = "FAIL";
        BatchOperationCompleted(sender, _results);
      }
      if (_jobs.Count > 0)
      {
        currJob = _jobs.Dequeue();
        currJob.ExecuteJob(_zel10Client, NetworkAddresses.ServerAddress);
      }
      else
      {
        BatchOperationCompleted(sender, _results);
      }
    }

    public void AddNetJob(INetWork job)
    {
      _jobs.Enqueue(job);
    }

    public bool Execute()
    {
      if (_jobs.Count > 0)
      {
        currJob = _jobs.Dequeue();
        currJob.ExecuteJob(_zel10Client, NetworkAddresses.ServerAddress);
        return true;
      }
      return false;
    }
  }

  public class ContentUpload : INetWork
  {
    string _pk, _data;

    public ContentUpload(string data)
    {
      _data = data;
    }

    public void ExecuteJob(CookieAwareWebClient cawc, string address)
    {
      string toPost = string.Format("testo={0}&device=WindowsPhone&Submit=Inserisci", _data);
      cawc.Headers["Content-Type"] = "application/x-www-form-urlencoded";
      cawc.UploadStringAsync(new Uri(NetworkAddresses.ObtainBattuteUploadAddress(), UriKind.Absolute), toPost);
    }

    public string JobName()
    {
      return "upload";
    }
  }

  public class Login : INetWork
  {
    string _user, _pass;
    public Login(string user, string pass)
    {
      _user = user; _pass = pass;
    }

    public void ExecuteJob(CookieAwareWebClient cawc, string address)
    {
      string toPost = string.Format("myusername={0}&mypassword={1}&Submit=Login", _user, _pass);
      cawc.Headers["Content-Type"] = "application/x-www-form-urlencoded";
      cawc.UploadStringAsync(new Uri(NetworkAddresses.ObtainLoginAddress(), UriKind.Absolute), toPost);
    }

    public string JobName()
    {
      return "login";
    }
  }

  public class GetBattute : INetWork
  {
    public GetBattute()
    {

    }

    public void ExecuteJob(CookieAwareWebClient cawc, string address)
    {
      cawc.DownloadStringAsync(new Uri(NetworkAddresses.ObtainBattuteDownloadAddress()));
    }

    public string JobName()
    {
      return "battute";
    }
  }

  //I didn't wrote this, took from stackoverflow.com
  public class CookieAwareWebClient : WebClient
  {
    [System.Security.SecuritySafeCritical]
    public CookieAwareWebClient()
      : base()
    {
    }
    private CookieContainer m_container = new CookieContainer();
    protected override WebRequest GetWebRequest(Uri address)
    {
      WebRequest request = base.GetWebRequest(address);
      if (request is HttpWebRequest)
      {
        (request as HttpWebRequest).CookieContainer = m_container;
      }
      return request;
    }
  }
}
