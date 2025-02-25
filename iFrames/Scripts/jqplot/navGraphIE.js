$.jqplot.config.enablePlugins = true;
var plot;
var firstTime = true;
var oldDate = '';

$('#chartOptions').change(function () {
    var day = -1 * parseInt($('#chartOptions').val());
    PullChart(Date.parse($('#selectedDate').val()).toString("dd MMM yyyy"), Date.parse($('#selectedDate').val()).add(day).days().toString("dd MMM yyyy"));
});

$(function () {
    $('.icon-calendar').tooltipster({ position: 'top' });
    $('.icon-forward').tooltipster({ position: 'bottom' });

    $('#navForm').submit(function () {
        $('#iconCalender').datepicker('hide');
        var regex = new RegExp("^[0-9]{2} (Jan|Feb|Mar|Apr|May|Jun|Jul|Aug|Sep|Oct|Nov|Dec) [0-9]{4}$", "i");
        if (!$('#selectedDate').val().match(regex)) {
            alert('Please provide the date in dd MMM yyyy format, e.g. 12 Jan 2012');
            return false;
        }
        if (Date.parse($('#selectedDate').val(), "dd MMM yyyy") == null) {
            alert('Please provide the date in dd MMM yyyy format, e.g. 12 Jan 2012');
            return false;
        }
        if (!dtvalid(Date.parse($('#selectedDate').val(), "dd MMM yyyy").toString("dd/MM/yyyy"))) {
            alert('Please provide the date in dd MMM yyyy format, e.g. 12 Jan 2012');
            return false;
        }
        if (!firstTime) {
            var height = $('#nav').outerHeight();
            $('#nav').height(height);
        }
        firstTime = false;
        $('#nav').html('');
        $('#loadingDiv').show();
        var data_save = $(this).serializeArray()
        data_save.push({ name: "schemeId", value: $('#SchemeId').val() });
        $.post($(this).attr('action'), data_save, function (json) {
            var r = new Array(), j = -1;
            r[++j] = '<table class="table table-bordered table-condensed table-hover" style="font-size: 0.9em;">';
            var jsonData = eval(json);
            for (var key = 0, size = eval(json).length; key < size; key++) {
                r[++j] = '<tr><td style="text-align: left; vertical-align: middle;"><input checked="checked" class="pull-left" style="visibility:hidden;" type="checkbox" value="' + jsonData[key].Id + '|' + jsonData[key].IsIndex + '"></input>&nbsp;' + jsonData[key].Name + '</td>';
                for (var subkey = 0, subsize = jsonData[key].ValueAndDate.length; subkey < subsize; subkey++) {
                    r[++j] = '<td style="text-align: center; vertical-align: middle;">' + jsonData[key].ValueAndDate[subkey].Value + '<hr>(' +
                    Date.parse(jsonData[key].ValueAndDate[subkey].Date).toString("dd/MM/yyyy")
                        + ')</td>';
                }
                r[++j] = '</tr>';
            }
            r[++j] = '</table>';
            $('#nav').html(r.join(''));
            $('div#nav input[type=checkbox]').change(function (event) {
                if ($('div#nav input:checked').length == 0) {
                    alert('Can not remove the last series from chart');
                    this.checked = true;
                    return;
                }
                var scheme = $(this).closest('td').text();
                if (typeof (plot) != 'undefined') {
                    for (var i = 0; i < plot.series.length; i += 1) {
                        if (plot.series[i].label.trim() == scheme.trim()) {
                            plot.series[i].show = this.checked;
                            plot.replot();
                            return;
                        }
                    }
                }
            });
            $('#loadingDiv').hide();
            var day = -1 * parseInt($('#chartOptions').val());
            if (eval(json).length != 0) {
                $('#chartContainer').show();
                $('#info').show();
                PullChart(Date.parse($('#selectedDate').val()).toString("dd MMM yyyy"), Date.parse($('#selectedDate').val()).add(day).days().toString("dd MMM yyyy"));
            }
            else {
                $('#nav').html('<p>NAV not available for the selected date period</p>')
                $('#chartContainer').hide();
                $('#info').hide();
            }
        }, 'json');

        return false;
    });

    $('#navForm').submit();
});

