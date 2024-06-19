

    document.getElementById('buyButton').addEventListener('click', function () {
        document.getElementById('overlay').style.display = 'block';
    document.getElementById('popup').style.display = 'block';
        });

    document.getElementById('confirmButton').addEventListener('click', function () {
        document.getElementById('orderForm').submit();
    document.getElementById('overlay').style.display = 'none';
    document.getElementById('popup').style.display = 'none';
    document.getElementById('successPage').style.display = 'block';
        });

    document.getElementById('cancelButton').addEventListener('click', function () {
        document.getElementById('overlay').style.display = 'none';
    document.getElementById('popup').style.display = 'none';
        });

    document.getElementById('okButton').addEventListener('click', function () {
        document.getElementById('successPage').style.display = 'none';
        });

