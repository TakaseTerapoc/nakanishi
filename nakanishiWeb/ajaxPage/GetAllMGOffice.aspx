<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="GetAllMGOffice.aspx.cs" Inherits="nakanishiWeb.ajaxPage.GetAllMGOffice" %>

    <label for="MGOfficeName">担当支店営業所</label>
    <select id="MGOfficeName" name="MGOffice" onchange="changeEndUserName(this,'endUNDiv'),changeClientName(this,'clientDiv')">
        <option value="">全て</option>
        <%if (adminBranchList != null)
            { %>
        <%for (int i = 0; i < adminBranchList.Count; i++)
            {%>
            <option value="<%=adminBranchList[i].branchID%><%=split%><%=adminBranchList[i].branchName%>"><%=adminBranchList[i].branchName%></option>       
        <%}
            }%>
    </select>
