<%@ Page Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="ClientProductList.aspx.cs" Inherits="nakanishiWeb.ClientProductList" %>

<asp:Content ID="BodyContent" ContentPlaceHolderID="MainContent" runat="server">
    <form id="changePager" target="_self" runat="server">
        <div id="mainPagesContents" class="grid main underNavContent close">
<!--*** 検索ボックス ***-->
            <div class="machineSearch searchBox grid">
                <div class="titleBox hoverP">
                    <h2 class="searchTitle white subTitle"><%=commonWordList[$"{common}_8"] %></h2>
                    <div class="menuButton">
                        <span></span><span>MENU<br>OPEN</span><span></span>
                    </div>
                </div>
                <div id="machineList" class="searchContent grid">
                    <div class="searchInputBox"><!--エンドユーザーコード-->
                        <label for="endUserCD"><%=commonWordList[$"{common}_11"] %></label>
                        <input type="number" id="endUserCD" name="endUSerCD" placeholder="<%=commonWordList[$"{common}_31"]%>" />
                    </div>
                    <div class="searchSelectBox"><!--エンドユーザー名-->
                        <label for="endUserName"><%=commonWordList[$"{common}_12"] %></label>
                        <select id="endUserName" name="endUserName">
                            <option value=""><%=commonWordList[$"{common}_29"] %></option>
                            <%if (clientBranchList != null)
                                { %>
                            <%for (int i = 0; i < clientBranchList.Count; i++)
                                {%>
                                <option value="<%=clientBranchList[i].branchID%><%=split%><%=clientBranchList[i].branchName%>"><%=clientBranchList[i].branchName%></option>       
                            <%}
                                }%>
                        </select>
                    </div>
                    <div class="searchInputDateBox"><!--設置年月-->
                        <label for="settingDate"><%=commonWordList[$"{common}_13"] %></label>
                        <input type="date" id="settingDate" name="settingDate" />
                    </div>
                    <div class="searchSelectBox"><!--製品群-->
                        <label for="modelID"><%=commonWordList[$"{common}_14"] %></label>
                        <select id="modelID" name="modelID" onchange="changeMachineType(this,'typeSelectDiv'),changeTypeIok(this,'iokSelectDiv')">
                            <option value=""><%=commonWordList[$"{common}_29"] %></option>
                            <%if (modelList != null)
                                { %>
                            <%for (int i = 0; i < modelList.Count; i++)
                                {%>
                                <option value="<%=modelList[i].modelID%><%=split%><%=modelList[i].modelName%>"><%=modelList[i].modelName%></option>       
                            <%}
                                }%>
                        </select>
                    </div>
                    <div id="typeSelectDiv" class="searchSelectBox"><!--品名-->
                        <label for="typeID"><%=commonWordList[$"{common}_15"] %></label>
                        <select id="typeID" name="typeID">
                            <option value=""><%=commonWordList[$"{common}_29"] %></option>
                            <%if (typeList != null)
                                { %>
                            <%for (int i = 0; i < typeList.Count; i++)
                                {%>
                                <option value="<%=typeList[i].typeID %><%=split%><%=typeList[i].typeName%>"><%=typeList[i].typeName%></option>       
                            <%}
                                }%>
                        </select>
                    </div>
                    <div id="iokSelectDiv" class="searchSelectBox"><!--型式-->
                        <label for="machineID"><%=commonWordList[$"{common}_16"] %></label>
                        <select id="machineID" name="machineID">
                            <option value=""><%=commonWordList[$"{common}_29"] %></option>
                            <%if (typeIokList != null)
                                { %>
                            <%foreach(int key in typeIokList.Keys)
                                {%>
                                <option value="<%=key%><%=split%><%=typeIokList[key]%>"><%=typeIokList[key]%></option>       
                            <%}
                                }%>
                        </select>
                    </div>
                    <div class="searchInputBox"><!--S/N-->
                        <label for="serialNumber"><%=commonWordList[$"{common}_17"] %></label>
                        <input type="text" id="serialNumber" name="serialNumber" placeholder="<%=commonWordList[$"{common}_32"] %>" />
                    </div>
                    <div class="searchInputBox"><!--稼働時間-->
                        <label for="operatinngTime"><%=commonWordList[$"{common}_19"] %></label>
                        <input type="number" id="operatingTime" name="operatingTime" placeholder="<%=commonWordList[$"{common}_33"] %>" />
                    </div>
                    <div class="searchSelectBox"><!--担当支店営業所-->
                        <label for="MGOffice"><%=commonWordList[$"{common}_20"] %></label>
                        <select id="MGOffice" name="MGOffice">
                            <option value=""><%=commonWordList[$"{common}_29"] %></option>
                            <%if (adminBranchList != null)
                                { %>
                            <%for (int i = 0; i < adminBranchList.Count; i++)
                                {%>
                                <option value="<%=adminBranchList[i].branchID%><%=split%><%=adminBranchList[i].branchName %>"><%=adminBranchList[i].branchName%></option>       
                            <%}
                                }%>
                        </select>
                    </div>
                    <div class="searchInputBox"><!--顧客呼称-->
                        <label for="machineName"><%=commonWordList[$"{common}_21"] %></label>
                        <input type="text" id="machineName" name="machineName" placeholder="<%=commonWordList[$"{common}_34"] %>" />
                    </div>
                    <div class="searchSelectBox"><!--部品交換アラート-->
                        <label for="partsAlert"><%=commonWordList[$"{common}_7"] %></label>
                        <select name="partsAlert" id="partsAlert">
                            <option value=""><%=commonWordList[$"{common}_29"] %></option>
                            <option value="1"><%=commonWordList[$"{common}_46"]%></option>
                            <option value="2"><%=commonWordList[$"{common}_47"]%></option>
                        </select>
                    </div>
                    <div class="searchSelectBox"><!--機械アラート-->
                        <label for="machineAlert"><%=commonWordList[$"{common}_6"] %></label>
                        <select name="machineAlert" id="machineAlert">
                            <option value=""><%=commonWordList[$"{common}_29"] %></option>
                            <option value="1"><%=commonWordList[$"{common}_46"]%></option>
                            <option value="2"><%=commonWordList[$"{common}_47"]%></option>
                        </select>
                    </div>
                    <div class=""></div>
                    <div class=""></div>
                    <div class=""></div>
                    <div class=""></div>
                    <input type="hidden" name="searchBT" value="" id="searchHideInput"/>
                    <button type="button" onclick="searchBTClick()" class="change relative"><%=commonWordList[$"{common}_38"] %></button>
                    <button type="submit" id="searchBT" class="hide"></button>
                </div>
            </div>
