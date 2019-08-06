Imports System.Collections.Generic
Imports System.Linq
Imports System.Text
Imports System.Net
Imports System.Xml
Imports System.IO

Namespace Gv.InfraStructure.Helpers
    Public Class HttpSoapHelper

        Public Function RequestWebServiceOmnidata(xml As String, url As String, action As String, username As String, passwd As String) As String
            Dim retorno As String = ""

            Dim XMLResponse As XmlDocument = Nothing
            Dim httpResponse As HttpWebResponse = Nothing
            Dim requestStream As Stream = Nothing
            Dim responseStream As Stream = Nothing
            Dim xmlReader As XmlTextReader

            Try

                Dim httpRequest As WebRequest = WebRequest.Create(url)
                Dim myCredentials As New NetworkCredential(username, passwd)
                httpRequest.Credentials = myCredentials

                httpRequest = DirectCast(WebRequest.Create(url), HttpWebRequest)

                'http://stackoverflow.com/questions/4326598/connecting-to-salesforce-bulk-api-using-c-sharp site de referencia

                'XmlDeclaration xmldecl;
                'xmldecl = doc.CreateXmlDeclaration("1.0", null, null);
                'xmldecl.Encoding = "UTF-8";
                'xmldecl.Standalone = "yes";  

                Dim bytes As Byte() = Encoding.ASCII.GetBytes(xml)
                httpRequest.Method = "POST"
                httpRequest.ContentLength = bytes.Length
                httpRequest.ContentType = "text/xml; charset=UTF-8"
                httpRequest.Headers.Add("SOAPAction: login")
                httpRequest.Headers(System.Net.HttpRequestHeader.Authorization) = "Basic " + Convert.ToBase64String(Encoding.UTF8.GetBytes(Convert.ToString(username & Convert.ToString(":")) & passwd))

                System.Net.ServicePointManager.Expect100Continue = False

                requestStream = httpRequest.GetRequestStream()
                requestStream.Write(bytes, 0, bytes.Length)
                requestStream.Close()

                httpResponse = DirectCast(httpRequest.GetResponse(), HttpWebResponse)

                If httpResponse.StatusCode = HttpStatusCode.OK Then
                    responseStream = httpResponse.GetResponseStream()

                    xmlReader = New XmlTextReader(responseStream)

                    Dim xmldoc As New XmlDocument()
                    xmldoc.Load(xmlReader)

                    XMLResponse = xmldoc
                    xmlReader.Close()

                    retorno = XMLResponse.InnerXml.ToString()
                End If

                httpResponse.Close()
            Catch ex As Exception
                'throw;
            End Try

            Return retorno

        End Function



    End Class
End Namespace