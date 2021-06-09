// Please see documentation at https://docs.microsoft.com/aspnet/core/client-side/bundling-and-minification
// for details on configuring this project to bundle and minify static web assets.

// Write your JavaScript code.
$(".subjects-filter-input").change(async function () {
    const filterValue = $(".subjects-filter-input").val();
    const result = await $.get("/Subject/Index", $.param({ filterValue: filterValue }));
    $(".subjects-table-data").html(result);
});