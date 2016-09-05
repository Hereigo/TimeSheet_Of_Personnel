$(document).ready(function () {

    // RESET BOOTSTRAP TO CENTRALIZE THE CALENDAR TABLE :
    $(window).resize(function () {
        var $c = $('.container'),
            $w = $('.well'),
            totalWidth = $('.navbar').outerWidth(),
            wellWidth = $c.outerWidth(),
            diff = totalWidth - wellWidth,
            marg = -Math.floor(diff / 2) + 'px';
        $w.each(function () {
            $(this).css({
                'margin-left': marg,
                'margin-right': marg
            });
        });
    });
    $(window).resize();


    // STRETCH VERTICAL HEADER TH-CELLS DEPENDS ON ITS CONTENT:
    var header_height = 0;
    $('table th span').each(function () {
        if ($(this).outerWidth() > header_height) header_height = $(this).outerWidth();
    });

    $('table th').height(header_height);
    // DO NOT WORK !!!!!!!!!
    // DO NOT WORK !!!!!!!!!
    // DO NOT WORK !!!!!!!!!




    // CELL VALUES COUNTERS :
    var table = document.getElementById("monthViewTable");

    // FOREACH ALL CELLS IN TABLE :
    for (var i = 0, row; row = table.rows[i]; i++) {

        // SKIP HEADER ROW :
        if (i > 0) {
            var hoursCount = 0;
            var daysCount = 0;
            var vacations = 0;
            var workTrips = 0;
            var timeOffDays = 0;
            var unknownReason = 0;
            var skillsImprove = 0;
            var hospitalDay = 0;
            var weekends = 0;

            for (var j = 0, col; col = row.cells[j]; j++) {
                // ROWS COUNTER :
                if (j < 1) { row.cells[0].innerHTML = i; }
                // Columns : 0=Counter, 1=EmployeeName, 2=EmployPosition, 3=IsAWoman, 4=TimesheetNum
                if (j > 4 && j < 36) {
                    // FILLING MONTH TABLE WITH DEFAULT 8 HOURS VALUES :
                    if ($.trim(row.cells[j].innerHTML) == '8') {
                        row.cells[j].style.color = "#adadad";
                        //row.cells[j].innerHTML = 8;
                    }
                    else if ($.trim(row.cells[j].innerHTML).toLowerCase() == 'в') { vacations += 1; }
                    else if ($.trim(row.cells[j].innerHTML).toLowerCase() == 'ч') { vacations += 1; }
                    else if ($.trim(row.cells[j].innerHTML).toLowerCase() == 'бз') { vacations += 1; }
                    else if ($.trim(row.cells[j].innerHTML).toLowerCase() == 'дд') { vacations += 1; }

                    else if ($.trim(row.cells[j].innerHTML).toLowerCase() == 'вд') { workTrips += 1; }
                    else if ($.trim(row.cells[j].innerHTML).toLowerCase() == 'дв') { timeOffDays += 1; }
                    else if ($.trim(row.cells[j].innerHTML).toLowerCase() == 'нз') { unknownReason += 1; }
                    else if ($.trim(row.cells[j].innerHTML).toLowerCase() == 'с') { skillsImprove += 1; } // Семінар
                    else if ($.trim(row.cells[j].innerHTML).toLowerCase() == 'тн') { hospitalDay += 1; } // Тимчасова непрацездатність
                }
            }

            for (var k = 0, col; col = row.cells[k]; k++) {
                // Columns : 0=Counter, 1=EmployeeName, 2=EmployPosition, 3=IsAWoman, 4=TimesheetNum
                // UNTIL SUMMARY-COLUMNS Not Started:
                if (k > 4 && k < 36) {
                    var currCell = $.trim(parseInt(row.cells[k].innerHTML));
                    if (currCell > 0) {
                        hoursCount += parseInt(currCell);
                        daysCount += 1;
                    }
                }
            }

            // WRITE COUNT RESULTS IN THE LAST COLUMNS :
            $(row.cells[36].innerHTML = hoursCount);
            $(row.cells[37].innerHTML = daysCount);
            $(row.cells[38].innerHTML = vacations);
            $(row.cells[39].innerHTML = workTrips);
            $(row.cells[40].innerHTML = timeOffDays);
            $(row.cells[41].innerHTML = unknownReason);
            $(row.cells[42].innerHTML = skillsImprove);
            $(row.cells[43].innerHTML = hospitalDay);
            $(row.cells[44].innerHTML = weekends);
        }
    }
});