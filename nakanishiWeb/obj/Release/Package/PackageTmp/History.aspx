<%@ Page Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="History.aspx.cs" Inherits="nakanishiWeb.History" %>
<%@ Import Namespace="nakanishiWeb.Const" %>

<asp:Content ID="BodyContent" ContentPlaceHolderID="MainContent" runat="server">
    <form id="changePager" target="_self" runat="server">
        <div id="mainPagesContents" class="grid main underNavContentH">
            <!--<div>-->
<!--*** ページタイトル ***-->
            <div id="titleContent" class="inactive">
                <div class="title">
                    <h1 id="HistoryPage"><%=HistoryPageWords[(int)LanguageTable.HistoryPageStrId.History_0] %></h1>
                </div>
<!--*** 選択中の機器情報 ***-->
                    <div id="choosedMachineInfo_HP" class="relative inactive">
                        <div id="choosedMachineTitle" class="absolute hoverP" onclick="informationToggle_HP('<%=CommonWords[(int)LanguageTable.CommonPageStrId.Common_57]%>','<%=CommonWords[(int)LanguageTable.CommonPageStrId.Common_58] %>')">
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
                                        <p class="inline small"><%=CommonWords[(int)LanguageTable.CommonPageStrId.Common_17] %></p>
                                        <span class="middleTD inline absolute"></span>
                                        <p class="inline bold pushRight"><%=machine.serialNumber%></p><!--S/N-->
                                    </div>
                                <div class="toggleContent hide">
                                    <div class="relative infoLine">
                                        <p class="inline small"><%=CommonWords[(int)LanguageTable.CommonPageStrId.Common_11] %></p>
                                        <span class="middleTD inline absolute"></span>
                                        <p class="inline bold pushRight"><%=machine.endUserCode%></p><!--エンドユーザーCD-->
                                    </div>
                                    <div class="relative infoLine">
                                        <p class="inline small"><%=CommonWords[(int)LanguageTable.CommonPageStrId.Common_14] %></p>
                                        <span class="middleTD inline absolute"></span>
                                        <p class="inline bold pushRight"><%=machine.modelName%></p><!--製品群-->
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
                                        <p class="inline small"><%=CommonWords[(int)LanguageTable.CommonPageStrId.Common_15] %></p>
                                        <span class="middleTD inline absolute"></span>
                                        <p class="inline bold pushRight"><%=machine.typeName%></p><!--品名-->
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
                                        <p class="inline small"><%=CommonWords[(int)LanguageTable.CommonPageStrId.Common_20] %></p>
                                        <span class="middleTD inline absolute"></span>
                                        <p class="inline bold pushRight"><%=machine.managementOffice%></p><!--担当支店-->
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
            </div>
<!--*** ページタイトルここまで ***-->

<!--*** ページャーとテーブル ***-->
                <div id="pager_rightSide" class="grid">
<!--*** ページャー ***-->
                    <button type="button" onclick="location.href='Detail.aspx'" class="relative change" id="toDetail"><%=HistoryPageWords[(int)LanguageTable.HistoryPageStrId.History_1] %></button>
                    <div>
                        <p class="margin-left"><%=CommonWords[(int)LanguageTable.CommonPageStrId.Common_25]%><%=pager.totalObject %><%=CommonWords[(int)LanguageTable.CommonPageStrId.Common_26] %> : <%=pager.GetStartObjectCount() %> ～ <%=pager.GetEndObjectCount() %><%=CommonWords[(int)LanguageTable.CommonPageStrId.Common_27] %></p>
                        <div id="pagerBox" class="grid inline">
                            <input type="hidden" id="clickedPager" name="clickedPager" value="" />
                            <input type="submit" class="hide" id="changePageButton" />

                         <%if (pager.nowPageNo > 1){ %>
                            <div class="inline attend hoverP" onclick="changePage(<%=1 %>)">FIRST</div>
                         <%}else{ %>
                            <div class="inline attend inactive" >FIRST</div>
                         <%} %>
                         <%if (pager.nowPageNo > 1){ %>
                            <div class="inline attend hoverP" onclick="changePage(<%=Math.Max(pager.nowPageNo - 1, 1) %>)">PREV</div>
                         <%}else{ %>
                            <div class="inline attend inactive" >PREV</div>
                         <%} %>
                            
                            <div class="inline">
                                <%for (int i = Math.Max(1,Math.Min(pager.totalPageCount-(pagerMax-1),pager.nowPageNo-pagerHalf)) ; i <= Math.Min(pager.totalPageCount,Math.Max(1,pager.nowPageNo-pagerHalf)+pagerMax-1) ; i++) {
                                        if(i == pager.nowPageNo)
                                        {%>
                                            <p class="inline bold point inactive" name="<%=i %>" ><%=i %></p>
                                        <%}
                                        else
                                        {%>
                                            <p class="inline" name="<%=i %>" onclick="changePage(<%=i %>)"><%=i %></p>
                                        <%}
                                    } %>
                            </div>

                         <%if (pager.nowPageNo < pager.totalPageCount){ %>
                            <div class="inline attend hoverP" onclick="changePage(<%=Math.Min(pager.nowPageNo+1,pager.totalPageCount)%>)">NEXT</div>
                         <%}else{ %>
                            <div class="inline attend inactive" >NEXT</div>
                         <%} %>
                         <%if (pager.nowPageNo < pager.totalPageCount){ %>
                            <div class="inline attend hoverP" onclick="changePage(<%=pager.totalPageCount%>)">LAST</div>
                         <%}else{ %>
                            <div class="inline attend inactive" >LAST</div>
                         <%} %>
                        </div>
                    </div>
