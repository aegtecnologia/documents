Imports System.IO
Imports System.Xml

Public Class wsSascar
    Dim configurationAppSettings As System.Configuration.AppSettingsReader = New System.Configuration.AppSettingsReader()
    Dim WsUrl = configurationAppSettings.GetValue("webServiceUrl", GetType(System.String)).ToString
    Dim usuario = configurationAppSettings.GetValue("usuario", GetType(System.String)).ToString
    Dim senha = configurationAppSettings.GetValue("senha", GetType(System.String)).ToString
    Dim qtdPacote = configurationAppSettings.GetValue("qtdPacote", GetType(System.String)).ToString
    Dim PastaModelo As String = ".\modeloxml\"

    Public Function ObterGrupoAtuadores(ByRef dtRetorno As DataTable, Optional ByRef erro As String = "") As Boolean
        Try

            Dim ds As New DataSet()
            Dim local As String = PastaModelo & "obterGrupoAtuadores.xml"

            Dim leitura As StreamReader

            leitura = System.IO.File.OpenText(local)

            Dim xmlrequisicao As String

            xmlrequisicao = leitura.ReadToEnd()

            xmlrequisicao = xmlrequisicao.Replace("@usuario", usuario)
            xmlrequisicao = xmlrequisicao.Replace("@senha", senha)




            Dim xmlRetorno As String = ""

            xmlRetorno = WsConexao.RequestStringXml(xmlrequisicao, WsUrl)

            ds.ReadXml(New XmlTextReader(New System.IO.StringReader(xmlRetorno)))



            dtRetorno = ds.Tables("return")
            dtRetorno.TableName = "obterGrupoAtuadores"

            Return True

        Catch ex As Exception
            erro = ex.Message
            Return False
        End Try
    End Function
    Public Function ObterVeiculos(ByRef dtRetorno As DataTable, ByVal quantidade As Integer, ByVal idveiculo As Integer, Optional ByRef erro As String = "", Optional ByRef ultimoId As String = "", Optional ByRef qtdVeiculos As Integer = 0) As Boolean
        Try

            Dim ds As New DataSet()
            Dim local As String = PastaModelo & "obterVeiculos.xml"

            Dim leitura As StreamReader

            leitura = System.IO.File.OpenText(local)

            Dim xmlrequisicao As String

            xmlrequisicao = leitura.ReadToEnd()

            xmlrequisicao = xmlrequisicao.Replace("@usuario", usuario)
            xmlrequisicao = xmlrequisicao.Replace("@senha", senha)
            xmlrequisicao = xmlrequisicao.Replace("@quantidade", "300")
            'xmlrequisicao = xmlrequisicao.Replace("@quantidade", quantidade.ToString)
            'xmlrequisicao = xmlrequisicao.Replace("@idveiculo", idveiculo.ToString)
            xmlrequisicao = xmlrequisicao.Replace("@idveiculo", ultimoId)



            Dim xmlRetorno As String = ""

            xmlRetorno = WsConexao.RequestStringXml(xmlrequisicao, WsUrl)

            ds.ReadXml(New XmlTextReader(New System.IO.StringReader(xmlRetorno)))


            Try
                dtRetorno = ds.Tables("return")
                dtRetorno.TableName = "obterVeiculo"
            Catch ex As Exception
                qtdVeiculos = 0
                ultimoId = 0
                Return False
            End Try

            Dim strId As String = ""

            For Each row In dtRetorno.Rows
                strId = row("idveiculo").ToString()
            Next row

            ultimoId = strId
            qtdVeiculos = dtRetorno.Rows.Count()

            Return True

        Catch ex As Exception
            erro = ex.Message
            Return False
        End Try
    End Function
    Public Function ObterPosicoes(ByRef dsRetorno As DataSet, ByVal quantidade As Integer, Optional ByRef erro As String = "") As Boolean
        Try

            Dim ds As New DataSet()
            Dim local As String = PastaModelo & "obterPacotePosicoes.xml"

            Dim leitura As StreamReader

            leitura = System.IO.File.OpenText(local)

            Dim xmlrequisicao As String

            xmlrequisicao = leitura.ReadToEnd()

            xmlrequisicao = xmlrequisicao.Replace("@usuario", usuario)
            xmlrequisicao = xmlrequisicao.Replace("@senha", senha)
            xmlrequisicao = xmlrequisicao.Replace("@quantidade", quantidade.ToString)

            Dim xmlRetorno As String = ""

            xmlRetorno = WsConexao.RequestStringXml(xmlrequisicao, WsUrl)

            ds.ReadXml(New XmlTextReader(New System.IO.StringReader(xmlRetorno)))

            dsRetorno = ds

            Return True

        Catch ex As Exception
            erro = ex.Message
            Return False
        End Try
    End Function
    Public Function ObterMacroTd40(ByRef dtRetorno As DataTable, ByVal satelital As Integer, Optional ByRef erro As String = "") As Boolean
        Try

            Dim ds As New DataSet()
            Dim local As String = PastaModelo & "obterMacroTd40.xml"

            Dim leitura As StreamReader

            leitura = System.IO.File.OpenText(local)

            Dim xmlrequisicao As String

            xmlrequisicao = leitura.ReadToEnd()

            xmlrequisicao = xmlrequisicao.Replace("@usuario", usuario)
            xmlrequisicao = xmlrequisicao.Replace("@senha", senha)
            xmlrequisicao = xmlrequisicao.Replace("@satelital", satelital.ToString)


            Dim xmlRetorno As String = ""

            xmlRetorno = WsConexao.RequestStringXml(xmlrequisicao, WsUrl)

            ds.ReadXml(New XmlTextReader(New System.IO.StringReader(xmlRetorno)))



            dtRetorno = ds.Tables("return")
            dtRetorno.TableName = "obterMacroTd40"

            Return True

        Catch ex As Exception
            erro = ex.Message
            Return False
        End Try
    End Function
    Public Function obterMacroTd50Tmcd(ByRef dtRetorno As DataTable, ByVal tipoTeclado As String, Optional ByRef erro As String = "") As Boolean
        Try

            Dim ds As New DataSet()
            Dim local As String = PastaModelo & "obterMacroTd50Tmcd.xml"

            Dim leitura As StreamReader

            leitura = System.IO.File.OpenText(local)

            Dim xmlrequisicao As String

            xmlrequisicao = leitura.ReadToEnd()

            xmlrequisicao = xmlrequisicao.Replace("@usuario", usuario)
            xmlrequisicao = xmlrequisicao.Replace("@senha", senha)
            xmlrequisicao = xmlrequisicao.Replace("@tipoteclado", tipoTeclado.ToUpper)

            Dim xmlRetorno As String = ""

            xmlRetorno = WsConexao.RequestStringXml(xmlrequisicao, WsUrl)

            ds.ReadXml(New XmlTextReader(New System.IO.StringReader(xmlRetorno)))

            dtRetorno = ds.Tables("return")
            dtRetorno.TableName = "SASCAR_MACRO_TD50TMCD"

            Return True

        Catch ex As Exception
            erro = ex.Message
            Return False
        End Try
    End Function
    Public Function ObterMacroTd50TmcdDetalhado(ByRef dtRetorno As DataTable, ByVal tipoTeclado As String, Optional ByRef erro As String = "") As Boolean
        Try
            'tipoTeclado

            Dim ds As New DataSet()
            Dim local As String = PastaModelo & "ObterMacroTd50TmcdDetalhado.xml"

            Dim leitura As StreamReader

            leitura = System.IO.File.OpenText(local)

            Dim xmlrequisicao As String

            xmlrequisicao = leitura.ReadToEnd()

            xmlrequisicao = xmlrequisicao.Replace("@usuario", usuario)
            xmlrequisicao = xmlrequisicao.Replace("@senha", senha)
            xmlrequisicao = xmlrequisicao.Replace("@tipoteclado", tipoTeclado.ToString)

            Dim xmlRetorno As String = ""

            xmlRetorno = WsConexao.RequestStringXml(xmlrequisicao, WsUrl)

            ds.ReadXml(New XmlTextReader(New System.IO.StringReader(xmlRetorno)))



            dtRetorno = ds.Tables("return")
            dtRetorno.TableName = "ObterMacroTd50TmcdDetalhado"

            Return True

        Catch ex As Exception
            erro = ex.Message
            Return False
        End Try
    End Function
End Class
