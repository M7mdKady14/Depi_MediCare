document.addEventListener("DOMContentLoaded", function () {
    const printBtn = document.getElementById("printBtn");
    printBtn.addEventListener("click", function () {
        window.print();
    });
});