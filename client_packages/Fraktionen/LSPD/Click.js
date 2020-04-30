$('#AcceptBtn').click(() => {
    $('.alert').remove();
    mp.trigger('SearchTheNumberplate', $('#InputEndziffer').val());
});

$('#AcceptBtn2').click(() => {
    $('.alert').remove();
    mp.trigger('SearchThisPeople', $('#fname').val(), $('#lname').val());
});

$('#CloseBtn').click(() => {
    $('.alert').remove();
    mp.trigger("closelspdcomputer");
});

$('#CloseBtn2').click(() => {
    $('.alert').remove();
    mp.trigger("closelspdcomputer");
});