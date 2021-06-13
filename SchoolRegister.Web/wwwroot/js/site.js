$(".subjects-filter-input").change(async function() {
    const filterValue = $(".subjects-filter-input").val();
    const result = await $.get("/Subject/Index", $.param({ filterValue: filterValue }));
    $(".subjects-table-data").html(result);
});