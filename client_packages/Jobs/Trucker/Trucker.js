$('#trucklevel1').click(() => {

    $('.alert').remove();
    mp.trigger('TruckerLevelToClient1');
});

$('#trucklevel2').click(() => {

    $('.alert').remove();
    mp.trigger('TruckerLevelToClient2');
});

$('#trucklevel3').click(() => {

    $('.alert').remove();
    mp.trigger('TruckerLevelToClient3');
});


$('#CloseButton2').click(() => {

    $('.alert').remove();
    mp.trigger('DeleteTruckerBrowser');
});


