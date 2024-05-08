<%@ Page Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="AlertMailSetting.aspx.cs" Inherits="nakanishiWeb.AlertMailSetting" %>
<%@ Import Namespace="nakanishiWeb.Const" %>

<asp:Content ID="BodyContent" ContentPlaceHolderID="MainContent" runat="server">
    <form id="changePager" target="_self" runat="server">
        <div id="mainPagesContents" class="grid main underNavContentH">
            <!--<div>-->
<!--*** ページタイトル ***-->
            <div id="titleContent" class="inactive">
                <div class="title">
                    <h1 id="HistoryPage"><%=DetailPageWords[(int)LanguageTable.DetailPageStrId.Detail_29] %></h1>
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

            <div id="pager_rightSide" class="grid">
<!--*** ページャー ***-->
                <button type="button" onclick="location.href='Detail.aspx'" class="relative change" id="toDetail"><%=HistoryPageWords[(int)LanguageTable.HistoryPageStrId.History_1] %></button>

                <input type="hidden" name="editSet" value="" id="editSetInput"/>
                <button type="button" onclick="clickMailSetting()" class="change relative"><%=CommonWords[(int)LanguageTable.CommonPageStrId.Common_65] %></button>
                <button type="submit" id="editSet" class="hide"></button>

<!--*** ページャーここまで ***-->
            </div>

<!--*** パーツ詳細テーブル ***-->
            <div class="mailset-margin-top mailSettingGrid relative">
                <table id="alertKindTable" class="fixedLayout">
                    <thead>
                        <tr class="hoverD">
                            <th id="alertName" class="par_L"><input type="checkbox" id="alertAllCheck" /> <%= AlertListWords[(int)LanguageTable.AlertListPageStrId.AlertList_1] %><span></span></th>
                        </tr>
                    </thead>
                    <tbody>
                        <%for (int i = 0; i < alertKindList.Count; i++)
                            {%>
                        <tr class="hoverD">
                            <td class="par_L">
                                <input type="checkbox" name="alertCheck[]" value="<%=i%>" <%=registeredAlertNoList.Contains(alertKindList[i].alertNo) ? "checked" : ""%> />
                                <span> <%=alertKindList[i].alertName %>  </span>
                            </td>
                        </tr>
                        <%}%>
                    </tbody>
                </table>
                <%if(alertKindList.Count == 0){ %>
                        <p class="bold absolute noData1"><%=DetailPageWords[(int)LanguageTable.DetailPageStrId.Detail_24]%></p>
                <%} %>
                <table id="partsKindTable" class="fixedLayout">
                    <thead>
                        <tr class="hoverD">
                            <th id="partsName" class="par_L"><input type="checkbox" id="partsAllCheck" /><span> <%= AlertListWords[(int)LanguageTable.AlertListPageStrId.AlertList_2] %></span></th>
                        </tr>
                    </thead>
                    <tbody>
                        <%for (int i = 0; i < partsKindList.Count; i++)
                            {%>
                        <tr class="hoverD">
                            <td class="par_L">
                                <input type="checkbox" name="partsCheck[]" value="<%=i%>" <%=registeredPartsIdList.Contains(partsKindList[i].partsKindId) ? "checked" : ""%> />
                                <span> <%=partsKindList[i].partsName %>  </span>
                            </td>
                        </tr>
                        <%}%>
                    </tbody>
                </table>
                <%if(partsKindList.Count == 0){ %>
                        <p class="bold absolute noData2"><%=DetailPageWords[(int)LanguageTable.DetailPageStrId.Detail_23] %></p>
                <%} %>
            </div>
<!--*** パーツ詳細テーブルここまで ***-->

        </div>
        <input type="hidden" name="pageChange" id="pageChange" />
        <button type="submit" id="pageChangeBT" class="hide"></button>
    </form>
    <script src="js/jquery-3.6.1.min.js"></script>
    <script src="js/Functions.js"></script>
    <script type="text/javascript">

        function changeAlertHeader() {
            if ( ($('#alertKindTable tbody input:checked').length == $('#alertKindTable tbody input').length) &&
                 ($('#alertKindTable tbody input').length > 0) ) {
                $('#alertAllCheck').prop('checked', true);
            }
            else {
                $('#alertAllCheck').prop('checked', false);
            }
        }
        function changePartsHeader() {
            if ( ($('#partsKindTable tbody input:checked').length == $('#partsKindTable tbody input').length) &&
                 ($('#partsKindTable tbody input').length > 0) ) {
                $('#partsAllCheck').prop('checked', true);
            }
            else {
                $('#partsAllCheck').prop('checked', false);
            }
        }

        $('#alertAllCheck').on('click', function () {
            var checkedState = this.checked;
            $('#alertKindTable :checkbox').each(function () {
                this.checked = checkedState;
            });
        });

        $('#partsAllCheck').on('click', function () {
            var checkedState = this.checked;
            $('#partsKindTable :checkbox').each(function () {
                this.checked = checkedState;
            });
        });

        $("input[name='alertCheck[]']").on('click', function () {
            changeAlertHeader();
        });

        $("input[name='partsCheck[]']").on('click', function () {
            changePartsHeader();
        });

        // 画面ロード時のfunction
        $(function () {
            changeAlertHeader();
            changePartsHeader();
        });
    </script>
</asp:Content>
