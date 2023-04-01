function ExportToExcel() {
    // Gets the data source from the grid.
    
    var dataSource = $("div#grid").data("kendoGrid").dataSource;

    // Gets the filter from the dataSource
    var filters = dataSource.filter();

    // Gets the full set of data from the data source
    var allData = dataSource.data();

    // Applies the filter to the data
    var query = new kendo.data.Query(allData);
    var filteredData = query.filter(filters).data;
    var that = $("div#grid").data("kendoGrid");
    // Define the data to be sent to the server to create the spreadsheet.
    var mydata = {
        model: JSON.stringify(that.columns),
        data: JSON.stringify(filteredData),
        title: "title"
    };
    $.ajax({
        type: 'post',
        url: '/Administrator/Financial/ExcelExportSave',
        dataType: 'json',
        traditional: true,
        data: mydata,
        success: function (result) {
            alert("در حال ایجاد فایل اکسل لطفا منتظر بمانید");
            window.location.replace(kendo.format("{0}?title={1}",
                "/Administrator/Financial/GetExcelFile",
                 "title"));
        },
        error: function (response) {
            var errorThrown = response.responseText;

            alert('request failed :' + errorThrown);

        }

    })


};