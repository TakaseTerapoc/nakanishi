<%@ Page Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="Main.aspx.cs" Inherits="nakanishiWeb.Main" %>
<%@ Import Namespace="nakanishiWeb.Const" %>

<asp:Content ID="BodyContent" ContentPlaceHolderID="MainContent" runat="server">
<p class="hide">このファイルは4/4 15:25に更新されました</p>
    <form id="form1" target="_self" runat="server">
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
                        <div class="searchSelectBox"><!--機械アラート-->
                            <label for="machineAlert"><%=CommonWords[(int)LanguageTable.CommonPageStrId.Common_6] %></label>
                            <select name="machineAlert" id="machine_Alert">
                                <option id="M_-1" class="M-alert" value=""><%=CommonWords[(int)LanguageTable.CommonPageStrId.Common_29] %></option>
                                <option id="M_1" class="M-alert" value="1"><%=CommonWords[(int)LanguageTable.CommonPageStrId.Common_46]%></option>
                                <option id="M_0" class="M-alert" value="0"><%=CommonWords[(int)LanguageTable.CommonPageStrId.Common_47]%></option>
                            </select>
                        </div>
                        <div class="searchSelectBox"><!--部品交換アラート-->
                            <label for="partsAlert"><%=CommonWords[(int)LanguageTable.CommonPageStrId.Common_7] %></label>
                            <select name="partsAlert" id="parts_Alert">
                                <option id="P_-1" class="P-alert" value=""><%=CommonWords[(int)LanguageTable.CommonPageStrId.Common_29] %></option>
                                <option id="P_1" class="P-alert" value="1"><%=CommonWords[(int)LanguageTable.CommonPageStrId.Common_46]%></option>
                                <option id="P_0" class="P-alert" value="0"><%=CommonWords[(int)LanguageTable.CommonPageStrId.Common_47]%></option>
                            </select>
                        </div>
                        <div class="searchInputDateBox longBorder"><!--設置年月 START-->
                            <label for="settingDate_s"><%=CommonWords[(int)LanguageTable.CommonPageStrId.Common_56] %></label>
                            <input type="date" id="settingDate_s" name="settingDate_start" max="<%=DateTime.Now.ToString("yyyy-MM-dd") %>" onchange="setMinDate(this,'settingDate_e')" value="<%=FieldsDateStringList[FdatestringKeys[0]] %>"/>
                        </div>
                        <div class="searchInputDateBox"><!--設置年月 END-->
                            <label for="settingDate_e" class="short"><!--=CommonWords[(int)LanguageTable.CommonPageStrId.Common_50] %-->～</label>
                            <input type="date" id="settingDate_e" name="settingDate_end"  max="<%=DateTime.Now.ToString("yyyy-MM-dd") %>" value="<%=FieldsDateStringList[FdatestringKeys[1]] %>"/>
                            <p class="inline"><%=CommonWords[(int)LanguageTable.CommonPageStrId.Common_61] %></p>
                        </div>
                    </div>
                    <div class="innerSearchContent grid">
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
                        <div id="branchDiv" class="ambigousBox">
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
                    <h1 id="Main" class=""><%=CommonWords[(int)LanguageTable.CommonPageStrId.Common_48] %></h1>
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
                            <input type="hidden" name="downloadCSVBT" value="False" id="downloadCSV"/>
                            <button type="button" onclick="downloadCSVClick()" class="change wide relative"><%=CommonWords[(int)LanguageTable.CommonPageStrId.Common_63]%></button>
                            <button type="submit" id="downloadCSVBT" class="hide"></button>
                        </div>
                    </div>
<!--*** ページャーここまで ***-->
                    <p class="sortInfo bold inline"><%=searchInfo %></p>
