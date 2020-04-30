$('#AcceptBtn').click(() => {
    $('.alert').remove();
    mp.trigger('CreateNewTicket', $('#TicketBetrag').val(), $('#TicketText').val());
});

$('#AcceptBtn2').click(() => {
    $('.alert').remove();
    mp.trigger('ReadNewTicket', $('#TicketBetrag').val());
});

$('#CancelBtn').click(() => {
    $('.alert').remove();
    mp.trigger('closeticket');
});