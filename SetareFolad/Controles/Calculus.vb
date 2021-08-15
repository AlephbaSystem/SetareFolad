Public Class Calculus
    Private Const _Max As Integer = 100
    Private _Width As Integer
    Private _RodSizes As Integer
    Private _ThrowingSpaceUpper As Integer
    Private _ThrowingSpaceLower As Integer
    Private _Rods As List(Of Integer)
    Private _Areas As New List(Of List(Of Integer))
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
    Private Sub Calculate(pos As Integer, pick As List(Of Integer))
        If pick Is Nothing Then pick = New List(Of Integer)
        Dim sum As Integer = pick.Sum(Function(x) x)
        If _Width < sum Then Return
        If Not _Areas.Contains(pick) Then
            Dim tmp As New List(Of Integer)
            tmp.AddRange(pick.ToArray)
            _Areas.Add(tmp)
        End If
        If pos = _Rods.Count() Then Return
        Dim count As Integer = _Width / _Rods(pos)
        For j = 0 To count
            For k = 0 To j
                pick.Add(_Rods(pos))
            Next
            Calculate(pos + 1, pick)
            For k = 0 To j
                pick.RemoveAt(pick.Count - 1)
            Next
        Next
    End Sub
    Public Function Calculate(width As Integer,
                              throwing_space_lower As Integer,
                              throwing_space_upper As Integer,
                              rods As List(Of Integer)) As List(Of KeyValuePair(Of Integer, Dictionary(Of Integer, Integer)))
        _Width = width
        _Rods = rods
        _ThrowingSpaceLower = throwing_space_upper
        _ThrowingSpaceUpper = throwing_space_lower
        _Areas.Clear()
        Calculate(0, Nothing)

        Dim best As New List(Of List(Of Integer))

        Dim answers As New List(Of KeyValuePair(Of Integer, Dictionary(Of Integer, Integer)))
        For Each i In _Areas
            Dim state As List(Of Integer) = i
            Dim sum As Integer = 0
            For Each j In state
                sum += j
            Next
            'if(width - sum > throwing_space_upper or width - sum < throwing_space_lower) continue;
            If _Width - sum > _ThrowingSpaceUpper Or _Width - sum < _ThrowingSpaceLower Then Continue For
            best.Add(state)
            Dim dic As New Dictionary(Of Integer, Integer)
            For Each st In state
                If dic.ContainsKey(st) Then
                    dic(st) += 1
                Else
                    dic.Add(st, 1)
                End If
            Next
            answers.Add(KeyValuePair.Create(Of Integer, Dictionary(Of Integer, Integer))(_Width - sum, dic))
            best.Add(state)
        Next

        'Calculate(0, 0, Nothing)
        'For Each an In _Ans
        '    Dim sum As Integer = 0
        '    For Each a In an
        '        sum = a.Key * a.Value
        '    Next
        '    answers.Add(KeyValuePair.Create(Of Integer, Dictionary(Of Integer, Integer))(sum, an))
        'Next
        QuickSortInt(answers, 0, answers.Count - 1)
        Return answers
    End Function
    'For (auto i : areas){
    '    vector <int> state = i;
    '    int sum = 0 ;
    '    For (int j = 0 ; j < state.size() ; ++j) {
    '      sum += state[j];
    '    }
    '    If (width - sum > throwing_space_upper Or width - sum < throwing_space_lower) Continue;
    '    best.push_back(state);
    '}

    'cout << "Answer is : \n";
    'For (int i = 0 ; i < best.size() ; ++i){
    '  cout << i+1 << ' ' ;
    '  For (int j = 0 ; j < best[i].size() ; ++j) {
    '    cout << best[i][j] << ' ';
    '  }
    '  cout << "\n-----------------------------\n";
    '}

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
