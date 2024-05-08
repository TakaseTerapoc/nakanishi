<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="GetAlertList_ofDay.aspx.cs" Inherits="nakanishiWeb.ajaxPage.GetAlertList" %>

<div id="timeline" class="relative"></div>

    <script src="js/d3.v3.min.js"></script>
    <script src="js/TimeChart.js"></script>
    <script type="text/javascript">
        const graphData = <%=graphData%> ;
        const sDate = new Date('<%=sStartDate%>');
        drawTimeChart("#timeline", graphData, sDate, 100, 800);
        console.log("ajax drawTimeChart");
    </script>
