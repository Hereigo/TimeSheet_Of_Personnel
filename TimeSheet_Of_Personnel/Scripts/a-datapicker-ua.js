$(function () {
    $("input[type='date']")
    .datepicker({ dateFormat: 'dd/mm/yy' })
    .get(0).setAttribute("type", "text");
    $.datepicker.regional['ua'] = {
        prevText: 'Попер.',
        nextText: 'Наст.',
        monthNames: ['Січень', 'Лютий', 'Березень', 'Квітень', 'Травень', 'Червень',
        'Липень', 'Серпень', 'Вересень', 'Жовтень', 'Листопад', 'Грудень'],
        monthNamesShort: ['Січ', 'Лют', 'Бер', 'Кві', 'Тра', 'Чер', 'Лип', 'Сер', 'Вер', 'Жов', 'Лис', 'Гру'],
        dayNames: ['неділя', 'понеделілок', 'вівторок', 'середа', 'четвер', 'пятниця', 'субота'],
        dayNamesShort: ['нед', 'пон', 'вів', 'сер', 'чтв', 'птн', 'сбт'],
        dayNamesMin: ['Нд', 'Пн', 'Вт', 'Ср', 'Чт', 'Пт', 'Сб'],
        weekHeader: 'Не',
        dateFormat: 'dd/mm/yy',
        firstDay: 1,
        isRTL: false,
        showMonthAfterYear: false,
        yearSuffix: ''
    };
    $.datepicker.setDefaults($.datepicker.regional['ua']);
    //$.validator.addMethod('date',
    //function (value, element) {
    //    var ok = true;
    //    try {
    //        $.datepicker.parseDate('dd/mm/yy', value);
    //    }
    //    catch (err) {
    //        ok = false;
    //    }
    //    return ok;
    //});
});