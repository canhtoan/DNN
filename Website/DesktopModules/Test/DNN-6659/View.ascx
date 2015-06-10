<%@ Control Language="C#" AutoEventWireup="false" Inherits="Test.DNN6659.View" CodeFile="View.ascx.cs" %>

<%@ Register TagPrefix="dnn" TagName="JavaScriptLibraryInclude" Src="~/admin/skins/JavaScriptLibraryInclude.ascx" %>

<dnn:JavaScriptLibraryInclude runat="server" Name="Knockout" />

<div id="<%:ClientID%>">
  <div class="dnnFormMessage dnnFormValidationSummary" data-bind="visible: !loaded">
    Knockout has not loaded
  </div>
  <p data-bind="text: date"></p>

  <p>Cached at <strong><%:DateTime.Now%></strong></p>
</div>
<script>
    jQuery(function() {
        ko.applyBindings({ date: new Date(), loaded: true }, document.getElementById('<%:ClientID%>'));
    });
</script>