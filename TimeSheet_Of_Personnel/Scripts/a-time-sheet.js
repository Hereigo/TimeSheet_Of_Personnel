$(document).ready(function () {

    var h = document.getElementsByTagName("h3");

    h[0].style.color = "blue";

    // CELL VALUES COUNTERS :
    var table = document.getElementById("monthViewTable");

    // FOREACH ALL CELLS IN TABLE :
    for (var i = 0, row; row = table.rows[i]; i++) {

        // SKIP HEADER ROW :
        if (i > 0) {

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
                }
            }
        }
    }

});