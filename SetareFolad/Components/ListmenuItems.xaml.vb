Public Class ListmenuItems
    Private _text As String
    Public Property text() As String
        Get
            Return _text
        End Get
        Set(ByVal value As String)
            Me.TXT.Content = value
            _text = value
        End Set
    End Property
    Private _cid As Integer
    Public Property cid() As Integer
        Get
            Return _cid
        End Get
        Set(ByVal value As Integer)
            Me.ID.Content = value
            _cid = value
        End Set
    End Property
    Public Sub Hid()
        DELETE.Visibility = Visibility.Hidden
    End Sub
    Public Event DeleteClick(ByVal sender As Object, ByVal e As EventArgs)
    Public Sub New(content As String, count As Integer)

        ' This call is required by the designer.
        InitializeComponent()

        ' Add any initialization after the InitializeComponent() call.
        AddHandler DELETE.Click, AddressOf Me.btnDelete_Click
        Me.text = content
        Me.cid = count
        DELETE.Name = "b_" + count.ToString
    End Sub

    Private Sub btnDelete_Click(sender As Object, e As EventArgs) Handles DELETE.Click
        RaiseEvent DeleteClick(sender, e)
    End Sub
End Class