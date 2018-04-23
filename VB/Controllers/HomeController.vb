Imports System
Imports System.Collections.Generic
Imports System.Web.Script.Serialization
Imports System.Web.Mvc

Public Class HomeController
    Inherits Controller
    Public Function Index() As ActionResult
        If Session("TypedListModel") Is Nothing Then
            Session("TypedListModel") = InMemoryModel.GetTypedListModel()
        End If

        Return View(Session("TypedListModel"))
    End Function

    Public Function TypedListDataBindingPartial() As ActionResult
        ViewData("items") = GetSerializedObject(Request("items"))

        Return PartialView(Session("TypedListModel"))
    End Function

    Private Function GetSerializedObject(ByVal inputString As String) As Dictionary(Of String, Boolean)
        If String.IsNullOrEmpty(inputString) Then
            Return Nothing
        End If

        Dim items As Dictionary(Of String, Boolean) = Nothing

        Try
            items = New JavaScriptSerializer().Deserialize(Of Dictionary(Of String, Boolean))(inputString)
        Catch
            ViewData("ErrorMessage") = "Invalid Input JSON String"
        End Try

        Return items
    End Function
End Class