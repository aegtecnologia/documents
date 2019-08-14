// Decompiled with JetBrains decompiler
// Type: ZapJet.WhatsAppGroupPosting
// Assembly: ZapJet, Version=4.5.0.0, Culture=neutral, PublicKeyToken=null
// MVID: A21AEBCE-0F2C-4754-8239-913DBC7B7D97
// Assembly location: C:\Program Files (x86)\Socialjet\ZAPJET SENDER\ZapJet.exe

using Microsoft.VisualBasic;
using Microsoft.VisualBasic.CompilerServices;
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Windows.Forms;

namespace ZapJet
{
  public class WhatsAppGroupPosting
  {
    private ChromeDriver ChromeDrv;
    public string Groups;
    public string Message;
    public string FileName;
    public string Caption;
    public long Speed;
    public int MessageType;
    public int MessageDelay;
    private Thread VerificationThread;
    public bool StopFilter;
    public bool IsWorking;
    public List<ListViewItem> MediaFiles;

    public event EventHandler OnPost;

    public event EventHandler OnProgressChange;

    public WhatsAppGroupPosting()
    {
      this.MediaFiles = new List<ListViewItem>();
    }

    public void StartPosting()
    {
      string message = this.Message;
      this.VerificationThread = new Thread(new ThreadStart(this.DoPosting));
      this.VerificationThread.Start();
    }

    public void StopSending()
    {
      try
      {
        this.VerificationThread.Abort();
        this.VerificationThread.Suspend();
        this.ChromeDrv.Quit();
      }
      catch (Exception ex)
      {
        ProjectData.SetProjectError(ex);
        ProjectData.ClearProjectError();
      }
    }

    private void DoPosting()
    {
      try
      {
        ChromeDriverService defaultService = ChromeDriverService.CreateDefaultService();
        defaultService.HideCommandPromptWindow = true;
        this.ChromeDrv = new ChromeDriver(defaultService, new ChromeOptions());
      }
      catch (Exception ex)
      {
        ProjectData.SetProjectError(ex);
        ProjectData.ClearProjectError();
      }
      this.IsWorking = true;
      if (this.StopFilter)
      {
        this.IsWorking = false;
        this.StopFilter = false;
        this.ChromeDrv.Close();
      }
      else
      {
        try
        {
          this.ChromeDrv.Navigate().GoToUrl("https://web.whatsapp.com/");
        }
        catch (Exception ex)
        {
          ProjectData.SetProjectError(ex);
          ProjectData.ClearProjectError();
        }
        while (!this.StopFilter)
        {
          Thread.Sleep(10);
          Thread.Sleep(checked ((int) this.Speed));
          if (this.IsLoggedIn())
          {
            this.ChromeDrv.ExecuteScript(ZapJet.My.Resources.Resources.JsExec.ToString());
            string[] strArray1 = Strings.Split(this.Groups, "\r\n", -1, CompareMethod.Binary);
            int num1 = 1;
            string[] strArray2 = strArray1;
            int index = 0;
            while (index < strArray2.Length)
            {
              string str = strArray2[index];
              try
              {
                this.ChromeDrv.ExecuteScript("document.getElementsByClassName('executor')[0].setAttribute(arguments[0], arguments[1]);", (object) "href", (object) str);
              }
              catch (Exception ex)
              {
                ProjectData.SetProjectError(ex);
                ProjectData.ClearProjectError();
              }
              try
              {
                this.ChromeDrv.FindElementById("sender").Click();
                Thread.Sleep(2000);
              }
              catch (Exception ex)
              {
                ProjectData.SetProjectError(ex);
                ProjectData.ClearProjectError();
              }
              try
              {
                this.ChromeDrv.FindElementByClassName("_3PQ7V").Click();
                Thread.Sleep(2000);
              }
              catch (Exception ex)
              {
                ProjectData.SetProjectError(ex);
                ProjectData.ClearProjectError();
              }
              EventArgs e = new EventArgs();
              if (this.IsChatDisplayed())
              {
                this.SendMsg(this.Message);
                List<ListViewItem>.Enumerator enumerator;
                if (this.MediaFiles.Count > 0)
                {
                  try
                  {
                    enumerator = this.MediaFiles.GetEnumerator();
                    while (enumerator.MoveNext())
                    {
                      ListViewItem current = enumerator.Current;
                      string text = current.SubItems[2].Text;
                      this.SendFile(Conversions.ToString(current.Tag), text);
                    }
                  }
                  finally
                  {
                    enumerator.Dispose();
                  }
                }
                // ISSUE: reference to a compiler-generated field
                EventHandler onPostEvent = this.OnPostEvent;
                if (onPostEvent != null)
                  onPostEvent((object) (str + "|Success"), e);
              }
              else
              {
                // ISSUE: reference to a compiler-generated field
                EventHandler onPostEvent = this.OnPostEvent;
                if (onPostEvent != null)
                  onPostEvent((object) (str + "|Failed"), e);
              }
              try
              {
                this.ChromeDrv.FindElementByClassName("_3PQ7V").Click();
                Thread.Sleep(2000);
              }
              catch (Exception ex)
              {
                ProjectData.SetProjectError(ex);
                ProjectData.ClearProjectError();
              }
              Thread.Sleep(2000);
              this.WaitTocompleteLoading(3);
              // ISSUE: reference to a compiler-generated field
              EventHandler progressChangeEvent = this.OnProgressChangeEvent;
              if (progressChangeEvent != null)
                progressChangeEvent((object) (Conversions.ToString(num1) + "/" + Conversions.ToString(Information.UBound((Array) strArray1, 1))), new EventArgs());
              checked { ++num1; }
              checked { ++index; }
            }
            Thread.Sleep(200);
            int num2 = (int) Interaction.MsgBox((object) "Publicação nos Grupos Finalizada!", MsgBoxStyle.Information, (object) ModuleConfig.ApplicationTitle);
            return;
          }
        }
        this.IsWorking = false;
        this.StopFilter = false;
        this.ChromeDrv.Close();
      }
    }

