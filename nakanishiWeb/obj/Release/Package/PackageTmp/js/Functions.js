/**JQUERY - FUNCTION */
/**得意先名用の入力できるセレクトボックス*/
function addClientAutoComp() {
    $('#client').on('focus', function () {
        var options = $(this).data("options").split('*');
        $(this).autocomplete({
            source: options,
            minLength: 0,
            delay: 1,
            autoFocus: false,
            scroll: true,
            position: {my: "right top",at:"right bottom",collision:"flip"}
        });
        $(this).autocomplete("search", "");
    });
}
/**エンドユーザー名用の入力できるセレクトボックス*/
function addEnduserAutoComp() {
    $('#endUser').on('focus', function () {
        var options = $(this).data("options").split('*');
        $(this).autocomplete({
            source: options,
            minLength: 0,
            delay: 1,
            autoFocus: false,
            scroll: true,
            position: {my: "right top",at:"right bottom",collision:"flip"}
        });
        $(this).autocomplete("search", "");
    });
}
/**担当支店営業用の入力できるセレクトボックス*/
function addMGOfficeAutoComp() {
    $('#mgoffice').on('focus', function () {
        var options = $(this).data("options").split('*');
        $(this).autocomplete({
            source: options,
            minLength: 0,
            delay: 1,
            autoFocus: false,
            scroll: true,
            position: {my: "right top",at:"right bottom",collision:"flip"}
        });
        $(this).autocomplete("search", "");
    });
}
/**JQUERYここまで */

/**
 * アラート履歴ページの選択機器情報の「開く」「閉じる」の実装(引数は多言語化対応のため)
 * @param {any} openString：「開く」に対応する文言
 * @param {any} closeString：「閉じる」に対応する文言
 */
function informationToggle_HP(openString,closeString) {
    const parentDiv = document.querySelector("#titleContent");
    const machine_infoBox = document.querySelector("#choosedMachineInfo_HP");
    const infoContent = document.querySelectorAll(".toggleContent");
    const titleSubMarker = document.querySelector("#titleSubMarker");
    if (machine_infoBox.classList.contains("inactive")) {
        parentDiv.classList.remove("inactive");
        machine_infoBox.classList.remove("inactive");
        for (let i = 0; i < infoContent.length; i++) {
            infoContent[i].classList.remove("hide");
        }
        titleSubMarker.textContent = closeString;
    } else {
        parentDiv.classList.add("inactive");
        machine_infoBox.classList.add("inactive");
        for (let i = 0; i < infoContent.length; i++) {
            infoContent[i].classList.add("hide");
        }
        titleSubMarker.textContent = openString;
    }
}

/**
 * 選択機器情報の「開く」「閉じる」の実装(引数は多言語化対応のため)
 * @param {any} openString：「開く」に対応する文言
 * @param {any} closeString：「閉じる」に対応する文言
 */
function informationToggle(openString,closeString) {
    const parentDiv = document.querySelector("#machineInfAndTimechart");
    const machine_infoBox = document.querySelector("#choosedMachineInfo");
    const infoContent = document.querySelectorAll(".toggleContent");
    if (machine_infoBox.classList.contains("inactive")) {
        parentDiv.classList.remove("inactive");
        machine_infoBox.classList.remove("inactive");
        for (let i = 0; i < infoContent.length; i++) {
            infoContent[i].classList.remove("hide");
        }
        titleSubMarker.textContent = closeString;
    } else {
        parentDiv.classList.add("inactive");
        machine_infoBox.classList.add("inactive");
        for (let i = 0; i < infoContent.length; i++) {
            infoContent[i].classList.add("hide");
        }
        titleSubMarker.textContent = openString;
    }
}

//検索用submitボタンDOMの取得
const submitBT = document.querySelector("#searchBT");
/**
 * クリックされたページャーの値を引数に、hiddenタイプのinputに引数の値を格納。
 *次にdisplay:noneのsubmitタイプのinputを取得してクリックさせる。
 * @param {any} page_Index :クリックされたページャーのナンバー
 */
function changePage(page_Index) {
    let hiddenInput = document.querySelector("#clickedPager");
    hiddenInput.value = page_Index;
    document.querySelector("#changePageButton").click();
}

