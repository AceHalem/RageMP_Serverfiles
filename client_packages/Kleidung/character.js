$(document).ready(function () {
    characterUI(1);
    $('#optionOberteile').click(() => {
        characterUI(1);
    });
    $('#optionHose').click(() => {
        characterUI(2);
    });
    $('#optionSchuhe').click(() => {
        characterUI(3);
    });
    $('#rotateRight').click(() => {

    });
    $('#rotateLeft').click(() => {

    });
});

function characterUI(page) {
    hideAll();
    switch (page) {
        case 1:
            $('#oberteileGUI').show();
            $('#optionOberteile').addClass("active");
            break;
        case 2:
            $('#hoseGUI').show();
            $('#optionHose').addClass("active");
            break;
        case 3:
            $('#schuheGUI').show();
            $('#optionSchuhe').addClass("active");
            break;
    }
}

function hideAll() {
    $('#oberteileGUI').hide();
    $('#hoseGUI').hide();
    $('#schuheGUI').hide();
    $('#optionOberteile').removeClass("active");
    $('#optionHose').removeClass("active");
    $('#optionSchuhe').removeClass("active");
}

$('#formAbbrechen').click(() => {
    $('.alert').remove();
    mp.trigger('KleidungAbbrechen');
});

$('#Hose1').click(() => {
    $('.alert').remove();
    mp.trigger('Hose1');
});

$('#Hose2').click(() => {
    $('.alert').remove();
    mp.trigger('Hose2');
});

$('#Hose3').click(() => {
    $('.alert').remove();
    mp.trigger('Hose3');
});




$('#Oberteil1').click(() => {
    $('.alert').remove();
    mp.trigger('Oberteil1');
});

$('#Oberteil2').click(() => {
    $('.alert').remove();
    mp.trigger('Oberteil2');
});

$('#Oberteil3').click(() => {
    $('.alert').remove();
    mp.trigger('Oberteil3');
});




$('#Schuhe1').click(() => {
    $('.alert').remove();
    mp.trigger('Schuhe1');
});

$('#Schuhe2').click(() => {
    $('.alert').remove();
    mp.trigger('Schuhe2');
});

$('#Schuhe3').click(() => {
    $('.alert').remove();
    mp.trigger('Schuhe3');
});

$('#formKaufen').click(() => {
    $('.alert').remove();
    mp.trigger('KleidungKaufen');
});