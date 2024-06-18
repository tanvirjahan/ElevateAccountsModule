Imports Microsoft.VisualBasic
Imports System.Data
Imports System.Data.SqlClient


Public Class ClassSellingSupplier
    Implements ITemplate

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
                lbl.Text = Field1
                lbl.ID = "txtHead" & k
                lbl.CssClass = "field_caption"
                lbl.ForeColor = Drawing.Color.White
                lbl.Font.Bold = True
                lbl.Columns = 15
                lbl.Height = 18
                lbl.BorderStyle = BorderStyle.None
                lbl.BackColor = Drawing.Color.Transparent
                lbl.ReadOnly = True
                Container.Controls.Add(lbl)
                k = k + 1
            Case ListItemType.Item

                If Field1 = "Room Type" Then
                    Dim txt As New TextBox
                    txt.ID = "txt" & i
                    txt.CssClass = "txtbox"
                    txt.ReadOnly = True
                    txt.Columns = 15
                    txt.BorderStyle = BorderStyle.None
                    AddHandler txt.DataBinding, AddressOf BindStringColumn
                    Container.Controls.Add(txt)
                ElseIf Field1 = "Room Type Name" Then
                    Dim txt As New TextBox
                    txt.ID = "txt" & i
                    txt.CssClass = "txtbox"
                    txt.ReadOnly = True
                    txt.Columns = 15
                    txt.BorderStyle = BorderStyle.None
                    AddHandler txt.DataBinding, AddressOf BindStringColumn
                    Container.Controls.Add(txt)
                ElseIf Field1 = "Sr No" Then
                    Dim txt As New TextBox
                    txt.ID = "txt" & i
                    txt.CssClass = "txtbox"
                    txt.ReadOnly = True
                    txt.Columns = 4
                    txt.BorderStyle = BorderStyle.None
                    AddHandler txt.DataBinding, AddressOf BindStringColumn
                    Container.Controls.Add(txt)
                Else

                    Dim ddl As New DropDownList
                    ddl.ID = "ddl" & i
                    ddl.CssClass = "drpdown"

                    Try
                        strSqlQry = "SELECT operatorsymbol FROM operatormast"
                        SqlConn = clsDBConnect.dbConnection               'Open connection
                        myCommand = New SqlCommand(strSqlQry, SqlConn)
                        myDataReader = myCommand.ExecuteReader()
                        While myDataReader.Read = True
                            ddl.Items.Add(New ListItem(myDataReader(0).ToString, myDataReader(0).ToString))
                        End While
                        'ddl.DataSource = myDataReader
                        'ddl.DataTextField = "operatorsymbol"
                        'ddl.DataValueField = "operatorsymbol"
                        'ddl.DataBind()
                        ddl.Items.Add("[Select]")
                        ddl.SelectedValue = "[Select]"

                    Catch ex As Exception

                    Finally
                        ' clsDBConnect.dbCommandClose(myCommand)                  'Close command 
                        'clsDBConnect.dbReaderClose(myDataReader)                'Close reader
                        clsDBConnect.dbConnectionClose(SqlConn)                 'Close connection
                    End Try

                    AddHandler ddl.DataBinding, AddressOf BindOperatorColumn
                    Container.Controls.Add(ddl)
                End If

                '---------------------------------
                If Field1 <> "Sr No" Then
                    Dim txt As New TextBox
                    txt.ID = "txt" & i
                    txt.Columns = 15
                    txt.CssClass = "txtbox"
                    'txt.MaxLength = 10
                    AddHandler txt.DataBinding, AddressOf BindStringColumn
                    Container.Controls.Add(txt)

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