/**
 * リストの行をクリックした際に対応するTimeChartのバーを点滅させる
 * @param {any} alertID：DOMを取得するためのアラートID
 */
function graphFlash(alertID) {
    const rectTags = document.querySelectorAll("rect");
    const rectTag = document.querySelector(`#rect_${alertID}`);
    for (let i = 0; i < rectTags.length; i++) {
        if (rectTags[i].id != `rect_${alertID}`) {
            rectTags[i].classList.remove("flash");
            //rectTags[i].classList.add("transparent");
        }
    }
    if (rectTag.classList.contains("flash")) {
        rectTag.classList.remove("flash");
        /*for (let i = 0; i < rectTags.length; i++) {
            rectTags[i].classList.remove("transparent");
        }*/
    } else {
        //rectTag.classList.remove("transparent");
        rectTag.classList.add("flash");
    }
}

/**
 * *
 * @param {any} self
 * @param {any} textBoxID
 * @param {any} hideTextBoxID
 */
function inputTextBox(self, textBoxID,hideTextBoxID) {
    const textBox = document.querySelector(`#${textBoxID}`);
    const hideTextBox = document.querySelector(`#${hideTextBoxID}`);
    const value = self.children[self.selectedIndex].value;
    if (self.selectedIndex != 0) {
        let valueTexts = value.split(",");
        textBox.value = valueTexts[1];
        hideTextBox.value = value;
    } else {
        textBox.value = "";
        hideTextBox.value = "";
    }

}

/**
 * *アラートメンテナンス時期情報ページで、マシンアラートテーブルかパーツテーブルかをC#サイドに送信する
 * @param {any} tableName：partsかalert
 */
function tableKindSet(tableName) {
    const tableKindInput = document.querySelector("#tableKind");
    tableKindInput.value = tableName;
}

/**
 * 並び替えできるTHに付与するクリックイベント
 * クラスリストの流れ : None ⇒ ASC ⇒ DESC ⇒ None
 * その後sortEventを実行する
 * @param {any} ths :「sort」というクラスを持ったTHタグの配列
 * @param {any} sortCol : C#側でソートを管理しているインスタンスにセットされてる値(sortKey)
 * @param {any} orderDirection : sortColと同じ(DESC or ASC)
 */
function addSortEvent(ths, sortCol, orderDirection, tableKind) {
    for (let i = 0; i < ths.length; i++) {
        if (ths[i].id == sortCol) {
            ths[i].classList.add(orderDirection);
        }
        ths[i].addEventListener("click", function () {
            for (let j = 0; j < ths.length; j++) {
                if (i == j) { continue; }
                ths[j].classList.remove("ASC","DESC");
            }
            let th = ths[i];
            if (th.classList.contains("DESC")) {
                th.classList.remove("DESC");
                th.classList.add("ASC");
            } else if(th.classList.contains("ASC")){
                th.classList.remove("ASC");
                th.classList.add("DESC");
            } 
            let isMultipleTable = false;
            if ((tableKind != null) && (tableKind != "")) { isMultipleTable = true; }
            sortEvent(th, isMultipleTable, tableKind);
        });
    }
}

/**
 * 並び替えできるTHがクリックされたときに呼ばれるメソッド
 * クリックされたTHのIDと、クラスにDescがあるかどうかでDESCかASCを決め
 * 2つを組み合わせてC#側にRequest.Formを送信する
 * @param {any} clickedObject :クリックされたTH自身
 */
function sortEvent(clickedObject,isMultipleTable,tableKind) {
    const sortBT = document.querySelector("#sortBT");
    const sortInput = document.querySelector("#sortInput");
    let sortOrder;
    if (clickedObject.classList.contains("DESC")) {
        sortOrder = "DESC";
    } else if (clickedObject.classList.contains("ASC")) {
        sortOrder = "ASC";
    } else {
        sortOrder = "default";
    }
    sortInput.value = `${clickedObject.id},${sortOrder}`;
    if (isMultipleTable) { tableKindSet(tableKind); }
    sortBT.click();
}

/**
 * １つ前のページに戻る
 * */
function goBack() {
    history.back(-1);
}

