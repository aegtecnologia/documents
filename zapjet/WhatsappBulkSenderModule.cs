// Decompiled with JetBrains decompiler
// Type: ZapJet.WhatsappBulkSenderModule
// Assembly: ZapJet, Version=4.5.0.0, Culture=neutral, PublicKeyToken=null
// MVID: A21AEBCE-0F2C-4754-8239-913DBC7B7D97
// Assembly location: C:\Program Files (x86)\Socialjet\ZAPJET SENDER\ZapJet.exe

using Microsoft.VisualBasic;
using Microsoft.VisualBasic.CompilerServices;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Net;
using System.Net.NetworkInformation;
using System.Runtime.CompilerServices;
using System.Security.Cryptography;
using System.Text;
using System.Windows.Forms;
using ZapJet.My;

namespace ZapJet
{
  [StandardModule]
  internal sealed class WhatsappBulkSenderModule
  {
    public static UserDetails _userdetails = new UserDetails();
    public static int CurrentPercentage = 0;
    public static int MaxValue = 0;
    public static bool BulkIsEnd = false;
    public static bool IsSending = false;
    public static string SentTillNow = "";
    public static string ValidNumber = "";
    public static string InvalidNumber = "";
    public static ClsSendingConfig SendingSetting = new ClsSendingConfig();
    public static int _SdialogResult = 0;
    public static bool IsDemoMode;
    public static bool IsPaused;
    public static bool IsStoped;
    public static bool IsVerificationPaused;
    public static bool IsVerificationStoped;
    public static string CurrentLog;
    public static string LastLog;
    public static int DelayBetweenSend;
    public static string total;
    public static string starttime;
    public static string endtime;
    public static string MessageSent;
    public static string Numbers;
    public static string CurrentFileName;
    public static string CurrentReportFile;
    public static string Attachments;
    public static string _SAccountName;
    public static string _SAccountPath;
    public static bool _SUseExsisting;
    public static string CriticalError;
    public static int TotalSent;
    public static int TotalFailed;
    public static string NumbersSent;
    public static int TypeMode;
    public static int TypeLimit;
    public static string TypeAccount;
    public static string TypeAccounts;

    public static void RestBulk()
    {
      WhatsappBulkSenderModule.CurrentPercentage = 0;
      WhatsappBulkSenderModule.MaxValue = 0;
      WhatsappBulkSenderModule.IsPaused = false;
      WhatsappBulkSenderModule.IsStoped = false;
      WhatsappBulkSenderModule.CurrentLog = "";
      WhatsappBulkSenderModule.LastLog = "";
      WhatsappBulkSenderModule.BulkIsEnd = false;
      WhatsappBulkSenderModule.DelayBetweenSend = 0;
      WhatsappBulkSenderModule.IsSending = false;
      WhatsappBulkSenderModule.SentTillNow = "";
      WhatsappBulkSenderModule.total = "";
      WhatsappBulkSenderModule.starttime = "";
      WhatsappBulkSenderModule.endtime = "";
      WhatsappBulkSenderModule.MessageSent = "";
      WhatsappBulkSenderModule.Numbers = "";
      WhatsappBulkSenderModule.CurrentFileName = "";
    }

    public static WhatsappBulkSenderModule.ValidateMobileNumberResult ValidateMobileNumber(string Number)
    {
      WhatsappBulkSenderModule.ValidateMobileNumberResult mobileNumberResult1 = new WhatsappBulkSenderModule.ValidateMobileNumberResult();
      if (Number.StartsWith("+"))
      {
        Number = Number.Replace(" ", "");
        Number = Number.Replace("+", "");
        Number = Number.Replace("\\", "");
        Number = Number.Replace("/", "");
        Number = Number.Replace("-", "");
        Number = Number.Replace("_", "");
        Number = Number.Replace(".", "");
      }
      WhatsappBulkSenderModule.ValidateMobileNumberResult mobileNumberResult2;
      if (Versioned.IsNumeric((object) Number))
      {
        if (Number.Length > 5 & Number.Length < 27)
        {
          mobileNumberResult1.IsValid = true;
          mobileNumberResult1.MobileNumber = Number;
          mobileNumberResult2 = mobileNumberResult1;
          goto label_8;
        }
        else
        {
          mobileNumberResult1.IsValid = false;
          mobileNumberResult1.MobileNumber = Number;
        }
      }
      else
      {
        mobileNumberResult1.IsValid = false;
        mobileNumberResult1.MobileNumber = Number;
      }
      mobileNumberResult2 = mobileNumberResult1;
label_8:
      return mobileNumberResult2;
    }

