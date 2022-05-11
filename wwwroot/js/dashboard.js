
$(function () {
    //run following functions when load the page 
    LoadPieChart();
    LoadBarChart();
})




function LoadPieChart() {
    var PieChart;
    if (PieChart != null && PieChart != "" && PieChart != undefined) {
        PieChart.dispose();
    }
    PieChart = echarts.init(document.getElementById('pie-chart'));
    fetch("/api/piechart").then(res=>res.json()).then(res => {
        console.log(res)
        var colors = res.map(c => c.color);
     
        option = {
        
            tooltip: {
                trigger: 'item',
                formatter: '{a} <br/>{b} : {c} ({d}%)'
            },
            legend: {
                orient: 'vertical',
                left: 'left',
            },
            series: [
                {
                    name: 'Job Ad Status',
                    type: 'pie',
                    radius: '70%',
                    data: res,
                    emphasis: {
                        itemStyle: {
                            shadowBlur: 10,
                            shadowOffsetX: 0,
                            shadowColor: 'rgba(0, 0, 0, 0.5)'
                        }
                    }
                }
            ],
            color: res.map(c => c.color)
        };
        PieChart.setOption(option);
        //regenerate chart when resize the browser
        $(window).on('resize', function () {
            PieChart.resize();

        });
    })

}
function LoadBarChart() {
    var BarChart;
    if (BarChart != null && BarChart != "" && BarChart != undefined) {
        BarChart.dispose();
    }
    BarChart = echarts.init(document.getElementById('bar-chart'));
    fetch("/api/category").then(res=>res.json()).then(res => {
      

        option = {
           
            tooltip: {
                confine: true,
                trigger: "axis",
                axisPointer: {
                    type: 'shadow'
                },
                formatter: function (p) {
                    console.log(p)
                    var item = p[0];
                    var index = item.dataIndex;
                    var res = `${item["data"]["name"]}:${item["data"]["value"]}  ` ;
                    return res;
                    //var res=
                }
            },
            grid: {

                x: 30,
                y: 80,
                x2: 30,
                show: false

            },
            yAxis: {
                type: 'category',
          
                triggerEvent: true,
                axisLabel: {
                    interval: 0,
                    margin: 5,
                    color: "#777777",
                    fontWeight: 'bold',
                    fontSize: 12,
                },
                inverse: true,
                show:false,
                position: 'top',

            },
            xAxis: {
                position: 'top',
                type: 'value',
                show: true,
           
            },
            series: {
                data: res,
                barWidth: "20",
                type: 'bar',
                itemStyle: {
                    normal: {
                        color: 'rgba(43,137,204, 0.8)',
                        barBorderColor: '#2b89cc'
                    }
                },
                label: {
                    normal: {
                        position: 'insideLeft',
                        align: "left",
                        color: 'black',
                        offset: [0, -25],
                        fontSize: 12,
                        show: true,
                        formatter: function (p) {
                            return p.name;
                        }
                    }
                }
            }
        };
        BarChart.setOption(option);
        //regenerate chart when resize the browser
        $(window).on('resize', function () {
            BarChart.resize();

        });
    })

}
   