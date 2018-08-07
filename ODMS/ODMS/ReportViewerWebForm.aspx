<%@ Page Language="C#" AutoEventWireup="True"  Inherits="ReportViewerForMvc.ReportViewerWebForm" %>

<%@ Register Assembly="Microsoft.ReportViewer.WebForms, Version=11.0.0.0, Culture=neutral, PublicKeyToken=89845dcd8080cc91" Namespace="Microsoft.Reporting.WebForms" TagPrefix="rsweb" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
    <style>
    /* this will remove the spinner */
        div#ReportViewer1_AsyncWait {
            
             display:block;
        } 
    
      
    </style>
</head>
<body style="margin: 0px; padding: 0px;">
    <form id="form1" runat="server" style="width:100%; height:100%;">
        <div  >
            <asp:ScriptManager ID="ScriptManager1" runat="server">
                <Scripts>
                    <asp:ScriptReference Assembly="ReportViewerForMvc" Name="ReportViewerForMvc.Scripts.PostMessage.js" />
                </Scripts>
            </asp:ScriptManager>
            

            <rsweb:ReportViewer ID="ReportViewer1"  runat="server" ></rsweb:ReportViewer>
        </div>
    </form>
</body>
</html>