    public static void AppendTextBox(TextBox TB, string txt)
    {
label_0:
      int num1;
      int num2;
      try
      {
        ProjectData.ClearProjectError();
        num1 = 1;
label_1:
        int num3 = 2;
        if (!TB.InvokeRequired)
          goto label_3;
label_2:
        num3 = 3;
        TB.Invoke((Delegate) new WhatsappBulkSenderModule.AppendTextBoxDelegate(WhatsappBulkSenderModule.AppendTextBox), (object) TB, (object) txt);
        goto label_10;
label_3:
        num3 = 5;
        TB.AppendText(txt);
        goto label_10;
label_5:
        num2 = num3;
        switch (num1)
        {
          case 1:
            int num4 = num2 + 1;
            num2 = 0;
            switch (num4)
            {
              case 1:
                goto label_0;
              case 2:
                goto label_1;
              case 3:
                goto label_2;
              case 4:
              case 6:
                goto label_10;
              case 5:
                goto label_3;
            }
        }
      }
      catch (Exception ex) when (ex is Exception & (uint) num1 > 0U & num2 == 0)
      {
        ProjectData.SetProjectError(ex);
        goto label_5;
      }
      throw ProjectData.CreateProjectError(-2146828237);
label_10:
      if (num2 == 0)
        return;
      ProjectData.ClearProjectError();
    }

    public static string TxtID()
    {
      VBMath.Randomize();
      return "MSG" + DateAndTime.Now.ToString("yyyyMMddhhmmss") + Conversions.ToString(Conversion.Int(VBMath.Rnd() * 10f)) + Conversions.ToString(Conversion.Int(VBMath.Rnd() * 10f)) + Conversions.ToString(Conversion.Int(VBMath.Rnd() * 10f)) + Conversions.ToString(Conversion.Int(VBMath.Rnd() * 10f)) + Conversions.ToString(Conversion.Int(VBMath.Rnd() * 10f)) + Conversions.ToString(Conversion.Int(VBMath.Rnd() * 10f)) + Conversions.ToString(Conversion.Int(VBMath.Rnd() * 10f));
    }

    public static string GenerateReport()
    {
      string str1 = ZapJet.My.Resources.Resources.Report.Replace("{{DATE}}", WhatsappBulkSenderModule.starttime).Replace("{{TOTAL}}", WhatsappBulkSenderModule.total).Replace("{{SUCCESS}}", Conversions.ToString(WhatsappBulkSenderModule.TotalSent)).Replace("{{FAILED}}", Conversions.ToString(WhatsappBulkSenderModule.TotalFailed)).Replace("{{MESSAGES}}", WhatsappBulkSenderModule.MessageSent).Replace("{{ATTACHMENTS}}", WhatsappBulkSenderModule.Attachments).Replace("{{NUMBERS}}", WhatsappBulkSenderModule.NumbersSent);
      VBMath.Randomize();
      WhatsappBulkSenderModule.CurrentFileName = DateAndTime.Now.ToString("yyyyMMddhhmmss") + Conversions.ToString(Conversion.Int(VBMath.Rnd() * 99999f)) + ".html";
      try
      {
        StreamWriter streamWriter = new StreamWriter(Path.GetTempPath() + WhatsappBulkSenderModule.CurrentFileName);
        string str2 = str1;
        streamWriter.Write(str2);
        streamWriter.Close();
      }
      catch (Exception ex)
      {
        ProjectData.SetProjectError(ex);
        ProjectData.ClearProjectError();
      }
      try
      {
        ZapJet.My.Resources.Resources.logo.Save(Path.GetTempPath() + "logo.png");
      }
      catch (Exception ex)
      {
        ProjectData.SetProjectError(ex);
        ProjectData.ClearProjectError();
      }
      return Path.GetTempPath() + WhatsappBulkSenderModule.CurrentFileName;
    }

