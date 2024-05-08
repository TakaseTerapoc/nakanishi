const searchBox = document.querySelector("#machineList");
const titleBox = document.querySelector(".titleBox");
const menuButton = document.querySelector(".menuButton");
const underNavContent = document.querySelector(".underNavContent");
const title = document.querySelector("h1");


if (searchBox != null) {
    if (searchBox.classList.contains("active")) {
        searchBox.classList.remove("active");
    }
    if (menuButton.classList.contains("active")) {
        menuButton.classList.remove("active");
    }
    if (!underNavContent.classList.contains("close")) {
        underNavContent.classList.add("close");
    }
}

// 検索ボックスの開閉ボタン用 //
if ((title.id != "DetailPage") && (title.id != "HistoryPage")) {
    titleBox.onclick = function () {
        menuButton.classList.toggle("active");
        searchBox.classList.toggle("active");
        underNavContent.classList.toggle("close");
    }
}

/**
 * *ホバーされているテーブルの行の色を変える
 * @param {any} target：テーブルの<tr>タグ
 */
function changeColor(target){
    target.addEventListener("mouseover",function(){
        target.classList.add("attention");
    });
    target.addEventListener("mouseleave",function(){
        target.classList.remove("attention");
    });
}

/**
 * *0埋め２桁の表示用
 * @param {any} integer：整数
 */
function addZero(integer){
    let num = 0;
    if(integer < 10){
        num = `0${integer}`;
    }else{
        num = integer;
    }
    return num;
}

/**
 * *サイドナビのオープンクローズ
 * @param {any} navID：サイドナビのDOMid
 * @param {any} dom：サイドナビのコントロールボタンDOM
 */
function sideNavOpenClose(navID, dom) {
    const nav = document.getElementById(navID);
    const body = document.querySelector("body");
    if (nav.classList.contains("close")) {
        nav.classList.remove("close");
        dom.classList.remove("close");
        body.classList.remove("close");
    } else {
        nav.classList.add("close");
        dom.classList.add("close");
        body.classList.add("close");
    }
}

//↓↓JQUERY - FUNCTION↓↓
$(".inner-grid-container").css("display", "none");
$(".side-open").on("click", function () {
    $(".inner-grid-container").css("display","none");//まず全てのプルダウンメニューを閉じる

    //*CSS用*クリックするメニューの親の親(<nav class="menu">)と自身から'open'クラス削除
    $(".grid-item").removeClass("open");
    //$(".grid-item").parent(".grid-container").parent(".menu").removeClass("open");

    let findEle = $(this).children(".inner-grid-container");
    if ($(this).hasClass("close")) {
        $(this).removeClass("close");
        $(".grid-item").removeClass("down");
    } else {
       // $(".close").removeClass("close");
        $(this).addClass('close');

        //ナビメニューが開いたとき、bodyに重ならないようにする
        $(".contentBox").addClass("down");
        $(findEle).slideDown(300);

        //*CSS用*クリックするメニューの親の親(<nav class="menu">)と自身に'open'クラス追加
        $(this).addClass("open");
        //$(this).parent(".grid-container").parent(".menu").addClass("open");
    }
});