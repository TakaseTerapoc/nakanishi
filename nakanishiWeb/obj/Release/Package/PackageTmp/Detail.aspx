<%@ Page Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="Detail.aspx.cs" Inherits="nakanishiWeb.Detail" %>
<%@ Import Namespace="nakanishiWeb.Const" %>

<asp:Content ID="BodyContent" ContentPlaceHolderID="MainContent" runat="server">
    <link rel="stylesheet" href="css/timechart.css" />
    <form id="changePager" target="_self" runat="server">
        <div id="mainPagesContents" class="grid main underNavContentD">
<!--*** ページタイトル ***-->
            <div class="title">
                <h1 id="DetailPage"><%=DetailPageWords[(int)LanguageTable.DetailPageStrId.Detail_0] %></h1>
            </div>
<!--*** ページタイトルここまで ***-->

<!--*** 日付変更ボタン ***-->
            <div class="content1 grid">
                <div>
                    <!--<p id="searchDate" class="bold inline"><%=subTitle %></p>-->
                    <input id="searchDateInput" type="date" class="inline bold subTitle" value="<%=searchDateString%>" onchange="onchangeDateInput(this)" max="<%=DateTime.Today.ToString("yyyy-MM-dd")%>"/>
                    <!--<button id="changeToDate" type="button" class="inline switch" onclick="changeDateOrMonth('Date','')"><%=DetailPageWords[(int)LanguageTable.DetailPageStrId.Detail_15] %></button>-->
                    <!--<button id="changeToMonth" type="button" class="inline switch" onclick="changeDateOrMonth('Month','<%=CommonWords[(int)LanguageTable.CommonPageStrId.Common_43] %>','<%=CommonWords[(int)LanguageTable.CommonPageStrId.Common_44] %>')"><%=DetailPageWords[(int)LanguageTable.DetailPageStrId.Detail_16] %></button>-->
                    <input type="hidden" id="DateOrMonth" value="" name="DayOrMonth"/>
                    <button type="submit" class="hide change" id="DateOrMonthBT"></button>
                    <button id="changeDateButton" type="submit" class="inline smallBT2 change" onclick="changeSearchDate(this,'DateOrMonth')"><%=DetailPageWords[(int)LanguageTable.DetailPageStrId.Detail_1] %></button>
                    <button type="submit" name="before" class="change smallBT2" value=""><%=DetailPageWords[(int)LanguageTable.DetailPageStrId.Detail_25] %></button>
                    <%if (searchDateS < DateTime.Today){ %>
                    <button type="submit" name="next" class="change smallBT2" value=""><%=DetailPageWords[(int)LanguageTable.DetailPageStrId.Detail_26] %></button>
                    <%}else{ %>
                    <button type="button" name="next" class="change smallBT2 inactive" value=""><%=DetailPageWords[(int)LanguageTable.DetailPageStrId.Detail_26] %></button>
                    <%} %>
                </div>
                <!--<div id="selectDateOrMonth">
                    <%if (DayOrMonth == "month"){%>
                    <select class="inline" id="searchYearSelect">
                        <%for (int i = DateTime.Now.Year ; i>2000; i--) { %>
                            <option value="<%=i %>"><%=i %></option>
                        <%}%>
                    </select>
                    <p class="inline">年</p>
                    <select class="inline" id="searchMonthSelect">
                        <%for (int i = 1; i <= 12; i++){ %>
                            <option value="<%=i %>"><%=i %></option>
                        <%} %>
                    </select>
                    <p class="inline">月</p>
                    <%}else{ %>
                    <input id="searchDateInput" type="date" class="inline bold subTitle point">
                    <%}%>
                    <input type="hidden" name="DayOrMonth" value="" id="DayOrMonth"/>
                </div>-->
                <button id="toBack" type="button" class="inline change relative" onclick="location.href='<%=beforePage %>.aspx'"><%=HistoryPageWords[(int)LanguageTable.HistoryPageStrId.History_1] %></button>
            </div>
<!--*** 日付変更ボタンここまで ***-->

