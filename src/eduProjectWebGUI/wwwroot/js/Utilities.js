function setTableHighlight(tableToHighlight, i, tablesToClear) {
    var table = document.getElementById(tableToHighlight);
    var rows = table.getElementsByTagName("tr");

    // clear highlight from selected table
    for (var j = 0; j < rows.length; j++) {
        rows[j].className -= " selected-row";
    }

    // highlight selected row
    var row = rows.item(i + 1);
    row.className += " selected-row"


    // clear highlight from all other tables
    for (i = 0; i < tablesToClear.length; i++) {
        var otherTable = document.getElementById(tablesToClear[i]);
        var otherRows = otherTable.getElementsByTagName("tr");

        for (var j = 0; j < otherRows.length; j++) {
            otherRows[j].className -= " selected-row";
        }
    }
}