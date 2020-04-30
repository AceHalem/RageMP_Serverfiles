$('#loginButton').click(() => {
    $('.alert').remove();
   
    var username = $('#loginUsernameText').val();
    var password = $('#loginPasswordText').val();

    mp.trigger('uiLogin_LoginButton', username, password);
    //mp.events.callRemote('LoginAccount', username, password);
});

$('#registerButton').click(() => {
    $('.alert').remove();
    var username = $('#registerUsernameText').val();
    var password = $('#registerPasswordText').val();
    var password2 = $('#registerPasswordTextRe').val();

    if (password == password2) {
        mp.trigger('uiLogin_registerButton', username, password);
    } else {
        mp.trigger('Notify', '~r~Passwörter stimmen nicht überein');
    }
   // mp.events.callRemote('RegisterAccount', username, password);
});