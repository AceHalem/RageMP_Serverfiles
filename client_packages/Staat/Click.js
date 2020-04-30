$('#AcceptBtn').click(() => {
    $('.alert').remove();
    mp.trigger('CreateNewNumberplate', $('#InputEndziffer').val());
});