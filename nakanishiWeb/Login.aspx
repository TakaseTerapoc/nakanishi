<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Login.aspx.cs" Inherits="nakanishiWeb.Login" %>
<%@ Import Namespace="nakanishiWeb.Const" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <link rel="stylesheet" href="css/reset.css" />
    <link rel="stylesheet" href="css/layout.css" />
    <link rel="stylesheet" href="css/login.css" />
<meta http-equiv="Content-Type" content="text/html; charset=utf-8"/>
    <title></title>
    <script type="text/javascript">
        <%-- takase0508--%>
        <%-- takase0507--%>
        <%--function CustomLogin(isLangChange) {
            const select = document.querySelector("#selector");
            const selectedIndex = select.selectedIndex;
            console.log(select.options[selectedIndex].value);
            if (isLangChange) {
                document.querySelector("#langChange").value = "on";
            }
            loginForm.submit();
        }--%>
    </script>
    <meta name="google-signin-client_id" content="168970079039-uta045hh51raiosrqmg0ab6ud61as2jc.apps.googleusercontent.com"/>
</head>
    <body id="login">
        <div class="rightNavContent grid">
            <div class="title">
                <h1 id="Login">LOGIN</h1>
            </div>
            <div class="main">
                <h2 class="bold">NAKANISHI IoT SYSTEM</h2>
                <%-- takase0507--%>
            <%--<p id="headDate"><%: DateTime.Now.Month%>月<%: DateTime.Now.Day%>日</p>--%>
                <%if(errorMsg != ""){ %>
                    <p class="point bold"><%=errorMsg%></p>
                <%} %>
            <form id="loginForm" runat="server">
                <div class="formGroup loginForm">
                    <label for="logIdInput"><%=LoginPageWords[(int)LanguageTable.LoginPageStrId.Login_1] %>：</label>
                    <input type="text" id="logIdInput" name="userID"/>
                </div>
                <div class="formGroup loginForm">
                    <label for="passInput"><%=LoginPageWords[(int)LanguageTable.LoginPageStrId.Login_2] %>：</label>
                    <input type="password" id="passInput" name="password"/>
                </div>
                <%-- takase0507test--%>
                <%-- takase0507--%>
                <%--
                <div id="langSelect" class="formGroup">
                       <select name="langID" onchange="CustomLogin(true)" id="selector">
                    <%if(langList != null){ %>
                    <%for (int i = 0; i < langList.Count; i++) {
                                if(langList[i].id == langID)
                                    {%>
                                        <option value="<%=langList[i].id %>" selected><%=langList[i].name %></option>
                                    <%}
                                else
                                {%>
                                    <option value="<%=langList[i].id%>"><%=langList[i].name %></option>
                                <%}
                               }
                          } %>
                       </select>
                </div>
                --%>
                <div class="formGroup">
                    <%-- takase0508--%>
                    <button type="submit" id="loginButton" <%-- onclick="CustomLogin(false)"--%> class="bold"><%=LoginPageWords[(int)LanguageTable.LoginPageStrId.Login_3] %></button>
                </div>
                <!--<div class="g-signin2" data-onsuccess="onSignIn"></div>-->
                <div class="formGroup">
                    <input id="langChange" name="langChange" value="" style="visibility:hidden" />
                </div>
            </form>
        </div>
        <footer class="grid loginFooter">
                &copy; <%: DateTime.Now.Year %> - NAKANISHI MFG. CO., LTD -
        </footer>
    </div>

</body>
</html>