<!--*** アラートテーブルとグラフ ***-->
            <div class="content2 grid">

                <!--<canvas id="chartCanvas" width="700px" height="250px"></canvas>-->
                <div id="machineInfAndTimechart" class="grid inactive">
        <!--*** 選択中の機器情報 ***-->
                    <div id="choosedMachineInfo" class="relative inactive">
                        <div id="choosedMachineTitle" class="absolute hoverP" onclick="informationToggle('<%=CommonWords[(int)LanguageTable.CommonPageStrId.Common_57]%>','<%=CommonWords[(int)LanguageTable.CommonPageStrId.Common_58] %>')">
                            <p class="bold inline"><%=CommonWords[(int)LanguageTable.CommonPageStrId.Common_42] %></p>
                            <p id="titleSubMarker" class="absolute"><%=CommonWords[(int)LanguageTable.CommonPageStrId.Common_57] %></p>
                        </div>
                        <div id="choosedMachineContent" class="grid">
                            <div class="Wmin Left">
                                    <div class="relative infoLine">
                                        <p class="inline small"><%=CommonWords[(int)LanguageTable.CommonPageStrId.Common_41] %></p>
                                        <span class="middleTD inline absolute"></span>
                                        <p class="inline bold pushRight"><%=machine.endUserName%></p><!--エンドユーザー-->
                                    </div>
                                    <div class="relative infoLine">
                                        <p class="inline small"><%=CommonWords[(int)LanguageTable.CommonPageStrId.Common_15] %></p>
                                        <span class="middleTD inline absolute"></span>
                                        <p class="inline bold pushRight"><%=machine.typeName%></p><!--品名-->
                                    </div>
                                <div class="toggleContent hide">
                                    <div class="relative infoLine">
                                        <p class="inline small"><%=CommonWords[(int)LanguageTable.CommonPageStrId.Common_14] %></p>
                                        <span class="middleTD inline absolute"></span>
                                        <p class="inline bold pushRight"><%=machine.modelName%></p><!--製品群-->
                                    </div>
                                    <div class="relative infoLine">
                                        <p class="inline small"><%=CommonWords[(int)LanguageTable.CommonPageStrId.Common_20] %></p>
                                        <span class="middleTD inline absolute"></span>
                                        <p class="inline bold pushRight"><%=machine.managementOffice%></p><!--担当支店-->
                                    </div>
                                    <div class="relative infoLine">
                                        <p class="inline small"><%=CommonWords[(int)LanguageTable.CommonPageStrId.Common_10] %></p>
                                        <span class="middleTD inline absolute"></span>
                                        <p class="inline bold pushRight"><%=machine.companyName%></p><!--得意先名-->
                                    </div>
                                </div>
                            </div>
                            <div class="Wmin Right">
                                <div>
                                    <div class="relative infoLine">
                                        <p class="inline small"><%=CommonWords[(int)LanguageTable.CommonPageStrId.Common_17] %></p>
                                        <span class="middleTD inline absolute"></span>
                                        <p class="inline bold pushRight"><%=machine.serialNumber%></p><!--S/N-->
                                    </div>
                                    <div class="relative infoLine">
                                        <p class="inline small"><%=CommonWords[(int)LanguageTable.CommonPageStrId.Common_19] %></p>
                                        <span class="middleTD inline absolute"></span>
                                        <p class="inline bold pushRight"><%=machine.operateHour%></p><!--稼働時間-->
                                    </div>
                                </div>
                                <div class="toggleContent hide">
                                    <div class="relative infoLine">
                                        <p class="inline small"><%=CommonWords[(int)LanguageTable.CommonPageStrId.Common_56] %></p>
                                        <span class="middleTD inline absolute"></span>
                                        <p class="inline bold pushRight"><%=machine.settingDate.ToString("yyyy/MM")%></p><!--設置日-->
                                    </div>
                                    <div class="relative infoLine">
                                        <p class="inline small"><%=CommonWords[(int)LanguageTable.CommonPageStrId.Common_11] %></p>
                                        <span class="middleTD inline absolute"></span>
                                        <p class="inline bold pushRight"><%=machine.endUserCode%></p><!--エンドユーザーCD-->
                                    </div>
                                    <div class="relative infoLine">
                                        <p class="inline small"><%=CommonWords[(int)LanguageTable.CommonPageStrId.Common_9] %></p>
                                        <span class="middleTD inline absolute"></span>
                                        <p class="inline bold pushRight"><%=machine.companyID%></p><!--得意先CD-->
                                    </div>
                                </div>
                            </div>
                    </div>
                </div>
        <!--*** 選択中の機器情報ここまで ***-->
                <div id="timelineBox">
                    <h2 class="TimeChartTitle bold"><%=DetailPageWords[(int)LanguageTable.DetailPageStrId.Detail_31] %></h2>
                    <div id="timeline" class="relative"></div>
                </div>
            </div>
                <div>
                    <h2 class="AlertListTitle bold"><%=DetailPageWords[(int)LanguageTable.DetailPageStrId.Detail_32] %></h2>

                    <section class="H230 relative">
                        <table id="DetailAlertTable" class="fixedLayout">
                            <thead>
                                <tr>
                                    <th id="occurTime" class="fixed sort sort_M alert"><%=DetailPageWords[(int)LanguageTable.DetailPageStrId.Detail_6] %></th>
                                    <th id="releaseTime" class="fixed sort sort_M alert"><%=DetailPageWords[(int)LanguageTable.DetailPageStrId.Detail_17] %></th>
                                    <th id="alertLevel" class="fixed sort sort_M alert"><%=HistoryPageWords[(int)LanguageTable.HistoryPageStrId.History_13] %></th>
                                    <th id="alertName" class="fixed sort sort_M alert"><%=DetailPageWords[(int)LanguageTable.DetailPageStrId.Detail_4] %></th>
                                </tr>
                            </thead>
                            <tbody>
                                    <%for (int i = 0; i < machineAlertList.Count; i++)
                                    {%> 
                                    <%if (i + 1 == alertNo)
                                        {%>
                                        <tr class="choosen hoverD" onclick="graphFlash(`<%=machineAlertList[i].alertID%>`)">
                                    <%}
                                        else
                                        { %>
                                        <tr class="hoverD" onclick="graphFlash(`<%=machineAlertList[i].alertID%>`)">
                                    <%} %>
                                            <td><%=machineAlertList[i].occurTime.ToString("HH:mm")%></td>
                                            <%if (machineAlertList[i].releaseTime.ToString("MM/dd HH:mm") == DateTime.Now.ToString("MM/dd HH:mm"))
                                                { %>
                                            <td> - </td>
                                            <%}else{ %>
                                            <td><%=machineAlertList[i].releaseTime.ToString("HH:mm")%></td>
                                            <%} %>
                                            <td class="level<%=machineAlertList[i].alertLevel%>"><%=machineAlertList[i].alertLevelString%></td>
                                            <td><%=machineAlertList[i].alertName%></td>
                                        </tr>
                                    <%}%>
                            </tbody>
                        </table>
                        <%if(machineAlertList.Count == 0){ %>
                                <p class="bold absolute noData"><%=DetailPageWords[(int)LanguageTable.DetailPageStrId.Detail_24]%></p>
                        <%} %>
                    </section>
                </div>
                <input type="hidden" id="alertNo" name="alertNo" value=""/>
                <button type="submit" id="changeAlertTable" class="change hide"></button>
            </div>
