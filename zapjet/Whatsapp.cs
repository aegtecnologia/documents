// Decompiled with JetBrains decompiler
// Type: ZapJet.Whatsapp
// Assembly: ZapJet, Version=4.5.0.0, Culture=neutral, PublicKeyToken=null
// MVID: A21AEBCE-0F2C-4754-8239-913DBC7B7D97
// Assembly location: C:\Program Files (x86)\Socialjet\ZAPJET SENDER\ZapJet.exe

using Microsoft.VisualBasic;
using Microsoft.VisualBasic.CompilerServices;
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Threading;
using System.Windows.Forms;

namespace ZapJet
{
  public class Whatsapp
  {
    private ChromeDriver ChromeDrv;
    public string Message;
    public string Caption;
    public int MessageType;
    private Thread SendingThread;
    public string Destinations;
    public List<string> Messages;
    public string FileName;
    public List<string> Friends;
    public List<string> FriendsMessage;
    public List<ListViewItem> MediaFiles;
    public long Speed;
    public int MessageDelay;
    public bool ActivateDialog;
    public int DialogAfter;
    public int DialogWait;
    public int DialoCount;
    public bool ActivateSleep;
    public int SleepAfter;
    public int SleepFor;
    public bool SendingMode;
    public bool DeleteAfterSending;
    public bool NewSession;
    public string SessionPath;
    private bool SuccessSent;
    public int DelayStart;
    public int DelayEnd;
    public List<string> DstListTx;
    public List<string> DstListNum;
    public List<string> DstListNames;
    public List<string> DstListVar1;
    public List<string> DstListVar2;
    public List<string> DstListVar3;
    public List<string> DstListVar4;
    public List<string> DstListVar5;
    public int acctypeMode;
    public int accRotationLimitation;
    public string Accsingle;
    public string accmulti;
    public int MessageCounting;
    public bool TurboMode;
    private int chnCounter;

    public event EventHandler OnSending;

    public event EventHandler OnBulkEnd;

    public event EventHandler OnChromeError;

    public Whatsapp()
    {
      this.Messages = new List<string>();
      this.Friends = new List<string>();
      this.FriendsMessage = new List<string>();
      this.MediaFiles = new List<ListViewItem>();
      this.SuccessSent = false;
      this.DstListTx = new List<string>();
      this.DstListNum = new List<string>();
      this.DstListNames = new List<string>();
      this.DstListVar1 = new List<string>();
      this.DstListVar2 = new List<string>();
      this.DstListVar3 = new List<string>();
      this.DstListVar4 = new List<string>();
      this.DstListVar5 = new List<string>();
      this.chnCounter = 0;
    }

    public void StartSending()
    {
      string destinations = this.Destinations;
      string message = this.Message;
      this.SendingThread = new Thread(new ThreadStart(this.DoSending));
      this.SendingThread.Start();
    }

    public void StopBulk()
    {
      try
      {
        WhatsappBulkSenderModule.IsStoped = true;
        WhatsappBulkSenderModule.IsSending = false;
        this.SendingThread.Abort();
        this.ChromeDrv.Quit();
      }
      catch (Exception ex)
      {
        ProjectData.SetProjectError(ex);
        ProjectData.ClearProjectError();
      }
    }

