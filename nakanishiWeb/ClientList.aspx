<%@ Page Language="C#" AutoEventWireup="true" MasterPageFile="~/Site.Master" CodeBehind="ClientList.aspx.cs" Inherits="nakanishiWeb.ClientList" %>
<%@ Import Namespace="nakanishiWeb.Const" %>

<asp:Content ID="BodyContent" ContentPlaceHolderID="MainContent" runat="server">
    <form id="changePager" target="_self" runat="server">
        <div id="mainPagesContents" class="grid main underNavContent close">
<!--*** 検索ボックス ***-->
            <div class="machineSearch searchBox grid">
                <div class="titleBox hoverP">
                    <h2 class="searchTitle white subTitle"><%=CommonWords[(int)LanguageTable.CommonPageStrId.Common_8] %></h2>
                    <div class="menuButton">
                        <span></span><span>MENU<br>OPEN</span><span></span>
                    </div>
                </div>
                <div id="machineList" class="searchContent oneRow grid">
                    <div class="innerSearchContent grid">
                        <div id="endUNDiv" class="ambigousBox"><!--エンドユーザー-->
                            <div class="searchInputBox">
                                <label for="endUser"><%=CommonWords[(int)LanguageTable.CommonPageStrId.Common_12]%></label>
                                    <input type="text" id="endUser" name="endUserName_ambi" placeholder="<%=CommonWords[(int)LanguageTable.CommonPageStrId.Common_12] %>" value="<%=FieldsStringList[FstringKeys[1]] %>" />
                            </div>
                        </div>
                        <div id="branchDiv" class="ambigousBox ">
                            <div id="MGOfficeNameDiv" class="searchInputBox"><!--担当支店営業所-->
                                <label for="mgoffice"><%=CommonWords[(int)LanguageTable.CommonPageStrId.Common_20] %></label>
                                        <input type="text" id="mgoffice" name="MGOffice_Ambigous" placeholder="<%=CommonWords[(int)LanguageTable.CommonPageStrId.Common_20] %>" value="<%=FieldsStringList[FstringKeys[4]] %>" />
                            </div>
                        </div>
                        <div id="clientDiv" class="ambigousBox"><!--得意先名-->
                            <div class="searchInputBox">
                                <label for="client"><%=CommonWords[(int)LanguageTable.CommonPageStrId.Common_10]%></label>
                                    <input type="text" id="client" name="clientName_ambi" placeholder="<%=CommonWords[(int)LanguageTable.CommonPageStrId.Common_10] %>" value="<%=FieldsStringList[FstringKeys[0]] %>" />
                                <!--<span class="ambiguous close" onclick="showList(this)"></span>-->
                            </div>
                        </div>
                        <div class="searchEmptyBox"></div>
                        <div class="searchEmptyBox"></div>
                        <div class="searchEmptyBox"></div>
                    </div>
                    <div>
                        <input type="hidden" name="searchBT" value="" id="searchHideInput"/>
                        <button type="button" onclick="searchBTClick()" class="change relative"><%=CommonWords[(int)LanguageTable.CommonPageStrId.Common_38] %></button>
                        <button type="submit" id="searchBT" class="hide"></button>
                    </div>
                </div>
            </div>
<!--*** 検索ボックスここまで ***-->
        <div class="relative">

<!--*** ページタイトル ***-->
            <div class="title">
                <h1 id="ClientListPage-1"><%=ClientListPageWords[(int)LanguageTable.ClientListPageStrId.ClientList_0]%></h1>
            </div>
<!--*** ページタイトルここまで ***-->

<!--*** ページャーとテーブル ***-->
            <div id="pager">
