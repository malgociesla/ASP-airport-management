function getToday() {
    return formatDate(Date.now());
}

function getTomorrow(date) {
    return formatDate(moment(date).add(24, 'hours'));
}

function formatDateTime(date) {
    return moment(date).format('L LTS');
}

function formatDate(date) {
    return moment(date).format("L");
}

function toJSONDate(date) {
    return moment(date).toJSON();
}