    private void DoSending()
    {
      WhatsappBulkSenderModule.NumbersSent = "";
      WhatsappBulkSenderModule.TotalSent = 0;
      WhatsappBulkSenderModule.TotalFailed = 0;
      WhatsappBulkSenderModule.CurrentPercentage = 0;
      this.MessageCounting = 0;
      try
      {
        ChromeDriverService defaultService = ChromeDriverService.CreateDefaultService();
        defaultService.HideCommandPromptWindow = true;
        ChromeOptions options = new ChromeOptions();
        switch (this.acctypeMode)
        {
          case 1:
            options.AddArguments(new string[1]
            {
              "user-data-dir=" + this.Accsingle
            });
            break;
          case 2:
            string[] strArray = Strings.Split(this.accmulti, "||", -1, CompareMethod.Binary);
            options.AddArguments(new string[1]
            {
              "user-data-dir=" + strArray[this.chnCounter]
            });
            this.chnCounter = checked (this.chnCounter + 1);
            break;
        }
        this.ChromeDrv = new ChromeDriver(defaultService, options);
      }
      catch (Exception ex1)
      {
        ProjectData.SetProjectError(ex1);
        if (!this.NewSession && ex1.Message.Contains("Chrome failed to start"))
        {
          EventArgs e = new EventArgs();
          // ISSUE: reference to a compiler-generated field
          EventHandler chromeErrorEvent = this.OnChromeErrorEvent;
          if (chromeErrorEvent != null)
            chromeErrorEvent((object) this, e);
          ProjectData.ClearProjectError();
          return;
        }
        WhatsappBulkSenderModule.IsStoped = true;
        WhatsappBulkSenderModule.IsSending = false;
        WhatsappBulkSenderModule.BulkIsEnd = true;
        try
        {
          this.ChromeDrv.Quit();
        }
        catch (Exception ex2)
        {
          ProjectData.SetProjectError(ex2);
          ProjectData.ClearProjectError();
        }
        this.EndReport();
        ProjectData.ClearProjectError();
        return;
      }
      WhatsappBulkSenderModule.starttime = DateAndTime.Now.ToString("dd MMMM yyyy HH:mm");
      WhatsappBulkSenderModule.TotalFailed = 0;
      WhatsappBulkSenderModule.TotalSent = 0;
      WhatsappBulkSenderModule.Attachments = "";
      WhatsappBulkSenderModule.IsSending = true;
      WhatsappBulkSenderModule.MaxValue = this.DstListNum.Count;
      WhatsappBulkSenderModule.total = Conversions.ToString(WhatsappBulkSenderModule.MaxValue);
      List<string> stringList1 = new List<string>();
      List<string> stringList2 = new List<string>();
      List<string> stringList3 = new List<string>();
      WhatsappBulkSenderModule.Numbers = "";
      this.LoadInjector();
      try
      {
        this.ChromeDrv.Navigate().GoToUrl("https://web.whatsapp.com/");
      }
      catch (Exception ex)
      {
        ProjectData.SetProjectError(ex);
        ProjectData.ClearProjectError();
      }
      Thread.Sleep(1200);
      do
      {
        Thread.Sleep(10);
      }
      while (!this.IsLoggedIn());
      this.WaitTocompleteLoading(4);
      int num1 = 0;
      int num2 = 0;
      WhatsappBulkSenderModule.Attachments = "";
      List<ListViewItem>.Enumerator enumerator1;
      try
      {
        enumerator1 = this.MediaFiles.GetEnumerator();
        while (enumerator1.MoveNext())
        {
          ListViewItem current = enumerator1.Current;
          WhatsappBulkSenderModule.Attachments = WhatsappBulkSenderModule.Attachments + "<tr><td style='width: 207px'><span _locid='OverviewSolutionSpan'>" + current.Tag.ToString() + "<br>" + current.SubItems[2].Text + "</span></td></tr>";
        }
      }
      finally
      {
        enumerator1.Dispose();
      }
      WhatsappBulkSenderModule.MessageSent = "";
      List<string>.Enumerator enumerator2;
      try
      {
        enumerator2 = this.Messages.GetEnumerator();
        while (enumerator2.MoveNext())
        {
          string current = enumerator2.Current;
          WhatsappBulkSenderModule.MessageSent = WhatsappBulkSenderModule.MessageSent + "<tr><td style='width: 207px'><span _locid='OverviewSolutionSpan'>" + current + "</span></td></tr>";
        }
      }
      finally
      {
        enumerator2.Dispose();
      }
      int num3 = checked (this.DstListNum.Count - 1);
      int index = 0;
      while (index <= num3)
      {
        string str1 = "";
        string str2 = "";
        string str3 = "";
        string str4 = "";
        string str5 = "";
        if (this.DstListVar1.Count > index && !Information.IsNothing((object) this.DstListVar1[index]))
          str1 = this.DstListVar1[index];
        if (this.DstListVar2.Count > index && !Information.IsNothing((object) this.DstListVar2[index]))
          str2 = this.DstListVar2[index];
        if (this.DstListVar3.Count > index && !Information.IsNothing((object) this.DstListVar3[index]))
          str3 = this.DstListVar3[index];
        if (this.DstListVar4.Count > index && !Information.IsNothing((object) this.DstListVar4[index]))
          str4 = this.DstListVar4[index];
        if (this.DstListVar5.Count > index && !Information.IsNothing((object) this.DstListVar5[index]))
          str5 = this.DstListVar5[index];
        checked { ++num1; }
        checked { ++num2; }
        if (this.ActivateDialog && num1 == this.DialogAfter)
        {
          num1 = 0;
          int num4 = 0;
          List<string>.Enumerator enumerator3;
          try
          {
            enumerator3 = this.Friends.GetEnumerator();
            while (enumerator3.MoveNext())
            {
              string current = enumerator3.Current;
              checked { ++num4; }
              if (num4 != this.DialoCount)
              {
                try
                {
                  VBMath.Randomize();
                  int num5 = checked ((int) Conversion.Int(unchecked (VBMath.Rnd() * (float) this.FriendsMessage.Count)));
                }
                catch (Exception ex)
                {
                  ProjectData.SetProjectError(ex);
                  int num5 = (int) Interaction.MsgBox((object) ex.Message, MsgBoxStyle.OkOnly, (object) null);
                  ProjectData.ClearProjectError();
                }
                this.WaitTocompleteLoading(3);
                Thread.Sleep(this.DialogWait);
              }
              else
                break;
            }
          }
          finally
          {
            enumerator3.Dispose();
          }
        }
        if (this.ActivateSleep && num2 == checked (this.SleepAfter + 1))
        {
          num2 = 0;
          this.AddLog("Sleep for:" + Conversions.ToString((double) this.SleepFor / 1000.0) + " Seconds");
          Thread.Sleep(this.SleepFor);
        }
        Application.DoEvents();
        string str6 = this.CleanNumber(this.DstListNum[index]);
        string str7;
        try
        {
          VBMath.Randomize();
          str7 = this.Messages[checked ((int) Conversion.Int(unchecked (VBMath.Rnd() * (float) this.Messages.Count)))];
        }
        catch (Exception ex)
        {
          ProjectData.SetProjectError(ex);
          str7 = "";
          ProjectData.ClearProjectError();
        }
        if (Operators.CompareString(str7, "", false) == 0)
          str7 = "{{{emplty}}}";
        if (!this.TurboMode)
        {
          long delay = this.GetDelay(this.DelayStart, this.DelayEnd);
          this.AddLog("Wait for:" + Conversions.ToString(delay / 1000L) + " Seconds ");
          Thread.Sleep(checked ((int) delay));
        }
        checked { ++WhatsappBulkSenderModule.CurrentPercentage; }
        if (Operators.CompareString(str7, "", false) != 0)
        {
          EventArgs e = new EventArgs();
          string str8 = this.ApplySpinText(str7);
          if (this.TurboMode)
          {
            string str9 = str8.Replace("[[randomtag]]", this.RandomTag()).Replace("[[VAR1]]", str1).Replace("[[VAR2]]", str2).Replace("[[VAR3]]", str3).Replace("[[VAR4]]", str4).Replace("[[VAR5]]", str5);
            string dstListName = this.DstListNames[index];
            string str10 = !(Operators.CompareString(dstListName, "", false) == 0 | Operators.CompareString(dstListName, "N/A", false) == 0) ? str9.Replace("[[fullname]]", dstListName) : str9.Replace("[[fullname]]", "");
            string[] strArray = Strings.Split(dstListName, " ", -1, CompareMethod.Binary);
            string str11 = ((IEnumerable<string>) strArray).Count<string>() <= 1 ? str10.Replace("[[firstname]]", "").Replace("[[lastname]]", "") : str10.Replace("[[firstname]]", strArray[0]).Replace("[[lastname]]", strArray[1]);
            WhatsappBulkSenderModule.NumbersSent = WhatsappBulkSenderModule.NumbersSent + "<tr><td class='IconSuccessEncoded' style='width: 15px'></td><td style='width: 207px'><span _locid='OverviewSolutionSpan'>" + this.CleanNumber(str6) + "</span></td><td style='width: 63px'>Sent</td><td class='auto-style2' style='width: 246px'><span class='auto-style1'></span></td></tr>";
            // ISSUE: reference to a compiler-generated field
            EventHandler onSendingEvent = this.OnSendingEvent;
            if (onSendingEvent != null)
              onSendingEvent((object) (this.DstListTx[index] + "|1"), e);
          }
          else if (!this.SendMsg(this.DstListTx[index], str6, str7, this.DstListNames[index], this.RandomTag(), false, str1, str2, str3, str4, str5))
          {
            // ISSUE: reference to a compiler-generated field
            EventHandler onSendingEvent = this.OnSendingEvent;
            if (onSendingEvent != null)
            {
              onSendingEvent((object) (this.DstListTx[index] + "|0"), e);
              goto label_73;
            }
            else
              goto label_73;
          }
          else
          {
            // ISSUE: reference to a compiler-generated field
            EventHandler onSendingEvent = this.OnSendingEvent;
            if (onSendingEvent != null)
              onSendingEvent((object) (this.DstListTx[index] + "|1"), e);
          }
        }
        List<ListViewItem>.Enumerator enumerator4;
        if (!this.TurboMode && this.SuccessSent)
        {
          if (this.MediaFiles.Count > 0)
          {
            try
            {
              enumerator4 = this.MediaFiles.GetEnumerator();
              while (enumerator4.MoveNext())
              {
                ListViewItem current = enumerator4.Current;
                string text = current.SubItems[2].Text;
                this.SendFile(Conversions.ToString(current.Tag), text, this.DstListNames[index], this.RandomTag());
              }
            }
            finally
            {
              enumerator4.Dispose();
            }
          }
        }
        do
        {
          Thread.Sleep(10);
        }
        while (WhatsappBulkSenderModule.IsPaused);
label_73:
        if (this.acctypeMode == 2)
        {
          this.MessageCounting = checked (this.MessageCounting + 1);
          if (this.accRotationLimitation <= checked (this.MessageCounting - 1))
          {
            this.MessageCounting = 0;
            this.AddLog("Switch account...");
            Thread.Sleep(1000);
            try
            {
              this.ChromeDrv.Quit();
            }
            catch (Exception ex)
            {
              ProjectData.SetProjectError(ex);
              ProjectData.ClearProjectError();
            }
            ChromeDriverService defaultService = ChromeDriverService.CreateDefaultService();
            defaultService.HideCommandPromptWindow = true;
            ChromeOptions options = new ChromeOptions();
            switch (this.acctypeMode)
            {
              case 2:
                string[] strArray = Strings.Split(this.accmulti, "||", -1, CompareMethod.Binary);
                options.AddArguments(new string[1]
                {
                  "user-data-dir=" + strArray[this.chnCounter]
                });
                this.chnCounter = checked (this.chnCounter + 1);
                if (this.chnCounter >= checked (Information.UBound((Array) strArray, 1) + 1))
                {
                  this.chnCounter = 0;
                  break;
                }
                break;
            }
            this.ChromeDrv = new ChromeDriver(defaultService, options);
            this.LoadInjector();
            try
            {
              this.ChromeDrv.Navigate().GoToUrl("https://web.whatsapp.com/");
            }
            catch (Exception ex)
            {
              ProjectData.SetProjectError(ex);
              ProjectData.ClearProjectError();
            }
            Thread.Sleep(1200);
            do
            {
              Thread.Sleep(10);
            }
            while (!this.IsLoggedIn());
            this.WaitTocompleteLoading(4);
          }
        }
        checked { ++index; }
      }
      this.EndReport();
    }

