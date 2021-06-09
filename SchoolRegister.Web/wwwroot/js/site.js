(".subjects-filter-input").change(async function() {
    const filterValue = $(".subjects-filter-input").val();
    const result = await $.get("/Subject/Index", $.param({ filterValue: filterValue }));

    // Write your JavaScript code.
    $(".subjects-table-data").html(result);
});

$(".students-filter-input").change(async function() {
    const filterValue = $(".students-filter-input").val();
    const result = await $.get("/Student/Index", $.param({ filterValue: filterValue }));

    $(".students-table-data").html(result);
});