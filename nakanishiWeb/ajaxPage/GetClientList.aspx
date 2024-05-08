<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="GetClientList.aspx.cs" Inherits="nakanishiWeb.ajaxPage.GetClientList" %>

<label for="client">得意先名</label>
<select id="client" name="clientName" onchange="changeEndUserName(this,'endUNDiv'),setDefaultMGOffice(this)">
    <%if (clientList != null) {
            if(clientList.Count == 0){%>
                    <option value="">得意先が登録されていません</option>
       <%}else{%>
            <option value="">全て</option>
            <%for (int i = 0; i < clientList.Count; i++){%>
                <option value="<%=clientList[i].companyID%><%=split%><%=clientList[i].companyName%>"><%=clientList[i].companyName%></option>
            <%} %>
       <%}
        } %>
</select>
