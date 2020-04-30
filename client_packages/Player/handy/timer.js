
Date.prototype.timeNow = function() {
    return ((this.getHours() < 10) ? "0" : "") + this.getHours() + ":" + ((this.getMinutes() < 10) ? "0" : "") + this.getMinutes() + ":" + ((this.getSeconds() < 10) ? "0" : "") + this.getSeconds();
}
function setTime() {
    var currentdate = new Date();
    $("#time").text(currentdate.timeNow());
}


$(document).ready(function() {
    setInterval(setTime, 1000)
});