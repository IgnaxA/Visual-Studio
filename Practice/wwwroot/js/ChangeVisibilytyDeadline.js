
$(document).ready(function () {
    document.getElementById("DeadlineFilter").onchange = function () {
        if (document.getElementById("DeadlinesTable") === null) {
            return;
        }

        var selectedValue = document.getElementById("DeadlineFilter");

        var selectedOption = selectedValue.options[selectedValue.selectedIndex];

        var selectedValue = selectedOption.getAttribute("data-selectedValue");

        var studentsTableRow = document.getElementById("DeadlinesTable");
        var rows = studentsTableRow.getElementsByTagName("tr");

        for (var i = 0; i < rows.length; i++) {
            var endIndex = -1;

            if (rows[i].getAttribute('data-category') === "ToChangeVisibilityTheme") {

                if (i + 1 == rows.length) {
                    endIndex = rows.length;
                }

                for (var j = i + 1; j < rows.length; ++j) {
                    if (rows[j].getAttribute('data-category') === "ToChangeVisibilityTheme") {
                        endIndex = j;
                        break;
                    }
                    else if (j == rows.length - 1) {
                        endIndex = rows.length;
                        break;
                    }

                }

                var counter = 1;
                for (var j = i; j < endIndex; j++) {
                    var cells = rows[j].getElementsByTagName("td");
                    if (cells[0].textContent != "" && counter != 1) {
                        cells[0].textContent = "";
                    }
                    ++counter;
                }
            }
            if (endIndex !== -1) {
                i = endIndex - 1;
            }
        }



        if (selectedValue === "-1") {
            for (var i = 0; i < rows.length; ++i) {
                rows[i].style.display = 'table-row';
            }
            return;
        }

        for (var i = 0; i < rows.length; i++) {
            var endIndex = -1;

            if (rows[i].getAttribute('data-category') === "ToChangeVisibilityTheme") {

                if (i + 1 == rows.length) {
                    endIndex = rows.length;
                }

                for (var j = i + 1; j < rows.length; ++j) {
                    if (rows[j].getAttribute('data-category') === "ToChangeVisibilityTheme") {
                        endIndex = j;
                        break;
                    }
                    else if (j == rows.length - 1) {
                        endIndex = rows.length;
                        break;
                    }

                }

                var nameCounter = 1;
                var firstRow = rows[i].getElementsByTagName("td");
                var cellName = firstRow[0].textContent;
                var isVisible = false;
                for (var j = i; j < endIndex - 1; j++) {
                    var cells = rows[j].getElementsByTagName("td");
                    for (let cell of cells) {
                        if (cell.hasAttribute('data-attendantMark')) {
                            if (cell.getAttribute('data-attendantMark') === selectedValue) {
                                if (nameCounter == 1) {
                                    cells[0].textContent = cellName;
                                }
                                rows[j].style.display = 'table-row';
                                isVisible = true;
                                nameCounter += 1;
                            }
                            else {
                                rows[j].style.display = 'none';
                            }

                        }
                    }
                }
                if (!isVisible && endIndex !== -1) {
                    rows[endIndex - 1].style.display = 'none';
                }
            }
            if (endIndex !== -1) {
                i = endIndex - 1;
            }
        }
    };

    document.getElementById("ConsultationFilter").onchange = function () {
        if (document.getElementById("ConsultationsTable") === null) {
            return;
        }

        var selectedValue = document.getElementById("ConsultationFilter");

        var selectedOption = selectedValue.options[selectedValue.selectedIndex];

        var selectedValue = selectedOption.getAttribute("data-selectedValue");

        var studentsTableRow = document.getElementById("ConsultationsTable");
        var rows = studentsTableRow.getElementsByTagName("tr");

        if (selectedValue === "Нет") {
            for (var i = 0; i < rows.length; ++i) {
                rows[i].style.display = 'table-row';
            }
            return;
        }

        for (var i = 1; i < rows.length; i++) {
            var temp = rows[i].getAttribute("data-selectedValue");
            var temp2 = selectedValue;
            if (rows[i].getAttribute("data-selectedValue") === selectedValue) {
                rows[i].style.display = 'table-row';
            }
            else {
                rows[i].style.display = 'none';
            }
        }
    };
});