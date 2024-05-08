<%@ Page Language="C#" AutoEventWireup="true" MasterPageFile="~/Site.Master" CodeBehind="SettingComplete.aspx.cs" Inherits="nakanishiWeb.SettingComplete" %>
<%@ Import Namespace="nakanishiWeb.Const" %>

<asp:Content ID="BodyContent" ContentPlaceHolderID="MainContent" runat="server">
    <form id="changePager" target="_self" runat="server">
        <div id="mainPagesContents" class="grid main underNavContentMS">
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

<!--*** ページャーここまで ***-->
            </div>

<!--*** パーツ詳細テーブル ***-->
            <div class="CompleteMsg"><%=CommonWords[(int)LanguageTable.CommonPageStrId.Common_67] %></div>
<!--*** パーツ詳細テーブルここまで ***-->

            <input type="hidden" name="pageChange" id="pageChange" />
            <button type="submit" id="pageChangeBT" class="hide"></button>
        </div>
    </form>
    <script src="js/jquery-3.6.1.min.js"></script>
    <script src="js/jquery-ui.js"></script>
    <script src="js/Functions.js"></script>
    <script type="text/javascript">
    </script>
</asp:Content>