    public void CloseChome()
    {
      try
      {
        this.ChromeDrv.Quit();
      }
      catch (Exception ex)
      {
        ProjectData.SetProjectError(ex);
        ProjectData.ClearProjectError();
      }
    }

    public void EndReport()
    {
      Whatsapp.BulkResult bulkResult = new Whatsapp.BulkResult();
      WhatsappBulkSenderModule.endtime = DateAndTime.Now.ToString("dd MMMM yyyy HH:mm");
      bulkResult.EndTime = WhatsappBulkSenderModule.endtime;
      WhatsappBulkSenderModule.IsSending = false;
      WhatsappBulkSenderModule.BulkIsEnd = true;
      WhatsappBulkSenderModule.CurrentReportFile = WhatsappBulkSenderModule.GenerateReport();
      EventArgs e = new EventArgs();
      // ISSUE: reference to a compiler-generated field
      EventHandler onBulkEndEvent = this.OnBulkEndEvent;
      if (onBulkEndEvent == null)
        return;
      onBulkEndEvent((object) bulkResult, e);
    }

    private int GetMsgType(string FileName)
    {
      string[] strArray = Strings.Split(FileName, ".", -1, CompareMethod.Binary);
      int Rank = 1;
      int index = Information.UBound((Array) strArray, Rank);
      string Left = Strings.LCase(strArray[index]);
      return Operators.CompareString(Left, "jpg", false) == 0 ? 2 : (Operators.CompareString(Left, "gif", false) == 0 ? 2 : (Operators.CompareString(Left, "png", false) == 0 ? 2 : (Operators.CompareString(Left, "mp4", false) == 0 ? 2 : 3)));
    }

