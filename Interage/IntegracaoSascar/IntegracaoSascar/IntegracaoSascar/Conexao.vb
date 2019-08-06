
Imports System.Data.SqlClient

Public Class Conexao

    Dim provider As String = ""
    Dim conn As SqlConnection
    Dim dtVeiculos As DataTable

    Public Sub New()

        provider = "Data Source=192.168.146.42;Initial Catalog=gvCSN;User ID=sa;Password=as"
        conn = New SqlConnection(provider)

    End Sub
    Private Function Conectar() As Boolean
        Try
            conn.Open()

            Return True
        Catch ex As Exception
            Return False
        End Try
    End Function
    Private Function Desconectar() As Boolean
        Try
            conn.Close()
            Return True
        Catch ex As Exception
            Return False
        End Try
    End Function
    Public Function TesteConexao() As Boolean

        Try

            If Not Me.Conectar Then
                Return False
            End If

            Return Desconectar()

        Catch ex As Exception
            Return False
        End Try


    End Function
    Public Function ExecutaComando(ByVal sql As String) As Boolean
        Try
            Dim cmd As New SqlCommand()
            cmd.CommandText = sql
            cmd.Connection = conn

            Conectar()

            cmd.ExecuteNonQuery()


            Return True
        Catch ex As Exception
            Return False
        Finally
            Desconectar()

        End Try
    End Function
    Public Function GetDataTable(ByRef dt As DataTable, ByVal sql As String) As Boolean
        Try

            Dim cmd As New SqlCommand
            cmd.CommandText = sql
            cmd.Connection = conn

            Conectar()

            Dim da As New SqlDataAdapter(cmd)

            da.Fill(dt)

            Desconectar()


            Return True
        Catch ex As Exception
            Return False
        End Try
    End Function

    Public Sub Teste()
        Try
            Dim con As New SqlConnection(provider)
            Dim parametro As New SqlParameter()
            parametro.DbType = DbType.Int32
            parametro.ParameterName = "SASCAR_LALO_BUSCAR"
            parametro.Value = 1

        Catch ex As Exception

        End Try
    End Sub

    Public Function SalvaLote(ByVal dt As DataTable, Optional ByRef retorno As String = "") As Boolean
        Try


            If dt.TableName = "obterVeiculo" Then
                Dim row As DataRow
                For Each row In dt.Rows
                    Call GravaVeiculo(row("idveiculo").ToString(), row("placa").ToString(), row("idcliente").ToString(), row("descricao").ToString(), row("idequipamento").ToString())
                Next row
            Else
                Conectar()
                Dim bulkData As SqlBulkCopy = New SqlBulkCopy(conn)

                bulkData.DestinationTableName = dt.TableName
                bulkData.WriteToServer(dt)

                bulkData.Close()

            End If

            Return True

        Catch ex As Exception

            retorno = ex.Message
            Return False

        Finally

            Desconectar()

        End Try



    End Function

    Public Sub GravaVeiculo(ByVal idveiculo As Integer, ByVal placa As String, ByVal idcliente As Integer, ByVal desccricao As String, ByVal idequipamento As String)
        conn.Open()

        Dim cmd As New SqlCommand("SASCAR_InsereVeiculo", conn)

        Try
            cmd.CommandType = CommandType.StoredProcedure
            cmd.Parameters.AddWithValue("@IDVEICULO", idveiculo)
            cmd.Parameters.AddWithValue("@PLACA", placa)
            cmd.Parameters.AddWithValue("@IDCLIENTE", idcliente)
            cmd.Parameters.AddWithValue("@DESCRICAO", desccricao)
            cmd.Parameters.AddWithValue("@IDEQUIPAMENTO", idequipamento)
            cmd.ExecuteNonQuery()
        Finally
            If cmd IsNot Nothing Then cmd.Dispose()
            If conn IsNot Nothing AndAlso conn.State <> ConnectionState.Closed Then conn.Close()
        End Try
    End Sub





End Class
