// Decompiled with JetBrains decompiler
// Type: ZapJet.WhatsappVerification
// Assembly: ZapJet, Version=4.5.0.0, Culture=neutral, PublicKeyToken=null
// MVID: A21AEBCE-0F2C-4754-8239-913DBC7B7D97
// Assembly location: C:\Program Files (x86)\Socialjet\ZAPJET SENDER\ZapJet.exe

using Microsoft.VisualBasic;
using Microsoft.VisualBasic.CompilerServices;
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using System;
using System.Collections;
using System.Drawing;
using System.Threading;
using System.Windows.Forms;

namespace ZapJet
{
  public class WhatsappVerification
  {
    private ChromeDriver ChromeDrv;
    public ListBox Destinations;
    public string Message;
    public string FileName;
    public string Caption;
    public long Speed;
    public int MessageType;
    public int MessageDelay;
    private Thread VerificationThread;
    public bool StopFilter;
    public bool IsWorking;
    public int VerificationSpeed;

    public void StartVerification()
    {
      string message = this.Message;
      this.VerificationThread = new Thread(new ThreadStart(this.DoVerification));
      this.VerificationThread.Start();
    }

    private void DoVerification()
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
        try
        {
          this.ChromeDrv.Close();
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
            this.ChromeDrv.Manage().Window.Position = new Point(-10000, -10000);
            this.ChromeDrv.ExecuteScript(ZapJet.My.Resources.Resources.JsExec.ToString());
            IEnumerator enumerator;
            try
            {
              enumerator = this.Destinations.Items.GetEnumerator();
label_28:
              while (enumerator.MoveNext())
              {
                string Number = Conversions.ToString(enumerator.Current);
                try
                {
                  this.ChromeDrv.ExecuteScript("document.getElementsByClassName('executor')[0].setAttribute(arguments[0], arguments[1]);", (object) "href", (object) ("https://api.whatsapp.com/send?phone=" + this.CleanNumber(Number)));
                  this.ChromeDrv.FindElementById("sender").Click();
                  Thread.Sleep(this.VerificationSpeed);
                  while (!this.StopFilter)
                  {
                    if (this.IsInvaid())
                    {
                      WhatsappBulkSenderModule.InvalidNumber = Number;
                      try
                      {
                        while (this.IsInvaid())
                          this.ChromeDrv.FindElementByClassName("_3PQ7V").Click();
                        goto label_28;
                      }
                      catch (Exception ex)
                      {
                        ProjectData.SetProjectError(ex);
                        ProjectData.ClearProjectError();
                        goto label_28;
                      }
                    }
                    else if (this.IsChatDisplayed())
                    {
                      WhatsappBulkSenderModule.ValidNumber = Number;
                      goto label_28;
                    }
                  }
                  this.IsWorking = false;
                  this.StopFilter = false;
                  this.ChromeDrv.Close();
                  return;
                }
                catch (Exception ex)
                {
                  ProjectData.SetProjectError(ex);
                  ProjectData.ClearProjectError();
                }
              }
            }
            finally
            {
              if (enumerator is IDisposable)
                (enumerator as IDisposable).Dispose();
            }
            this.IsWorking = false;
            try
            {
              this.ChromeDrv.Close();
            }
            catch (Exception ex)
            {
              ProjectData.SetProjectError(ex);
              ProjectData.ClearProjectError();
            }
            int num = (int) Interaction.MsgBox((object) "Finish Verifying numbers.", MsgBoxStyle.Information, (object) ModuleConfig.ApplicationTitle);
            return;
          }
        }
        this.IsWorking = false;
        this.StopFilter = false;
        try
        {
          this.ChromeDrv.Close();
        }
        catch (Exception ex)
        {
          ProjectData.SetProjectError(ex);
          ProjectData.ClearProjectError();
        }
      }
    }

    public void EndReport()
    {
      WhatsappBulkSenderModule.endtime = DateAndTime.Now.ToString("dd MMMM yyyy HH:mm");
      WhatsappBulkSenderModule.IsSending = false;
      WhatsappBulkSenderModule.BulkIsEnd = true;
      WhatsappBulkSenderModule.CurrentReportFile = WhatsappBulkSenderModule.GenerateReport();
    }

    private void SendMsg(string Msg, int msgtype)
    {
      if (msgtype != 0)
      {
        string[] strArray1 = Strings.Split(Msg, "||||", -1, CompareMethod.Binary);
        string[] strArray2 = strArray1;
        int index = 0;
        while (index < strArray2.Length)
        {
          this.ChromeDrv.FindElementByClassName("_3u328").SendKeys(strArray2[index]);
          Thread.Sleep(10);
          int num;
          if (num < Information.UBound((Array) strArray1, 1))
            this.ChromeDrv.FindElementByClassName("_3u328").SendKeys(OpenQA.Selenium.Keys.Shift + OpenQA.Selenium.Keys.Enter);
          checked { ++num; }
          checked { ++index; }
        }
      }
      else
      {
        Msg = Msg.Replace("||||", "\r\n");
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
        }
        catch (Exception ex)
        {
          ProjectData.SetProjectError(ex);
          ProjectData.ClearProjectError();
        }
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
      try
      {
        this.ChromeDrv.FindElements(By.ClassName("_3AwwN"))[0].SendKeys("Bulk Whatsapp sender");
      }
      catch (Exception ex)
      {
        ProjectData.SetProjectError(ex);
        ProjectData.ClearProjectError();
      }
    }
  }
}
