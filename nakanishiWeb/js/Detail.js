let lineChart;

/*ここから全てfunction*/
/*function drawChart(label,data,canvas){
    if(lineChart != null){
        lineChart.destroy();
    }
    let day = someday==null ? new Date() : someday;
    lineChart = new Chart(canvas,{
        type:"line",
        data:{
            labels:label,
            datasets:[{
                label:`${makeDateString(day)}`,
                data:data,
                borderColor:"#cf0000",
            }],
        },
        options:{
            responsive:false,
            plugins:{
                title:{
                    position:"top",
                    display: true,
                    text:`${makeDateString(day)}のアラート発生状況`,
                    font:{
                        size:16
                    }
                }
            },
            scales:{
                y:{
                    min:0,
                    max:5,
                    ticks:{
                        stepSize:1
                    }
                }
            }
        }
    })
}*/