<!--*** テーブル ***-->
                    <section>
                        <table id="machineTable" class="H350 fixedLayout">
                            <thead>
                                <tr>
                                    <th id="endUserName" class="fixed sort par_XL"><%=CommonWords[(int)LanguageTable.CommonPageStrId.Common_12] %></th>
                                    <!--<th id="type" class="fixed sort par_XS"><%=CommonWords[(int)LanguageTable.CommonPageStrId.Common_16] %></th>-->
                                    <th id="modelName" class="fixed sort par_S"><%=CommonWords[(int)LanguageTable.CommonPageStrId.Common_14] %></th>
                                    <th id="typeName" class="fixed sort par_ML"><%=CommonWords[(int)LanguageTable.CommonPageStrId.Common_15] %></th>
                                    <th id="serialNumber" class="fixed sort par_XS"><%=CommonWords[(int)LanguageTable.CommonPageStrId.Common_17] %></th>
                                    <th id="deliveryDate" class="fixed sort par_S"><%=CommonWords[(int)LanguageTable.CommonPageStrId.Common_56] %></th>
                                    <th id="operatingTime" class="fixed par_S sort"><%=CommonWords[(int)LanguageTable.CommonPageStrId.Common_19] %><br />(HH:mm)</th>
                                    <th id="clientName" class="fixed sort par_ML"><%=CommonWords[(int)LanguageTable.CommonPageStrId.Common_10] %></th>
                                    <th id="MGOffice" class="fixed sort par_M"><%=CommonWords[(int)LanguageTable.CommonPageStrId.Common_20] %></th>
                                    <th id="lastTime" class="fixed sort par_M"><%=MainPageWords[(int)LanguageTable.MainPageStrId.Main_1] %></th>
                                    <th id="machineAlert" class="fixed small sort"><%=CommonWords[(int)LanguageTable.CommonPageStrId.Common_59] %></th>
                                    <th id="partsAlert" class="fixed small sort"><%=CommonWords[(int)LanguageTable.CommonPageStrId.Common_60] %></th>
                                </tr>
                            </thead>
                            <tbody>
                                <%for (int i = 0; i < machineList.Count ; i++) { %>
                                        <tr onclick="clickTR(<%=machineList[i].machineID %>)">
                                            <td class="par_XL"><%=machineList[i].endUserName %></td>
                                            <!--<td class="par_XS"><%=machineList[i].typeID %></td>-->
                                            <td class="par_S"><%=machineList[i].modelName %></td>
                                            <td class="par_ML"><%=machineList[i].typeName %></td>
                                            <td class="par_XS"><%=machineList[i].serialNumber %></td>
                                            <%if(machineList[i].settingDate.ToString("yyyy/MM/dd") == "0001/01/01"){ %>
                                            <td class="par_S"> - </td>
                                            <%}else{ %>
                                            <td class="par_S"><%=machineList[i].settingDate.ToString("yyyy/MM/dd") %></td>
                                            <%} %>
                                            <td class="par_S"><%=machineList[i].operateHour %></td>
                                            <td class="par_ML"><%=machineList[i].companyName %></td>
                                            <td class="par_M"><%=machineList[i].managementOffice %></td>
                                            <%if(machineList[i].lastTime.ToString("yyyy/MM/dd") == "0001/01/01"){ %>
                                            <td class="par_M"> - </td>
                                            <%}else{ %>
                                            <td class="par_M"><%=machineList[i].lastTime.ToString("yyyy/MM/dd\nHH:mm:ss") %></td>
                                            <%} %>
                                            <td class="small"><%if (machineList[i].isMachineAlert) {%><img src="images/error.png" class="error"/><%} %></td>
                                            <td class="small"><%if (machineList[i].isPartsAlert) {%><img src="images/error.png" class="error"/><%} %></td>
                                        </tr>
                                <%} %>
                            </tbody>
                        </table>
                    </section>
                    <%if ((machineList == null) || (machineList.Count == 0)) { %><p class="bold absolute noData"><%=CommonWords[(int)LanguageTable.CommonPageStrId.Common_66] %></p><%} %>
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
        const M_alert = <%=FieldsIntList[FintKeys[2]]%>;
        if (M_alert != -1) {
            let SelectedM_alert = document.querySelector(`#M_${M_alert}`);
            SelectedM_alert.setAttribute("selected",true);
        }
        const P_alert = <%=FieldsIntList[FintKeys[3]]%>;
        if (P_alert != -1) {
            let SelectedP_alert = document.querySelector(`#P_${P_alert}`);
            SelectedP_alert.setAttribute("selected",true);
        }

        const sortCol = '<%=sortBase.columnsKey%>';
        const orderDirection = '<%=sortBase.orderDirection%>';
        const sortTHs = document.querySelectorAll("th.sort");
        addSortEvent(sortTHs, sortCol, orderDirection);
        const table = document.querySelector("#machineTable");
        addMouseOverOut(table);
        addClickEvent(table, "", false);

        const clientDataSets = '<%=clientDatasets%>';
        const clientAmbigousInput = document.querySelector("#client");
        clientAmbigousInput.setAttribute("data-options", clientDataSets);
        addClientAutoComp();

        const enduserDataSets = '<%=enduserDatasets%>';
        const enduserAmbigousInput = document.querySelector("#endUser");
        enduserAmbigousInput.setAttribute("data-options", enduserDataSets);
        addEnduserAutoComp();
        //addInputAssistance("address","suggestTest");

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