<!--*** アラートテーブルとグラフここまで ***-->

<!--*** パーツ詳細テーブル ***-->
            <div class="content3 grid">
                <div class="grid detailGrid">
                    <h2 class="bold"><%=DetailPageWords[(int)LanguageTable.DetailPageStrId.Detail_21] %>【<%=DateTime.Now.ToString("yyyy年MM月dd日") %><%=DetailPageWords[(int)LanguageTable.DetailPageStrId.Detail_22]%>】</h2>
                    <button id="toAlertMailSetting" type="button" onclick="location.href='AlertMailSetting.aspx'" class="change relative inline"><%=DetailPageWords[(int)LanguageTable.DetailPageStrId.Detail_29] %></button>
                    <button id="toHistory" type="button" onclick="location.href='History.aspx'" class="change relative inline"><%=DetailPageWords[(int)LanguageTable.DetailPageStrId.Detail_2] %></button>
                </div>
                <section class="H260 relative">
                    <table id="DetailExchangeTable" class="fixedLayout">
                        <thead>
                            <tr>
                                <th id="partsName" class="fixed sort sort_P parts"><%=DetailPageWords[(int)LanguageTable.DetailPageStrId.Detail_14] %></th>
                                <th id="remainingOperate" class="fixed sort sort_P parts par_L"><%=DetailPageWords[(int)LanguageTable.DetailPageStrId.Detail_9] %></th>
                                <th id="operatingTime" class="fixed sort sort_P parts par_M"><%=DetailPageWords[(int)LanguageTable.DetailPageStrId.Detail_8] %></th>
                                <th id="lastExchangeDate" class="fixed sort sort_P parts par_M"><%=DetailPageWords[(int)LanguageTable.DetailPageStrId.Detail_10] %></th>
                                <th id="exchangeCount" class="fixed sort sort_P parts par_XS"><%=DetailPageWords[(int)LanguageTable.DetailPageStrId.Detail_12] %></th>
                                <th id="exchangeGuidline" class="fixed sort sort_P parts par_M"><%=DetailPageWords[(int)LanguageTable.DetailPageStrId.Detail_7] %></th>
                                <th class="fixed par_S"></th>
                            </tr>
                        </thead>
                        <tbody>
                             <%for (int i = 0; i < partsList.Count; i++)
                                 {
                                     if (partsList[i].isExchangeAlert) {%>
                                <tr class="attention hoverD">
                                    <%} else {%>
                                <tr class="hoverD">
                                    <%} %>
                                    <td><%=partsList[i].partsName %></td>
                                    <td class="par_L"><%=partsList[i].remainingOperateHourString %><br /><%=partsList[i].remainingOperateCountString%></td>
                                    <td class="par_M"><%=partsList[i].operateHourString%><br /><%=partsList[i].operateCountString%></td>
                                    <td class="par_M"><%=partsList[i].lastExchangeDate.ToString("yyyy/MM/dd") %></td>
                                    <td class="par_XS"><%=partsList[i].exchangeCount %></td>
                                    <td class="par_M"><%=partsList[i].exchangeGuideTimeString%><br /><%=partsList[i].exchangeGuideCountString%></td>
                                    <td class="par_S"><button type="button" class="smallBT change" onclick="exchangeConfirm('<%=partsList[i].partsName%>','<%=partsList[i].partsID %>')"><%=CommonWords[(int)LanguageTable.CommonPageStrId.Common_55] %></button></td>
                                </tr>
                            <%}%>
                        </tbody>
                    </table>
                    <%if(partsList.Count == 0){ %>
                            <p class="bold absolute noData"><%=DetailPageWords[(int)LanguageTable.DetailPageStrId.Detail_23] %></p>
                    <%} %>
            </section>
            <input type="hidden" id="tableKind" name="tableKind" value=""/>
            <input type="hidden" id="sortInput" name="changeSort" value=""/>
            <input type="hidden" id="exchange" name="exchange" value=""/>
            <button type="submit" class="hide" id="sortBT"></button>
            </div>