/**
 * *カレンダーinputタグのmin値をセットするメソッド
 * @param {any} DOM
 * @param {any} inputID
 */
function setMinDate(DOM,inputID) {
    const inputDate = document.querySelector(`#${inputID}`);
    inputDate.setAttribute("min",DOM.value);
}

/**
 * *ajax用（未使用）
 * @param {any} buttonID
 * @param {any} alertNo
 */
function changeAlertTable(buttonID, alertNo) {
    const bt = document.querySelector(`#${buttonID}`);
    const hideInput = document.querySelector("#alertNo");
    hideInput.value = alertNo;
    bt.click();
   /* let URL;
    if (document.getElementById("searchDateInput")) {
        URL = `/ajaxPage/GetAlertList_ofDay.aspx`;
    } else {
        URL = `/ajaxPage/GetAlertList_ofMonth.aspx`;
    }
    const jqXHR = $.ajax({
        type: "GET",
        url: `${URL}`,
        data: {
            alertNo: alertNo
        },
        dataType: "html",
        cache: false,
        scriptCharset: "utf-8",
        timeout:5000,
    });

    jqXHR.done(function (data, textStatus, jqXHR) {
        console.log("success");
        console.log(data);
        const timelineBox = document.querySelector("#timelineBox");
        clearAllChildElement(timelineBox);
        timelineBox.innerHTML = data;
    });
    jqXHR.fail(function (jqXHR, textStatus, errorThrown) {
        let message = errorThrown;
        console.log(message);
    });
    jqXHR.always(function (jqXHR, textStatus) {
        console.log("finally");
    })*/
}

/**
 * *「交換完了」ボタンをクリックした時にダイアログボックスを表示する
 * @param {any} partsName：パーツ名
 * @param {any} partsID：パーツID
 */
function exchangeConfirm(partsName,partsID) {
    result = confirm(`${partsName}を交換済みにしますか？\n※交換済みにすると「推奨交換目安までの残量」がリセットされます`);
    if (result) {
        //完了済みボタンが押された時の処理
        const exchangeInput = document.querySelector("#exchange");
        const exBT = document.querySelector("#sortBT");
        exchangeInput.value = partsID;
        exBT.click();
    }
}
//旧バージョン（使ってない）
function partsExchanged(value) {
    confirm(`${value}を交換済みにしますか？<br>※交換済みにすると「推奨交換目安までの残量」がリセットされます`);
}

/**
 * Detailページの月ごとか日ごとかを選択するボタン用のメソッド
 * valueによって中の処理を変える
 * @param {any} value : Date(日ごと) / Month(月ごと)
 * @param {any} str1 : 月ごとが選ばれた時の表示用の文字(「年」)
 * @param {any} str2 : 月ごとが選ばれた時の表示用の文字(「月」)
 */
function changeDateOrMonth(value,str1,str2) {
    const DoMsubmitBT = document.querySelector("#DateOrMonthBT");
    const temp_BT = document.querySelector("#changeDateButton");
    const temp_imput = document.querySelector("#DayOrMonth");
    const dateOrMonthDiv = document.querySelector("#selectDateOrMonth");
    const today = new Date();
    if (value == "Month") {
        clearAllChildElement(dateOrMonthDiv);
        //年選択用
        let selectYear = document.createElement("select");
        let pTag_Y = document.createElement("p");
        pTag_Y.textContent = str1;
        pTag_Y.classList.add("inline");
        selectYear.classList.add("inline");
        selectYear.setAttribute("id", "searchYearSelect");
        for (let i = today.getFullYear(); i >= 2000; i--) {
            let option = document.createElement("option");
            option.value = i;
            option.textContent = i;
            selectYear.add(option);
        }
        //月選択用
        let selectMonth = document.createElement("select");
        let pTag_M = document.createElement("p");
        pTag_M.textContent = str2;
        pTag_M.classList.add("inline");
        selectMonth.classList.add("inline");
        selectMonth.setAttribute("id","searchMonthSelect");
        for (let i = 1; i <= 12; i++) {
            let option = document.createElement("option");
            option.value = i;
            option.textContent = i;
            selectMonth.add(option);
        }
        //Divに追加
        dateOrMonthDiv.appendChild(selectYear);
        dateOrMonthDiv.appendChild(pTag_Y);
        dateOrMonthDiv.appendChild(selectMonth);
        dateOrMonthDiv.appendChild(pTag_M);
        dateOrMonthDiv.appendChild(temp_imput);
        dateOrMonthDiv.appendChild(temp_BT);
    } else {
        clearAllChildElement(dateOrMonthDiv);
        let input = document.createElement("input");
        input.classList.add("inline");
        input.type = "date";
        input.setAttribute("id", "searchDateInput");
        dateOrMonthDiv.appendChild(input);
        dateOrMonthDiv.appendChild(temp_imput);
        dateOrMonthDiv.appendChild(temp_BT);
    }
}

