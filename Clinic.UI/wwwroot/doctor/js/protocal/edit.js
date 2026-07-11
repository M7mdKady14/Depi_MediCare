document.addEventListener("DOMContentLoaded", function () {
    const table = document.querySelector("#medicineTable tbody");
    const addRowBtn = document.getElementById("addRow");
    const form = document.getElementById("prescriptionForm");
    const emptyError = document.getElementById("emptyTableError");

    let index = window.medicineRowIndex || 0;

    function buildRow(i) {
        return `
        <tr>
            <td>
                <input type="hidden" name="MedicineProtocal[${i}].Id" value="00000000-0000-0000-0000-000000000000" />
                <input class="form-control" name="MedicineProtocal[${i}].MedicineName" />
            </td>
            <td>
                <input class="form-control" name="MedicineProtocal[${i}].Dosage" />
            </td>
            <td>
                <input class="form-control" name="MedicineProtocal[${i}].Frequency" />
            </td>
            <td>
                <input class="form-control" name="MedicineProtocal[${i}].Duration" />
            </td>
            <td>
                <input class="form-control" name="MedicineProtocal[${i}].Instructions" />
            </td>
            <td>
                <button type="button" class="btn btn-danger removeRow">Remove</button>
            </td>
        </tr>`;
    }

    addRowBtn.addEventListener("click", function () {
        table.insertAdjacentHTML("beforeend", buildRow(index));
        index++;
        emptyError.style.display = "none";
    });

    table.addEventListener("click", function (e) {
        if (e.target.classList.contains("removeRow")) {
            const rows = table.querySelectorAll("tr");
            if (rows.length === 1) {
                emptyError.style.display = "block";
                return;
            }
            e.target.closest("tr").remove();
        }
    });

    form.addEventListener("submit", function (e) {
        if (table.querySelectorAll("tr").length === 0) {
            e.preventDefault();
            emptyError.style.display = "block";
        }
    });
});