    public static void SetTheme(ref Form frm)
    {
      frm.FormBorderStyle = FormBorderStyle.None;
    }

    public static string getMacAddress()
    {
      return NetworkInterface.GetAllNetworkInterfaces()[1].GetPhysicalAddress().ToString();
    }

    public static string ConvertMactolong(string mac)
    {
      string str1 = "";
      string str2 = mac;
      int index = 0;
      while (index < str2.Length)
      {
        char ch = str2[index];
        str1 += Conversions.ToString((int) ch);
        checked { ++index; }
      }
      return str1;
    }

    public static string GetLongMac()
    {
      return WhatsappBulkSenderModule.ConvertMactolong(WhatsappBulkSenderModule.getMacAddress());
    }

    public static long GetDate()
    {
      WebClient webClient = new WebClient();
      long num;
      try
      {
        num = checked ((long) Math.Round(Conversion.Val(WhatsappBulkSenderModule.ServerDecrypt(webClient.DownloadString(ModuleConfig.ServerURL)))));
      }
      catch (Exception ex)
      {
        ProjectData.SetProjectError(ex);
        num = checked ((long) Math.Round(Conversion.Val(DateAndTime.Now.ToString("yyyyMMdd"))));
        ProjectData.ClearProjectError();
      }
      return num;
    }

    public static string Encrypt(string plainText)
    {
      string strPassword = "yourPassPhrase";
      string s1 = "mySaltValue";
      string strHashName = "SHA1";
      int iterations = 2;
      string s2 = "@1B2c3D4e5F6g7H8";
      int num1 = 256;
      byte[] bytes1 = Encoding.ASCII.GetBytes(s2);
      byte[] bytes2 = Encoding.ASCII.GetBytes(s1);
      byte[] bytes3 = Encoding.UTF8.GetBytes(plainText);
      byte[] bytes4 = new PasswordDeriveBytes(strPassword, bytes2, strHashName, iterations).GetBytes(num1 / 8);
      RijndaelManaged rijndaelManaged = new RijndaelManaged();
      int num2 = 1;
      rijndaelManaged.Mode = (CipherMode) num2;
      byte[] rgbKey = bytes4;
      byte[] rgbIV = bytes1;
      ICryptoTransform encryptor = rijndaelManaged.CreateEncryptor(rgbKey, rgbIV);
      MemoryStream memoryStream = new MemoryStream();
      CryptoStream cryptoStream = new CryptoStream((Stream) memoryStream, encryptor, CryptoStreamMode.Write);
      byte[] buffer = bytes3;
      int offset = 0;
      int length = bytes3.Length;
      cryptoStream.Write(buffer, offset, length);
      cryptoStream.FlushFinalBlock();
      byte[] array = memoryStream.ToArray();
      memoryStream.Close();
      cryptoStream.Close();
      return Convert.ToBase64String(array);
    }

