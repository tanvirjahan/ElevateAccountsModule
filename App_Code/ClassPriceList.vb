Imports Microsoft.VisualBasic
Imports System.Data
Imports System.Data.SqlClient

Public Class ClassPriceList
    Implements ITemplate
    Dim Container As Control
    Dim myDataReader As SqlDataReader
    Dim SqlConn As SqlConnection
    Dim myCommand As SqlCommand
    Dim myDataAdapter As SqlDataAdapter
    Dim TemplateType As ListItemType
    Private htControls As New System.Collections.Hashtable()
    Private htBindPropertiesNames As New System.Collections.Hashtable()
    Private htBindExpression As New System.Collections.Hashtable()

    Dim Field1 As String

    Dim Field2 As String
    Dim infotype As String
    'Dim i As Long = 0
    Shared i = 0
    Shared k = 0

    Sub New(ByVal type As ListItemType, ByVal fld1 As String, ByVal fld2 As String) ', ByVal control_type As String)
        i = 0
        k = 0
        TemplateType = type

        Field1 = fld1

        Field2 = fld2

        ' infotype = control_type

    End Sub

    Sub InstantiateIn(ByVal Container As Control) Implements ITemplate.InstantiateIn

        Dim lbl1 As DropDownList = New DropDownList()
        Dim strSqlQry As String = ""
        Dim lc1 As LiteralControl = New LiteralControl()
        Dim lc2 As LiteralControl = New LiteralControl()

        Select Case TemplateType
            Case ListItemType.Header

                Dim lbl As New TextBox

                If Field1 = "No.of.Nights Room Rate" Or Field1 = "Extra Person Supplement" Then
                    lbl.Text = Field1
                    lbl.ID = "txtHead" & k
                    lbl.ForeColor = Drawing.Color.White
                    'lbl.Font.Bold = True
                    lbl.Columns = 10 ' 15
                    lbl.Height = 30
                    lbl.BorderStyle = BorderStyle.None
                    lbl.BackColor = Drawing.Color.Transparent
                    lbl.CssClass = "field_caption"
                    lbl.Width = 150
                    ' lbl.TextMode = TextBoxMode.MultiLine
                    ' lbl.Wrap = True
                    'changed by mohamed on 20/02/2018
                    If Field1 = "Extra Person Supplement" Then
                        lbl.Columns = 15 ' 15
                    Else
                        lbl.Columns = 10 ' 15
                    End If
                Else
                    lbl.Text = Field1
                    lbl.ID = "txtHead" & k
                    lbl.ForeColor = Drawing.Color.White
                    'lbl.Font.Bold = True
                    'lbl.Columns = 10 ' 15 'changed by mohamed on 20/02/2018 'commented here, and moved to down
                    lbl.Height = 18
                    lbl.BorderStyle = BorderStyle.None
                    lbl.BackColor = Drawing.Color.Transparent
                    lbl.CssClass = "field_caption"

                    'changed by mohamed on 20/02/2018
                    If Field1 <> "Sr No" And Field1 <> "No.of.Nights Room Rate" And Field1 <> "Extra Person Supplement" And Field1 <> "Min Nights" And Field1 <> "Pkg" And Field1 <> "Remark" And Field1 <> "Booking Code" Then
                        lbl.Columns = 15 ' 15
                    Else
                        lbl.Columns = 10 ' 15
                    End If
                End If
                lbl.ReadOnly = True
                Container.Controls.Add(lbl)

                'changed by mohamed on 20/02/2018
                If Field1 <> "Sr No" And Field1 <> "No.of.Nights Room Rate" And Field1 <> "Min Nights" And Field1 <> "Pkg" And Field1 <> "Remark" And Field1 <> "Booking Code" Then '--And Field1 <> "Extra Person Supplement" --changed by mohamed on 28/03/2018
                    Dim lblTV As New TextBox
                    lblTV.Text = "TV"
                    lblTV.ID = "txtTVHead" & k
                    lblTV.ForeColor = Drawing.Color.White
                    lblTV.BorderStyle = BorderStyle.None
                    lblTV.BackColor = Drawing.Color.Transparent
                    lblTV.CssClass = "field_caption"
                    lblTV.ReadOnly = True
                    lblTV.Columns = 2 ' 15
                    Container.Controls.Add(lblTV)

                    Dim lblNTV As New TextBox
                    lblNTV.Text = "NTV"
                    lblNTV.ID = "txtNTVHead" & k
                    lblNTV.ForeColor = Drawing.Color.White
                    lblNTV.BorderStyle = BorderStyle.None
                    lblNTV.BackColor = Drawing.Color.Transparent
                    lblNTV.CssClass = "field_caption"
                    lblNTV.ReadOnly = True
                    lblNTV.Columns = 2 ' 15
                    Container.Controls.Add(lblNTV)

                    Dim lblVAT As New TextBox
                    lblVAT.Text = "VAT"
                    lblVAT.ID = "txtVATHead" & k
                    lblVAT.ForeColor = Drawing.Color.White
                    lblVAT.BorderStyle = BorderStyle.None
                    lblVAT.BackColor = Drawing.Color.Transparent
                    lblVAT.CssClass = "field_caption"
                    lblVAT.ReadOnly = True
                    lblVAT.Columns = 2 ' 15
                    Container.Controls.Add(lblVAT)
                End If

                k = k + 1
            Case ListItemType.Item

                'If Field1 = "Room Type" Then
                '    Dim txt As New TextBox
                '    txt.ID = "txt" & i
                '    txt.ReadOnly = True
                '    txt.Columns = 15
                '    txt.BorderStyle = BorderStyle.None
                '    AddHandler txt.DataBinding, AddressOf BindStringColumn
                '    Container.Controls.Add(txt)
                'ElseIf Field1 = "Room Type Name" Then
                '    Dim txt As New TextBox
                '    txt.ID = "txt" & i
                '    txt.ReadOnly = True
                '    txt.Columns = 15
                '    txt.BorderStyle = BorderStyle.None
                '    AddHandler txt.DataBinding, AddressOf BindStringColumn
                '    Container.Controls.Add(txt)
                If Field1 = "Sr No" Then
                    Dim txt As New TextBox
                    txt.ID = "txt" & i
                    txt.ReadOnly = True
                    txt.Columns = 4
                    txt.BorderStyle = BorderStyle.None
                    txt.CssClass = "field_input"
                    AddHandler txt.DataBinding, AddressOf BindStringColumn
                    Container.Controls.Add(txt) '
                ElseIf Field1 = "No.of.Nights Room Rate" Or Field1 = "Extra Person Supplement" Then
                    Dim txt As New TextBox
                    txt.ID = "txt" & i
                    If Field1 = "Extra Person Supplement" Then 'changed by mohamed on 28/03/2018
                        txt.Columns = 15
                    Else
                        txt.Columns = 6 '15
                    End If

                    txt.Text = 1
                    txt.MaxLength = 10
                    txt.Style("text-align") = "center"
                    txt.CssClass = "field_input"
                    AddHandler txt.DataBinding, AddressOf BindStringColumn
                    Container.Controls.Add(txt)
                    If Field1 = "Extra Person Supplement" Then
                        txt.Style.Add("FieldHeader", Field1)
                        GoTo addTVDetailTextBox 'changed by mohamed on 28/03/2018
                    End If
                ElseIf Field1 = "Min Nights" Then
                    Dim txt As New TextBox
                    txt.ID = "txt" & i
                    txt.Columns = 6 '15
                    txt.Text = 1
                    txt.MaxLength = 10
                    txt.Style("text-align") = "center"
                    txt.CssClass = "field_input"
                    AddHandler txt.DataBinding, AddressOf BindStringColumn
                    Container.Controls.Add(txt)
                ElseIf Field1 = "Booking Code" Then
                    Dim txt As New TextBox
                    txt.ID = "txt" & i
                    txt.Columns = 6 '15
                    txt.Text = 1
                    txt.MaxLength = 50
                    txt.Style("text-align") = "center"
                    txt.CssClass = "field_input"
                    AddHandler txt.DataBinding, AddressOf BindStringColumn
                    Container.Controls.Add(txt)
                ElseIf Field1 = "Pkg" Then
                    Dim txt As New TextBox
                    txt.ID = "txt" & i
                    txt.Columns = 6 '15
                    'txt.Text = 0
                    txt.MaxLength = 10
                    txt.CssClass = "field_input"
                    AddHandler txt.DataBinding, AddressOf BindStringColumn
                    Container.Controls.Add(txt)
                ElseIf Field1 = "Remark" Then
                    Dim txt As New TextBox
                    txt.ID = "txt" & i
                    txt.Columns = 50
                    txt.MaxLength = 50
                    txt.CssClass = "field_input"
                    AddHandler txt.DataBinding, AddressOf BindStringColumn
                    Container.Controls.Add(txt)
                Else
                    Dim txt As New TextBox
                    txt.ID = "txt" & i
                    txt.Columns = 15 '10 'Changed by mohamed on 20/02/2018 '15
                    txt.MaxLength = 10
                    txt.CssClass = "field_input"
                    txt.Style.Add("FieldHeader", Field1)
                    AddHandler txt.DataBinding, AddressOf BindStringColumn
                    Container.Controls.Add(txt)

                    Dim ajxacext As New AjaxControlToolkit.AutoCompleteExtender
                    ajxacext.ID = "ajxacext" & i
                    ajxacext.CompletionInterval = 10
                    ajxacext.ServiceMethod = "getFillRateType"
                    ajxacext.CompletionListCssClass = "autocomplete_completionListElement"
                    ajxacext.CompletionListHighlightedItemCssClass = "autocomplete_highlightedListItem"
                    ajxacext.CompletionListItemCssClass = "autocomplete_listItem"
                    ajxacext.CompletionSetCount = 1
                    ajxacext.EnableCaching = False
                    ajxacext.FirstRowSelected = True
                    ajxacext.MinimumPrefixLength = 1
                    ajxacext.TargetControlID = "txt" & i
                    ajxacext.FirstRowSelected = False
                    Container.Controls.Add(ajxacext)

                    'Changed by mohamed on 20/02/2018
                    'Dim lblTV As New TextBox
                    'lblTV.Text = "TV" & i
                    'lblTV.ID = "txtTV" & i
                    'lblTV.ForeColor = Drawing.Color.White
                    ''lblTV.Font.Bold = True
                    'lblTV.Columns = 10 ' 15
                    'lblTV.Height = 30
                    'lblTV.BorderStyle = BorderStyle.None
                    'lblTV.BackColor = Drawing.Color.Transparent
                    'lblTV.CssClass = "field_caption"
                    'lblTV.Width = 150
                    'lblTV.ReadOnly = True
                    'Container.Controls.Add(lblTV)
