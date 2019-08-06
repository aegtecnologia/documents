
Namespace IntegraJabur.Service.RequestJabur
    Public Class VeiculoOnixSat

        Public Property VeiculoOnixSatId() As Integer
            Get
                Return m_VeiculoOnixSatId
            End Get
            Set(value As Integer)
                m_VeiculoOnixSatId = Value
            End Set
        End Property
        Private m_VeiculoOnixSatId As Integer

        Public Property PlacaOnixSatId() As String
            Get
                Return m_PlacaOnixSatId
            End Get
            Set(value As String)
                m_PlacaOnixSatId = Value
            End Set
        End Property
        Private m_PlacaOnixSatId As String

        Public Property vsOnixSatId() As String
            Get
                Return m_vsOnixSatId
            End Get
            Set(value As String)
                m_vsOnixSatId = value
            End Set
        End Property
        Private m_vsOnixSatId As String

        Public Property tCmdOnixSatId() As String
            Get
                Return m_tCmdOnixSatId
            End Get
            Set(value As String)
                m_tCmdOnixSatId = value
            End Set
        End Property
        Private m_tCmdOnixSatId As String

        Public Property tpOnixSatId() As String
            Get
                Return m_tpOnixSatId
            End Get
            Set(value As String)
                m_tpOnixSatId = value
            End Set
        End Property
        Private m_tpOnixSatId As String

        Public Property taOnixSatId() As String
            Get
                Return m_taOnixSatId
            End Get
            Set(value As String)
                m_taOnixSatId = value
            End Set
        End Property
        Private m_taOnixSatId As String

        Public Property eqpOnixSatId() As String
            Get
                Return m_eqpOnixSatId
            End Get
            Set(value As String)
                m_eqpOnixSatId = value
            End Set
        End Property
        Private m_eqpOnixSatId As String

    End Class
End Namespace
