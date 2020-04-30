$('#autoButton').click(() => {
    $('.alert').remove();
    mp.trigger('autoButton_check');
});