<!--*** ページャー ***-->
                    <div>
                        <p class="margin-left inline"><%=CommonWords[(int)LanguageTable.CommonPageStrId.Common_25]%><%=pager.totalObject %><%=CommonWords[(int)LanguageTable.CommonPageStrId.Common_26] %> : <%=pager.GetStartObjectCount() %> ～ <%=pager.GetEndObjectCount() %><%=CommonWords[(int)LanguageTable.CommonPageStrId.Common_27] %></p>
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

                        <div class="putRight position-left inline">
                            <input type="hidden" name="downloadCSVBT" value="" id="downloadCSV"/>
                            <button type="button" onclick="downloadCSVClick()" class="change wide relative"><%=CommonWords[(int)LanguageTable.CommonPageStrId.Common_63]%></button>
                            <button type="submit" id="downloadCSVBT" class="hide"></button>
                        </div>
                    </div>
<!--*** ページャーここまで ***-->
                <p class="sortInfo bold inline"><%=searchInfo %></p>
<!--*** テーブル ***-->
                <section>
                    <table id="clientListTable" class="H350 fixedLayout" >
                        <thead>
                            <tr>
                                <th id="endUserName" class="fixed sort"><%=CommonWords[(int)LanguageTable.CommonPageStrId.Common_12] %></th>
                                <th id="clientName" class="fixed sort"><%=CommonWords[(int)LanguageTable.CommonPageStrId.Common_10] %></th>
                                <th id="MGOffice" class="fixed sort par_XL"><%=CommonWords[(int)LanguageTable.CommonPageStrId.Common_20] %></th>
                            </tr>
                        </thead>
                        <tbody>
                            <%for (int i = 0; i < searchEndUserList.Count ; i++) { %>
                                  <tr onclick="clickTR('<%=searchEndUserList[i].companyID %><%=split %><%=searchEndUserList[i].companyName %>')">
                                    <td><%=searchEndUserList[i].companyName%></td>
                                    <td><%=searchEndUserList[i].connectionCompanyName%></td>
                                    <td class="par_XL"><%=searchEndUserList[i].MGOfficeName%></td>
                                </tr>    
                            <%} %>
                        </tbody>
                    </table>
                </section>
                <%if (searchEndUserList.Count == 0) { %><p class="bold absolute noData"><%=CommonWords[(int)LanguageTable.CommonPageStrId.Common_66] %></p><%} %>
                <input type="hidden" id="click_tr" name="companyID" value=""/>
                <button type="submit" class="hide" id="nextPageBT"></button>
                    <input type="hidden" id="sortInput" name="changeSort" value=""/>
                    <button type="submit" class="hide" id="sortBT"></button>
<!--*** テーブルここまで ***-->
            </div>
<!--*** ページャーとテ―ブルここまで ***-->
        </div>
        <input type="hidden" name="pageChange" id="pageChange" />
        <button type="submit" id="pageChangeBT" class="hide"></button>
        </div>
    </form>
    <script src="js/jquery-3.6.1.min.js"></script>
    <script src="js/jquery-ui.js"></script>
    <script src="js/Functions.js"></script>
    <script type="text/javascript">
        const sortCol = '<%=sortBase.columnsKey%>';
        const orderDirection = '<%=sortBase.orderDirection%>';
        const sortTHs = document.querySelectorAll("th.sort");
        addSortEvent(sortTHs, sortCol, orderDirection);
        addMouseOverOut(document.querySelector("#clientListTable"));
        let table = document.querySelector("#clientListTable");
        addClickEvent(table, "", false);

        const clientDataSets = '<%=clientDatasets%>';
        const clientAmbigousInput = document.querySelector("#client");
        clientAmbigousInput.setAttribute("data-options", clientDataSets);
        addClientAutoComp();

        const enduserDataSets = '<%=enduserDatasets%>';
        const enduserAmbigousInput = document.querySelector("#endUser");
        enduserAmbigousInput.setAttribute("data-options", enduserDataSets);
        addEnduserAutoComp();

        const mgofficeDataSets = '<%=mgofficeDatasets%>';
        const mgofficeImput = document.querySelector("#mgoffice");
        mgofficeImput.setAttribute("data-options", mgofficeDataSets);
        addMGOfficeAutoComp()

        $(function () {
            $("input").on("keydown", function (e) {
                if ((e.which && e.which === 13) || (e.keyCode && e.keyCode === 13)) {
                    return false;
                } else {
                    return true;
                }
            });
        });
    </script>
</asp:Content>
