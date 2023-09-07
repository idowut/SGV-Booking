document.addEventListener('DOMContentLoaded', function () {
    const bamboo = document.getElementById('bamboo');
    const oeste = document.getElementById('oeste');
    const mexikana = document.getElementById('mexikana');

    bamboo.addEventListener('mouseover', function () {
        bamboo.style.transform = 'scale(1.1)';
    });

    bamboo.addEventListener('mouseout', function () {
        bamboo.style.transform = 'scale(1)';
    });

    oeste.addEventListener('mouseover', function () {
        oeste.style.transform = 'scale(1.1)';
    });

    oeste.addEventListener('mouseout', function () {
        oeste.style.transform = 'scale(1)';
    });

    mexikana.addEventListener('mouseover', function () {
        mexikana.style.transform = 'scale(1.1)';
    });

    mexikana.addEventListener('mouseout', function () {
        mexikana.style.transform = 'scale(1)';
    });
});
