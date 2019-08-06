
Imports IntegracaoJabur.Gv.InfraStructure.Helpers
Imports System.Collections.Generic
Imports System.IO
Imports System.IO.Compression
Imports System.Linq
Imports System.Net
Imports System.Text

Namespace IntegraJabur.Service.RequestJabur
    Public Class HttpProtocolHelper
        '*
        '        private static HttpWebRequest CreateRequest(string strRequest, string webServiceAddress)
        '        {
        '            var httpRequest = (HttpWebRequest)WebRequest.Create(webServiceAddress);
        '            httpRequest.Method = "POST";
        '            httpRequest.ContentType = "text/xml";
        '            return httpRequest;
        '        }
        '         *


        Public Shared Function RequestXml2(strRequest As String, webServiceAddress As String) As Byte()
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

                Dim encode As Encoding = System.Text.Encoding.GetEncoding("UTF-8")


                'using (StreamReader reader = new StreamReader(responseStream,encode))
                '{
                '    stringXml += reader.ReadToEnd();
                '}

                Dim readStream As New StreamReader(responseStream, encode)
                Console.WriteLine(vbLf & "Response stream received")
                Dim read As [Char]() = New [Char](255) {}

                ' Read 256 charcters at a time.    
                Dim count As Integer = readStream.Read(read, 0, 256)
                Console.WriteLine("HTML..." & vbCr & vbLf)

                While count > 0
                    ' Dump the 256 characters on a string and display the string onto the console.
                    Dim str As New [String](read, 0, count)
                    Console.Write(str)
                    count = readStream.Read(read, 0, 256)
                End While

                responseStream.Dispose()

                response.Close()
            Catch ex As Exception

                Dim msg As String = " RequestStringXml - O seguinte erro ocorreu: {0} <br/> StackTrace: {1} <br/> Source: {2} <br/>" + "Inner Exception: {3} <br/> Inner Exception StackTrace: {4} <br/> Inner Exception Source: {5}"

                If ex.InnerException IsNot Nothing Then
                    msg = String.Format(msg, ex.Message, ex.StackTrace, ex.Source, ex.InnerException.Message, ex.InnerException.StackTrace, _
                        ex.InnerException.Source)
                Else
                    msg = String.Format(msg, ex.Message, ex.StackTrace, ex.Source, "", "", _
                        "")
                End If


                'throw ex;
            End Try

            Return stringXml
        End Function

        Public Shared Function Decompressgzip2(gzip As Byte()) As Byte()
            ' Create a GZIP stream with decompression mode.
            ' ... Then create a buffer and write into while reading from the GZIP stream.
            Using stream As New GZipStream(New MemoryStream(gzip), CompressionMode.Decompress)
                Const size As Integer = 4096
                Dim buffer As Byte() = New Byte(size - 1) {}
                Using memory As New MemoryStream()
                    Dim count As Integer = 0
                    Do
                        count = stream.Read(buffer, 0, size)
                        If count > 0 Then
                            memory.Write(buffer, 0, count)
                        End If
                    Loop While count > 0
                    Return memory.ToArray()
                End Using
            End Using
        End Function

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

        Public Shared Function RequestStringXml2(strRequest As String, webServiceAddress As String) As String
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

                Dim encode As Encoding = System.Text.Encoding.GetEncoding("UTF-8")

                'using (StreamReader reader = new StreamReader(responseStream,encode))
                '{
                '    stringXml += reader.ReadToEnd();
                '}

                Dim readStream As New StreamReader(responseStream, encode)
                Console.WriteLine(vbLf & "Response stream received")
                Dim read As [Char]() = New [Char](255) {}

                ' Read 256 charcters at a time.    
                Dim count As Integer = readStream.Read(read, 0, 256)
                Console.WriteLine("HTML..." & vbCr & vbLf)

                While count > 0
                    ' Dump the 256 characters on a string and display the string onto the console.
                    Dim str As New [String](read, 0, count)
                    Console.Write(str)
                    count = readStream.Read(read, 0, 256)
                End While


                responseStream.Dispose()

                response.Close()
            Catch ex As Exception


                Dim msg As String = " RequestStringXml - O seguinte erro ocorreu: {0} <br/> StackTrace: {1} <br/> Source: {2} <br/>" + "Inner Exception: {3} <br/> Inner Exception StackTrace: {4} <br/> Inner Exception Source: {5}"

                If ex.InnerException IsNot Nothing Then
                    msg = String.Format(msg, ex.Message, ex.StackTrace, ex.Source, ex.InnerException.Message, ex.InnerException.StackTrace, _
                        ex.InnerException.Source)
                Else
                    msg = String.Format(msg, ex.Message, ex.StackTrace, ex.Source, "", "", _
                        "")
                End If

            End Try

            Return stringXml
        End Function

        Public Shared Function Decompressgzip(gzip As Byte()) As Byte()
            ' Create a GZIP stream with decompression mode.
            ' ... Then create a buffer and write into while reading from the GZIP stream.
            Using stream As New GZipStream(New MemoryStream(gzip), CompressionMode.Decompress)
                Const size As Integer = 4096
                Dim buffer As Byte() = New Byte(size - 1) {}
                Using memory As New MemoryStream()
                    Dim count As Integer = 0
                    Do
                        count = stream.Read(buffer, 0, size)
                        If count > 0 Then
                            memory.Write(buffer, 0, count)
                        End If
                    Loop While count > 0
                    Return memory.ToArray()
                End Using
            End Using
        End Function

    End Class
End Namespace
