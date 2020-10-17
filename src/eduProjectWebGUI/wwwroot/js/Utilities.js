function selectCollaboratorProfile(profileType, i) {
    var tableId = profileType == "faculty member profile" ? "faculty-member-profiles" : "student-profiles";
    var otherTableId = profileType != "faculty member profile" ? "faculty-member-profiles" : "student-profiles";

    var table = document.getElementById(tableId);
    var otherTable = document.getElementById(otherTableId);

    var rows = table.getElementsByTagName("tr");
    var otherRows = otherTable.getElementsByTagName("tr");

    // clearing highlight from both tables
    for (var j = 0; j < otherRows.length; j++) {
        otherRows[j].className -= " row-selected";
    }

    for (var j = 0; j < rows.length; j++) {
        rows[j].className -= " row-selected";
    }

    // adding highlight to selected row
    var row = rows.item(i + 1);
    row.className += " row-selected"
}