<!--*** 検索ボックスここまで ***-->
            <div>

<!--*** ページタイトル ***-->
                <div class="title">
                    <h1 id="ClientListPage-2"><%=selectCompany.companyName %></h1>
                </div>
<!--*** ページタイトルここまで ***-->

<!--*** ページャーとテーブル ***-->
                <div id="pager">
<!--*** ページャー ***-->
                    <div>
                        <p class="margin-left inline"><%=commonWordList[$"{common}_25"]%><%=pager.totalObject %><%=commonWordList[$"{common}_26"] %> : <%=pager.GetStartObjectCount() %> ～ <%=pager.GetEndObjectCount() %><%=commonWordList[$"{common}_27"] %></p>
                        <div id="pagerBox" class="grid inline">
                            <input type="hidden" id="clickedPager" name="clickedPager" value="" />
                            <input type="submit" class="hide" id="changePageButton" />
                            <div class="inline attend" onclick="changePage(<%=Math.Max(pager.nowPageNo-1,1) %>)">PREV</div>
                            <div class="inline">
                                <%for (int i = 1; i <= pager.totalPageCount; i++) {
                                        if(i == pager.nowPageNo)
                                        {%>
                                            <p class="inline bold point" name="<%=i %>" ><%=i %></p>
                                        <%}
                                        else
                                        {%>
                                            <p class="inline" name="<%=i %>" onclick="changePage(<%=i %>)"><%=i %></p>
                                        <%}
                                    } %>
                            </div>
                            <div class="inline attend" onclick="changePage(<%=Math.Min(pager.nowPageNo+1,pager.totalPageCount)%>)">NEXT</div>
                        </div>
                    </div>
<!--*** ページャーここまで ***-->
                    <p class="sortInfo bold inline"><%= searchInfo%></p>
<!--*** テーブル ***-->
                    <table id="clientTable" class="H350">
                        <thead>
                            <tr>
                                <th><%=commonWordList[$"{common}_11"] %></th>
                                <th><%=commonWordList[$"{common}_12"] %></th>
                                <th><%=commonWordList[$"{common}_13"] %></th>
                                <th><%=commonWordList[$"{common}_14"] %></th>
                                <th><%=commonWordList[$"{common}_15"] %></th>
                                <th><%=commonWordList[$"{common}_16"] %></th>
                                <th><%=commonWordList[$"{common}_17"] %></th>
                                <th><%=commonWordList[$"{common}_19"] %></th>
                                <th><%=commonWordList[$"{common}_20"] %></th>
                                <th><%=commonWordList[$"{common}_21"] %></th>
                                <th><%=commonWordList[$"{common}_22"] %></th>
                                <th><%=commonWordList[$"{common}_23"] %></th>
                                <th><%=commonWordList[$"{common}_24"] %></th>
                                <th><%=commonWordList[$"{common}_39"] %></th>
                            </tr>
                        </thead>
                        <tbody>
                            <%for (int i = pager.offset; i < Math.Min(pager.totalObject,(pager.limit + pager.offset)); i++) { %>
                                    <tr>
                                    <%for(int j = 0; j < headerCount; j++) { %>
                                        <td>Sample</td>
                                    <%}%>
                                    </tr>
                            <%} %>
                        </tbody>
                    </table>
<!--*** テーブルここまで ***-->
                </div>
<!--*** ページャーとテ―ブルここまで ***-->
            </div>
        </div>
    </form>
    <script src="js/Functions.js"></script>
    <script type="text/javascript">
        const table = document.querySelector("#clientTable");
        addMouseOverOut(table);
        addClickEvent(table, "Detail", true);
    </script>
</asp:Content>