    public static string Decrypt(string cipherText)
    {
      string strPassword = "yourPassPhrase";
      string s1 = "mySaltValue";
      string strHashName = "SHA1";
      int iterations = 2;
      string s2 = "@1B2c3D4e5F6g7H8";
      int num1 = 256;
      byte[] bytes1 = Encoding.ASCII.GetBytes(s2);
      byte[] bytes2 = Encoding.ASCII.GetBytes(s1);
      byte[] buffer = Convert.FromBase64String(cipherText);
      byte[] bytes3 = new PasswordDeriveBytes(strPassword, bytes2, strHashName, iterations).GetBytes(num1 / 8);
      RijndaelManaged rijndaelManaged = new RijndaelManaged();
      int num2 = 1;
      rijndaelManaged.Mode = (CipherMode) num2;
      byte[] rgbKey = bytes3;
      byte[] rgbIV = bytes1;
      ICryptoTransform decryptor = rijndaelManaged.CreateDecryptor(rgbKey, rgbIV);
      MemoryStream memoryStream = new MemoryStream(buffer);
      CryptoStream cryptoStream = new CryptoStream((Stream) memoryStream, decryptor, CryptoStreamMode.Read);
      byte[] numArray = new byte[checked (buffer.Length - 1 + 1)];
      int count = cryptoStream.Read(numArray, 0, numArray.Length);
      memoryStream.Close();
      cryptoStream.Close();
      return Encoding.UTF8.GetString(numArray, 0, count);
    }

    public static string ServerDecrypt(string cipherText)
    {
      string strPassword = "date";
      string s1 = "mySaltValue";
      string strHashName = "SHA1";
      int iterations = 2;
      string s2 = "@1B2c3D4e5F6g7H8";
      int num1 = 256;
      byte[] bytes1 = Encoding.ASCII.GetBytes(s2);
      byte[] bytes2 = Encoding.ASCII.GetBytes(s1);
      byte[] buffer = Convert.FromBase64String(cipherText);
      byte[] bytes3 = new PasswordDeriveBytes(strPassword, bytes2, strHashName, iterations).GetBytes(num1 / 8);
      RijndaelManaged rijndaelManaged = new RijndaelManaged();
      int num2 = 1;
      rijndaelManaged.Mode = (CipherMode) num2;
      byte[] rgbKey = bytes3;
      byte[] rgbIV = bytes1;
      ICryptoTransform decryptor = rijndaelManaged.CreateDecryptor(rgbKey, rgbIV);
      MemoryStream memoryStream = new MemoryStream(buffer);
      CryptoStream cryptoStream = new CryptoStream((Stream) memoryStream, decryptor, CryptoStreamMode.Read);
      byte[] numArray = new byte[checked (buffer.Length - 1 + 1)];
      int count = cryptoStream.Read(numArray, 0, numArray.Length);
      memoryStream.Close();
      cryptoStream.Close();
      return Encoding.UTF8.GetString(numArray, 0, count);
    }

    public static string GetUserSettingsFolder()
    {
      string path = Environment.GetFolderPath(Environment.SpecialFolder.UserProfile) + "\\Local Settings\\" + ModuleConfig.ApplicationTitle;
      if (!Directory.Exists(path))
        Directory.CreateDirectory(path);
      return path;
    }

    public static DateTime GetServerDate()
    {
      DateTime dateTime;
      try
      {
        dateTime = Conversions.ToDate(WhatsappBulkSenderModule.ServerDecrypt(new WebClient().DownloadString(ModuleConfig.ServerURL)));
      }
      catch (Exception ex)
      {
        ProjectData.SetProjectError(ex);
        dateTime = DateAndTime.Now;
        ProjectData.ClearProjectError();
      }
      return dateTime;
    }