    public void EndReport()
    {
      WhatsappBulkSenderModule.endtime = DateAndTime.Now.ToString("dd MMMM yyyy HH:mm");
      WhatsappBulkSenderModule.IsSending = false;
      WhatsappBulkSenderModule.BulkIsEnd = true;
      WhatsappBulkSenderModule.CurrentReportFile = WhatsappBulkSenderModule.GenerateReport();
    }

    private void SendMsg(string Msg)
    {
      try
      {
        this.ChromeDrv.ExecuteScript("document.getElementsByClassName('_3u328')[0].innerText=arguments[0]", (object) Msg);
      }
      catch (Exception ex)
      {
        ProjectData.SetProjectError(ex);
        ProjectData.ClearProjectError();
      }
      try
      {
        this.ChromeDrv.FindElementByClassName("_3u328").SendKeys(OpenQA.Selenium.Keys.Shift + OpenQA.Selenium.Keys.Enter);
        Thread.Sleep(10);
        this.ChromeDrv.FindElementByClassName("_3u328").SendKeys(OpenQA.Selenium.Keys.Backspace);
        Thread.Sleep(10);
        this.ChromeDrv.FindElementByClassName("_3u328").SendKeys(OpenQA.Selenium.Keys.Enter);
        Thread.Sleep(1000);
        this.WaitTocompleteLoading(3);
      }
      catch (Exception ex)
      {
        ProjectData.SetProjectError(ex);
        ProjectData.ClearProjectError();
      }
    }

    private bool IsSendButtonDisplayed()
    {
      bool flag;
      try
      {
        flag = this.ChromeDrv.FindElementByClassName("_35EW6").Displayed;
      }
      catch (Exception ex)
      {
        ProjectData.SetProjectError(ex);
        flag = false;
        ProjectData.ClearProjectError();
      }
      return flag;
    }

    private bool IsMediaUploading()
    {
      bool flag;
      try
      {
        flag = this.ChromeDrv.FindElementByClassName("_1l3ap").Displayed;
      }
      catch (Exception ex)
      {
        ProjectData.SetProjectError(ex);
        if (ex.Message.Contains("Unable to locate element"))
        {
          flag = false;
          ProjectData.ClearProjectError();
        }
        else
        {
          flag = true;
          ProjectData.ClearProjectError();
        }
      }
      return flag;
    }

    private string RandomTag()
    {
      VBMath.Randomize();
      return Conversions.ToString(Conversion.Int(VBMath.Rnd() * 1E+07f));
    }

    private string CleanNumber(string Number)
    {
      Number = Number.Replace("+", "").Trim();
      Number = Number.Replace("-", "");
      Number = Number.Replace("(", "");
      Number = Number.Replace(")", "");
      Number = Number.Replace(" ", "");
      Number = Number.Replace("/", "");
      Number = Number.Replace("\\", "");
      Number = Number.Replace("\t", "");
      return Number.Trim();
    }

    private bool IsInvaid()
    {
      bool flag;
      try
      {
        Thread.Sleep(500);
        flag = this.ChromeDrv.FindElementByClassName("_3PQ7V").Displayed;
      }
      catch (Exception ex)
      {
        ProjectData.SetProjectError(ex);
        flag = false;
        ProjectData.ClearProjectError();
      }
      return flag;
    }

    private bool IsCaptionLoaded()
    {
      bool flag;
      try
      {
        flag = this.ChromeDrv.FindElement(By.ClassName("_3u328")).Displayed;
      }
      catch (Exception ex)
      {
        ProjectData.SetProjectError(ex);
        flag = false;
        ProjectData.ClearProjectError();
      }
      return flag;
    }

