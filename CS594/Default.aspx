<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Default.aspx.cs" Inherits="CS594Web.Default" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <link rel="stylesheet" href="http://bootswatch.com/superhero/bootstrap.min.css" />
    <script src="https://ajax.googleapis.com/ajax/libs/jquery/1.9.1/jquery.min.js"></script>
    <script src="http://maxcdn.bootstrapcdn.com/bootstrap/3.3.6/js/bootstrap.min.js"></script>
    <title></title>
    <script type="text/javascript">
        function showMethods() {
            alert('Hello');
            document.getElementById('btnDummy').click();
            document.getElementById('btnDummy').textContent = 'True';
            return false;
        }
    </script>
</head>
<body class="container">
    <form runat="server">
        <asp:ScriptManager ID="ScriptManager" runat="server" AjaxFrameworkMode="Enabled" />
        <asp:UpdatePanel runat="server" ID="upPanel">
            <ContentTemplate>

                <%-- Start: Code for Error Panel --%>
                <asp:Panel ID="pnlError" runat="server" CssClass="display:block;" align="center" Width="1000">
                    <div class="row">
                        <div class="form-group">
                            <asp:Label ID="lblErrorHeader" runat="server" />
                        </div>
                    </div>
                    <div class="row">
                        <div class="form-group">
                            <asp:Label ID="lblErrorMessage" runat="server" />
                        </div>
                    </div>
                    <div class="row">
                        <div class="form-group">
                            <asp:Button ID="btnCancelError" runat="server" Text="Ok" />
                        </div>
                    </div>
                </asp:Panel>
                <%-- End : Code for Error Panel --%>

                <%-- Start : Page Code --%>
                <header class="page-header">
                    <h1>WebService Performance Test ASP.NET</h1>
                </header>

                <%-- Start : Code for Left Menu --%>
                <div class="col-md-3">
                    <asp:Panel ID="menu" CssClass="panel panel-success" Style="padding: 20px;" GroupingText="Menu" runat="server">
                        <asp:Button ID="btnHome" Text="Home" CssClass="btn" OnClick="btnMenu_Click" runat="server" Width="90%" />
                        <asp:Button ID="btnPredefined" Text="New" CssClass="btn" OnClick="btnMenu_Click" runat="server" Width="90%" />
                        <asp:Button ID="btnComparison" Text="Comparison" CssClass="btn" OnClick="btnMenu_Click" runat="server" Width="90%" />
                    </asp:Panel>
                </div>
                <%-- End : Code for Left Menu --%>

                <%-- Start : Right panels --%>
                <div class="col-md-8">
                    <div class="row">
                        <%-- Start : Code for Predefined url Panel --%>
                        <asp:Panel ID="pnlPredefined" runat="server" Visible="true" Style="margin: 20px;">
                            <asp:Label runat="server" AssociatedControlID="ddlSavedServices" Text="Select URL" CssClass="control-label float-left required" />
                            <asp:DropDownList ID="ddlSavedServices" runat="server" CssClass="form-control" ValidationGroup="oldUrl" />
                            <asp:RequiredFieldValidator ID="rfvSavedServices" runat="server" ControlToValidate="ddlSavedServices" InitialValue="0"
                                ErrorMessage="Please Select Service" EnableClientScript="true" ValidationGroup="oldUrl" Display="None" />
                            <Ajax:ValidatorCalloutExtender runat="Server" ID="vceSavedServices" TargetControlID="rfvSavedServices" CssClass="alert" PopupPosition="BottomRight"/>
                        </asp:Panel>
                        <%-- End : Code for Predefined url Panel --%>


                        <%-- Start : Code for Add New url Panel --%>
                        <asp:Panel runat="server" ID="pnlAddNew" Visible="false" Style="margin: 20px;">
                            <asp:Label runat="server" AssociatedControlID="txtUrl" Text="Enter URL" CssClass="control-label float-left required" />
                            <asp:TextBox runat="server" ID="txtUrl" CssClass="form-control float-right" ValidationGroup="addUrl" MaxLength="250" />
                            <asp:RequiredFieldValidator ID="RFVtxtIntA" runat="server" ControlToValidate="txtUrl" 
                                ErrorMessage="Please Enter URL" EnableClientScript="true" ValidationGroup="addUrl" Display="None" />
                            <Ajax:ValidatorCalloutExtender runat="Server" ID="VCEtxtIntA" TargetControlID="RFVtxtIntA" CssClass="alert" PopupPosition="BottomRight"/>
                        </asp:Panel>
                        <%-- End : Code for Add New url Panel --%>
                    </div>

                    <div class="row">
                        <%--<asp:Button ID="btnDummy" runat="server" Text="hello" /> 
                        <Ajax:CollapsiblePanelExtender ID="cprExtender" runat="server" TargetControlID="pnlMethods" ExpandControlID="btnDummy"
                            Collapsed="true" ExpandDirection="Vertical" SuppressPostBack="true"  />--%>
                        <asp:Panel ID="pnlMethods" runat="server" Style="margin: 20px;" Height="60" Visible="false">
                            <asp:Label runat="server" AssociatedControlID="ddlMethods" Text="Select Method" CssClass="control-label float-left required" />
                            <asp:DropDownList ID="ddlMethods" runat="server" CssClass="form-control"></asp:DropDownList>
                        </asp:Panel>
                    </div>

                    <%-- Start : Code for Dynamic panel--%>
                    <div class="row">
                        <asp:Panel ID="pnlButtons" runat="server" Style="margin: 20px;" Visible="false">
                            <asp:Table runat="server" ID="tblButtons" Width="96%">
                            </asp:Table>
                        </asp:Panel>
                    </div>
                    <%-- End : Code for Dynamic panel--%>

                    <%-- Start : Submit button --%>
                    <div class="row">
                        <center>
                                <div class="form-group">
                                    <asp:Button ID="btnSubmit" runat="server" Text="Submit" CssClass="btn btn-primary"
                                     OnClick="btnSubmit_Click" ValidationGroup="oldUrl" />
                                    <asp:Button ID="btnNew" runat="server" Text="Home" CssClass="btn"
                                     OnClick="btnNew_Click" Visible="false" />
                                </div>
                        </center>
                    </div>
                    <%-- End : Submit button --%>

                    <%-- Start : Comparison Panel--%>
                    <div class="row">
                        <asp:Panel ID="pnlResult" runat="server" Visible="false" Style="margin: 20px;">

                            <div class="row">
                                <Ajax:BarChart ID="bcRecords" runat="server" ChartWidth="750" ChartHeight="250"
                                    ChartType="Column" ChartTitleColor="#f0ad4e" CategoryAxisLineColor="#f0ad4e" BorderStyle="None"
                                    ValueAxisLineColor="#f0ad4e" BaseLineColor="#f0ad4e">
                                   <Series ></Series>
                                </Ajax:BarChart>
                            </div>

                            <div class="row" style="margin-top: 20px;">
                                <div class="form-group col-sm-5">
                                    <asp:DropDownList ID="ddlSelectedCategory" CssClass="form-control" runat="server" OnSelectedIndexChanged="ddlRecordsCount_SelectedIndexChanged" AutoPostBack="true">
                                    </asp:DropDownList>
                                </div>

                                <div class="form-group col-sm-2">
                                    <asp:Label ID="lblComparatorOne" CssClass="control-label" runat="server" Text="Compare with" AssociatedControlID="ddlCompareOne" />
                                </div>

                                <div class="form-group col-md-5">
                                    <asp:DropDownList ID="ddlCompareOne" AutoPostBack="true" OnSelectedIndexChanged="ddlRecordsCount_SelectedIndexChanged" runat="server"
                                        CssClass="form-control">
                                    </asp:DropDownList>
                                </div>
                            </div>

                            <div class="row">
                                <div class="form-group col-sm-5">
                                    <asp:Label runat="server" Text="Select No. of records" CssClass="control-label" AssociatedControlID="ddlRecordsCount" />
                                    <asp:DropDownList ID="ddlRecordsCount" AutoPostBack="true" OnSelectedIndexChanged="ddlRecordsCount_SelectedIndexChanged"
                                        runat="server" CssClass="form-control">
                                        <asp:ListItem Value="5" Text="5" />
                                        <asp:ListItem Value="10" Text="10" />
                                        <asp:ListItem Value="20" Text="20" />
                                    </asp:DropDownList>
                                </div>

                                <div class="form-group col-sm-5 col-md-offset-2">
                                    <asp:Label runat="server" Text="Select No. of repetations done" CssClass="control-label" AssociatedControlID="ddlRecordsCount" />
                                    <asp:DropDownList ID="ddlRepetataions" AutoPostBack="true" OnSelectedIndexChanged="ddlRecordsCount_SelectedIndexChanged"
                                        runat="server" CssClass="form-control">
                                    </asp:DropDownList>
                                </div>
                            </div>

                        </asp:Panel>
                    </div>
                    <%-- End : Comparison Panel--%>
                </div>
                <%-- End : Right panels --%>

                <%-- End : Page COde --%>
            </ContentTemplate>
        </asp:UpdatePanel>
    </form>
</body>
</html>
