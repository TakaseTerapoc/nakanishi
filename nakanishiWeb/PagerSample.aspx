<%@ Page Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="PagerSample.aspx.cs" Inherits="nakanishiWeb.PagerSample" %>

<asp:Content ID="TitleContent" ContentPlaceHolderID="Title" runat="server">
    <div class="title">
        <h1 id="DetailPage">Pagerのサンプル</h1>
    </div>
</asp:Content>

<asp:Content ID="BodyContent" ContentPlaceHolderID="MainContent" runat="server">
    <div id="pager">
        <form id="changePager" target="_self" runat="server">
            <p>全<%=pager.totalObject%>件中 : <%=pager.GetStartObjectCount()%>件～<%=pager.GetEndObjectCount()%>件まで</p>
            <div id="pagerBox" class="grid">
                <input type="hidden" id="clickedPager" name="clickedPager" value="" />
                <input type="submit" class="hide" id="changePageButton" />
                <div class="inline attend" onclick="changePage(<%=Math.Max(pager.nowPageNo-1,1)%>)">PREV</div>
                <div class="inline">
                    <!--↓↓ページャーの生成-->
                    <%for (int i=1; i<=pager.totalPageCount; i++) {
                            if (i == pager.nowPageNo)
                            {%>
                                <p class="inline bold point" name="<%=i%>"><%=i%></p>
                        <%}
                            else
                            { %>
                                    <p class="inline" name="<%=i%>" onclick="changePage(<%=i %>)"><%=i%></p>
                        <%}%>
                    <%} %>
                </div>
                <div class="inline attend" onclick="changePage(<%=Math.Min(pager.nowPageNo+1,pager.totalPageCount)%>)">NEXT</div>
            </div>
            <!--↓↓データ表示-->
            <table>
            <%for (int i = pager.offset; i < Math.Min(pager.totalObject,(pager.limit + pager.offset)); i++)
                { %>
                    <tr>
                        <td><%=i+1%></td><td><%=sampleDict[i+1] %></td>
                    </tr>
            <%} %>
            </table>
        </form>
    </div>
    <!-- 自分用↓↓common.jsを作ってそこに置いてもいいかも-->
    <script type="text/javascript">
        //クリックされたページャーの値を引数に、hiddenタイプのinputに引数の値を格納。
        //次にdisplay:noneのsubmitタイプのinputを取得してクリックさせる。
        function changePage(page_Index) {
            let submitImput = document.querySelector("#clickedPager");
            submitImput.value = page_Index;
            document.querySelector("#changePageButton").click();
        }
    </script>
</asp:Content>
