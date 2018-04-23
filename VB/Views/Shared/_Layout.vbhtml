<!DOCTYPE HTML>
<html>
<head>
    <title>@ViewData("Title")</title>
    @Html.DevExpress().GetStyleSheets(
        New StyleSheet With {.ExtensionSuite = ExtensionSuite.NavigationAndLayout},
        New StyleSheet With {.ExtensionSuite = ExtensionSuite.Editors},
        New StyleSheet With {.ExtensionSuite = ExtensionSuite.GridView}
    )
    @Html.DevExpress().GetScripts(
        New Script With {.ExtensionSuite = ExtensionSuite.NavigationAndLayout},
        New Script With {.ExtensionSuite = ExtensionSuite.Editors},
        New Script With {.ExtensionSuite = ExtensionSuite.GridView}
    )
    <script src='@Url.Content("~/Scripts/jquery.json-2.3.min.js")' type="text/javascript"></script>
</head>
<body>
    @RenderBody()
</body>
</html>