    private bool SendMsg(string MsgRef, string Mobile, string Msg, string FullName = "", string RandomTag = "", bool isFamiliar = false, string Var1 = "", string Var2 = "", string Var3 = "", string Var4 = "", string Var5 = "")
    {
      bool flag = false;
      try
      {
        this.ChromeDrv.ExecuteScript(ZapJet.My.Resources.Resources.JsExec.ToString());
        Thread.Sleep(500);
        this.ChromeDrv.ExecuteScript("document.getElementsByClassName('executor')[0].setAttribute(arguments[0], arguments[1]);", (object) "href", (object) ("https://api.whatsapp.com/send?phone=" + this.CleanNumber(Mobile)));
        Thread.Sleep(200);
        this.ChromeDrv.FindElementById("sender").Click();
        Thread.Sleep(700);
        this.WaitTocompleteLoading(3);
        Thread.Sleep(checked ((int) this.Speed));
        try
        {
          if (this.InvalidExist())
          {
            this.ChromeDrv.FindElementByClassName("_3PQ7V").Click();
            Thread.Sleep(600);
            WhatsappBulkSenderModule.NumbersSent = WhatsappBulkSenderModule.NumbersSent + "<tr><td class='IconErrorEncoded' style='width: 15px'></td><td style='width: 207px'><span _locid='OverviewSolutionSpan'>" + Mobile + "</span></td><td style='width: 63px'>Failed</td><td class='auto-style2' style='width: 246px'><span class='auto-style1'>Invalid Number or blocked Number</span></td></tr>";
            this.SuccessSent = false;
            checked { ++WhatsappBulkSenderModule.TotalFailed; }
            this.AddLog("Sending failed to: " + Mobile);
            flag = false;
            goto label_44;
          }
          else if (!isFamiliar)
          {
            WhatsappBulkSenderModule.NumbersSent = WhatsappBulkSenderModule.NumbersSent + "<tr><td class='IconSuccessEncoded' style='width: 15px'></td><td style='width: 207px'><span _locid='OverviewSolutionSpan'>" + Mobile + "</span></td><td style='width: 63px'>Sent</td><td class='auto-style2' style='width: 246px'><span class='auto-style1'></span></td></tr>";
            this.SuccessSent = true;
            this.AddLog("Successful sending to: " + Mobile);
            flag = true;
          }
          else
          {
            this.AddLog("Familiar sending to: " + Mobile);
            flag = true;
          }
        }
        catch (Exception ex)
        {
          ProjectData.SetProjectError(ex);
          if (!isFamiliar)
          {
            WhatsappBulkSenderModule.NumbersSent = WhatsappBulkSenderModule.NumbersSent + "<tr><td class='IconSuccessEncoded' style='width: 15px'></td><td style='width: 207px'><span _locid='OverviewSolutionSpan'>" + Mobile + "</span></td><td style='width: 63px'>Sent</td><td class='auto-style2' style='width: 246px'><span class='auto-style1'></span></td></tr>";
            this.SuccessSent = true;
            this.AddLog("Successful sending to: " + Mobile);
          }
          else
            this.AddLog("Familiar sending to: " + Mobile);
          flag = true;
          ProjectData.ClearProjectError();
        }
        Thread.Sleep(500);
      }
      catch (Exception ex)
      {
        ProjectData.SetProjectError(ex);
        ProjectData.ClearProjectError();
      }
      WhatsappBulkSenderModule.TotalSent = checked ((int) Math.Round(unchecked (Conversions.ToDouble(WhatsappBulkSenderModule.total) - (double) WhatsappBulkSenderModule.TotalFailed)));
      Msg = Msg.Replace("{{{emplty}}}", "");
      Msg = Msg.Replace("[[randomtag]]", RandomTag);
      Msg = Msg.Replace("[[VAR1]]", Var1);
      Msg = Msg.Replace("[[VAR2]]", Var2);
      Msg = Msg.Replace("[[VAR3]]", Var3);
      Msg = Msg.Replace("[[VAR4]]", Var4);
      Msg = Msg.Replace("[[VAR5]]", Var5);
      Msg = this.ApplySpinText(Msg);
      Msg = !(Operators.CompareString(FullName, "", false) == 0 | Operators.CompareString(FullName, "N/A", false) == 0) ? Msg.Replace("[[fullname]]", FullName) : Msg.Replace("[[fullname]]", "");
      string[] strArray1 = Strings.Split(FullName, " ", -1, CompareMethod.Binary);
      if (((IEnumerable<string>) strArray1).Count<string>() > 1)
      {
        Msg = Msg.Replace("[[firstname]]", strArray1[0]);
        Msg = Msg.Replace("[[lastname]]", strArray1[1]);
      }
      else
      {
        Msg = Msg.Replace("[[firstname]]", "");
        Msg = Msg.Replace("[[lastname]]", "");
      }
      if (this.SendingMode)
      {
        string[] strArray2 = Strings.Split(Msg, "||||", -1, CompareMethod.Binary);
        string[] strArray3 = strArray2;
        int index1 = 0;
        while (index1 < strArray3.Length)
        {
          string str1 = strArray3[index1];
          if (Operators.CompareString(Msg, "", false) != 0)
          {
            string str2 = str1;
            int index2 = 0;
            while (index2 < str2.Length)
            {
              char ch = str2[index2];
              try
              {
                this.ChromeDrv.FindElementByClassName("_3u328").SendKeys(Conversions.ToString(ch));
              }
              catch (Exception ex)
              {
                ProjectData.SetProjectError(ex);
                Application.DoEvents();
                ProjectData.ClearProjectError();
              }
              checked { ++index2; }
            }
          }
          Thread.Sleep(10);
          int num;
          if (num < Information.UBound((Array) strArray2, 1))
          {
            try
            {
              this.ChromeDrv.FindElementByClassName("_3u328").SendKeys(OpenQA.Selenium.Keys.Shift + OpenQA.Selenium.Keys.Enter);
            }
            catch (Exception ex)
            {
              ProjectData.SetProjectError(ex);
              ProjectData.ClearProjectError();
            }
          }
          checked { ++num; }
          checked { ++index1; }
        }
        try
        {
          this.ChromeDrv.FindElementByClassName("_3u328").SendKeys(OpenQA.Selenium.Keys.Enter);
        }
        catch (Exception ex)
        {
          ProjectData.SetProjectError(ex);
          ProjectData.ClearProjectError();
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
        }
        catch (Exception ex)
        {
          ProjectData.SetProjectError(ex);
          ProjectData.ClearProjectError();
        }
        Thread.Sleep(10);
        try
        {
          this.ChromeDrv.FindElementByClassName("_3u328").SendKeys(OpenQA.Selenium.Keys.Backspace);
        }
        catch (Exception ex)
        {
          ProjectData.SetProjectError(ex);
          ProjectData.ClearProjectError();
        }
        Thread.Sleep(10);
        try
        {
          this.ChromeDrv.FindElementByClassName("_3u328").SendKeys(OpenQA.Selenium.Keys.Enter);
        }
        catch (Exception ex)
        {
          ProjectData.SetProjectError(ex);
          ProjectData.ClearProjectError();
        }
      }
label_44:
      return flag;
    }

