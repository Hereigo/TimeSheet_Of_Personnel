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

                // WARNING MAGIC NUMBER !!!!!!
                // WARNING MAGIC NUMBER !!!!!!
                // WARNING MAGIC NUMBER !!!!!!

                if (j > 4 && j < 36) {
                    // FILLING MONTH TABLE WITH DEFAULT 8 HOURS VALUES :
                    var cellsContent = $.trim(row.cells[j].innerHTML);

                    if (cellsContent.length === 0 || cellsContent === '') {
                        row.cells[j].style.backgroundColor = "#BAF5CB";
                    }
                    else if (cellsContent == '8') {
                        row.cells[j].style.color = "#adadad";

                    } else if (cellsContent == 'нз') {
                        row.cells[j].style.color = "red";
                    }
                    else if (cellsContent == 'до') {
                        row.cells[j].style.backgroundColor = "#B2CDEB";
                    }


                }
            }
        }
    }

});