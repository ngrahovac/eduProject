function setTableHighlight(tableToHighlight, i, tablesToClear) {
    console.log("hajlajturem red" + i);
    var table = document.getElementById(tableToHighlight);

    var rows = table.getElementsByTagName("tr");
    console.log("red ofarban");

    // clear highlight from selected table
    for (var j = 0; j < rows.length; j++) {
        rows[j].className = rows[j].className.replace(" selected-row", "");
    }

    // highlight selected row
    var row = rows.item(i + 1);             // rows[0] is header
    row.className += " selected-row"


    // clear highlight from all other tables
    for (i = 0; i < tablesToClear.length; i++) {
        var otherTable = document.getElementById(tablesToClear[i]);
        var otherRows = otherTable.getElementsByTagName("tr");

        for (var j = 0; j < otherRows.length; j++) {
            otherRows[j].className = otherRows[j].className.replace(" selected-row", "");
        }
    }
}


function highlightRecommendedProfile(tableToHighlight, i) {

    var table = document.getElementById(tableToHighlight);
    var rows = table.getElementsByTagName("tr");
    /*
    if (rows[i + i].className.includes("selected-row"))    // if row is selected, the table will reload and remove highlight
        rows[i + 1].className = rows[i + 1].className.replace(" recommended-profile", "");
    else if (!rows[i + 1].contains("recommended-profile"))*/
    rows[i + 1].className += " recommended-profile";
}