<!--*** ページャーここまで ***-->
                </div>
           <!-- </div>-->
            <div class="margin-top relative">
<!--*** ソート ***-->
                <div id="order" class="orderBox">
                   <!-- <div class="searchInputDateBox inline BGreversal">
                        <p class="inline"><%=HistoryPageWords[(int)LanguageTable.HistoryPageStrId.History_2] %></p>
                        <input type="date" name="start" />
                        <%=HistoryPageWords[(int)LanguageTable.HistoryPageStrId.History_3] %><input type="date" name="end" />
                    </div>-->
                </div>
<!--*** ソートここまで ***-->

<!--*** テーブル ***-->
                <section class="H460">
                    <table id="historyTable" class="fixedLayout">
                        <thead>
                            <tr>
                                <th id="occurTime" class="fixed sort par_M"><%=HistoryPageWords[(int)LanguageTable.HistoryPageStrId.History_9] %></th>
                                <th id="releaseTime" class="fixed sort par_M"><%=HistoryPageWords[(int)LanguageTable.HistoryPageStrId.History_21] %></th>
                                <th id="alertLevel" class="fixed sort par_M"><%=HistoryPageWords[(int)LanguageTable.HistoryPageStrId.History_13] %></th>
                                <th id="alertName" class="fixed sort par_XL"><%=HistoryPageWords[(int)LanguageTable.HistoryPageStrId.History_7] %></th>
                                <th id="partsName" class="fixed sort"><%=HistoryPageWords[(int)LanguageTable.HistoryPageStrId.History_15] %></th>
                                <th class="fixed"><%=HistoryPageWords[(int)LanguageTable.HistoryPageStrId.History_8] %></th>
                            </tr>
                        </thead>
                        <tbody>
                            <%for (int i = 0; i < alertList.Count; i++) { %>
                            <tr class="hoverD">
                                <td class="par_M"><%=alertList[i].occurTime.ToString("yyyy/MM/dd HH:mm:ss") %></td>
                                <% if (alertList[i].isNowAlert) { %>
                                    <td class="par_M">-</td>
                                <% } else  { %>
                                    <td class="par_M"><%=alertList[i].releaseTime.ToString("yyyy/MM/dd HH:mm:ss") %></td>
                                <% } %>
                                <td class="par_M"><span class="level<%=alertList[i].alertLevel%>"><%=alertList[i].alertLevelString %></span></td>
                                <td class="par_XL"><%=alertList[i].alertName %></td>
                                <td><%=alertList[i].partsName %></td>
                                <td><%=alertList[i].causeString %></td>
                            </tr>
                            <%} %>
                        </tbody>
                    </table>
                </section>
                <%if(alertList.Count == 0){ %>
                        <p class="bold absolute noData"><%=CommonWords[(int)LanguageTable.CommonPageStrId.Common_54] %></p>
                <%} %>
                <input type="hidden" id="sortInput" name="changeSort" value=""/>
                <button type="submit" class="hide" id="sortBT"></button>
<!--*** テーブルここまで ***-->
            </div>
<!--*** ページャーとテーブルここまで ***-->
        </div>
        <input type="hidden" name="pageChange" id="pageChange" />
        <button type="submit" id="pageChangeBT" class="hide"></button>
    </form>
    <script src="js/jquery-3.6.1.min.js"></script>
    <script src="js/Functions.js"></script>
    <script type="text/javascript">
        const sortCol = '<%=sortBase.columnsKey%>';
        const orderDirection = '<%=sortBase.orderDirection%>';
        const sortTHs = document.querySelectorAll("th.sort");
        addSortEvent(sortTHs, sortCol, orderDirection);
        const table = document.querySelector("#historyTable");
        addMouseOverOut(table);
        addClickEvent(table, "", false);
    </script>
</asp:Content>
