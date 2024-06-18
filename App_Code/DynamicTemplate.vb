Imports Microsoft.VisualBasic
Imports System
Imports System.Data
Imports System.Configuration
Imports System.Web
Imports System.Web.Security
Imports System.Web.UI
Imports System.Web.UI.WebControls
Imports System.Web.UI.WebControls.WebParts
Imports System.Web.UI.HtmlControls
Imports System.ComponentModel
Imports System.Collections

Public Class DynamicTemplate
    Dim templateType As System.Web.UI.WebControls.ListItemType
    Dim htControls As New System.Collections.Hashtable()
    Dim htBindPropertiesNames As New System.Collections.Hashtable()
    Dim htBindExpression As New System.Collections.Hashtable()
    Public Sub DynamicTemplate(ByVal type As System.Web.UI.WebControls.ListItemType)
        templateType = type
    End Sub
    Public Sub AddControl(ByVal wbControl As WebControl, ByVal BindPropertyName As String, ByVal BindExpression As String)
        htControls.Add(htControls.Count, wbControl)
        htBindPropertiesNames.Add(htBindPropertiesNames.Count, BindPropertyName)
        htBindExpression.Add(htBindExpression.Count, BindExpression)
    End Sub
    Public Sub InstantiateIn(ByVal container As System.Web.UI.Control)
        Dim ph As New PlaceHolder
        Dim i As Integer
        For i = 0 To htControls.Count
            Dim cntrl As Control
            cntrl = CType(htControls(i), Control)
            Select Case templateType
                Case ListItemType.Header
                Case ListItemType.Item
                    ph.Controls.Add(cntrl)
                Case ListItemType.AlternatingItem
                    ph.Controls.Add(cntrl)
                    ' ph.DataBind = New EventHandler(Item_DataBinding)
                Case ListItemType.Footer
            End Select
            '
        Next
        'ph.DataBind = New EventHandler(Item_DataBinding)
        container.Controls.Add(ph)
    End Sub
    Public Sub Item_DataBinding(ByVal sender As Object, ByVal e As System.EventArgs)
        Dim i As Long
        Dim s As Object = Nothing
        Dim ph As New PlaceHolder
        ph = CType(sender, PlaceHolder)
        Dim ri As GridViewRow
        ri = CType(ph.NamingContainer, GridViewRow)
        For i = 0 To htControls.Count
            If (htBindPropertiesNames(i).ToString().Length > 0) Then
                Dim tmpCtrl As Control
                tmpCtrl = CType(htControls(i), Control)
                Dim item1Value As String
                item1Value = DataBinder.Eval(ri.DataItem, htBindExpression(i).ToString())
                Dim ctrl As Control
                ctrl = ph.FindControl(tmpCtrl.ID)
                Dim t As Type
                t = ctrl.GetType()
                Dim pi As System.Reflection.PropertyInfo
                pi = t.GetProperty(htBindPropertiesNames(i).ToString())
                pi.SetValue(ctrl, item1Value.ToString(), s)

            End If

        Next


    End Sub
    'Private Function CloneControl(ByVal src_ctl As System.Web.UI.Control) As Control
    '    Dim t As Type
    '    t = src_ctl.GetType()
    '    Dim obj As Object
    '    obj = Activator.CreateInstance(t)
    '    Dim dst_ctl As Control
    '    dst_ctl = CType(obj, Control)
    '    Dim src_pdc As PropertyDescriptorCollection
    '    src_pdc = TypeDescriptor.GetProperties(src_ctl)
    '    Dim dst_pdc As PropertyDescriptorCollection
    '    dst_pdc = TypeDescriptor.GetProperties(dst_ctl)
    '    Dim i As Long
    '    Dim child As Object
    '    For i = 0 To src_pdc.Count
    '        If (src_pdc(i).Attributes.Contains(DesignerSerializationVisibilityAttribute.Content)) Then
    '            Dim collection_val As Object
    '            collection_val = CType(collection_val, IList)
    '            If ((collection_val) = True) Then
    '                For Each child In collection_val
    '                    Dim new_child As Control
    '                    new_child = CloneControl(CType(child, Control))
    '                    Dim dst_collection_val As Object
    '                    dst_collection_val = dst_pdc(i).GetValue(dst_ctl)
    '                    dst_collection_val.Add(new_child)
    '                Next
    '            End If
    '        Else
    '            dst_pdc(src_pdc(i).Name).SetValue(dst_ctl, src_pdc(i).GetValue(src_ctl))
    '        End If
    '    Next
    '    Return dst_ctl
    'End Function

End Class
