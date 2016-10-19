Private Sub CommandButton1_Click()

    Dim RegEx As Object
    Dim sqlText, sqlTextTemp01, sqlTextTemp02, sqlTextTemp03, sqlTextTemp04, sqlTextTemp05 As String
    Dim lineComment, longComment, whitespace, extTable01, extTable02, extTable03, extTable04 As String
    
    Dim RegMatches As Object
    
    Dim results(200000) As String
    Dim idx As Long
    
    Dim array01 As Variant
    
    array01 = "aa"
    array02 = "bb"
    
    'Range("C1") = array01(1)
    Sheets("Sheet1").Select
    
    Sheets("Sheet1").Columns("A:B").Select
    Selection.ClearContents
    
    Sheets("Sheet2").Select
    
    Sheets("Sheet2").Columns("A:A").Select
    Selection.ClearContents
    
    Sheets("Sheet1").Select
    
    '''''''''''''''''''''''''''''''''''
    ' Init RegExp
    '''''''''''''''''''''''''''''''''''
    Set RegEx = CreateObject("vbscript.regexp")
    With RegEx
    .MultiLine = False
    .Global = True
    .IgnoreCase = True
    .Pattern = "[a-zA-Z]{5}[0-9]{4}"
    End With
    
    '''''''''''''''''''''''''''''''''''
    ' Set sqlText
    '''''''''''''''''''''''''''''''''''
    sqlText = Sheet1.TextBox1.Text
    
    '''''''''''''''''''''''''''''''''''
    ' Define Reg
    '''''''''''''''''''''''''''''''''''
    lineComment1 = "--.*"
    lineComment2 = "//.*"
    whitespace = "[\t\n]"
    longComment = "/\*([^*]|\*+[^/*])*\*+/"
    
    extTable01 = "[a-zA-Z]{5}[0-9]{4}" 'ex:RMKME0006
    extTable02 = "CV?_[a-zA-Z]?[a-zA-Z]?[a-zA-Z]?[a-zA-Z]?[a-zA-Z]?[a-zA-Z]?[a-zA-Z]?[a-zA-Z]?[a-zA-Z]?[a-zA-Z]?[a-zA-Z]?[a-zA-Z]?[a-zA-Z]?"
    
    '''''''''''''''''''''''''''''''''''
    ' Execute Parsing
    '''''''''''''''''''''''''''''''''''
    
    '''''''''''''''''''''''''''''''''''
    ' Del lineComment
    '''''''''''''''''''''''''''''''''''
    RegEx.Pattern = lineComment1
    sqlTemp01Text = RegEx.Replace(sqlText, "")
    
    RegEx.Pattern = lineComment2
    sqlTemp01Text = RegEx.Replace(sqlTemp01Text, "")
    
    Worksheets("Sheet2").Range("A1") = sqlTemp01Text
    
    '''''''''''''''''''''''''''''''''''
    ' Del whitespace (1 line)
    '''''''''''''''''''''''''''''''''''
    RegEx.Pattern = whitespace
    sqlTemp02Text = RegEx.Replace(sqlTemp01Text, "")
    Worksheets("Sheet2").Range("A2") = sqlTemp02Text
    
    '''''''''''''''''''''''''''''''''''
    ' Del longComment
    '''''''''''''''''''''''''''''''''''
    RegEx.Pattern = longComment
    sqlTemp03Text = RegEx.Replace(sqlTemp02Text, "")
    Worksheets("Sheet2").Range("A3") = sqlTemp03Text
    
    '''''''''''''''''''''''''''''''''''
    ' Ext Table01
    '''''''''''''''''''''''''''''''''''
    RegEx.Pattern = extTable01
    'sqlTemp04Text = RegEx.Replace(sqlTemp03Text, "")
    'sqlTemp04Text = RegEx(sqlTemp03Text).submatches(0)
    
    Worksheets("Sheet2").Range("A4") = RegEx.Test(sqlTemp03Text)
    
    Set RegMatches = RegEx.Execute(sqlTemp03Text)
    
    ActiveSheet.Range("A05") = RegMatches.Count
    'If RegMatches.Count <> 0 Then
    '    Worksheets("Sheet2").Range("A13") = RegMatches.Item(0)
    'End If
    
    For i = 0 To RegMatches.Count - 1
       idx = i
       results(i) = RegMatches.Item(i)
       Worksheets("Sheet2").Range("A" & (i + 13)) = RegMatches.Item(i)
    Next
    
    
    '''''''''''''''''''''''''''''''''''
    ' Ext Table02
    '''''''''''''''''''''''''''''''''''
    RegEx.Pattern = extTable02
    Set RegMatches = RegEx.Execute(sqlTemp03Text)
    
    For i = 0 To RegMatches.Count - 1
       idx = idx + (i + 1)
       results(idx) = RegMatches.Item(i)
       'Worksheets("Sheet2").Range("B" & (i + 13)) = RegMatches.Item(i)
    Next
    
    For i = 0 To idx
       Worksheets("Sheet2").Range("A" & (i + 10)) = results(i)
    Next
    
    
    '''''''''''''''''''''''''''''''''''
    ' Order Disstinct
    '''''''''''''''''''''''''''''''''''
    'Order
    Sheets("Sheet2").Select
    ActiveSheet.Range("A10:A200").Select
    Worksheets("Sheet2").Sort.SortFields.Clear
    Worksheets("Sheet2").Sort.SortFields.Add Key:=Range("A10"), _
        SortOn:=xlSortOnValues, Order:=xlAscending, DataOption:=xlSortNormal
        
    With ActiveWorkbook.Worksheets("Sheet2").Sort
    End With
    
    'Distinct
    Sheets("Sheet2").Range("A10:A200").AdvancedFilter Action:=xlFilterCopy, CopyToRange:=Sheets("Sheet1").Range("A10"), Unique:=True
    Sheets("Sheet1").Select
    Range("A10").Select
    
    'delete Disticnt bug
    If Range("A10") = Range("A11") Then
       Rows("10:10").Select
       Selection.Delete Shift:=xlUp
    End If
    
       
       
    
    
End Sub


Private Sub TextBox1_Change()

End Sub