addTVDetailTextBox:
                    Dim txtTV As New TextBox
                    txtTV.ID = "txtTV" & i
                    txtTV.Columns = 4 '15
                    'txtTV.MaxLength = 10
                    txtTV.CssClass = "field_input"
                    txtTV.Style.Add("FieldHeader", "")
                    txtTV.Style.Add("font-size", "8pt")
                    txtTV.ReadOnly = True
                    txtTV.TabIndex = -1 'changed by mohamed on 09/09/2018
                    Container.Controls.Add(txtTV)

                    Dim txtNTV As New TextBox
                    txtNTV.ID = "txtNTV" & i
                    txtNTV.Columns = 4 '15
                    'txtNTV.MaxLength = 10
                    txtNTV.CssClass = "field_input"
                    txtNTV.Style.Add("FieldHeader", "")
                    txtNTV.Style.Add("font-size", "8pt")
                    txtNTV.ReadOnly = True
                    txtNTV.TabIndex = -1 'changed by mohamed on 09/09/2018
                    Container.Controls.Add(txtNTV)

                    Dim txtVAT As New TextBox
                    txtVAT.ID = "txtVAT" & i
                    txtVAT.Columns = 4 '15
                    'txtVAT.MaxLength = 10
                    txtVAT.CssClass = "field_input"
                    txtVAT.Style.Add("FieldHeader", "")
                    txtVAT.Style.Add("font-size", "8pt")
                    txtVAT.ReadOnly = True
                    txtVAT.TabIndex = -1 'changed by mohamed on 09/09/2018
                    Container.Controls.Add(txtVAT)
                End If

                i = i + 1
        End Select
    End Sub

    Sub BindOperatorColumn(ByVal Sender As Object, ByVal e As EventArgs)
        'Dim strSqlQry As String
        Dim ddl As DropDownList = CType(Sender, DropDownList)

        Dim Container As GridViewRow = CType(ddl.NamingContainer, GridViewRow)

        'ddl.SelectedValue = DataBinder.Eval(Container.DataItem, "[Select]")

    End Sub

    Sub BindStringColumn(ByVal sender As Object, ByVal e As EventArgs)

        Dim lbl2 As TextBox = CType(sender, TextBox)

        Dim Container As GridViewRow = CType(lbl2.NamingContainer, GridViewRow)
        Try
            lbl2.Text = DataBinder.Eval(Container.DataItem, Field1)
            Dim t As Type = lbl2.[GetType]()
            Dim pi As System.Reflection.PropertyInfo = t.GetProperty("Text")

            pi.SetValue(lbl2, lbl2.Text.ToString(), Nothing)
        Catch ex As Exception

        End Try

        'For i As Integer = 0 To htControls.Count - 1
        '    If htBindPropertiesNames(i).ToString().Length > 0 Then
        '        Dim tmpCtrl As Control = DirectCast(htControls(i), Control)
        '        Dim item1Value As String = DirectCast(DataBinder.Eval(Container.DataItem, htBindExpression(i).ToString()), String)
        '        Dim ctrl As Control = sender.FindControl(tmpCtrl.ID)
        '        Dim t As Type = ctrl.[GetType]()
        '        Dim pi As System.Reflection.PropertyInfo = t.GetProperty(htBindPropertiesNames(i).ToString())

        '        pi.SetValue(ctrl, item1Value.ToString(), Nothing)

        '    End If
        'Next


    End Sub
    Public Sub Item_DataBinding(ByVal sender As Object, ByVal e As System.EventArgs)
        Dim ph As PlaceHolder = DirectCast(sender, PlaceHolder)
        Dim ri As GridViewRow = DirectCast(ph.NamingContainer, GridViewRow)
        For i As Integer = 0 To htControls.Count - 1
            If htBindPropertiesNames(i).ToString().Length > 0 Then
                Dim tmpCtrl As Control = DirectCast(htControls(i), Control)
                Dim item1Value As String = DirectCast(DataBinder.Eval(ri.DataItem, htBindExpression(i).ToString()), String)
                Dim ctrl As Control = ph.FindControl(tmpCtrl.ID)
                Dim t As Type = ctrl.[GetType]()
                Dim pi As System.Reflection.PropertyInfo = t.GetProperty(htBindPropertiesNames(i).ToString())
                pi.SetValue(ctrl, item1Value.ToString(), Nothing)
            End If
        Next
    End Sub

End Class
