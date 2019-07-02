$(document).ready(function () {
    var elems = document.querySelectorAll('.sidenav');
    var options = { edge: 'right', draggable: true, closeOnClick: true };
    var instance = M.Sidenav.init(elems, options);
    $('.modal').modal();
    $('select').formSelect();
});
//function ShowMessage(message) {

//    $("#message").text(message);
//    $('#modalmessage').modal('open');
//}

function Close() {
    $('.sidenav').sidenav('close');
}

function LoadingStart() {
    $('#loader').show();
    $('#modalLoader').modal('open');
}

function LoadingEnd() {
    $('#loader').hide();
    $('#modalLoader').modal('close');
}
