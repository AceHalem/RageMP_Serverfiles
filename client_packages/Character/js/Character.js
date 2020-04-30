$('#CharacterButton').click(() => {

    $('.alert').remove();
    mp.trigger('CharacterInformationToServer', $('#CharacterVorname').val(), $('#Characternachname').val());
});
