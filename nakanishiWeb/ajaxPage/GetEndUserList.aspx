<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="GetEndUserList.aspx.cs" Inherits="nakanishiWeb.ajaxPage.GetEndUserList" %>

<label for="endUserID">エンドユーザー名</label>
<select id="endUserID" name="endUserName"<%if (parentPageName != "client"){%> onchange="changeModel_ofCompany(this,'modelID','enduser'),changeMachineType('typeSelectDiv',this)"<%} %> >
    <%if (endUserList != null) {
            if(endUserList.Count == 0){%>
                    <option value="">エンドユーザーが登録されていません</option>
       <%}else{%>
            <option value="">全て</option>
            <%for (int i = 0; i < endUserList.Count; i++){%>
                <option value="<%=endUserList[i].companyID%><%=split%><%=endUserList[i].companyName%>"><%=endUserList[i].companyName%></option>
            <%} %>
       <%}
        } %>
</select>