/***
 * 検索する日にちを変更する
 * サブタイトルには日時をセットする
 * */
function changeSearchDate(buttonDOM,hideInputID) {
    const inputDate = document.querySelector("#searchDateInput");
    const hideInput = document.querySelector(`#${hideInputID}`);
    const select_Y = document.querySelector("#searchYearSelect");
    const select_M = document.querySelector("#searchMonthSelect");
    //const date = document.querySelector("#searchDate");
    if ((inputDate != null) && (inputDate.value)) {
        let someday = new Date(inputDate.value);
        hideInput.value = `${someday.getFullYear()},${someday.getMonth()},${someday.getDate()},day`;
        //date.textContent = `${makeDateStr(someday)}の情報`;
    }
    if (select_Y != null){
        let year = select_Y.options[select_Y.selectedIndex].value;
        let month = select_M.options[select_M.selectedIndex].value;
        hideInput.value = `${year},${month},month`;
        //date.textContent = `${year}年${month}月の情報`
    }
    buttonDOM.click();
}

/**
 * *Dateインスタンスから表示用の文字列を作成
 * @param {any} date：Dateインスタンス
 */
function makeDateStr(date){
    let dateString = `${date.getFullYear()}年${date.getMonth()+1}月${date.getDate()}日`;
    return dateString;
}

/***
 * button[type=submit]のボタンをクリックして送信する
 * */
function searchBTClick() {
    let searchHideInput = document.querySelector("#searchHideInput");
    searchHideInput.value = "true";
    document.querySelector("#searchBT").click();
}

/***
 * button[type=submit]のボタンをクリックして送信する
 * */
function downloadCSVClick() {
    let downloadCSV = document.querySelector("#downloadCSV");
    downloadCSV.value = "true";
    document.querySelector("#downloadCSVBT").click();
    downloadCSV.value = "";
}

/***
 * button[type=submit]のボタンをクリックして送信する
 * */
function editUserSetting() {

    const passInput = document.getElementById("passInput").value;
    const passInputNew1 = document.getElementById("passInputNew1").value;
    const passInputNew2 = document.getElementById("passInputNew2").value;

    if (passInput.length == 0) {
        alert('パスワードが未入力です')
        return;
    }

    if (passInputNew1.length > 0) {
    	if (passInputNew1.length >= 8 && passInputNew1.length <= 16)
    	{
            var regexp = /^(?=.*?[a-z])(?=.*?[A-Z])(?=.*?\d)[a-zA-Z\d]{8,16}$/;
            if (regexp.test(passInputNew1) != true) {
                alert('パスワードには数字、英小文字、英大文字を含めてください');
                return;
            }
            else {
                if (passInputNew1 != passInputNew2) {
                    alert('確認用パスワードが異なります');
                    return;
                }
            }
        }
        else
        {
            alert('パスワードは8-16文字にしてください');
            return;
        }
    }
    
    let editSetInput = document.querySelector("#editSetInput");
    editSetInput.value = "true";
    document.querySelector("#editSet").click();
    editSetInput.value = "";
};

/***
 * button[type=submit]のボタンをクリックして送信する
 * */
function clickMailSetting() {
    let editSetInput = document.querySelector("#editSetInput");
    editSetInput.value = "true";
    document.querySelector("#editSet").click();
    editSetInput.value = "";
}

/**
 * テーブルのtr要素がクリックされたとき、その行のマシンIDをinput要素に入れて送信する
 * @param {any} machineID
 */
