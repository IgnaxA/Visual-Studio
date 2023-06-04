
$(document).ready(function () {

    document.getElementById("StudentFacultyFilter").onchange = function () {
        if (document.getElementById("StudentsTable") === null) {
            return;
        }
        var selectedListValue = document.getElementById("StudentFacultyFilter");
        var selectedOption = selectedListValue.options[selectedListValue.selectedIndex];
        var selectedValue = selectedOption.getAttribute("data-selectedValue");
        var studentsTableRow = document.getElementById("StudentsTable");
        var rows = studentsTableRow.getElementsByTagName("tr");
        
        for (var i = 0; i < rows.length; i++) {
            var endIndex = -1;

            if (rows[i].getAttribute('data-category') === "ToChangeVisibilityTheme") {
                var isVisible = false;
                

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

                for (var j = i; j < endIndex; j++) {
                    var cells = rows[j].getElementsByTagName("td");
                    for (let cell of cells) {
                        if (cell.hasAttribute('data-faculty')) {
                            if (cell.getAttribute('data-faculty') === selectedValue) {
                                isVisible = true;
                                break;

                            }
                        }
                    }
                    if (isVisible) {
                        break;
                    }
                }

                for (var j = i; j < endIndex; j++) {
                    if (isVisible) {
                        rows[j].style.display = 'table-row';
                    }
                    else {
                        rows[j].style.display = 'none';
                    }
                }
            }
            if (endIndex !== -1) {
                i = endIndex + 1;
            }
        }
    };
});