Public Class Calculus
    Private Const _Max As Integer = 100
    Private _Width As Integer
    Private _RodSizes As Integer
    Private _ThrowingSpaceUpper As Integer
    Private _ThrowingSpaceLower As Integer
    Private _Rods As List(Of Integer)
    Private _Ans As New List(Of Dictionary(Of Integer, Integer))
    Private Function IsSquare(x As Integer, y As Integer) As Boolean
        Return x <> y
    End Function
    Private Function Area(x As Integer, y As Integer) As Double
        If IsSquare(x, y) Then
            Return 4 * x
        Else
            Return 2 * (x + y)
        End If
    End Function
    Private Sub Calculate(sum As Integer, pos As Integer, areas As List(Of Integer))
        If areas Is Nothing Then areas = New List(Of Integer)
        If pos = _Rods.Count And sum <= _Width Then
            If _ThrowingSpaceUpper >= _Width - sum And _Width - sum >= _ThrowingSpaceLower Then
                Dim dic As New Dictionary(Of Integer, Integer)
                For Each are In areas
                    If dic.ContainsKey(are) Then
                        dic(are) += 1
                    Else
                        dic.Add(are, 1)
                    End If
                Next
                _Ans.Add(dic)
            End If
            Return
            End If
            For i = pos To _Rods.Count - 1
            Dim count As Integer = _Width / _Rods(i)
            For j = 0 To count
                Dim x As Integer = (j * _Rods(i))
                For k = 0 To j - 1
                    areas.Add(_Rods(i))
                Next
                Calculate(sum + x, pos + 1, areas)
                For k = 0 To j - 1
                    areas.RemoveAt(areas.Count - 1)
                Next
            Next
        Next
    End Sub
    Private Function Calculate() As List(Of KeyValuePair(Of Integer, Integer))
        Dim len As Integer = _Rods.Count
        Dim ret As Integer = 0
        Dim best As Integer = 0
        Dim ot As New List(Of KeyValuePair(Of Integer, Integer))
        For i = 0 To (1 << len) - 1
            Dim temp As Integer = 0
            For j = 0 To (len) - 1
                Dim test As Integer = (i And (1 << j))
                If test > 0 Then
                    temp += _Rods(j)
                    temp -= 4
                End If
            Next
            If temp <= _Width Then
                If ret < temp Then
                    ret = temp
                    best = i
                    ot.Add(KeyValuePair.Create(Of Integer, Integer)(ret, best))
                End If
            End If
        Next

        Return ot
    End Function
    Public Function Calculate(width As Integer,
                              throwing_space_lower As Integer,
                              throwing_space_upper As Integer,
                              rods As List(Of Integer)) As List(Of KeyValuePair(Of Integer, Dictionary(Of Integer, Integer)))
        _Width = width
        _Rods = rods
        _ThrowingSpaceLower = throwing_space_upper
        _ThrowingSpaceUpper = throwing_space_lower
        Dim best_rods As New List(Of KeyValuePair(Of Integer, Integer))

        Dim answers As New List(Of KeyValuePair(Of Integer, Dictionary(Of Integer, Integer)))
        Calculate(0, 0, Nothing)
        For Each an In _Ans
            'Dim _return As New List(Of KeyValuePair(Of Integer, Integer))
            'Dim tmp As KeyValuePair(Of Integer, Integer)
            'tmp = KeyValuePair.Create(Of Integer, Integer)(an.Key + sum, an.Value)

            'For Each i In _Rods
            '    Dim test As Integer = (an.Value And (1 << _Rods.IndexOf(i)))
            '    If test > 0 Then
            '        _return.Add(i)
            '    End If
            'Next
            Dim sum As Integer = 0
            For Each a In an
                sum = a.Key * a.Value
            Next
            answers.Add(KeyValuePair.Create(Of Integer, Dictionary(Of Integer, Integer))(sum, an))
        Next
        QuickSortInt(answers, 0, answers.Count - 1)
        'answers.Reverse()
        Return answers
    End Function

    Public Shared Sub QuickSortInt(ByRef awIntArr As List(Of KeyValuePair(Of Integer, Dictionary(Of Integer, Integer))),
           ByVal aLoIdx As Integer, ByVal aHiIdx As Integer)
        If awIntArr.Count <= 0 Then Return
        Dim xTmp As KeyValuePair(Of Integer, Dictionary(Of Integer, Integer))
        Dim xLo As Integer = aLoIdx
        Dim xHi As Integer = aHiIdx
        Dim xPivot As Integer = awIntArr((xLo + xHi) \ 2).Key
        Do Until xLo > xHi
            Do While awIntArr(xLo).Key < xPivot
                xLo += 1
            Loop
            Do While awIntArr(xHi).Key > xPivot
                xHi -= 1
            Loop
            If xLo <= xHi Then
                xTmp = awIntArr(xLo)
                awIntArr(xLo) = awIntArr(xHi)
                awIntArr(xHi) = xTmp
                xLo += 1
                xHi -= 1
            End If
        Loop
        If (xLo < aHiIdx) Then QuickSortInt(awIntArr, xLo, aHiIdx)
        If (xHi > aLoIdx) Then QuickSortInt(awIntArr, aLoIdx, xHi)
    End Sub


End Class
