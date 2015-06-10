<%@ Control Language="C#" ClassName="Test.DNN6662.View" Inherits="DotNetNuke.Entities.Modules.PortalModuleBase" %>
<%@ Register TagPrefix="dnn" TagName="JavaScriptLibraryInclude" Src="~/admin/Skins/JavaScriptLibraryInclude.ascx" %>
  
<dnn:JavaScriptLibraryInclude runat="server" Name="kendo.mobile.actionsheet" />

<link rel="stylesheet" href="http://cdn.kendostatic.com/2014.3.1411/styles/kendo.common.min.css" />
<link rel="stylesheet" href="http://cdn.kendostatic.com/2014.3.1411/styles/kendo.default.min.css" />
<link rel="stylesheet" href="http://cdn.kendostatic.com/2014.3.1411/styles/kendo.dataviz.min.css" />
<link rel="stylesheet" href="http://cdn.kendostatic.com/2014.3.1411/styles/kendo.dataviz.default.min.css" />
<link rel="stylesheet" href="http://cdn.kendostatic.com/2014.3.1411/styles/kendo.default.mobile.min.css" />

<div id="<%:ClientID%>">
<div id="example">

    <div class="demo-section k-header">
        <div class="head">
            <div id="action-result"></div>
            <h4>Select a file to edit</h4>
        </div>
        <table id="grid">
            <colgroup>
                <col />
                <col />
                <col style="width:110px" />
                <col style="width:120px" />
                <col style="width:130px" />
            </colgroup>
            <thead>
                <tr>
                    <th data-field="name">Name</th>
                    <th data-field="type">Type</th>
                    <th data-field="size">Size</th>
                </tr>
            </thead>
            <tbody>
                <tr>
                    <td>index.html</td>
                    <td>HTML file</td>
                    <td>1 KB</td>
                </tr>
                <tr>
                    <td>services.html</td>
                    <td>HTML file</td>
                    <td>1 KB</td>
                </tr>
                <tr>
                    <td>aboutus.html</td>
                    <td>HTML file</td>
                    <td>1 KB</td>
                </tr>
                <tr>
                    <td>contacts.html</td>
                    <td>HTML file</td>
                    <td>1 KB</td>
                </tr>
                <tr>
                    <td>logo.png</td>
                    <td>PNG file</td>
                    <td>1 KB</td>
                </tr>
                <tr>
                    <td>phone.png</td>
                    <td>PNG file</td>
                    <td>15 KB</td>
                </tr>
                <tr>
                    <td>map.jpg</td>
                    <td>JPG image</td>
                    <td>12 KB</td>
                </tr>
            </tbody>
        </table>
    </div>

    <ul id="actions">
        <li><a href="#" data-action="view">View</a></li>
        <li><a href="#" data-action="rename">Rename</a></li>
        <li><a href="#" data-action="del">Delete</a></li>
        <li><a href="#" data-action="permissions">Set permissions ...</a></li>
    </ul>
</div>

<script>
    $(function() {
    try{
        $("#actions").kendoMobileActionSheet({ type: "tablet" });
}  catch (e) {
    console.log(e);
    $('<div></div>', { 'class': 'dnnFormMessage dnnFormValidationSummary', text: 'Failure to load' }).appendTo('#<%:ClientID%>')
  }

        $("#grid").on("click", "tr", function() {
            $("#actions").data("kendoMobileActionSheet").open(this);
        });
    });

    function view(e) {
        $("#action-result").html("Action clicked: View");
    }

    function rename(e) {
        $("#action-result").html("Action clicked: Rename");
    }

    function del(e) {
        $("#action-result").html("Action clicked: Delete");
    }
    function permissions(e) {
        $("#action-result").html("Action clicked: Set permissions ...");
    }
</script>

<style scoped>
    .demo-section {
        width: 600px;
    }
    .head {
        height: 30px;
    }
    #action-result {
        color: #ff0000;
        float: right;
        width: 200px;
        text-align: right;
    }
    .k-group {
        background-color: transparent;
    }
    .k-popup {
        box-shadow: none;
        -webkit-box-shadow: none;
    }
</style>
</div>
