<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Http500Form.aspx.cs" Inherits="nakanishiWeb.Http500Form" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <link rel="stylesheet" href="css/layout.css" />
    <link rel="stylesheet" href="css/main.css" />
<meta http-equiv="Content-Type" content="text/html; charset=utf-8"/>
    <title></title>
    <script type="text/javascript">
        function EndNakanishiWeb() {
            let timer = setTimeout(function () {
                location.href = "./login.aspx";
            },1000)
        }
    </script>
</head>
<body class="timeout" onload="EndNakanishiWeb()">
    <div class="grid center">
        <h1>SYSTEM ERROR HTTP500</h1>
        <%if (errorMessage != null) {%>
        <p><%=errorMessage %></p>
        <%} %>
    </div>
</body>
</html>
