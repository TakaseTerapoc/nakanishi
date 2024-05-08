<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="GetMachineModelList.aspx.cs" Inherits="nakanishiWeb.ajaxPage.GetMachineModelList" %>

<label for="modelID">型式</label>
<select id="modelID" name="modelID" onchange="changeMachineType('typeSelectDiv',this)">
<%if (modelList != null) {
        if (modelList.Count == 0)
        {%>
           <option value="">登録された製品群がありません
           </option>    
    <%}
        else {%>
            <option value="">全て</option>
            <%foreach (var model in modelList)
                {%>
                <option value="<%=model.modelID%><%=split%><%=model.modelName%>"><%=model.modelName%></option>       
            <%}
        } 
    }%>
</select>