function clickTR(machineInfo) {
    const input = document.querySelector("#click_tr");
    const bt = document.querySelector("#nextPageBT");
    input.value = machineInfo;
    bt.click();
}

/**
 * companyIDをもとに、製品群リストを取得してセレクトタグを入れ替える
 * @param {any} modelSelctTag :model選択のセレクトタグ
 * @param {any} selectorDivID :変更したいセレクトタグが置いてあるDivのID
 */
function changeModel_ofCompany(SelctTag,selectorDivID,parent) {
    const selectedIndex = SelctTag.selectedIndex;
    const companyID = SelctTag.children[selectedIndex].value;
    const jqXHR = $.ajax({
        type: "GET",
        url: `/ajaxPage/GetMachineModelList.aspx`,
        data: {
            companyID: companyID,
            parent: parent
        },
        dataType: "html",
        cache: false,
        scriptCharset: "utf-8",
        timeout:5000,
    });

    jqXHR.done(function (data, textStatus, jqXHR) {
        console.log("success");
        const selectDiv = document.getElementById(selectorDivID);
        clearAllChildElement(selectDiv);
        //ajaxで取ってきたselectタグを、引数で指定したDIVに置く
        selectDiv.innerHTML = data;
    });
    jqXHR.fail(function (jqXHR, textStatus, errorThrown) {
        let message = errorThrown;
        console.log(message);
    });
    jqXHR.always(function (jqXHR, textStatus) {
    })
}

/**
 * *検索ボックスの担当支店営業用　選択された顧客名等によって、セレクトで表示する一覧を変更する
 * @param {any} OBJ：セレクトタグDOM
 */
function setDefaultMGOffice(OBJ) {
    if (OBJ.children[OBJ.selectedIndex].value == "") {//"全て"が選択されていたらメソッドを抜ける
        return;
    } else {
        const MGOfficeDOM = document.getElementById("MGOfficeNameDiv");
        const jqXHR = $.ajax({
            type: "GET",
            url: `/ajaxPage/GetAllMGOffice.aspx`,
            data: {
            },
            dataType: "html",
            cache: false,
            scriptCharset: "utf-8",
            timeout:5000,
        });
        jqXHR.done(function (data, textStatus, jqXHR) {
            console.log("success");
            clearAllChildElement(MGOfficeDOM);
            //ajaxで取ってきたselectタグを、引数で指定したDIVに置く
            MGOfficeDOM.innerHTML = data;
        });
        jqXHR.fail(function (jqXHR, textStatus, errorThrown) {
            let message = errorThrown;
            console.log(message);
        });
        jqXHR.always(function (jqXHR, textStatus) {
        })
    }
}

/**
 * 検索ボックスで使用
 * @param {any} selectTag :onchangeを書く自身のタグ(thisで渡す)
 * @param {any} clientDivID :クライアント名のセレクタID
 */
function changeClientName(selectTag, clientDivID) {
    const selectedIndex = selectTag.selectedIndex;
    const ID = selectTag.children[selectedIndex].value;
    const jqXHR = $.ajax({
        type: "GET",
        url: `/ajaxPage/GetClientList.aspx`,
        data: {
            id: ID
        },
        dataType: "html",
        cache: false,
        scriptCharset: "utf-8",
        timeout:5000,
    });
    jqXHR.done(function (data, textStatus, jqXHR) {
        console.log("success");
        const selectDiv = document.getElementById(clientDivID);
        clearAllChildElement(selectDiv);
        //ajaxで取ってきたselectタグを、引数で指定したDIVに置く
        selectDiv.innerHTML = data;
    });
    jqXHR.fail(function (jqXHR, textStatus, errorThrown) {
        let message = errorThrown;
        console.log(message);
    });
    jqXHR.always(function (jqXHR, textStatus) {
    })
}

/**
 * clientIDをもとに、endUserリストを取得してセレクトタグを入れ替える
 * @param {any} selctTag :clientもしくは担当支店選択のセレクトタグ
 * @param {any} endUserDivID :変更したいセレクトタグが置いてあるDivのID
 */
