//Marking With Selection

function markAsCompleted() {
    var checkboxes = document.getElementsByName('checkboxes');
    var selectedCheckboxes = [];

    checkboxes.forEach(function (checkbox) {
        if (checkbox.checked) {
            selectedCheckboxes.push(checkbox.value);
        }
    });

    if (selectedCheckboxes.length > 0) {
        var form = document.createElement('form');
        form.method = 'POST';
        form.action = '/UserPanel/Ticket/MarkAsCompleted';

        selectedCheckboxes.forEach(function (id) {
            var input = document.createElement('input');
            input.type = 'hidden';
            input.name = 'checkboxes';
            input.value = id;
            form.appendChild(input);
        });

        document.body.appendChild(form);
        form.submit();
    } else {
        // No checkboxes selected
    }
}

function unmarkAsCompleted() {
    var checkboxes = document.getElementsByName('checkboxes');
    var selectedCheckboxes = [];

    checkboxes.forEach(function (checkbox) {
        if (checkbox.checked) {
            selectedCheckboxes.push(checkbox.value);
        }
    });

    if (selectedCheckboxes.length > 0) {
        var form = document.createElement('form');
        form.method = 'POST';
        form.action = '/UserPanel/Ticket/UnmarkAsCompleted';

        selectedCheckboxes.forEach(function (id) {
            var input = document.createElement('input');
            input.type = 'hidden';
            input.name = 'checkboxes';
            input.value = id;
            form.appendChild(input);
        });

        document.body.appendChild(form);
        form.submit();
    } else {

    }
}

function deleteAsBatch() {
    var checkboxes = document.getElementsByName('checkboxes');
    var selectedCheckboxes = [];

    checkboxes.forEach(function (checkbox) {
        if (checkbox.checked) {
            selectedCheckboxes.push(checkbox.value);
        }
    });

    if (selectedCheckboxes.length > 0) {
        var form = document.createElement('form');
        form.method = 'POST';
        form.action = '/UserPanel/Ticket/DeleteAsBatch';

        selectedCheckboxes.forEach(function (id) {
            var input = document.createElement('input');
            input.type = 'hidden';
            input.name = 'checkboxes';
            input.value = id;
            form.appendChild(input);
        });

        document.body.appendChild(form);
        form.submit();
    } else {
        // No checkboxes selected
    }
}


//Checkbox Selector

function changeBackground(checkbox) {
    var tr = checkbox.closest('tr');
    if (checkbox.checked) {
        tr.classList.add('checked');
    } else {
        tr.classList.remove('checked');
    }
}

function selectAllRows() {
    var allCheckbox = document.getElementById('All');
    var tbodyCheckboxes = document.querySelectorAll('tbody input[type="checkbox"]');

    for (var i = 0; i < tbodyCheckboxes.length; i++) {
        var checkbox = tbodyCheckboxes[i];
        checkbox.checked = allCheckbox.checked;

        var tr = checkbox.closest('tr');
        if (allCheckbox.checked) {
            tr.classList.add('checked');
        } else {
            tr.classList.remove('checked');
        }
    }
}

//Sorting  -- Sorunlu

var currentSortColumn = -1;
var isSortAscending = true;

function sortTable(clickedTh) {
    var table = document.querySelector("table");
    var thElements = table.querySelectorAll("th");
    var columnIndex = Array.from(thElements).indexOf(clickedTh);

    if (currentSortColumn === columnIndex) {
        isSortAscending = !isSortAscending;
    } else {
        currentSortColumn = columnIndex;
        isSortAscending = true;
    }

    var sortIcon = table.querySelectorAll(".sort-icon");
    sortIcon.forEach(function (icon) {
        icon.classList.remove("ascending");
        icon.classList.remove("descending");
    });

    var icon = clickedTh.querySelector(".sort-icon");
    if (isSortAscending) {
        icon.classList.add("ascending");
    } else {
        icon.classList.add("descending");
    }

    var rows = Array.from(table.querySelectorAll("tbody tr"));
    rows.sort(function (rowA, rowB) {
        var valueA = rowA.querySelectorAll("td")[columnIndex].textContent.toLowerCase().trim();
        var valueB = rowB.querySelectorAll("td")[columnIndex].textContent.toLowerCase().trim();

        if (isSortAscending) {
            return valueA.localeCompare(valueB);
        } else {
            return valueB.localeCompare(valueA);
        }
    });

    var tbody = table.querySelector("tbody");
    rows.forEach(function (row) {
        tbody.appendChild(row);
    });
}

document.addEventListener("DOMContentLoaded", function () {
    var thElements = document.querySelectorAll("th[onclick]");
    thElements.forEach(function (th) {
        th.onclick = function () {
            sortTable(this);
        };
    });
});


//Dynamic Search

$(document).ready(function () {
    // Trigger the search whenever the search input changes
    $('#searchInput').on('input', function () {
        var searchText = $(this).val().toLowerCase();

        // Loop through each row in the table
        $('#tableBody tr').each(function () {
            var rowText = $(this).text().toLowerCase();

            // Show or hide the row based on the search input match
            $(this).toggle(rowText.indexOf(searchText) > -1);
        });
    });
});
