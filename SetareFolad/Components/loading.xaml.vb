Imports System.Threading
Imports System.Windows
Public Class loading
    Private currentLabelValue As String
    Public Property CureentLabel() As String
        Get
            Return currentLabelValue
        End Get
        Set(ByVal value As String)
            pbDetail.Content = value
            currentLabelValue = value
        End Set
    End Property
    Private OveralLabelValue As String
    Public Property OveralLabel() As String
        Get
            Return OveralLabelValue
        End Get
        Set(ByVal value As String)
            pbProgre.Content = value
            OveralLabelValue = value
        End Set
    End Property
    Private ProgressValue As Integer
    Public Property Progress() As Integer
        Get
            Return ProgressValue
        End Get
        Set(ByVal value As Integer)
            If pbStatus.Value + value > MProgressValue Then MProgressValue += MProgressValue / 10
            pbStatus.Value += 1
            ProgressValue = pbStatus.Value
        End Set
    End Property

    Private MProgressValue As Integer
    Public Property MProgress() As Integer
        Get
            Return MProgressValue
        End Get
        Set(ByVal value As Integer)
            If value <= 0 Then value = 100
            pbStatus.Maximum = value
            MProgressValue = value
        End Set
    End Property
    Public Event cancelClicked(ByVal sender As Object, ByVal e As EventArgs)

    Private Sub laqv_click(sender As Object, e As EventArgs) Handles laqv.Click
        RaiseEvent cancelClicked(sender, e)
    End Sub
    Public Sub New()

        ' This call is required by the designer.
        InitializeComponent()

        ' Add any initialization after the InitializeComponent() call.

    End Sub
End Class