function changeEndUserName(selectTag,endUserDivID) {
    const selectedIndex = selectTag.selectedIndex;
    let parent;
    if (selectTag.id == "clientCD") {
        parent = "client";
    } else {
        parent = "MGOffice";
    }
    const values = selectTag.children[selectedIndex].value.split(",");
    const parentID = values[0];
    const jqXHR = $.ajax({
        type: "GET",
        url: `/ajaxPage/GetEndUserList.aspx`,
        data: {
            parent: parent,
            parentID: parentID
        },
        dataType: "html",
        cache: false,
        scriptCharset: "utf-8",
        timeout:5000,
    });

    jqXHR.done(function (data, textStatus, jqXHR) {
        console.log("success");
        const selectDiv = document.getElementById(endUserDivID);
        clearAllChildElement(selectDiv);
        //ajaxで取ってきたselectタグを、引数で指定したDIVに置く
        selectDiv.innerHTML = data;
    });
    jqXHR.fail(function (jqXHR, textStatus, errorThrown) {
        let message = errorThrown;
        console.log(message);
    });
    jqXHR.always(function (jqXHR, textStatus) {
    })
}

/**
 * summary:モデルIDをもとにJsonデータを取得後、セレクトタグを入れ替える
 * @param {any} selectorDivID :ajax通信したい対象のセレクトタグが置かれているDIVのID
 */
function changeMachineType(selectorDivID,OBJ) {
    let modelID = "";
    let parent = "";
    let companyID;
    let strings;
    const mgOffice = document.querySelector("#MGOfficeName");
    const endUS = document.querySelector("#endUserID");
    const client = document.querySelector("#clientCD");
    const model = document.querySelector("#modelID");
    let mgOfficeInfo = 0;//mgOffice.children[mgOffice.selectedIndex].value;
    let clientInfo = 0;//client.children[client.selectedIndex].value.split(",");
    let endUSInfo = 0;//endUS.children[endUS.selectedIndex].value;
    if (OBJ.id == "MGOfficeName") {
        companyID = getIdFromValue(mgOfficeInfo);
        parent = "MGOffice";
        modelID = -1;
    }else if (OBJ.id == "client") {
        companyID = getIdFromValue(clientInfo[0]);
        parent = "client";
        modelID = -1;
    }else if (OBJ.id == "endUserID") {
        companyID = getIdFromValue(endUSInfo);
        parent = "enduser";
        modelID = -1;
    }else if (OBJ.id == "modelID") {
        let modelInfo = model.children[model.selectedIndex].value;
        modelID = getIdFromValue(modelInfo);
        if (mgOfficeInfo != "") {
            parent = "MGOffice";
            companyID = getIdFromValue(mgOfficeInfo);
        }else if(endUSInfo != "") {
            parent = "enduser";
            companyID = getIdFromValue(endUSInfo);
        } else if (clientInfo != "") {
            parent = "client";
            companyID = getIdFromValue(clientInfo);
        } else {
            parent = "model";
        }
    } else {
        parent = "";
        companyID = -1;
        modelID = -1;
    }
    const jqXHR = $.ajax({
        type: "GET",
        url: `/ajaxPage/GetTypeList.aspx`,
        data: {
            modelID: modelID,
            companyID: companyID,
            parent: parent,
        },
        dataType: "html",
        cache: false,
        scriptCharset: "utf-8",
        timeout:5000,
    });

    jqXHR.done(function (data, textStatus, jqXHR) {
        console.log("success");
        const selectDiv = document.getElementById(selectorDivID);
        clearAllChildElement(selectDiv);
        //ajaxで取ってきたselectタグを、引数で指定したDIVに置く
        selectDiv.innerHTML = data;
    });
    jqXHR.fail(function (jqXHR, textStatus, errorThrown) {
        let message = errorThrown;
        console.log(message);
    });
    jqXHR.always(function (jqXHR, textStatus) {
    })
}

/**
 * *カンマ区切りの文字列から先頭にあった値を切り離して返す
 * 空文字だった時は-1を返す
 * @param {any} valueString：カンマ区切りの文字列
 */
function getIdFromValue(valueString) {
    let result;
    if (valueString != "") {
        let strings = valueString.split(",");
        result = strings[0];
    } else {
        result = -1;
    }
    return result;
}

