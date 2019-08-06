Imports System
Imports System.Collections.Generic
Imports System.Linq
Imports System.Text
Imports System.Net
Imports System.Xml
Imports System.IO

Public Class WsConexao
    Public Sub New()
    End Sub

    Private Shared Function CreateRequest(strRequest As String, webServiceAddress As String) As HttpWebRequest
        Dim httpRequest = DirectCast(WebRequest.Create(webServiceAddress), HttpWebRequest)
        httpRequest.Method = "POST"
        httpRequest.ContentType = "text/xml"
        Return httpRequest
    End Function

    Public Shared Function RequestXml(strRequest As String, webServiceAddress As String) As Byte()
        Dim result As Byte() = Nothing

        Try
            ' requisição xml em bytes
            Dim sendData As Byte() = UTF8Encoding.UTF8.GetBytes(strRequest)

            ' cria a requisicão
            Dim request = CreateRequest(strRequest, webServiceAddress)
            Dim requestStream = request.GetRequestStream()

            ' envia requisição
            requestStream.Write(sendData, 0, sendData.Length)
            requestStream.Flush()
            requestStream.Dispose()

            ' captura resposta
            Dim response = DirectCast(request.GetResponse(), HttpWebResponse)
            Dim responseStream = response.GetResponseStream()

            Dim output = New MemoryStream()
            Dim buffer As Byte() = New Byte(255) {}
            Dim byteReceived As Integer = -1

            Do
                byteReceived = responseStream.Read(buffer, 0, buffer.Length)
                output.Write(buffer, 0, byteReceived)
            Loop While byteReceived > 0

            responseStream.Dispose()
            response.Close()

            buffer = output.ToArray()
            output.Dispose()

            result = buffer
        Catch ex As Exception
            Throw ex
        End Try

        Return result
    End Function

    Public Shared Function RequestStringXml(strRequest As String, webServiceAddress As String) As String
        Dim stringXml As String = ""

        Try
            ' requisição xml em bytes
            Dim sendData As Byte() = UTF8Encoding.UTF8.GetBytes(strRequest)

            ' cria a requisicão
            Dim request = CreateRequest(strRequest, webServiceAddress)
            Dim requestStream = request.GetRequestStream()

            ' envia requisição
            requestStream.Write(sendData, 0, sendData.Length)
            requestStream.Flush()
            requestStream.Dispose()

            ' captura resposta
            Dim response = DirectCast(request.GetResponse(), HttpWebResponse)
            Dim responseStream = response.GetResponseStream()

            Using reader As New StreamReader(responseStream)
                stringXml += reader.ReadToEnd()
            End Using

            responseStream.Dispose()

            response.Close()
        Catch ex As Exception

            'LogTxtHelper.Log(" Requisição " + strRequest, "SS");
            'LogTxtHelper.Log(" Retorno " + stringXml, "SS");

            Dim msg As String = " RequestStringXml - O seguinte erro ocorreu: {0} <br/> StackTrace: {1} <br/> Source: {2} <br/>" + "Inner Exception: {3} <br/> Inner Exception StackTrace: {4} <br/> Inner Exception Source: {5}"

            If ex.InnerException IsNot Nothing Then
                msg = String.Format(msg, ex.Message, ex.StackTrace, ex.Source, ex.InnerException.Message, ex.InnerException.StackTrace, _
                    ex.InnerException.Source)
            Else
                msg = String.Format(msg, ex.Message, ex.StackTrace, ex.Source, "", "", _
                    "")

                'LogTxtHelper.Log(" Erro " + msg, "SS");

                'throw ex;
            End If
        End Try

        Return stringXml
    End Function
End Class