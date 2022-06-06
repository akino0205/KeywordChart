function SetPieChart(title, chartdata) {
    Highcharts.chart('pie_container', {
        chart: {
            plotBackgroundColor: null,
            plotBorderWidth: null,
            plotShadow: false,
            type: 'pie'
        },
        title: {
            text: title
        },
        tooltip: {
            pointFormat: '<b>{point.percentage:.1f}%</b>'
        },
        plotOptions: {
            pie: {
                size: 180,
                allowPointSelect: true,
                cursor: 'pointer',
                dataLabels: {
                    enabled: true,
                    format: '<b>{point.name}</b> ({point.percentage:.1f}%)'
                }
            }
        },
        series: [{
            colorByPoint: true,
            data: chartdata
        }]
    });
}

function SetStackedColumnChart(title, categories, chartdata) {
    Highcharts.chart('column_container', {
        chart: {
            type: 'column'
        },
        title: {
            text: title
        },
        xAxis: {
            categories: categories
        },
        yAxis: {
            min: 0,
            title: {
                text: '월간검색수'
            }
        },
        legend: {
            align: 'right',
            x: -30,
            verticalAlign: 'top',
            y: 25,
            floating: true,
            backgroundColor:
                Highcharts.defaultOptions.legend.backgroundColor || 'white',
            borderColor: '#CCC',
            borderWidth: 1,
            shadow: false
        },
        tooltip: {
            formatter: function () {
                var regexp = /\B(?=(\d{3})+(?!\d))/g;
                var new_y = this.y.toString().replace(regexp, ',');
                var new_stackTotal = this.point.stackTotal.toString().replace(regexp, ',');
                return this.series.name + ': ' + new_y + '<br/>' +
                    'Total: ' + new_stackTotal;
            }
        },
        plotOptions: {
            column: {
                size: 180,
                stacking: 'normal'
            }
        },
        series: chartdata
    });
}