function Plot(dataConsolidated) {
    var max = dataConsolidated.MaxDate;
    var min = dataConsolidated.MinDate;
    var data = dataConsolidated.SimpleNavReturnModel;
    var seriesNames = Array();
    var dataPlot = [[[]]];
    for (var i = 0; i < data.length; i += 1) {
        seriesNames.push(data[i].Name);
        var points = [];
        for (var j = 0; j < data[i].ValueAndDate.length; j += 1) {
            points.push([data[i].ValueAndDate[j].Date, data[i].ValueAndDate[j].Value]);
        }
        dataPlot.push(points);
    }
    dataPlot.shift();
    $('#chart').remove();
    if (data.length < 1) {
        $('#chartContainer').append('<div style="width: 100%; height:100%; text-align: center; padding-top: 10%;" id="chart">Data not Available for the selected date range</div>');
        $('#chartContainer').effect("highlight", {}, 3000);
        return;
    }
    $('#chartContainer').append('<div style="width: 97%; height:100%;" id="chart" ></div>');
    var plot2 = $.jqplot('chart', dataPlot,
        {
            seriesColors: ["#4bb2c5", "#c5b47f", "#EAA228", "#579575", "#839557", "#958c12", "#953579", "#4b5de4", "#d8b83f", "#ff5800", "#0085cc"],
            animate: true,
            animateReplot: true,
            axes: {
                xaxis: {
                    min: min,
                    max: max,
                    renderer: $.jqplot.DateAxisRenderer,
                    rendererOptions: { tickRenderer: $.jqplot.CanvasAxisTickRenderer },
                    //tickInterval: '7 day',
                    tickOptions: { formatString: '%b %#d, %y' }
                    //tickOptions: { formatString: '%#d/%#m/%Y' }
                },
                yaxis:
                    {
                        label: 'Value [Rebased]',
                        tickOptions: { formatString: '%.2f' },
                        labelRenderer: $.jqplot.CanvasAxisLabelRenderer
                    }
            },
            seriesDefaults: { showMarker: false, lineWidth: 2, rendererOptions: { animation: { speed: 1000 } } },
            highlighter: { show: true, sizeAdjust: 7.5 },
            cursor: { show: true, zoom: true, showTooltip: false },
            legend: {
                renderer: $.jqplot.EnhancedLegendRenderer,
                show: true,
                location: 's',
                rendererOptions: {
                    numberRows: 4,
                    numberColumns: 3
                },
                placement: 'outsideGrid',
                seriesToggle: 'on',
                fontSize: '1em',
                border: '0px solid black'
            },
            grid: {
                shadow: false,
                borderWidth: 0,
                background: 'rgba(0,0,0,0)'
            }
        });
    for (var i = 0; i < seriesNames.length; i += 1) {
        plot2.series[i].label = seriesNames[i];
    }
    plot2.replot();
    $('#info').removeAttr("style");
}

function PullChart(tdt, fdt) {
    var val = [];
    $('div#nav input:checked').each(function (i) {
        val[i] = $(this).val();
    });
    var dataToPush = $('#navForm').serializeArray();
    dataToPush.push({ name: 'selection', value: val.join() });
    dataToPush.push({ name: 'fromDate', value: fdt });
    dataToPush.push({ name: 'toDate', value: tdt });
    $('#loadingDiv').show();
    $.post('/MF/Scheme/NavHistory', dataToPush, function (dataConsolidated, textStatus) {
        Plot(dataConsolidated);
        $('#loadingDiv').hide();
    }, "json");
}

$('#iconCalender').datepicker().on('changeDate', function (ev) {
    $('#selectedDate').val(Date.parse($('#iconCalender').data('date'), "MM/dd/yyyy").toString("dd MMM yyyy"));
    //oldDate = $('#selectedDate').val();
    $('#navForm').submit();
});

$('.icon-forward').click(function () {
    //oldDate = $('#selectedDate').val();
    $('#navForm').submit();
});