    public static WhatsappBulkSenderModule.appLicense CheckCurrentLicence()
    {
      WhatsappBulkSenderModule.appLicense appLicense1 = new WhatsappBulkSenderModule.appLicense();
      string setting = Interaction.GetSetting(ModuleConfig.ApplicationTitle, "license", "key", "");
      string str1;
      try
      {
        str1 = WhatsappBulkSenderModule.Decrypt(WhatsappBulkSenderModule.Decrypt(setting));
      }
      catch (Exception ex)
      {
        ProjectData.SetProjectError(ex);
        str1 = "";
        ProjectData.ClearProjectError();
      }
      bool flag = false;
      string str2 = "Expired";
      WhatsappBulkSenderModule.appLicense appLicense2;
      try
      {
        if (Microsoft.VisualBasic.CompilerServices.Operators.CompareString(str1, "", false) != 0)
        {
          string[] strArray = Strings.Split(str1, "||||", -1, CompareMethod.Binary);
          int index1 = 0;
          string Right = strArray[index1];
          int index2 = 1;
          long num = Conversions.ToLong(strArray[index2]);
          int index3 = 2;
          string str3 = strArray[index3];
          if (Microsoft.VisualBasic.CompilerServices.Operators.CompareString(WhatsappBulkSenderModule.GetDriveSerialNumber(), Right, false) == 0 && num > WhatsappBulkSenderModule.GetDate())
          {
            flag = true;
            str2 = str3;
          }
        }
        appLicense1.valid = flag;
        appLicense1.Validtill = str2;
        appLicense2 = appLicense1;
      }
      catch (Exception ex)
      {
        ProjectData.SetProjectError(ex);
        appLicense2 = new WhatsappBulkSenderModule.appLicense()
        {
          valid = false,
          Validtill = "Expired"
        };
        ProjectData.ClearProjectError();
      }
      return appLicense2;
    }

    public static WhatsappBulkSenderModule.appLicense CheckCurrentLicence(string License)
    {
      WhatsappBulkSenderModule.appLicense appLicense1 = new WhatsappBulkSenderModule.appLicense();
      string cipherText = License;
      string str1;
      try
      {
        str1 = WhatsappBulkSenderModule.Decrypt(WhatsappBulkSenderModule.Decrypt(cipherText));
      }
      catch (Exception ex)
      {
        ProjectData.SetProjectError(ex);
        str1 = "";
        ProjectData.ClearProjectError();
      }
      bool flag = false;
      string str2 = "Expired";
      WhatsappBulkSenderModule.appLicense appLicense2;
      try
      {
        if (Microsoft.VisualBasic.CompilerServices.Operators.CompareString(str1, "", false) != 0)
        {
          string[] strArray = Strings.Split(str1, "||||", -1, CompareMethod.Binary);
          int index1 = 0;
          string Right = strArray[index1];
          int index2 = 1;
          string str3 = strArray[index2];
          int index3 = 2;
          string str4 = strArray[index3];
          if (Microsoft.VisualBasic.CompilerServices.Operators.CompareString(WhatsappBulkSenderModule.GetDriveSerialNumber(), Right, false) == 0 && Conversions.ToDouble(str3) > (double) WhatsappBulkSenderModule.GetDate())
          {
            flag = true;
            str2 = str4;
          }
        }
        appLicense1.valid = flag;
        appLicense1.Validtill = str2;
        appLicense2 = appLicense1;
      }
      catch (Exception ex)
      {
        ProjectData.SetProjectError(ex);
        appLicense2 = new WhatsappBulkSenderModule.appLicense()
        {
          valid = false,
          Validtill = "Expired"
        };
        ProjectData.ClearProjectError();
      }
      return appLicense2;
    }

    public static void SaveAccounts(ListView lst)
    {
      try
      {
        DataSet dataSet = new DataSet();
        DataTable table = new DataTable();
        table.TableName = "Accounts";
        DataColumn column1 = new DataColumn("AccountName", Type.GetType("System.String"));
        DataColumn column2 = new DataColumn("AccountPath", Type.GetType("System.String"));
        table.Columns.Add(column1);
        table.Columns.Add(column2);
        try
        {
          foreach (ListViewItem listViewItem in lst.Items)
          {
            DataRow row = table.NewRow();
            row["AccountName"] = (object) listViewItem.Text;
            row["AccountPath"] = RuntimeHelpers.GetObjectValue(listViewItem.Tag);
            table.Rows.Add(row);
          }
        }
        finally
        {
          IEnumerator enumerator;
          if (enumerator is IDisposable)
            (enumerator as IDisposable).Dispose();
        }
        dataSet.DataSetName = "WhatsApp";
        dataSet.Tables.Add(table);
        NewLateBinding.LateCall((object) dataSet, (Type) null, "WriteXml", new object[1]
        {
          Microsoft.VisualBasic.CompilerServices.Operators.ConcatenateObject(WhatsappBulkSenderModule.GetDataProfile(), (object) "\\Accounts.xml")
        }, (string[]) null, (Type[]) null, (bool[]) null, true);
      }
      catch (Exception ex)
      {
        ProjectData.SetProjectError(ex);
        int num = (int) Interaction.MsgBox((object) ex.Message, MsgBoxStyle.Critical, (object) ModuleConfig.ApplicationTitle);
        ProjectData.ClearProjectError();
      }
    }

