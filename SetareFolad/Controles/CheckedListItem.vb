Imports System.ComponentModel

Public Class CheckedListItem(Of T)
    Implements INotifyPropertyChanged

    Public Event PropertyChanged(sender As Object, e As System.ComponentModel.PropertyChangedEventArgs) Implements System.ComponentModel.INotifyPropertyChanged.PropertyChanged
    Private _IsChecked As Boolean
    Private _Item As T

    Public Sub New()
    End Sub

    Public Sub New(item As T, Optional isChecked As Boolean = False)
        Me._Item = item
        Me._IsChecked = isChecked
    End Sub

    Public Property Item() As T
        Get
            Return _Item
        End Get
        Set(value As T)
            _Item = value
            RaiseEvent PropertyChanged(Me, New PropertyChangedEventArgs("Item"))
        End Set
    End Property

    Public Property IsChecked() As Boolean
        Get
            Return _IsChecked
        End Get
        Set(value As Boolean)
            If Not _IsChecked = value Then
                _IsChecked = value
                RaiseEvent PropertyChanged(Me, New PropertyChangedEventArgs("IsChecked"))
            End If
        End Set
    End Property
End Class
