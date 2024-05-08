function drawTimeChart(divid, gData, timeStr, h, w) {
    const margin = {
        top: 30,
        right: 50,
        left: 100,
        bottom: 20
    };

    const labels = d3.nest()
        .key(function (d) {
            return d.key;
        }).entries(gData);


    // svg
    const svg = d3.select(divid)
        .append("svg")
        .attr("width", w)
        .attr("height", h);

    var eDate = new Date(timeStr);
    eDate.setDate(eDate.getDate() + 1);

    // xAxis
    var x = d3.time.scale().domain([
        timeStr, eDate
    ])
        .clamp(true)
        .range([0, w - (margin.left + margin.right)]);

    var xAxis = d3.svg.axis()
        .scale(x)
        .orient('top')
        .ticks(d3.time.hours, 1)
        .tickFormat(d3.time.format('%_H:00'))
        .innerTickSize((h - (margin.top + margin.bottom)))
        .outerTickSize(1);

    svg.append('g')
        .attr("class", "axis x-axis")
        .attr("transform", "translate(" + margin.left + ", " + (h - margin.top - 10) + ")")
        .call(xAxis)
        .selectAll('text')
        .attr('transform', function () {
            if (h == 150) {
                return 'translate(70, 94) rotate(-45)';		// monitor
            }
            return 'translate(35, 58) rotate(-45)';			// work history
        });

    // yAxis
    var y = d3.scale.ordinal()
        .domain(labels.map(function (d) {
            return d.key;
        }))
        .rangeRoundBands([margin.top, h - margin.bottom], 0.5);

    var yAxis = d3.svg.axis()
        .scale(y)
        .orient('left')
        .innerTickSize(0)
        .outerTickSize(0);

    svg.append('g')
        .attr("class", "axis y-axis")
        .attr("transform", "translate(" + margin.left + ", 0)")
        .attr({
            class: 'axis y-axis',
            transform: 'translate(' + margin.left + ', 0)'
        })
        .call(yAxis)
        .selectAll('text')
        .each(function (d, i) {
            var el = d3.select(this);
            el.remove();
        });

    // Bar
    svg.selectAll('.active')
        .data(gData)
        .enter()
        .append('rect')
        .attr({
            x: function (d) {
                return x(d.start) + margin.left + 1;
            },
            y: function (d, i) {
                return y(d.key) - 20;
            },
            width: 0,
            height: 20,
            fill: function (d) {
                if (d.flag == 'True') {
                    return '#d32f2f';
                }
                return '#d32f2f'; 
            },
            class: 'active',
            id: function (d) {
                //<svg>タグにidを追加
                return d.id;
            }
        })
        .transition()
        .attr({
            width: function (d) {
                if (d.start == 0) {
                    return 0;
                } else {
                    return Math.max(x(d.end) - x(d.start) - 1, 1);
                }
            }
        });

    // グラフ内の文字
    svg.selectAll('text.hoge')
        .data(gData)
        .enter()
        .append('text')
        .text(function (d) { return d.lavel; })
        .attr({
            x: function (d) {
                return x(d.start) + margin.left + 3;
            },
            y: function (d, i) {
                return y(d.key) - 5;
            },
            fill: '#000000',
            class: 'rect-label'
        });
}