    private string LoadInjector()
    {
      return ZapJet.My.Resources.Resources.JavaScriptInject;
    }

    private bool IsLoggedIn()
    {
      bool flag;
      try
      {
        flag = this.ChromeDrv.FindElement(By.ClassName("_2rZZg")).Displayed;
      }
      catch (Exception ex)
      {
        ProjectData.SetProjectError(ex);
        flag = false;
        ProjectData.ClearProjectError();
      }
      return flag;
    }

    private bool IsInProgress()
    {
      bool flag;
      try
      {
        this.WaitTocompleteLoading(2);
        flag = this.ChromeDrv.FindElementByClassName("progress-container").Displayed;
      }
      catch (Exception ex)
      {
        ProjectData.SetProjectError(ex);
        this.WaitTocompleteLoading(2);
        flag = false;
        ProjectData.ClearProjectError();
      }
      return flag;
    }

    private bool WaitTocompleteLoading(int TimerOutInSecond)
    {
      bool flag;
      try
      {
        this.ChromeDrv.Manage().Timeouts().ImplicitWait = TimeSpan.FromSeconds((double) TimerOutInSecond);
        long num = 0;
        do
        {
          Thread.Sleep(1);
          if (num >= (long) checked (TimerOutInSecond * 1000))
          {
            flag = false;
            goto label_6;
          }
        }
        while (!this.ChromeDrv.ExecuteScript("return document.readyState").Equals((object) "complete"));
      }
      catch (Exception ex)
      {
        ProjectData.SetProjectError(ex);
        flag = false;
        ProjectData.ClearProjectError();
        goto label_6;
      }
      flag = true;
label_6:
      return flag;
    }

    private bool Isloaded()
    {
      bool flag;
      try
      {
        flag = this.ChromeDrv.FindElementByClassName("_3ZW2E").Displayed;
      }
      catch (Exception ex)
      {
        ProjectData.SetProjectError(ex);
        flag = false;
        ProjectData.ClearProjectError();
      }
      return flag;
    }

    private bool IsChatDisplayed()
    {
      bool flag;
      try
      {
        flag = this.ChromeDrv.FindElementByXPath("//*[@id=\"main\"]/footer/div[1]/div[2]/div/div[2]").Displayed;
      }
      catch (Exception ex)
      {
        ProjectData.SetProjectError(ex);
        flag = false;
        ProjectData.ClearProjectError();
      }
      return flag;
    }

    public void AddLog(string Log)
    {
      WhatsappBulkSenderModule.CurrentLog = WhatsappBulkSenderModule.CurrentLog + "<" + DateAndTime.Now.ToString("yyyy-MM-dd hh:mm:ss") + ">\r\n" + Log + "\r\n";
    }

    private void BrandBrowser()
    {
    }

    private void SendFile(string FileName, string Caption)
    {
      while (true)
      {
        try
        {
          this.ChromeDrv.FindElement(By.CssSelector("span[data-icon='clip']")).Click();
          Thread.Sleep(1200);
          Thread.Sleep(checked ((int) this.Speed));
          this.ChromeDrv.FindElement(By.CssSelector("input[type='file']")).SendKeys(FileName);
          break;
        }
        catch (Exception ex)
        {
          ProjectData.SetProjectError(ex);
          ProjectData.ClearProjectError();
        }
      }
      this.WaitTocompleteLoading(3);
      if (Operators.CompareString(Caption, "", false) != 0)
      {
        do
        {
          Thread.Sleep(10);
        }
        while (!this.IsCaptionLoaded());
        Thread.Sleep(100);
        this.WaitTocompleteLoading(3);
        Thread.Sleep(checked ((int) this.Speed));
        Thread.Sleep(1000);
        try
        {
          this.SendMsg(Caption);
          Thread.Sleep(10);
        }
        catch (Exception ex)
        {
          ProjectData.SetProjectError(ex);
          ProjectData.ClearProjectError();
        }
      }
      else
      {
        try
        {
          do
          {
            Thread.Sleep(10);
          }
          while (!this.IsSendFileButtonDisplayed());
          this.ChromeDrv.FindElement(By.CssSelector("span[data-icon='send-light']")).Click();
        }
        catch (Exception ex)
        {
          ProjectData.SetProjectError(ex);
          ProjectData.ClearProjectError();
        }
      }
      this.WaitTocompleteLoading(15);
    }

    private bool IsSendFileButtonDisplayed()
    {
      bool flag;
      try
      {
        flag = this.ChromeDrv.FindElement(By.CssSelector("span[data-icon='send-light']")).Displayed;
      }
      catch (Exception ex)
      {
        ProjectData.SetProjectError(ex);
        flag = false;
        ProjectData.ClearProjectError();
      }
      return flag;
    }
  }
}