    public static List<WhatsappBulkSenderModule.AccountDetails> LoadAccount()
    {
      List<WhatsappBulkSenderModule.AccountDetails> accountDetailsList1;
      try
      {
        DataSet dataSet = new DataSet();
        NewLateBinding.LateCall((object) dataSet, (Type) null, "ReadXml", new object[1]
        {
          Microsoft.VisualBasic.CompilerServices.Operators.ConcatenateObject(WhatsappBulkSenderModule.GetDataProfile(), (object) "\\Accounts.xml")
        }, (string[]) null, (Type[]) null, (bool[]) null, true);
        List<WhatsappBulkSenderModule.AccountDetails> accountDetailsList2 = new List<WhatsappBulkSenderModule.AccountDetails>();
        try
        {
          foreach (DataRow row in dataSet.Tables[0].Rows)
            accountDetailsList2.Add(new WhatsappBulkSenderModule.AccountDetails()
            {
              AccountName = row["AccountName"].ToString(),
              AccountPath = row["AccountPath"].ToString()
            });
        }
        finally
        {
          IEnumerator enumerator;
          if (enumerator is IDisposable)
            (enumerator as IDisposable).Dispose();
        }
        accountDetailsList1 = accountDetailsList2;
      }
      catch (Exception ex)
      {
        ProjectData.SetProjectError(ex);
        accountDetailsList1 = new List<WhatsappBulkSenderModule.AccountDetails>();
        ProjectData.ClearProjectError();
      }
      return accountDetailsList1;
    }

    public static WhatsappBulkSenderModule.AccountSwticherDetails AccountSwticher()
    {
      int num = (int) MyProject.Forms.FrmAccountSwticher.ShowDialog();
      return new WhatsappBulkSenderModule.AccountSwticherDetails()
      {
        dialogResult = WhatsappBulkSenderModule._SdialogResult,
        AccountName = WhatsappBulkSenderModule._SAccountName,
        AccountPath = WhatsappBulkSenderModule._SAccountPath,
        UseExsisting = WhatsappBulkSenderModule._SUseExsisting
      };
    }

    public static bool CheckConnection()
    {
      bool flag;
      try
      {
        new WebClient().DownloadString(ModuleConfig.ServerURL);
        flag = true;
      }
      catch (Exception ex)
      {
        ProjectData.SetProjectError(ex);
        flag = false;
        ProjectData.ClearProjectError();
      }
      return flag;
    }

