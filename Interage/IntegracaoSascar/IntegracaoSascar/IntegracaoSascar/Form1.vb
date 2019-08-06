
Imports IntegracaoSascar.IntegraJabur.Service.RequestJabur
Imports System.Collections.Generic
Imports System.Configuration
Imports System.Data.SqlClient
Imports System.Linq
Imports System.Text
Imports System.Xml
Imports System.Collections.Specialized
Imports System.Threading


Public Class Form1

    Dim DtVeiculos As DataTable
    Dim DtMacro_TD50TMCD As DataTable
    Dim DtMacro_TD40 As DataTable
    Dim DtPosicoes As New DataTable
    Dim DtGrupoAtuadores As DataTable


    Private thread_Executa As Thread = Nothing

    Private ultimoPacote As DateTime = Nothing

    Private inicio As Boolean

    Private Sub Form1_FormClosing(sender As [Object], e As FormClosingEventArgs) Handles Me.FormClosing
        While thread_Executa.IsAlive = True
            thread_Executa.Abort()
        End While
    End Sub

    Private Sub Form1_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        CheckForIllegalCrossThreadCalls = False
        inicio = True
        thread_Executa = New Thread(AddressOf ExecutaMetodo)
        thread_Executa.Start()
    End Sub

    Public Sub ExecutaMetodo()
        While inicio = True
            salvaVeiculo()
            IntegracaoSascar()
        End While
    End Sub

    Public Sub salvaVeiculo()

        Dim VEICULOS As New wsSascar
        Dim dtVeiculo As New DataTable
        Dim retornoVeiculo As String = ""

        Dim quantidadeVeiculo As Integer = 0
        Dim strUltimoId As String = "0"