    public bool InvalidExist()
    {
      bool flag;
      try
      {
        flag = this.ChromeDrv.FindElementsByClassName("_3PQ7V").Count > 0;
      }
      catch (Exception ex)
      {
        ProjectData.SetProjectError(ex);
        flag = true;
        ProjectData.ClearProjectError();
      }
      return flag;
    }

    public string ExecuteChromeScript(string Script)
    {
      string message;
      try
      {
        message = this.ChromeDrv.ExecuteScript(Script).ToString();
      }
      catch (Exception ex)
      {
        ProjectData.SetProjectError(ex);
        message = ex.Message;
        ProjectData.ClearProjectError();
      }
      return message;
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

    private void SendFile(string FileName, string Caption, string FullName = "", string RandomTag = "")
    {
      try
      {
        string[] strArray = Strings.Split(FileName, "\\", -1, CompareMethod.Binary);
        this.AddLog("Sending file: " + strArray[Information.UBound((Array) strArray, 1)]);
      }
      catch (Exception ex)
      {
        ProjectData.SetProjectError(ex);
        ProjectData.ClearProjectError();
      }
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
      Thread.Sleep(1000);
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
        try
        {
          Caption = Caption.Replace("||||", "\r\n");
          Caption = Caption.Replace("[[randomtag]]", RandomTag);
          Caption = !(Operators.CompareString(FullName, "", false) == 0 | Operators.CompareString(FullName, "N/A", false) == 0) ? Caption.Replace("[[fullname]]", FullName) : Caption.Replace("[[fullname]]", "");
          string[] strArray = Strings.Split(FullName, " ", -1, CompareMethod.Binary);
          if (((IEnumerable<string>) strArray).Count<string>() > 1)
          {
            Caption = Caption.Replace("[[firstname]]", strArray[0]);
            Caption = Caption.Replace("[[lastname]]", strArray[1]);
          }
          else
          {
            Caption = Caption.Replace("[[firstname]]", "");
            Caption = Caption.Replace("[[lastname]]", "");
          }
          try
          {
            this.ChromeDrv.ExecuteScript("document.getElementsByClassName('_3u328')[0].innerText=arguments[0]", (object) Caption);
          }
          catch (Exception ex)
          {
            ProjectData.SetProjectError(ex);
            ProjectData.ClearProjectError();
          }
          try
          {
            this.ChromeDrv.FindElementByClassName("_3u328").SendKeys(OpenQA.Selenium.Keys.Shift + OpenQA.Selenium.Keys.Enter);
          }
          catch (Exception ex)
          {
            ProjectData.SetProjectError(ex);
            ProjectData.ClearProjectError();
          }
          Thread.Sleep(10);
          try
          {
            this.ChromeDrv.FindElementByClassName("_3u328").SendKeys(OpenQA.Selenium.Keys.Backspace);
          }
          catch (Exception ex)
          {
            ProjectData.SetProjectError(ex);
            ProjectData.ClearProjectError();
          }
          Thread.Sleep(100);
          try
          {
            this.ChromeDrv.FindElementByClassName("_3u328").SendKeys(OpenQA.Selenium.Keys.Enter);
          }
          catch (Exception ex)
          {
            ProjectData.SetProjectError(ex);
            ProjectData.ClearProjectError();
          }
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
      this.WaitTocompleteLoading(3);
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
        flag = this.ChromeDrv.FindElementByClassName("_3lLzD").Displayed;
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
      WhatsappBulkSenderModule.CurrentLog = WhatsappBulkSenderModule.CurrentLog + DateAndTime.Now.ToString("hh:mm:ss") + ">" + Log + "\r\n";
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

    public long GetDelay(int startDelay, int EndDelay)
    {
      VBMath.Randomize();
      return checked ((long) Math.Round(unchecked ((double) startDelay + (double) Conversion.Int(VBMath.Rnd() * (float) EndDelay) * 1000.0)));
    }

    public string ApplySpinText(string Text)
    {
      string str1 = Text;
      List<DictionaryEntry> dictionaryEntryList = new List<DictionaryEntry>();
      string oldValue = "{{";
      string newValue = "||{{";
      string[] strArray1 = Strings.Split(str1.Replace(oldValue, newValue).Replace("}}", "}}||"), "||", -1, CompareMethod.Binary);
      int index1 = 0;
      DictionaryEntry dictionaryEntry;
      while (index1 < strArray1.Length)
      {
        string str2 = strArray1[index1];
        if (str2.Trim().StartsWith("{{") & str2.Trim().EndsWith("}}"))
        {
          string[] strArray2 = str2.Replace("{{", "").Replace("}}", "").Split('|');
          if (((IEnumerable<string>) strArray2).Count<string>() > 0)
          {
            VBMath.Randomize();
            int num = 0;
            int index2;
            do
            {
              index2 = checked ((int) Conversion.Int(unchecked (VBMath.Rnd() * (float) ((IEnumerable<string>) strArray2).Count<string>())));
              checked { ++num; }
            }
            while (num <= 30);
            dictionaryEntry = new DictionaryEntry((object) str2, (object) strArray2[index2]);
            dictionaryEntryList.Add(dictionaryEntry);
          }
        }
        checked { ++index1; }
      }
      string str3 = Text;
      List<DictionaryEntry>.Enumerator enumerator;
      try
      {
        enumerator = dictionaryEntryList.GetEnumerator();
        while (enumerator.MoveNext())
        {
          dictionaryEntry = enumerator.Current;
          object[] objArray;
          bool[] flagArray;
          object obj = NewLateBinding.LateGet((object) str3, (Type) null, "Replace", objArray = new object[2]
          {
            dictionaryEntry.Key,
            dictionaryEntry.Value
          }, (string[]) null, (Type[]) null, flagArray = new bool[2]
          {
            true,
            true
          });
          if (flagArray[0])
            dictionaryEntry.Key = RuntimeHelpers.GetObjectValue(RuntimeHelpers.GetObjectValue(objArray[0]));
          if (flagArray[1])
            dictionaryEntry.Value = RuntimeHelpers.GetObjectValue(RuntimeHelpers.GetObjectValue(objArray[1]));
          str3 = Conversions.ToString(obj);
        }
      }
      finally
      {
        enumerator.Dispose();
      }
      return str3;
    }

    public struct BulkResult
    {
      public string EndTime;
    }
  }
}
