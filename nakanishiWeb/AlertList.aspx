<%@ Page Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="AlertList.aspx.cs" Inherits="nakanishiWeb.AlertList" %>
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
                <div id="machineList" class="searchContent grid">
                    <div class="innerSearchContent grid">
                        <div class="searchSelectBox"><!--製品群-->
                            <label for="modelID"><%=CommonWords[(int)LanguageTable.CommonPageStrId.Common_14] %></label>
                            <select id="modelID" name="modelID" onchange="changeMachineType('typeSelectDiv',this)">
                                <option value=""><%=CommonWords[(int)LanguageTable.CommonPageStrId.Common_29] %></option>
                                <%if (modelList != null)
                                    { %>
                                <%for (int i = 0; i < modelList.Count; i++)
                                    {
                                        if ((modelList[i].modelID != -1) && (modelList[i].modelID == FieldsIntList[FintKeys[1]]))
                                        {%>
                                    <option value="<%=modelList[i].modelID%><%=split%><%=modelList[i].modelName %>" selected><%=modelList[i].modelName%></option>       
                                <%}else{%>
                                    <option value="<%=modelList[i].modelID%><%=split%><%=modelList[i].modelName %>"><%=modelList[i].modelName%></option>       
                                    <%}
                                    }
                                    }%>
                            </select>
                        </div>
                        <div id="typeSelectDiv" class="searchSelectBox"><!--品名 type_name-->
                            <label for="typeID"><%=CommonWords[(int)LanguageTable.CommonPageStrId.Common_15] %></label>
                            <select id="typeID" name="typeID">
                                <option value=""><%=CommonWords[(int)LanguageTable.CommonPageStrId.Common_29] %></option>
                                <%if (typeList != null)
                                    { %>
                                    <%for (int i = 0; i < typeList.Count; i++)
                                        {
                                          if (typeList[i].typeID == FieldsStringList[FstringKeys[2]]) {%>
                                        <option value="<%=typeList[i].typeID %><%=split%><%=typeList[i].typeName%>" selected><%=typeList[i].typeName%></option>       
                                    <%}else{%> 
                                        <option value="<%=typeList[i].typeID %><%=split%><%=typeList[i].typeName%>" ><%=typeList[i].typeName%></option>       
                                    <%}
                                        }
                                    }%>
                            </select>
                        </div>
                        <div class="searchInputDateBox longBorder"><!--設置年月 START-->
                            <label for="settingDate_s"><%=CommonWords[(int)LanguageTable.CommonPageStrId.Common_56] %></label>
                            <input type="date" id="settingDate_s" name="settingDate_start" max="<%=DateTime.Now.ToString("yyyy-MM-dd") %>" onchange="setMinDate(this,'settingDate_e')" value="<%=FieldsDateStringList[FdatestringKeys[0]] %>"/>
                        </div>
                        <div class="searchInputDateBox"><!--設置年月 END-->
                            <label for="settingDate_e" class="short">～<!--=CommonWords[(int)LanguageTable.CommonPageStrId.Common_56] --></label>
                            <input type="date" id="settingDate_e" name="settingDate_end"  max="<%=DateTime.Now.ToString("yyyy-MM-dd") %>" value="<%=FieldsDateStringList[FdatestringKeys[1]] %>"/>
                            <p class="inline"><%=CommonWords[(int)LanguageTable.CommonPageStrId.Common_61] %></p>
                        </div>
                        <div class="searchInputDateBox longBorder"><!--アラート発生日時 START-->
                            <label for="alert_start"><%=CommonWords[(int)LanguageTable.CommonPageStrId.Common_62] %></label>
                            <input type="date" id="alert_start" name="alert_start"  max="<%=DateTime.Now.ToString("yyyy-MM-dd") %>" onchange="setMinDate(this,'alert_end')" value="<%=FieldsDateStringList[FdatestringKeys[2]] %>"/>
                        </div>
                        <div class="searchInputDateBox"><!--アラート発生日時 END-->
                            <label for="alert_end" class="short">～<!--=wordList[$"{pageName}_6] %--></label>
                            <input type="date" id="alert_end" name="alert_end"  max="<%=DateTime.Now.ToString("yyyy-MM-dd") %>" value="<%=FieldsDateStringList[FdatestringKeys[3]] %>"/>
                            <p class="inline"><%=CommonWords[(int)LanguageTable.CommonPageStrId.Common_61] %></p>
                        </div>
                    </div>
                    <div class="innerSearchContent grid">
                        <div id="alertLvDiv" class="searchSelectBox"><!--アラートLV-->
                            <label for="alertLv"><%=AlertListPageWords[(int)LanguageTable.AlertListPageStrId.AlertList_7] %></label>
                            <select id="alertLv" name="alertLevel">
                                <option value=""><%=CommonWords[(int)LanguageTable.CommonPageStrId.Common_29] %></option>
                                <%if (alertLevelList != null)
                                    { %>
                                <%for (int i = 0; i < alertLevelList.Count; i++)
                                    {%>
                                    <option id="alertLv_<%=alertLevelList[i].alertLevel%>" value="<%=alertLevelList[i].alertLevel%><%=split%><%=alertLevelList[i].alertLevelString%>"><%=alertLevelList[i].alertLevelString%></option>       
                                <%}
                                    }%>
                            </select>
                        </div>
                        <div id="alertNoDiv" class="searchSelectBox"><!--アラートタイプ-->
                            <label for="alertNo">AlertListPageWords[(int)LanguageTable.AlertListPageStrId.AlertList_4] %></label>
                            <select id="alertNo" name="alertNo">
                                <option value=""><%=CommonWords[(int)LanguageTable.CommonPageStrId.Common_29] %></option>
                                <%if (alertTypeList != null)
                                    { %>
                                <%for (int i = 0; i < alertTypeList.Count; i++)
                                    {%>
                                    <option id="alert_<%=alertTypeList[i].alertID%>" value="<%=alertTypeList[i].alertID%><%=split%><%=alertTypeList[i].alertName%>"><%=alertTypeList[i].alertName%></option>       
                                <%}
                                    }%>
                            </select>
                        </div>
                        <div class="searchInputBox"><!--S/N-->
                            <label for="serialNo"><%=CommonWords[(int)LanguageTable.CommonPageStrId.Common_17] %></label>
                            <input type="text" id="serialNo" name="serialNumber" placeholder="<%=CommonWords[(int)LanguageTable.CommonPageStrId.Common_32] %>" value="<%=FieldsStringList[FstringKeys[3]] %>" />
                        </div>
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
                    <h1 id="AlertListPage">
                    <%if (titleNumber == 0){%>
                        <%=AlertListPageWords[(int)LanguageTable.AlertListPageStrId.AlertList_1] %><%=AlertListPageWords[(int)LanguageTable.AlertListPageStrId.AlertList_0]%>
                    <%}else{ %>
                        <%=AlertListPageWords[(int)LanguageTable.AlertListPageStrId.AlertList_2] %><%=AlertListPageWords[(int)LanguageTable.AlertListPageStrId.AlertList_0]%>
                    <%} %>
                    </h1>
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
                    <section class="overX">
                        <table id="alertTable" class="H350 fixedLayout">
                            <thead>
                                <tr>
                                    <th id="occurTime" class="fixed sort size_S"><%=AlertListPageWords[(int)LanguageTable.AlertListPageStrId.AlertList_8] %></th>
                                    <th id="releaseTime" class="fixed sort size_S"><%=CommonWords[(int)LanguageTable.CommonPageStrId.Common_53] %></th>
                                    <th id="alertLevel" class="fixed sort size_XS"><%=AlertListPageWords[(int)LanguageTable.AlertListPageStrId.AlertList_7] %></th>
                                    <th id="alertName" class="fixed sort size_L"><%=AlertListPageWords[(int)LanguageTable.AlertListPageStrId.AlertList_4] %></th>
                                    <th id="endUserName" class="fixed sort size_ML"><%=CommonWords[(int)LanguageTable.CommonPageStrId.Common_12] %></th>
                                    <th id="modelName" class="fixed sort size_M"><%=CommonWords[(int)LanguageTable.CommonPageStrId.Common_14] %></th>
                                    <th id="typeName" class="fixed sort size_ML"><%=CommonWords[(int)LanguageTable.CommonPageStrId.Common_15] %></th>
                                    <th id="serialNumber" class="fixed sort size_S"><%=CommonWords[(int)LanguageTable.CommonPageStrId.Common_17] %></th>
                                    <th id="deliveryDate" class="fixed sort size_S"><%=CommonWords[(int)LanguageTable.CommonPageStrId.Common_56] %></th>
                                    <th id="MGOffice" class="fixed sort size_M"><%=CommonWords[(int)LanguageTable.CommonPageStrId.Common_20] %></th>
                                    <th id="clientName" class="fixed sort size_M"><%=CommonWords[(int)LanguageTable.CommonPageStrId.Common_10] %></th>
                                </tr>
                            </thead>
                            <tbody>
                                <%for (int i = 0; i < alertList.Count; i++) { %>
                                        <tr onclick="clickTR('<%=alertList[i].machineID%><%=split%><%=alertList[i].occurTime%>')">
                                            <td class="size_S"><%=alertList[i].occurTime.ToString("yyyy/MM/dd HH:mm:ss") %></td>
                                            <% if (alertList[i].isNowAlert) { %>
                                                <td class="size_S">-</td>
                                            <% } else  { %>
                                                <td class="size_S"><%=alertList[i].releaseTime.ToString("yyyy/MM/dd HH:mm:ss") %></td>
                                            <% } %>
                                            <td class="size_XS level<%=alertList[i].alertLevel%>"><%=alertList[i].alertLevelString%></td>
                                            <td class="size_L"><%=alertList[i].alertName%></td>
                                            <td class="size_ML"><%=alertList[i].endUserName%></td>
                                            <td class="size_M"><%=alertList[i].modelName%></td>
                                            <td class="size_ML"><%=alertList[i].typeName%></td>
                                            <td class="size_S"><%=alertList[i].machineSerialNumber%></td>
                                            <td class="size_S"><%=alertList[i].settingDate.ToString("yyyy/MM/dd")%></td>
                                            <td class="size_M"><%=alertList[i].MGOfficeName%></td>
                                            <td class="size_M"><%=alertList[i].companyName%></td>
                                        </tr>
                                <%} %>
                            </tbody>
                        </table>
                    </section>
                    <%if ((alertList == null) || (alertList.Count == 0)) { %><p class="bold absolute noData"><%=CommonWords[(int)LanguageTable.CommonPageStrId.Common_54] %></p><%} %>
                    <input type="hidden" id="click_tr" name="chooseMachineID" value=""/>
                    <button type="submit" class="hide" id="nextPageBT"></button>
                    <input type="hidden" id="sortInput" name="changeSort" value=""/>
                    <button type="submit" class="hide" id="sortBT"></button>
<!--*** テーブルここまで ***-->
                </div>
<!--*** ページャーとテ―ブルここまで ***-->
            </div>
        </div>
        <input type="hidden" name="pageChange" id="pageChange" />
        <button type="submit" id="pageChangeBT" class="hide"></button>
    </form>
    <script src="js/jquery-3.6.1.min.js"></script>
    <script src="js/jquery-ui.js"></script>
    <script src="js/Functions.js"></script>
    <script type="text/javascript">
        const sortCol = '<%=sortBase.columnsKey%>';
        const orderDirection = '<%=sortBase.orderDirection%>';
        const sortTHs = document.querySelectorAll("th.sort");
        addSortEvent(sortTHs, sortCol, orderDirection);
        const table = document.querySelector("#alertTable");
        addMouseOverOut(table);
        addClickEvent(table, "", false);

        const alertNo = <%=FieldsIntList[FintKeys[4]]%>;
        if (alertNo != -1) {
            let SelectedAlert = document.querySelector(`#alert_${alertNo}`);
            SelectedAlert.setAttribute("selected",true);
        }
        const alertLv = <%=FieldsIntList[FintKeys[5]]%>;
        if (alertLv != -1) {
            let SelectedAlertLv = document.querySelector(`#alertLv_${alertLv}`);
            SelectedAlertLv.setAttribute("selected",true);
        }

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
