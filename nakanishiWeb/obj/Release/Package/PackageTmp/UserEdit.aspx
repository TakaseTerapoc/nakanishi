<%@ Page Language="C#" AutoEventWireup="true" MasterPageFile="~/Site.Master" CodeBehind="UserEdit.aspx.cs" Inherits="nakanishiWeb.UserEdit" %>
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

            <div class="editFormGroup">
                <label for="name"><%=UserEditPageWords[(int)LanguageTable.UserEditPageStrId.UserEdit_0] %></label>
                <label for="name"><%=userName %></label>
            </div>
            <div class="editFormGroup">
                <label for="id"><%=UserEditPageWords[(int)LanguageTable.UserEditPageStrId.UserEdit_1] %></label>
                <label for="id"><%=loginID %></label>
            </div>
            <div class="editFormGroup">
                <label for="password"><%=UserEditPageWords[(int)LanguageTable.UserEditPageStrId.UserEdit_2] %></label>
                <input type="password" id="passInput" name="password" required="required" />
            </div>
            <div class="editFormGroup">
                <label for="password"><%=UserEditPageWords[(int)LanguageTable.UserEditPageStrId.UserEdit_3] %></label>
                <input type="password" id="passInputNew1" name="newPassword1" size="16" maxlength="16"/>
                <%=UserEditPageWords[(int)LanguageTable.UserEditPageStrId.UserEdit_6] %>
            </div>
            <div class="editFormGroup">
                <label for="password"><%=UserEditPageWords[(int)LanguageTable.UserEditPageStrId.UserEdit_4] %></label>
                <input type="password" id="passInputNew2" name="newPassword2"/>
            </div>
            <div class="editFormGroup">
                <label for="password"><%=UserEditPageWords[(int)LanguageTable.UserEditPageStrId.UserEdit_5] %></label>

                <select name="s_timeH">
                    <%for (int i = 0; i < 24; i++)
                      {%>
                    <option value="<%=i%>" <%=(i == mailStartTime.Hours) ? "selected" : ""%>><%=i.ToString("00")%></option>
                    <%}%>
                </select>
                時
                <select name="s_timeM">
                    <!-- 15分刻み -->
                    <%for (int i = 0; i < 60; i+=15)
                      {%>
                    <option value="<%=i%>" <%=(i == mailStartTime.Minutes) ? "selected" : ""%>><%=i.ToString("00")%></option>
                    <%}%>
                </select>
                分　～　
                <select name="e_timeH">
                    <%for (int i = 0; i < 24; i++)
                      {%>
                    <option value="<%=i%>" <%=(i == mailEndTime.Hours) ? "selected" : ""%>><%=i.ToString("00")%></option>
                    <%}%>
                </select>
                時
                <select name="e_timeM">
                    <%for (int i = 0; i < 60; i+=15)
                      {%>
                    <option value="<%=i%>" <%=(i == mailEndTime.Minutes) ? "selected" : ""%>><%=i.ToString("00")%></option>
                    <%}%>
                </select>
                分
            </div>
            <div class="editFormGroup center">
                <input type="hidden" name="editSet" value="" id="editSetInput"/>
                <button type="button" onclick="editUserSetting()" class="change relative"><%=CommonWords[(int)LanguageTable.CommonPageStrId.Common_65] %></button>
                <button type="submit" id="editSet" class="hide"></button>
            </div>
        </div>
        <input type="hidden" name="pageChange" id="pageChange" />
        <button type="submit" id="pageChangeBT" class="hide" formnovalidate></button>
        </div>
    </form>
    <script src="js/jquery-3.6.1.min.js"></script>
    <script src="js/jquery-ui.js"></script>
    <script src="js/Functions.js"></script>
    <script type="text/javascript">
        $(function () {
            $("input").on("keydown", function (e) {
                if ((e.which && e.which === 13) || (e.keyCode && e.keyCode === 13)) {
                    return false;
                } else {
                    return true;
                }
            });
        });
    </script>
</asp:Content>
