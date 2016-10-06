$(document).ready(function () {

    var h = document.getElementsByTagName("h3");

    h[0].style.color = "blue";

    // CELL VALUES COUNTERS :
    var table = document.getElementById("monthViewTable");

    // TODO:
    // REFACTORE ME !
    // REFACTORE ME !
    // REFACTORE ME !
    var lastRow = table.rows.length - 1;

    // FOREACH ALL CELLS IN TABLE :
    for (var i = 0, row; row = table.rows[i]; i++) {

        if (i == lastRow) {
            for (var j = 0, col; col = row.cells[j]; j++) {

                // TODO:
                // REFACTORE ME !
                // REFACTORE ME !
                // REFACTORE ME !
                if (j < 4 || j == 6) {
                    row.cells[j].innerHTML = "";
                }
            }
        }
        // SKIP HEADER ROW :
        else if (i > 0) {

            for (var j = 0, col; col = row.cells[j]; j++) {
                // ROWS COUNTER :
                if (j < 1) { row.cells[0].innerHTML = i; }
                // Columns : 0=Counter, 1=EmployeeName, 2=EmployPosition, 3=IsAWoman, 4=TimesheetNum

                // WARNING MAGIC NUMBER !!!!!!
                // WARNING MAGIC NUMBER !!!!!!
                // WARNING MAGIC NUMBER !!!!!!
                // WARNING MAGIC NUMBER !!!!!!
                // WARNING MAGIC NUMBER !!!!!!
                // WARNING MAGIC NUMBER !!!!!!
                // WARNING MAGIC NUMBER !!!!!!

                if (j > 6 && j < 38) {

                    // FILLING MONTH TABLE WITH DEFAULT 8 HOURS VALUES :
                    var cellsContent = $.trim(row.cells[j].innerHTML);

                    if (cellsContent.length === 0 || cellsContent === '') {
                        row.cells[j].style.backgroundColor = "#BAF5CB";
                    }
                    else if (cellsContent == '8') {
                        row.cells[j].style.color = "#adadad";
                    }
                    else if (cellsContent == '7') {
                        row.cells[j].style.fontWeight = "bold";
                    }
                    else if (cellsContent == '6') {
                        row.cells[j].style.fontWeight = "bold";
                    }
                    else if (cellsContent == '5') {
                        row.cells[j].style.fontWeight = "bold";
                    }
                    else if (cellsContent == '-') {
                    }
                    else if (cellsContent == 'по') {
                    }
                    else if (cellsContent == 'нз') {
                        row.cells[j].style.color = "red";
                        row.cells[j].style.backgroundColor = "yellow";
                    }
                    else {
                        row.cells[j].style.backgroundColor = "#A7C0D4";
                    }
                }
            }
        }
    }

});