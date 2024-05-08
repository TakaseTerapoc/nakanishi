const historyTableDOM = document.querySelector("#historyTable");
//const popup = document.querySelector("#tooltip");
const clientPagePopup = document.querySelector("#ClientListTooltip");
const h1Title = document.querySelector("h1");
const selectorDIV = document.querySelector(".selector");
const contextmenu = document.querySelector("#contextMenu");
const dropDownMenuDOM = document.querySelector("#order");

const selectFactory = ["A","B","C","D","E","F","G","H","I","J","K","L"];
const machineThs = ["顧客名","設置場所","設置時期","成型機<br>タイプ","S/N","顧客呼称","保証契約<br>開始日","付帯設備","郵便番号","住所","Net<br>Weight","Gross<br>Weight","機械<br>アラート","部品交換<br>アラート"];
const clientThs = ["機械名","機種型式","S/N","設置日","稼働時間","機械<br>アラート","部品交換<br>アラート"];
const alertTableThs = ["発生日時","部品交換<br>アラート","顧客名","工場","機械名","機械型式","S/N","設置日","稼働時間"];
const historyThs = ["エラーコード","説明","最新発生","サービス<br>メーター","発生回数","モニタ<br>表示","コール","アクション<br>レベル","タイプ","論理名"];
const tdMax = 10;
const orderOptions = ["昇順","降順"];
const conditions = ["エラーコード","発生日時","発生回数","アクションレベル","タイプ","論理名"];
const types = ["電気系","機械系"];
const types2 = ["有","無"];
const logicNames = ["HST","ENG/M"];
let historyTds;

h1Title.oncontextmenu = "return false;";

function makeSelector(options,selectorDOM){
    let select = document.createElement("select");
    for(let i=0 ; i<options.length ; i++){
        let option = document.createElement("option");
        option.textContent = options[i];
        select.appendChild(option);
    }
    selectorDOM.appendChild(select);
}

if(selectorDIV != null){
    let select = document.createElement("select");
    for(let i=0 ; i<selectFactory.length ; i++){
        let optionFactory = document.createElement("option");
        optionFactory.textContent = `工場${selectFactory[i]}`;
        select.appendChild(optionFactory);
    }
    selectorDIV.appendChild(select);
    let button = document.createElement("button");
    button.textContent = "切り替える";
    button.classList.add("change");
    selectorDIV.appendChild(button);
}

if(h1Title.id == "HistoryPage"){
   // makeTable(historyTableDOM,historyThs,historyTds);
    //makeSelector(conditions,dropDownMenuDOM);
    //makeSelector(orderOptions,dropDownMenuDOM);
    /*let sortButton = document.createElement("button");
    sortButton.setAttribute("type","button");
    sortButton.classList.add("change");
    sortButton.textContent = "並べ替え";*/
    //dropDownMenuDOM.appendChild(sortButton);
}

function makeTable(DOM,ths,tds){
    const thead = document.createElement("thead");
    const tbody = document.createElement("tbody");
    for(let i=0 ; i<=tdMax ; i++){
        historyTds = [`${getRandomNumber(100,10)}@TE`,"SAMPLE",`${getRandomNumber(3,2019)}/${addZero(getRandomNumber(12,1))}/${addZero(getRandomNumber(28,1))}<br>&ensp;&ensp;&ensp;&ensp;&ensp;&ensp;${addZero(getRandomNumber(24,0))}:${addZero(getRandomNumber(60,0))}`,`${getRandomNumber(4000,600)}.0H`,`${getRandomNumber(10,1)}`,`${types2[getRandomNumber(2,0)]}`,`${types2[getRandomNumber(2,0)]}`,`L${addZero(getRandomNumber(3,1))}`,`${types[getRandomNumber(2,0)]}`,`${logicNames[getRandomNumber(2,0)]}`];
        let tr = document.createElement("tr");
        for(let j=0 ; j<ths.length ; j++){
            if(i==0){
                let th = document.createElement("th");
                th.innerHTML = ths[j];
                tr.appendChild(th);
            }else{
                let td = document.createElement("td");
                td.innerHTML = tds == ""?`SAMPLE`:historyTds[j];
                td.setAttribute("mouseenter",function(e){
                    e.target.parentNode.classList.add("over")
                })
                tr.appendChild(td);
                if(DOM.id != "historyTable"){
                    tr.addEventListener("click", function (e) {
                        if (e.target.tagName.toLowerCase() == "td") {
                            let timer = setTimeout(function () {
                                location.href = "Detail.aspx";
                            }, 300)
                        }
                    });
                }
            }
        }
        if(i==0){
            thead.appendChild(tr);
        }else{
            tbody.appendChild(tr);
        }
    }
    DOM.appendChild(thead);
    DOM.appendChild(tbody);
}

function getRandomNumber(val,min){
    let value = Math.floor(Math.random()*val)+min;
    return value;
}
function addZero(num){
    let newNum = num<10 ? `0${num}` : num;
    return newNum;
}
//ツールチップ表示用メソッド
/*function showTooltip() {
    if (popup != null) {
        popup.style.display = "block";
    } else if (clientPagePopup != null) {
        clientPagePopup.style.display = "block";
    } else {}
}
function hideTooltip() {
    if(popup != null){
        popup.style.display = "none";
    } else if (clientPagePopup != null) {
        clientPagePopup.style.display = "none";
    }else{}
}*/


/*
function showSMALLtooltip(e){
    const Yoffset = 110;
    const Xoffset = 220;
    const clientRect = e.target.getBoundingClientRect();
    let top = Math.floor(window.pageYOffset + clientRect.top)-Yoffset;
    let left = Math.floor(window.pageXOffset + clientRect.left)-Xoffset;
    SNpopup.style.position = "absolute";
    SNpopup.style.top = `${top}px`;
    SNpopup.style.left = `${left}px`;
    SNpopup.style.display = "block";
}*/