    public static object GetDataProfile()
    {
      if (!Directory.Exists(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData) + "\\BulkWhatsappSender"))
      {
        try
        {
          Directory.CreateDirectory(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData) + "\\BulkWhatsappSender");
        }
        catch (Exception ex)
        {
          ProjectData.SetProjectError(ex);
          int num = (int) Interaction.MsgBox((object) ex.Message, MsgBoxStyle.Critical, (object) ModuleConfig.ApplicationTitle);
          ProjectData.ClearProjectError();
        }
      }
      if (!Directory.Exists(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData) + "\\BulkWhatsappSender\\Accounts"))
      {
        try
        {
          Directory.CreateDirectory(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData) + "\\BulkWhatsappSender\\accounts");
        }
        catch (Exception ex)
        {
          ProjectData.SetProjectError(ex);
          int num = (int) Interaction.MsgBox((object) ex.Message, MsgBoxStyle.Critical, (object) ModuleConfig.ApplicationTitle);
          ProjectData.ClearProjectError();
        }
      }
      return (object) (Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData) + "\\BulkWhatsappSender");
    }

    public static string GetDriveSerialNumber()
    {
      object objectValue1 = RuntimeHelpers.GetObjectValue(Interaction.CreateObject("Scripting.FileSystemObject", ""));
      object Instance1 = objectValue1;
      // ISSUE: variable of the null type
      __Null local1 = null;
      string MemberName1 = "GetDrive";
      object[] Arguments1 = new object[1];
      int index1 = 0;
      object Instance2 = objectValue1;
      object Instance3 = Instance2;
      // ISSUE: variable of the null type
      __Null local2 = null;
      string MemberName2 = "GetDriveName";
      object[] Arguments2 = new object[1];
      int index2 = 0;
      string startupPath;
      object obj1 = (object) (startupPath = Application.StartupPath);
      Arguments2[index2] = (object) startupPath;
      // ISSUE: variable of the null type
      __Null local3 = null;
      // ISSUE: variable of the null type
      __Null local4 = null;
      // ISSUE: variable of the null type
      __Null local5 = null;
      object obj2 = NewLateBinding.LateGet(Instance3, (Type) local2, MemberName2, Arguments2, (string[]) local3, (Type[]) local4, (bool[]) local5);
      Arguments1[index1] = obj2;
      object[] objArray = Arguments1;
      // ISSUE: variable of the null type
      __Null local6 = null;
      // ISSUE: variable of the null type
      __Null local7 = null;
      bool[] CopyBack;
      bool[] flagArray = CopyBack = new bool[1]{ true };
      object obj3 = NewLateBinding.LateGet(Instance1, (Type) local1, MemberName1, Arguments1, (string[]) local6, (Type[]) local7, CopyBack);
      if (flagArray[0])
        NewLateBinding.LateSetComplex(Instance2, (Type) null, "GetDriveName", new object[2]
        {
          obj1,
          objArray[0]
        }, (string[]) null, (Type[]) null, true, false);
      object objectValue2 = RuntimeHelpers.GetObjectValue(obj3);
      return (!Conversions.ToBoolean(NewLateBinding.LateGet(objectValue2, (Type) null, "IsReady", new object[0], (string[]) null, (Type[]) null, (bool[]) null)) ? -1 : Conversions.ToInteger(NewLateBinding.LateGet(objectValue2, (Type) null, "SerialNumber", new object[0], (string[]) null, (Type[]) null, (bool[]) null))).ToString("X2");
    }

    public class Messages
    {
      public static string DELETE_NUMBER = Language.GetTranslation("BWS_DELETE_NUMBERS");
      public static string CLEAR_LIST = Language.GetTranslation("BWS_CLEAR_LIST");
      public static string NEW_BULK = Language.GetTranslation("BWS_NEW_BULK");
      public static string NO_NUMBERS = Language.GetTranslation("BWS_NO_NUMBERS");
      public static string NO_MESSAGE = Language.GetTranslation("BWS_NO_MESSAGE");
      public static string STOP_BULK = Language.GetTranslation("BWS_STOP_BULK");
    }

    public struct ValidateMobileNumberResult
    {
      public bool IsValid;
      public string MobileNumber;
    }

    public delegate void AppendTextBoxDelegate(TextBox TB, string txt);

    public struct appLicense
    {
      public bool valid;
      public string Validtill;
    }

    public struct AccountDetails
    {
      public string AccountName;
      public string AccountPath;
    }

    public struct AccountSwticherDetails
    {
      public string AccountName;
      public string AccountPath;
      public bool UseExsisting;
      public int dialogResult;
      public List<WhatsappBulkSenderModule.AccountDetails> rotationList;
      public int limitbetweenswitch;
    }
  }
}
