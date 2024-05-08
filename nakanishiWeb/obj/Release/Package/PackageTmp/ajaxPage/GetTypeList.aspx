<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="GetTypeList.aspx.cs" Inherits="nakanishiWeb.ajaxPage.GetTypeList" %>

<label for="typeID">品名</label>
<select id="typeID" name="typeID">
<%if (typeList != null) {
        if (typeList.Count == 0)
        { %>
            <option value="">品名が登録されていません</option>
   <%}
       else
       {%>
            <option value="">全て</option>
       <%for (int i = 0; i < typeList.Count; i++)
           {%>
            <option value="<%=typeList[i].typeID %><%=split%><%=typeList[i].typeName%>"><%=typeList[i].typeName%></option>
       <%}
         }
    }%>
</select>
