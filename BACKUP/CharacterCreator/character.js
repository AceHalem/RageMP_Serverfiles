$(document).ready(function () {
    characterUI(1);
    $('#optionGeburtsurkunde').click(() => {
        characterUI(1);
    });
    $('#optionEltern').click(() => {
        characterUI(2);
    });
    $('#optionAussehen').click(() => {
        characterUI(3);
    });
    $('#optionGesicht').click(() => {
        characterUI(4);
    });
    $('#optionBehaarung').click(() => {
        characterUI(5);
    });
    $('#optionMakeup').click(() => {
        characterUI(6);
    });
    $('#rotateRight').click(() => {

    });
    $('#rotateLeft').click(() => {

    });
    $('#formElternRandomize').click(() => {

    });
});

function characterUI(page) {
    hideAll();
    switch (page) {
        case 1:
            $('#geburtsurkundenGUI').show();
            $('#optionGeburtsurkunde').addClass("active");
            break;
        case 2:
            $('#elternGUI').show();
            $('#optionEltern').addClass("active");
            break;
        case 3:
            $('#aussehenGUI').show();
            $('#optionAussehen').addClass("active");
            break;
        case 4:
            $('#gesichtGUI').show();
            $('#optionGesicht').addClass("active");
            break;
        case 5:
            $('#behaarungGUI').show();
            $('#optionBehaarung').addClass("active");
            break;
        case 6:
            $('#makeupGUI').show();
            $('#optionMakeup').addClass("active");
            break;
    }
}

function hideAll() {
    $('#geburtsurkundenGUI').hide();
    $('#elternGUI').hide();
    $('#aussehenGUI').hide();
    $('#gesichtGUI').hide();
    $('#behaarungGUI').hide();
    $('#makeupGUI').hide();
    $('#optionGeburtsurkunde').removeClass("active");
    $('#optionEltern').removeClass("active");
    $('#optionAussehen').removeClass("active");
    $('#optionGesicht').removeClass("active");
    $('#optionBehaarung').removeClass("active");
    $('#optionMakeup').removeClass("active");
}

function getCount(elementId) {
    var element = document.getElementById(elementId);
    return element.innerHTML;
}

var slider1 = document.getElementById("formAehnlichkeit");
var output1 = document.getElementById("formAehnlichkeitCount");
var slider2 = document.getElementById("formHautton");
var output2 = document.getElementById("formHauttonCount");
var slider3 = document.getElementById("formSchönheitsfehler");
var output3 = document.getElementById("formSchönheitsfehlerCount");
var slider4 = document.getElementById("formGesichtsbehaarung");
var output4 = document.getElementById("formGesichtsbehaarungCount");
var slider5 = document.getElementById("formAugenbrauen");
var output5 = document.getElementById("formAugenbrauenCount");
var slider6 = document.getElementById("formAlterung");
var output6 = document.getElementById("formAlterungCount");
var slider7 = document.getElementById("formMakeup");
var output7 = document.getElementById("formMakeupCount");
var slider8 = document.getElementById("formRoetungen");
var output8 = document.getElementById("formRoetungenCount");
var slider9 = document.getElementById("formGesichtsfarbe");
var output9 = document.getElementById("formGesichtsfarbeCount");
var slider10 = document.getElementById("formSonnenbrand");
var output10 = document.getElementById("formSonnenbrandCount");
var slider11 = document.getElementById("formLippenstift");
var output11 = document.getElementById("formLippenstiftCount");
var slider12 = document.getElementById("formMuttermalSommersprossen");
var output12 = document.getElementById("formMuttermalSommersprossenCount");
var slider13 = document.getElementById("formBrustbehaarung");
var output13 = document.getElementById("formBrustbehaarungCount");

output1.innerHTML = slider1.value;
output2.innerHTML = slider2.value;
output3.innerHTML = slider3.value;
output4.innerHTML = slider4.value;
output5.innerHTML = slider5.value;
output6.innerHTML = slider6.value;
output7.innerHTML = slider7.value;
output8.innerHTML = slider8.value;
output9.innerHTML = slider9.value;
output10.innerHTML = slider10.value;
output11.innerHTML = slider11.value;
output12.innerHTML = slider12.value;
output13.innerHTML = slider13.value;


slider1.oninput = function () {
    output1.innerHTML = this.value;
};

slider2.oninput = function () {
    output2.innerHTML = this.value;
};

slider3.oninput = function () {
    output3.innerHTML = this.value;
};

slider4.oninput = function () {
    output4.innerHTML = this.value;
};

slider5.oninput = function () {
    output5.innerHTML = this.value;
};

slider6.oninput = function () {
    output6.innerHTML = this.value;
};

slider7.oninput = function () {
    output7.innerHTML = this.value;
};

slider8.oninput = function () {
    output8.innerHTML = this.value;
};

slider9.oninput = function () {
    output9.innerHTML = this.value;
};

slider10.oninput = function () {
    output10.innerHTML = this.value;
};

slider11.oninput = function () {
    output11.innerHTML = this.value;
};

slider12.oninput = function () {
    output12.innerHTML = this.value;
};

slider13.oninput = function () {
    output13.innerHTML = this.value;
};