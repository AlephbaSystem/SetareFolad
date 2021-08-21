Imports System.Collections.ObjectModel
Imports System.ComponentModel
Imports System.Globalization
Imports System.Text.RegularExpressions
Imports System.Windows.Threading
Imports Microsoft.Win32

Class MainWindow
    Implements INotifyPropertyChanged
    Public Event PropertyChanged(ByVal sender As Object, ByVal e As System.ComponentModel.PropertyChangedEventArgs) Implements System.ComponentModel.INotifyPropertyChanged.PropertyChanged

    Private rods As New List(Of KeyValuePair(Of String, Integer))
    Private _Width As Double = 0
    Private Calcul As New Calculus
    Private Result As New List(Of KeyValuePair(Of Integer, Dictionary(Of Integer, Integer)))
    Private CurrentPage As Integer = 0
    Sub New()
        ' This call is required by the designer.
        FixRods()

        InitializeComponent()

        clbSections.ItemsSource = Sections
    End Sub

    Private Sub clean_click(sender As Object, e As EventArgs) Handles Clean.Click
        CurrentPage = 0
        SizeVaraq.Text = Nothing
        MizanEstefade.Content = "N/A"
        TedadNavar.Content = "N/A"
        Tarikh.Content = "N/A"
        Jamkol.Content = "N/A"
        MizanPerti.Content = "N/A"
        HalatLbl.Content = "حالت"
        History.clean()
        PertiMax.Text = Nothing
        PertiMin.Text = Nothing
        For Each s In Sections
            s.IsChecked = False
        Next
        _Width = 0
        Result.Clear()
    End Sub
    Private _Sections As ObservableCollection(Of CheckedListItem(Of String))
    Public Property Sections() As ObservableCollection(Of CheckedListItem(Of String))
        Get
            Return _Sections
        End Get
        Set(value As ObservableCollection(Of CheckedListItem(Of String)))
            _Sections = value
            AddHandler Sections.CollectionChanged, AddressOf Sections_CollectionChanged
        End Set
    End Property

    Private Sub Sections_CollectionChanged(sender As Object, e As Specialized.NotifyCollectionChangedEventArgs)
        '# This means we're adding new items to the list.
        If e.NewItems IsNot Nothing Then
            For Each Item As CheckedListItem(Of String) In e.NewItems
                '# Add a handler for this item so that we can listen for the checkbox element
                '# being ticked or cleared.
                AddHandler Item.PropertyChanged, AddressOf CheckedListItem_PropertyChanged
            Next
        End If
        '# This means we're removing existing items from the list.
        If e.OldItems IsNot Nothing Then
            For Each Item As CheckedListItem(Of String) In e.OldItems
                '# Remove the handler for each item.
                RemoveHandler Item.PropertyChanged, AddressOf CheckedListItem_PropertyChanged
            Next
        End If
        '# Clearing the list doesn't provide an OldItems collections, so if the action is a reset and there's
        '# nothing in either collection, do whatever would happen if you removed each item individually.
        If e.Action = Specialized.NotifyCollectionChangedAction.Reset AndAlso e.OldItems Is Nothing AndAlso e.NewItems Is Nothing Then
            '# Do whatever
        End If
    End Sub

    Private Sub CheckedListItem_PropertyChanged(sender As Object, e As PropertyChangedEventArgs)
        '# Check that it was the IsChecked property that changed.
        If e.PropertyName = "IsChecked" Then
            Dim Item = TryCast(sender, CheckedListItem(Of String))
            '# If the item was ticked, add it to the collection of selected jobs unless it's already there.
            If Item.IsChecked Then
                '# Do whatever
            Else
                '# Do whatever
            End If
        End If
    End Sub

    Private Function CalculateArea(x As Double, y As Double)
        If x = y Then
            Return 4 * x - 4
        Else
            Return 2 * (x + y) - 4
        End If
    End Function
    Private Sub FixRods()
        _Sections = New ObservableCollection(Of CheckedListItem(Of String))
        Sections = New ObservableCollection(Of CheckedListItem(Of String))
        rods.Clear()

        rods.Add(KeyValuePair.Create(Of String, Integer)("20x20", CalculateArea(20, 20)))
        rods.Add(KeyValuePair.Create(Of String, Integer)("20x30", CalculateArea(20, 30)))
        rods.Add(KeyValuePair.Create(Of String, Integer)("30x30, 20x40", CalculateArea(30, 30)))
        rods.Add(KeyValuePair.Create(Of String, Integer)("30x60", CalculateArea(30, 60)))
        rods.Add(KeyValuePair.Create(Of String, Integer)("40x40, 507, 508, 509, 30x50", CalculateArea(40, 40)))
        rods.Add(KeyValuePair.Create(Of String, Integer)("40x60, 50x50", CalculateArea(40, 60)))
        rods.Add(KeyValuePair.Create(Of String, Integer)("40x80, 60x60", CalculateArea(40, 80)))
        rods.Add(KeyValuePair.Create(Of String, Integer)("کم فرانسوی", CalculateArea(46, 68.5)))
        Dim i As Integer = 0
        For Each r In rods
            Sections.Add(New CheckedListItem(Of String)(r.Key))
            i += 1
        Next
    End Sub
    Private Sub NumberValidationTextBox(sender As Object, e As TextCompositionEventArgs)
        Dim egex As New Regex("[^0-9]+")
        e.Handled = egex.IsMatch(e.Text)
    End Sub
    Private Sub CalculateThread(width As Integer, pMin As Integer, pMax As Integer, rdtocal As List(Of Integer))
        Dim tr As New Threading.Thread(Sub()
                                           Dim isen, isan As Boolean
                                           Me.Dispatcher.Invoke(Sub()
                                                                    isen = Not Me.IsEnabled
                                                                    If wn Is Nothing Then
                                                                        isan = False
                                                                    Else
                                                                        isan = True
                                                                    End If
                                                                End Sub)
                                           While isen And isan
                                               Me.Dispatcher.Invoke(Sub()
                                                                        isen = Not Me.IsEnabled
                                                                        If wn Is Nothing Then
                                                                            isan = False
                                                                        Else
                                                                            isan = True
                                                                        End If
                                                                    End Sub)
                                               Application.Current.Dispatcher.Invoke(DispatcherPriority.Background,
                                           New Action(Function()
                                                          If wn IsNot Nothing Then
                                                              wn.Progress = 1
                                                              wn.CureentLabel += 1
                                                              wn.pbStatus.IsIndeterminate = False
                                                          End If
                                                      End Function))
                                               Threading.Thread.Sleep(100)
                                           End While
                                       End Sub)
        tr.Start()

        trp.Add(tr)

        Result = Calcul.Calculate(width, pMin, pMax, rdtocal)
        Application.Current.Dispatcher.Invoke(DispatcherPriority.Background,
    New Action(Function()
                   FinishedCalculate()
               End Function))
    End Sub
    Private Sub FinishedCalculate()
        wn.pbStatus.Value = wn.MProgress

        If Result.Count > 0 Then
            Dim xDate As DateTime = Convert.ToDateTime(Now().ToString)
            Dim persianDateString As String = xDate.ToString("yyyy/MM/dd hh:mm", New CultureInfo("fa-IR"))

            MizanEstefade.Content = Math.Round(Result(CurrentPage).Value.Sum(Function(x) x.Key * x.Value) * 100 / _Width) & " %"
            TedadNavar.Content = Result(CurrentPage).Value.Count
            MizanPerti.Content = _Width - Result(CurrentPage).Value.Sum(Function(x) x.Key * x.Value)
            Jamkol.Content = Result(CurrentPage).Value.Sum(Function(x) x.Key * x.Value)
            Tarikh.Content = persianDateString
            History.clean()
            For Each c In Result(CurrentPage).Value
                History.add(c.Key.ToString + " * " + c.Value.ToString, True)
            Next
            HalatLbl.Content = String.Format("حالت {0} از {1}", CurrentPage + 1, Result.Count)
        End If

        wn.Close()
        Me.IsEnabled = True
        Me.Opacity = 100
        releaseObject(wn)
    End Sub
    Public wn As loading
    Private Sub releaseObject(ByVal obj As Object)
        Try
            System.Runtime.InteropServices.Marshal.ReleaseComObject(obj)
            obj = Nothing
        Catch ex As Exception
            obj = Nothing
        Finally
            GC.Collect()
        End Try
    End Sub
    Public trp As New List(Of Threading.Thread)
    Private Sub cancel_pg_clicked(sender As Object, e As EventArgs)
        FinishedCalculate()
        trp.Reverse()
        For Each t In trp
            t.Interrupt()
        Next
        trp.Clear()
    End Sub
    Private Sub Calc_click(sender As Object, e As EventArgs) Handles Calc.Click
        Me.IsEnabled = False
        Me.Opacity = 50
        Result.Clear()
        CurrentPage = 0
        History.clean()
        wn = New loading
        AddHandler wn.cancelClicked, AddressOf cancel_pg_clicked

        wn.MProgress = 100
        wn.OveralLabel = "در حال محاسبه"
        wn.CureentLabel = "~"

        wn.Show()

        If Not IsNumeric(PertiMin.Text) OrElse Not IsNumeric(PertiMax.Text) OrElse Not IsNumeric(SizeVaraq.Text) OrElse rods.Count <= 0 Then
            Return
        End If
        If Not Double.TryParse(SizeVaraq.Text, _Width) Then Return
        Dim rodsToCal As New List(Of Integer)
        Dim bigint As New System.Numerics.BigInteger(1)
        For Each r In rods
            wn.Progress = 10
            If Sections.Where(Function(t) t.Item = r.Key And t.IsChecked = True).Count > 0 Then
                rodsToCal.Add(r.Value)
            End If
            Dim bitmp As New Numerics.BigInteger(_Width / r.Value)
            bigint *= bitmp
        Next
        wn.OveralLabel = "در حال محاسبه" & " ~ " & bigint.ToString
        wn.CureentLabel = 0
        wn.MProgress = 100
        Dim pmin, pmax, widt As Integer
        pmin = PertiMin.Text
        pmax = PertiMax.Text
        widt = _Width
        Dim tr As New Threading.Thread(Sub() CalculateThread(widt, pmin, pmax, rodsToCal))
        tr.IsBackground = True
        trp.Add(tr)
        tr.Start()
    End Sub
    Private Sub ForwardBtn_click(sender As Object, e As EventArgs) Handles ForwardBtn.Click
        If Result.Count = 0 Then Return
        CurrentPage += 1
        If CurrentPage = Result.Count Then
            CurrentPage = 0
        End If
        HalatLbl.Content = String.Format("حالت {0} از {1}", CurrentPage + 1, Result.Count)

        MizanEstefade.Content = Math.Round(Result(CurrentPage).Value.Sum(Function(x) x.Key * x.Value) * 100 / _Width) & " %"
        TedadNavar.Content = Result(CurrentPage).Value.Count
        MizanPerti.Content = _Width - Result(CurrentPage).Value.Sum(Function(x) x.Key * x.Value)
        Jamkol.Content = Result(CurrentPage).Value.Sum(Function(x) x.Key * x.Value)

        History.clean()
        For Each c In Result(CurrentPage).Value
            History.add(c.Key.ToString + " * " + c.Value.ToString, True)
        Next
    End Sub
    Private Sub BackBtn_click(sender As Object, e As EventArgs) Handles BackBtn.Click
        If Result.Count = 0 Then Return
        CurrentPage -= 1
        If CurrentPage = -1 Then
            CurrentPage = Result.Count - 1
        End If
        HalatLbl.Content = String.Format("حالت {0} از {1}", CurrentPage + 1, Result.Count)

        MizanEstefade.Content = Math.Round(Result(CurrentPage).Value.Sum(Function(x) x.Key * x.Value) * 100 / _Width) & " %"
        TedadNavar.Content = Result(CurrentPage).Value.Count
        MizanPerti.Content = _Width - Result(CurrentPage).Value.Sum(Function(x) x.Key * x.Value)
        Jamkol.Content = Result(CurrentPage).Value.Sum(Function(x) x.Key * x.Value)

        History.clean()
        For Each c In Result(CurrentPage).Value
            History.add(c.Key.ToString + " * " + c.Value.ToString, True)
        Next
    End Sub
    Private Sub sv_click(sender As Object, e As EventArgs) Handles Save.Click
        Me.IsEnabled = False
        If Result.Count <= 0 Then Return
        Dim stf As New List(Of Tuple(Of Integer, List(Of Tuple(Of String, Integer, Integer, Integer))))
        For Each r As KeyValuePair(Of Integer, Dictionary(Of Integer, Integer)) In Result
            Dim l As New List(Of Tuple(Of String, Integer, Integer, Integer))
            Dim item0 As Integer = 0
            For Each v In r.Value
                For Each rd In rods
                    If rd.Value = v.Key Then
                        Dim item1 As String = rd.Key
                        Dim item2 As Integer = rd.Value
                        Dim item3 As Integer = v.Value
                        Dim item4 As Double = v.Value * v.Key
                        item0 += item4
                        l.Add(Tuple.Create(Of String, Integer, Integer, Integer)(item1, item2, item3, item4))
                        Exit For
                    End If
                Next
            Next
            stf.Add(Tuple.Create(Of Integer, List(Of Tuple(Of String, Integer, Integer, Integer)))(_Width - item0, l))
        Next
        Dim saveFileDialog As New SaveFileDialog()
        saveFileDialog.Filter = "Excel Files|*.xls;*.xlsx;*.xlsm" 'CSV files (*.csv)|*.csv|
        If (saveFileDialog.ShowDialog() = True) Then
            Dim o As New IOptions
            Select Case IO.Path.GetExtension(saveFileDialog.FileName).ToLower()
                Case ".xls"
                    o.Save(saveFileDialog.FileName, stf, True)

                Case ".xlsx"
                    o.Save(saveFileDialog.FileName, stf, True)

                Case ".xlsm"
                    o.Save(saveFileDialog.FileName, stf, True)

                Case ".csv"
                    o.Save(saveFileDialog.FileName, stf, False)

            End Select
        End If
        Me.IsEnabled = True
    End Sub
End Class