linhaObterVeiculo:

        VEICULOS.ObterVeiculos(dtVeiculo, 0, 0, retornoVeiculo, strUltimoId, quantidadeVeiculo)
        Try
            Dim conn As New Conexao


            If Not conn.SalvaLote(dtVeiculo, retornoVeiculo) Then

            End If
        Catch ex As Exception

        End Try

        If quantidadeVeiculo <> 0 Then
            GoTo linhaObterVeiculo
        End If



    End Sub

    Public Sub IntegracaoSascar()
        ultimoLabel.Text = "Último serviço iniciado em " & DateTime.Now.ToString("dd/MM/yyyy HH:mm:ss")

        Dim dt As New DataTable
        Dim retorno As String = ""
        SalvarPosicoes(dt, retorno)

        'System.Threading.Thread.Sleep(180000)
    End Sub


    Public Function SalvarPosicoes(ByRef dtRetorno As DataTable, ByRef retorno As String) As Boolean
        Try

            If Not CarregaPosicoes(dtRetorno, retorno) Then
                Return False
            End If

            retorno += "SalvarInicio = " & System.DateTime.Now & vbNewLine

            Dim conn As New Conexao

            If Not conn.SalvaLote(dtRetorno, retorno) Then
                Return False
            End If

            retorno += "SalvarFim = " & System.DateTime.Now & vbNewLine

            conn = Nothing

            SalvarLog(dtRetorno.TableName, "Inserido " & dtRetorno.Rows.Count & " registros")

            Return True
        Catch ex As Exception
            retorno += "Erro = " & System.DateTime.Now & vbNewLine
            retorno += ex.Message
            Return False

        End Try
    End Function


    Private Sub CriaDtPosicoes()
        Try

            DtPosicoes = New DataTable
            DtPosicoes.TableName = "SASCAR_Posicao"

            DtPosicoes.Columns.Add("ID", System.Type.GetType("System.Int32"))
            DtPosicoes.Columns.Add("IDVEICULO", System.Type.GetType("System.Int32"))
            DtPosicoes.Columns.Add("DATAPOSICAO", System.Type.GetType("System.DateTime"))
            DtPosicoes.Columns.Add("DATAPACOTE", System.Type.GetType("System.DateTime"))
            DtPosicoes.Columns.Add("LATITUDE", System.Type.GetType("System.String"))
            DtPosicoes.Columns.Add("LONGITUDE", System.Type.GetType("System.String"))
            DtPosicoes.Columns.Add("VELOCIDADE", System.Type.GetType("System.Int32"))
            DtPosicoes.Columns.Add("IGNICAO", System.Type.GetType("System.Int32"))
            DtPosicoes.Columns.Add("UF", System.Type.GetType("System.String"))
            DtPosicoes.Columns.Add("CIDADE", System.Type.GetType("System.String"))
            DtPosicoes.Columns.Add("RUA", System.Type.GetType("System.String"))
            DtPosicoes.Columns.Add("PONTOREFERENCIA", System.Type.GetType("System.String"))
            DtPosicoes.Columns.Add("TEMPERATURA1", System.Type.GetType("System.Int32"))
            DtPosicoes.Columns.Add("CODIGOMACRO", System.Type.GetType("System.Int32"))


            DtPosicoes.Columns.Add("TEMPERATURA2", System.Type.GetType("System.Int32"))
            DtPosicoes.Columns.Add("TEMPERATURA3", System.Type.GetType("System.Int32"))
            DtPosicoes.Columns.Add("RPM", System.Type.GetType("System.Int32"))
            DtPosicoes.Columns.Add("ODOMETRO", System.Type.GetType("System.Int32"))
            DtPosicoes.Columns.Add("TENSAO", System.Type.GetType("System.Int32"))
            DtPosicoes.Columns.Add("GPS", System.Type.GetType("System.Int32"))
            DtPosicoes.Columns.Add("ANGULOREFERENCIA", System.Type.GetType("System.Int32"))
            DtPosicoes.Columns.Add("SAIDA1", System.Type.GetType("System.Int32"))
            DtPosicoes.Columns.Add("SAIDA2", System.Type.GetType("System.Int32"))
            DtPosicoes.Columns.Add("SAIDA3", System.Type.GetType("System.Int32"))
            DtPosicoes.Columns.Add("SAIDA4", System.Type.GetType("System.Int32"))
            DtPosicoes.Columns.Add("SAIDA5", System.Type.GetType("System.Int32"))
            DtPosicoes.Columns.Add("SAIDA6", System.Type.GetType("System.Int32"))
            DtPosicoes.Columns.Add("SAIDA7", System.Type.GetType("System.Int32"))
            DtPosicoes.Columns.Add("SAIDA8", System.Type.GetType("System.Int32"))
            DtPosicoes.Columns.Add("ENTRADA1", System.Type.GetType("System.Int32"))
            DtPosicoes.Columns.Add("ENTRADA2", System.Type.GetType("System.Int32"))
            DtPosicoes.Columns.Add("ENTRADA3", System.Type.GetType("System.Int32"))
            DtPosicoes.Columns.Add("ENTRADA4", System.Type.GetType("System.Int32"))
            DtPosicoes.Columns.Add("ENTRADA5", System.Type.GetType("System.Int32"))
            DtPosicoes.Columns.Add("ENTRADA6", System.Type.GetType("System.Int32"))
            DtPosicoes.Columns.Add("ENTRADA7", System.Type.GetType("System.Int32"))
            DtPosicoes.Columns.Add("ENTRADA8", System.Type.GetType("System.Int32"))
            DtPosicoes.Columns.Add("EVENTOS", System.Type.GetType("System.String"))
            DtPosicoes.Columns.Add("STATUS_MACROS", System.Type.GetType("System.Int32"))
            DtPosicoes.Columns.Add("STATUS_EVENTOS", System.Type.GetType("System.Int32"))
            DtPosicoes.Columns.Add("STATUS_POSICAO", System.Type.GetType("System.Int32"))


            'RPM()
            'ODOMETRO()
            'TENSAO()
            'SAIDA()
            'ENTRADA()
            'TEMPERATURA()
            'GPS()
            'anguloReferencia()


        Catch ex As Exception

        End Try
    End Sub
    Private Function CarregaPosicoes(ByRef tabela As DataTable, ByRef retorno As String) As Boolean
        Try

            Dim wsDataset As New DataSet
            Dim ws As New wsSascar()
            Dim configurationAppSettings As System.Configuration.AppSettingsReader = New System.Configuration.AppSettingsReader()

            Dim ultimoId As Integer = 0
            Dim quantidade As Integer = configurationAppSettings.GetValue("qtdPacote", GetType(System.String)).ToString

            retorno += "WsInicio = " & System.DateTime.Now & vbNewLine

            CriaDtPosicoes()


            ws.ObterPosicoes(wsDataset, quantidade)


            If Not wsDataset.Tables.Contains("return") Then
                Return False
            End If
            quantidade = wsDataset.Tables("return").Rows.Count
            tabela.Merge(wsDataset.Tables("return"))

            If quantidade = 0 Then
                retorno += "WsFim(sem retorno) = " & System.DateTime.Now & vbNewLine
                Return False
            End If

            'If quantidade = 1000 Then
            '    ultimoId = wsTabela.Rows(wsTabela.Rows.Count - 1).Item("idveiculo")
            '    GoTo inicio
            'End If

            retorno += "WsFim = " & System.DateTime.Now & vbNewLine

            retorno += "FormatInicio = " & System.DateTime.Now & vbNewLine

            'If Not tabela.Columns.Contains("CODIGOMACRO") Then
            '    Return False
            'End If

            Dim codigoMacro As String

            For Each item As DataRow In tabela.Rows

                'codigoMacro = IIf(IsDBNull(item.Item("CODIGOMACRO")), "", item.Item("CODIGOMACRO"))

                'If codigoMacro <> "" Then


                Dim indice As Integer = 0
                DtPosicoes.Rows.Add()
                indice = DtPosicoes.Rows.Count - 1
                Dim eventos As String = ""

                For Each dr As DataRow In wsDataset.Tables("eventos").Select("return_id=" & item.Item("return_id"))

                    eventos += dr.Item("codigo") & ";"

                Next

                DtPosicoes.Rows(indice).Item("eventos") = eventos


                For i As Integer = 0 To DtPosicoes.Columns.Count - 1
                    Dim nomeTabela As String = DtPosicoes.Columns(i).ColumnName

                    If nomeTabela.ToLower <> "id" Then

                        If nomeTabela.ToLower = "STATUS_MACROS" Or nomeTabela.ToLower = "STATUS_EVENTOS" Or nomeTabela.ToLower = "STATUS_POSICAO" Then
                            'DtPosicoes.Rows(indice).Item(i) = Nothing
                        End If

                        If tabela.Columns.Contains(nomeTabela) Then
                            DtPosicoes.Rows(indice).Item(i) = item.Item(nomeTabela)

                        End If

                    End If

                Next

                'End If

            Next

            'FINAL:


            tabela = New DataTable
            tabela = DtPosicoes

            retorno += "FormatFim = " & System.DateTime.Now & vbNewLine

            Return True

        Catch ex As Exception
            retorno += "Erro = " & System.DateTime.Now & vbNewLine
            retorno += ex.Message
            Return False
        End Try
    End Function
    Private Function SalvarLog(ByVal tabela As String, ByVal descricao As String) As Boolean
        Try
            Dim conn As New Conexao
            Dim sql As New StringBuilder

            sql.AppendLine("insert into sascar_log values (GETDATE(),")
            sql.Append("'")
            sql.Append(tabela)
            sql.AppendLine("',")
            sql.Append("'")
            sql.Append(descricao)
            sql.AppendLine("')")

            If conn.ExecutaComando(sql.ToString) Then
                Return True
            Else
                Return False
            End If



        Catch ex As Exception
            Return False
        End Try

    End Function
    Sub _Executar(strQuery As String)
        Try

            Dim configurationAppSettings As System.Configuration.AppSettingsReader = New System.Configuration.AppSettingsReader()

            Dim connection As String = configurationAppSettings.GetValue("ConexaoGVLogistica", GetType(System.String)).ToString() 'ConfigurationManager.ConnectionStrings("GvLogisticaDbContext").ConnectionString

            Using cn As New SqlConnection(connection)
                Dim cmd As New SqlCommand(strQuery, cn)
                cmd.CommandTimeout = 5000
                cmd.CommandType = System.Data.CommandType.Text
                cn.Open()
                cmd.ExecuteNonQuery()
                cn.Close()
            End Using
        Catch ex As Exception
            'LogTxtHelper.Log(" Erro ao buscar as posicoes " + ex.ToString(), "SascarService")
        End Try

    End Sub

    Private Function _obterIdJabur() As String

        Dim idJabur As String = "0"

        Try

            Dim reader As SqlDataReader = Nothing

            Dim configurationAppSettings As System.Configuration.AppSettingsReader = New System.Configuration.AppSettingsReader()

            Dim connection As String = configurationAppSettings.GetValue("ConexaoGVLogistica", GetType(System.String)).ToString() 'ConfigurationManager.ConnectionStrings("GvLogisticaDbContext").ConnectionString

            '"select max(CodRequisicaoTecnologia) as CodRequisicaoTecnologia from PedidoPosicao" +
            '" where CodigoPedido in ( select CodigoPedido From GVLogistica.dbo.PedidoVeiculo " +
            '" Where CodTipoRastreador = 5 and TipoVeiculoId <> 1 " +
            '" and CodigoPedido in (Select CodigoPedido From GVLogistica.dbo.Pedido ) )";
            Dim sqlViagensSascar As String = "select CodigoRequisicao from UltimaRequisicao"

            Using cn As New SqlConnection(connection)
                Dim cmd As New SqlCommand(sqlViagensSascar, cn)
                cmd.CommandTimeout = 5000
                cmd.CommandType = System.Data.CommandType.Text
                cn.Open()
                reader = cmd.ExecuteReader()

                If reader.HasRows Then
                    Do While reader.Read()
                        idJabur = reader("CodigoRequisicao").ToString()
                    Loop
                End If
                cn.Close()
            End Using
        Catch ex As Exception
            'LogTxtHelper.Log(" Erro ao buscar as posicoes " + ex.ToString(), "SascarService")
        End Try

        Return idJabur

    End Function

    Private Sub ultimoLabel_Click(sender As Object, e As EventArgs) Handles ultimoLabel.Click

    End Sub
End Class