<!--*** パーツ詳細テーブルここまで ***-->
        </div>
        <input type="hidden" id="pageFlag" name="pageFlag" value="false" />
        <input type="hidden" name="pageChange" id="pageChange" />
        <button type="submit" id="pageChangeBT" class="hide"></button>
    </form>
    <script src="js/jquery-3.6.1.min.js"></script>
    <script src="js/d3.v3.min.js"></script>
    <script src="js/TimeChart.js"></script>
    <script src="js/Functions.js"></script>
    <script type="text/javascript">
        const changeDateBT = document.querySelector("#changeDateButton");
        let temp_searchDate = document.querySelector("#searchDateInput").value;
        function onchangeDateInput(input_typeDate) {
            if (temp_searchDate == input_typeDate) { return; }
            changeSearchDate(changeDateBT,'DateOrMonth');
        }
        const sortCol_M = '<%=sortBase_Alert.columnsKey%>';
        const orderDirection_M = '<%=sortBase_Alert.orderDirection%>';
        const sort_MTHs = document.querySelectorAll("th.sort_M");
        addSortEvent(sort_MTHs, sortCol_M, orderDirection_M,'alert');
        const sortCol_P = '<%=sortBase_Parts.columnsKey%>';
        const orderDirection_P = '<%=sortBase_Parts.orderDirection%>';
        const sort_PTHs = document.querySelectorAll("th.sort_P");
        addSortEvent(sort_PTHs, sortCol_P, orderDirection_P,"parts");
        const graphData = <%=graphData%>;
        let sDate = new Date('<%=sStartDate%>');
        console.log(sDate);
        drawTimeChart("#timeline",graphData,sDate,100,800);
        const table_1 = document.querySelector("#DetailAlertTable");
        const table_2 = document.querySelector("#DetailExchangeTable");
        addMouseOverOut(table_1);
        addMouseOverOut(table_2);
        addClickEvent(table_1, "", false);
        addClickEvent(table_2, "", false);
    </script>
</asp:Content>
