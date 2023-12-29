<!-- default badges list -->
![](https://img.shields.io/endpoint?url=https://codecentral.devexpress.com/api/v1/VersionRange/128550757/11.2.11%2B)
[![](https://img.shields.io/badge/Open_in_DevExpress_Support_Center-FF7200?style=flat-square&logo=DevExpress&logoColor=white)](https://supportcenter.devexpress.com/ticket/details/E3979)
[![](https://img.shields.io/badge/📖_How_to_use_DevExpress_Examples-e9f6fc?style=flat-square)](https://docs.devexpress.com/GeneralInformation/403183)
<!-- default badges end -->
<!-- default file list -->
*Files to look at*:

* [HomeController.cs](./CS/CS/Controllers/HomeController.cs) (VB: [HomeController.vb](./VB/Controllers/HomeController.vb))
* [Model.cs](./CS/CS/Models/Model.cs) (VB: [Model.vb](./VB/Models/Model.vb))
* [Index.cshtml](./CS/CS/Views/Home/Index.cshtml)
* [TypedListDataBindingPartial.cshtml](./CS/CS/Views/Home/TypedListDataBindingPartial.cshtml)
* [_Layout.cshtml](./CS/CS/Views/Shared/_Layout.cshtml)
<!-- default file list end -->
# GridView - How to track changes made with an unbound CheckBox in the DataItemTemplate


<p>This example illustrates how to:<br />
- Define a custom editor (for example, the CheckBox) inside the column's <strong>DateItemTemplate </strong>(via the <a href="http://documentation.devexpress.com/#AspNet/DevExpressWebMvcGridViewSettings_SetDataItemTemplateContenttopic"><u>GridViewSettings.SetDataItemTemplateContent</u></a> method);<br />
- Specify the <strong>CheckBox.Name</strong> property based on the corresponding DataRow's <strong>keyValue</strong> (this operation requires specifying the <strong>GridViewSettings.KeyFieldName</strong> property);<br />
- Subscribe the client-side <strong>ASPxClientCheckBox.CheckedChanged</strong> event;<br />
- Store the CheckBox's state via a custom JavaScript object like a <strong>dictionary/key value pairs</strong> (the CheckBox.Name - Checked State)</p><p>An end-user is able to select/deselect the CheckBoxes displayed within the column.<br />
When the user tries to perform <strong>any GridView's callback</strong> (sorting, paging, etc.), the stored key value pairs collection <strong>is serialized</strong> via the "toJSON" plugin (available with the <a href="http://code.google.com/p/jquery-json/"><u>jquery-json</u></a> library). The serialized object is passed to a Controller as custom argument according to the "<a href="http://documentation.devexpress.com/#AspNet/CustomDocument9941"><u>Passing Values to Controller Action Through Callbacks</u></a>" technique.<br />
The passed serialized <strong>object is de-serialized</strong> on the Controller side and passed to the View object to restore an unbound CheckBox state.</p><p><strong>View:</strong><br />
</p>

```js
<script type="text/javascript">
    var DXHiddenObject = { };

    function OnBeginCallback(s, e) {
        e.customArgs["items"] = $.toJSON(DXHiddenObject);
    }

    function OnCheckedChanged(s, e) {
        if(s.GetChecked())
            DXHiddenObject[s.name] = true;
        else
            delete DXHiddenObject[s.name];
    }
</script> 

```

<p> </p><p><strong>PartialView:</strong></p><p><strong>C#:</strong><br />
</p>

```cs
@Html.DevExpress().GridView(settings => {
    settings.Name = "gvTypedListDataBinding";
    settings.CallbackRouteValues = new { Controller = "Home", Action = "TypedListDataBindingPartial" };
    settings.KeyFieldName = "ID";
    ...
    settings.Columns.Add(column => {
        column.SetDataItemTemplateContent(c => {
            Html.DevExpress().CheckBox(checkboxSettings => {
                checkboxSettings.Name = "cb_" + c.KeyValue.ToString();
                checkboxSettings.Properties.ClientSideEvents.CheckedChanged = "OnCheckedChanged";
                if(ViewData["items"] != null) {
                    Dictionary<string, bool> items = (Dictionary<string, bool>)ViewData["items"];
                    checkboxSettings.Checked = items.ContainsKey(checkboxSettings.Name) && (bool)items[checkboxSettings.Name];
                }              
            }).Render();
        });
    });

    settings.ClientSideEvents.BeginCallback = "OnBeginCallback";

}).Bind(Model).GetHtml()

```

<p><strong>VB.NET:</strong><br />
</p>

```vb
@Html.DevExpress().GridView( _
    Sub(settings)
            settings.Name = "gvTypedListDataBinding"
            settings.CallbackRouteValues = New With {Key .Controller = "Home", Key .Action = "TypedListDataBindingPartial"}
            settings.KeyFieldName = "ID"
            ...        
            settings.Columns.Add( _
                Sub(column)
                        column.SetDataItemTemplateContent( _
                            Sub(c)
                                    Html.DevExpress().CheckBox( _
                                        Sub(checkboxSettings)
                                                checkboxSettings.Name = "cb_" + c.KeyValue.ToString()
                                                checkboxSettings.Properties.ClientSideEvents.CheckedChanged = "OnCheckedChanged"
                                                If (ViewData("items") IsNot Nothing) Then
                                                    Dim items As Dictionary(Of String, Boolean) = CType(ViewData("items"), Dictionary(Of String, Boolean))
                                                    checkboxSettings.Checked = items.ContainsKey(checkboxSettings.Name) AndAlso CBool(items(checkboxSettings.Name))
                                                End If
                                        End Sub).Render()
                            End Sub)
                End Sub)

            settings.ClientSideEvents.BeginCallback = "OnBeginCallback"
    End Sub).Bind(Model).GetHtml()

```

<p><strong>Controller:<br />
</strong></p>

```cs
public class HomeController : Controller {
	public ActionResult Index() {
		if(Session["TypedListModel"] == null)
			Session["TypedListModel"] = InMemoryModel.GetTypedListModel();
		return View(Session["TypedListModel"]);
	}

	public ActionResult TypedListDataBindingPartial() {
		ViewData["items"] = GetSerializedObject(Request["items"]);
		return PartialView(Session["TypedListModel"]);
	}

	private Dictionary<string, bool> GetSerializedObject(string inputString) {
		if(string.IsNullOrEmpty(inputString))
			return null;

		Dictionary<string, bool> items = null;
		try {
			items = new JavaScriptSerializer().Deserialize<Dictionary<string, bool>>(inputString);
		}
		catch {
			ViewData["ErrorMessage"] = "Invalid Input JSON String";
		}
		return items;
	}
}

```

<p><strong>VB.NET</strong></p>

```vb
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
```

<p> </p>

<br/>


