<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="GetTypeIokList.aspx.cs" Inherits="nakanishiWeb.ajaxPage.GetTypeIokList" %>

<label for="machineID">型式</label>
<select id="machineID" name="machineID">
<%if (typeIokList != null) {
        if (typeIokList.Count == 0)
        {%>
           <option value="">型式が登録されていません
           </option>    
    <%}
        else {%>
            <option value="">全て</option>
            <%foreach (int key in typeIokList.Keys)
                {%>
                <option value="<%=key%><%=split%><%=typeIokList[key]%>"><%=typeIokList[key]%></option>       
            <%}
        } 
    }%>
</select>
