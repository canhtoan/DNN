<%@ Control Language="C#" AutoEventWireup="false" Inherits="Test.ModuleMessageTest.View" CodeFile="View.ascx.cs" %>

<%@ Register TagPrefix="dnn" TagName="Label" Src="~/controls/LabelControl.ascx" %>

<div class="dnnForm">
   <div class="dnnFormItem">
      <dnn:label runat="server" id="lblTest" Text="Test" Suffix=":" CssClass="dnnFormRequired" />
      <asp:TextBox runat="server" id="txtTest" />
      <asp:RequiredFieldValidator runat="server" id="TestRequired" ControlToValidate="txtTest"
          CssClass="dnnFormMessage dnnFormError" text="Required" />
   </div>
   <div class="dnnFormItem">
      <dnn:label runat="server" id="lblTest2" Text="Test 2" Suffix=":" CssClass="dnnFormRequired" />
      <asp:RadioButtonList runat="server" id="rblDemo" CssClass="dnnFormRadioButtons" RepeatLayout="UnorderedList">
        <asp:ListItem Text="Item 1" Value="1" />
        <asp:ListItem Text="Item 2" Value="2" />
       </asp:RadioButtonList>
   </div>
   <ul class="dnnActions">
      <li><asp:LinkButton runat="Server" id="btnSave" text="Save" CssClass="dnnPrimaryAction" /></li>
      <li><a href="/" class="dnnSecondaryAction">Cancel</a></li>
   </ul>
</div>