/***
 * テーブルにクリックイベントを付与するメソッド
 * @param {any} table : クリックイベントを登録したいテーブルのDOM
 * @param {any} nextPageURL : 遷移先のURL
 * @param {any} isLocationChange : クリック時ページ遷移するかどうか(true⇒するfalse⇒しない)
 */
function addClickEvent(table,nextPageURL,isLocationChange) {
    table.addEventListener("click", function (e) {
        let trs = table.querySelectorAll("tr");
        if ((e.target.tagName.toLowerCase() != "th") && (e.target.tagName.toLowerCase() == "td")) {
            for (let i = 0; i < trs.length; i++) {
                if (trs[i] == e.target.parentNode) {
                    continue;
                } else {
                    trs[i].classList.remove("choosen");
                }
            }
            if (e.target.parentNode.classList.contains("choosen")) {
                console.log("contains choosen");
                e.target.parentNode.classList.remove("choosen");
            } else {
                console.log("not contains choosen");
                e.target.parentNode.classList.add("choosen");
            }
            if (isLocationChange) {
                //少しだけ時間をおいてページ遷移
                let timer = setTimeout(function () {
                    location.href = `${nextPageURL}.aspx`;
                }, 300)
            }
        }
    },false);
}

/***
 * テーブルにマウスオーバーとマウスアウトの時のイベントを付与する
 * @param {any} table : イベントを付与したいテーブルのDOM
 */
function addMouseOverOut(table){
    table.addEventListener("mouseover", function (e) {
        if(e.target.tagName.toLowerCase()=="td"){
            e.target.parentNode.classList.add("over");
        }
    },false);
    table.addEventListener("mouseout", function (e) {
        e.target.parentNode.classList.remove("over");
    })
}

/***
 * inputタグにサジェスト機能追加するメソッド
 * インプットされると下のsuggestCallメソッドが呼ばれる
 * @param {any} inputID : inputタグのID
 * @param {any} suggestBoxID : サジェスト機能用ulタグのID
 */
function addInputAssistance(inputID,suggestBoxID) {
    const input = document.querySelector(`#${inputID}`);
    input.addEventListener("input", function (e) {
        if (e.key == "Tab") { return; }
        suggestCall(this.value, suggestBoxID,this);
    });
}

/***
 * ajaxと同じでクエリパラメーターに入力された文字を渡して
 * DBとの照合結果をliタグに格納⇒ulタグにアペンド
 * @param {any} queryValue : クエリパラメーターに渡す値
 * @param {any} suggestBoxID : サジェスト機能用ulタグのID(addInputAssistanceから引継ぎ)
 * @param {any} input : inputイベントが付与されたinputタグ(addInputAssistanceの'this')
 */
async function suggestCall(queryValue,suggestBoxID,input) {
    try {
        const URL = `/suggestPage/CompanySuggest.aspx?word=${queryValue}`;
        const response = await fetch(URL);
        const suggestData = await response.json();
        const suggestBox = document.querySelector(`#${suggestBoxID}`);
        if (suggestData.length > 0) {
            suggestBox.classList.remove("hide");
        }
        //1度中身を全消し
        clearAllChildElement(suggestBox);
        for (let i = 0; i < suggestData.length; i++) {
            let li = document.createElement("li");
            li.onclick = function () {chooseMachineBT
                let clickedValue = this.textContent;
                input.value = clickedValue;
                clearAllChildElement(suggestBox);
                suggestBox.classList.add("hide");
            }
            li.textContent = suggestData[i];
            li.classList.add("hoverP");
            suggestBox.appendChild(li);
        }
    } catch (error) {
        console.log(`ERROR : ${error.message}`);
    } finally {
    }
}

/***
 * DOMの中身を全て消すメソッド
 * @param {any} DOM : 中身を消したいDOＭのID
 */
function clearAllChildElement(DOM) {
    while (DOM.firstChild) {
        DOM.removeChild(DOM.firstChild);
    }
}

/**
 * *０埋めで２桁にするメソッド
 * @param {any} value：０埋め表記にしたい値
 */
function addZero(value) {
    let result;
    if (value < 10) {
        result = `0{value}`;
    } else {
        result = value;
    }
    return result;
}
