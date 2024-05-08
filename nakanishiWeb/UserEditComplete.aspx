<%@ Page Language="C#" AutoEventWireup="true" MasterPageFile="~/Site.Master" CodeBehind="UserEditComplete.aspx.cs" Inherits="nakanishiWeb.UserEditComplete" %>
<%@ Import Namespace="nakanishiWeb.Const" %>

<asp:Content ID="BodyContent" ContentPlaceHolderID="MainContent" runat="server">
    <form id="changePager" target="_self" runat="server">
        <div id="mainPagesContents" class="grid main underNavContent close">

            <div class="titleBox hoverP">
            </div>

            <div class="relative">

<!--*** ページタイトル ***-->
                <div class="title">
                    <h1 id="UserEditPage-1"><%=CommonWords[(int)LanguageTable.CommonPageStrId.Common_64] %></h1>
                </div>
<!--*** ページタイトルここまで ***-->
                <div class="CompleteMsg">
                    <%=CommonWords[(int)LanguageTable.CommonPageStrId.Common_67] %>
                </div>

            </div>
        <input type="hidden" name="pageChange" id="pageChange" />
        <button type="submit" id="pageChangeBT" class="hide"></button>
        </div>
    </form>
    <script src="js/jquery-3.6.1.min.js"></script>
    <script src="js/jquery-ui.js"></script>
    <script src="js/Functions.js"></script>
    <script type="text/javascript">
    </script>
</asp:Content>
