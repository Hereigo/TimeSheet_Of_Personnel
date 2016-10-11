function showForm() {
    document.getElementById('formBox').style.display = 'block';
}

function hideForm() {
    document.getElementById('formBox').style.display = 'none';
}

function checkForm() {
    var dateField = document.getElementById('shortDayDate').value;
    var weekDays = ["Неділя", "Понеділок", "Вівторок", "Середа", "Четвер", "Пятниця", "Субота"];
    if (isValidDate(dateField)) {
        var bits = dateField.split('/');
        var formatedDate = bits[2] + '/' + bits[1] + '/' + bits[0];
        var date = new Date(formatedDate);
        if (confirm("Встановити скорочений день " + dateField + " (" + weekDays[date.getDay()] + ")?")) {
            document.getElementById('postForm').submit();
        }
    } else {
        alert("Не коректний формат дати!");
    }
}

function isValidDate(s) {
    var bits = s.split('/');
    var y = bits[2], m = bits[1], d = bits[0];
    // Assume not leap year by default (note zero index for Jan)
    var daysInMonth = [31, 28, 31, 30, 31, 30, 31, 31, 30, 31, 30, 31];
    // If evenly divisible by 4 and not evenly divisible by 100,
    // or is evenly divisible by 400, then a leap year
    if ((!(y % 4) && y % 100) || !(y % 400)) {
        daysInMonth[1] = 29;
    }
    return d <= daysInMonth[--m]
}