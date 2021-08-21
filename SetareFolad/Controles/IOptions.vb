Imports System.IO
Imports System.Windows.Threading
Imports Newtonsoft.Json
Imports Excel = Microsoft.Office.Interop.Excel
Public Class IOptions
    Dim wn As New loading
    Public Sub Json()
        File.WriteAllText("" & "\json.txt", JsonConvert.SerializeObject("", Newtonsoft.Json.Formatting.Indented))
    End Sub
    Private Function ConvertToCSV(ByVal dt As List(Of KeyValuePair(Of Integer, Integer))) As String
        Dim sb As New Text.StringBuilder()

        For Each r In dt
            sb.AppendLine(r.Key & "," & r.Value)
        Next

        Return sb.ToString()
    End Function
    Private Function FindSheet(ByVal workbook As Excel.Workbook,
    ByVal sheet_name As String) As Excel.Worksheet
        For Each sheet As Excel.Worksheet In workbook.Sheets
            If (sheet.Name = sheet_name) Then Return sheet
        Next sheet

        Return Nothing
    End Function
    Private Sub WriteToExcel(dir As String, ot As List(Of Tuple(Of Integer, List(Of Tuple(Of String, Integer, Integer, Integer)))), Optional nw As Boolean = True)

        Application.Current.Dispatcher.Invoke(DispatcherPriority.Background,
    New Action(Function()
                   wn.OveralLabel = "0/0"
                   wn.CureentLabel = ""
                   wn.MProgress = ot.Count

                   wn.Show()
               End Function))

        Dim excel_app As New Excel.Application

        If excel_app Is Nothing Then
            MessageBox.Show("Excel is not properly installed!!")
            Return
        End If

        Dim misValue As Object = System.Reflection.Missing.Value

        Dim workbook As Excel.Workbook

        If nw Then
            workbook = excel_app.Workbooks.Add(misValue)
        Else
            workbook = excel_app.Workbooks.Open(Filename:=dir)
        End If
        Dim cii As Integer = 0
        For Each o In ot
            cii += 1
            Application.Current.Dispatcher.Invoke(DispatcherPriority.Background,
    New Action(Function()
                   wn.OveralLabel = cii.ToString & "/" & ot.Count.ToString
                   wn.CureentLabel = o.Item1.ToString
                   wn.Progress = 1
               End Function))

            Threading.Thread.Sleep(100)

            ' See if the worksheet already exists.
            Dim sheet_name As String = o.Item1.ToString & "_" & cii.ToString()

            Dim sheet As Excel.Worksheet = FindSheet(workbook,
            sheet_name)
            If (sheet Is Nothing) Then
                ' Add the worksheet at the end.
                sheet = DirectCast(workbook.Sheets.Add(
                After:=workbook.Sheets(workbook.Sheets.Count),
                Count:=1,
                Type:=Excel.XlSheetType.xlWorksheet),
                    Excel.Worksheet)
                sheet.Name = sheet_name
            End If


            ' Make that range of cells bold and red.
            Dim header_range As Excel.Range = sheet.Range("B2",
            "E2")
            header_range.Font.Bold = True

            header_range = sheet.Range("B2",
            "E" & (o.Item2.Count + 2))
            header_range.BorderAround2()

            Dim value_range As Excel.Range = sheet.Range("B2", "E" & o.Item2.Count + 1)
            value_range.Value2 = "test"
            Dim c As Integer = 3
            For Each i2 As Tuple(Of String, Integer, Integer, Integer) In o.Item2
                Dim cn As String = "B" & c
                Dim rn = sheet.Range(cn, cn)
                rn.Value2 = i2.Item1.ToString

                cn = "C" & c
                rn = sheet.Range(cn, cn)
                rn.Value2 = i2.Item2.ToString

                cn = "D" & c
                rn = sheet.Range(cn, cn)
                rn.Value2 = i2.Item3.ToString

                cn = "E" & c
                rn = sheet.Range(cn, cn)
                rn.Value2 = i2.Item4.ToString

                c += 1
            Next

            ' Add some data to individual cells.
            sheet.Cells(2, 2) = ""
            sheet.Cells(2, 3) = "عرض نوار"
            sheet.Cells(2, 4) = "تعداد نوار"
            sheet.Cells(2, 5) = "عرض کل"
            sheet.Cells(o.Item2.Count + 3, 4) = "جمع کل"
            sheet.Cells(o.Item2.Count + 3, 5) = o.Item2.Sum(Function(item) item.Item4)

            releaseObject(sheet)
        Next

        If nw Then
            workbook.SaveAs(dir, Excel.XlFileFormat.xlWorkbookNormal, misValue, misValue, misValue, misValue,
         Excel.XlSaveAsAccessMode.xlExclusive, misValue, misValue, misValue, misValue, misValue)
        End If

        ' Save the changes and close the workbook.
        workbook.Close(SaveChanges:=True)

        ' Close the Excel server.
        excel_app.Quit()


        Application.Current.Dispatcher.Invoke(DispatcherPriority.Background,
    New Action(Function()
                   wn.Close()
                   releaseObject(wn)
               End Function))
        releaseObject(workbook)
        releaseObject(excel_app)
        MessageBox.Show("ذخیره شد.")
    End Sub

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
    Public Sub Save(dir As String, ot As List(Of Tuple(Of Integer, List(Of Tuple(Of String, Integer, Integer, Integer)))), Optional excel As Boolean = True)

        Try
            If File.Exists(dir) Then
                File.Delete(dir)
            End If
            If excel Then
                Dim tr As New Threading.Thread(Sub() WriteToExcel(dir, ot))
                tr.Start()
            End If
        Catch ex As Exception
            If ex.Message.Contains("office") Then
                MessageBox.Show("لطفا office را نصب کنید")
            End If
        End Try
    End Sub
End Class
