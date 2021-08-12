Public Class Listmenu
    Private _count As UInteger
    Public Property Count() As UInteger
        Get
            Return _count
        End Get
        Set(ByVal value As UInteger)
            _count = value
        End Set
    End Property
    Public Sub New()

        ' This call is required by the designer.
        InitializeComponent()

        ' Add any initialization after the InitializeComponent() call.
        Me._count = 0
    End Sub
    Public Sub add(txt As String, Optional history As Boolean = False)
        Me._count += 1

        Dim uc = New ListmenuItems(txt, _count)
        uc.Name = "lmi_" + _count.ToString
        TheGrid.Children.Add(uc)
        If history Then
            uc.DELETE.Visibility = Visibility.Hidden
        Else
            AddHandler uc.DELETE.Click, AddressOf delete
        End If
    End Sub
    Public Sub clean()
        TheGrid.Children.RemoveRange(0, TheGrid.Children.Count)
        Me._count = 0
    End Sub
    Private Sub delete(sender As Object, e As EventArgs)
        For Each t As ListmenuItems In TheGrid.Children
            If t.cid = sender.Parent.Parent.Parent.cid Then
                TheGrid.Children.Remove(t)
                Exit For
            End If
        Next
        Dim i As Integer = 0
        For Each t As ListmenuItems In TheGrid.Children
            i += 1
            t.cid = i
        Next
        _count = i
    End Sub
End Class