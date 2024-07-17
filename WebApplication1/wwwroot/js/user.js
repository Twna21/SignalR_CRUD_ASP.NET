
$(() => {
    LoadProdData();
    var connection = new signalR.HubConnectionBuilder().withUrl("/signalrServer").build();

    connection.start();
    connection.on("LoadUsers", function () {
        LoadProdData();
    })

    LoadProdData();
    function LoadProdData() {
        var tr = '';
        $.ajax({
            url: '/Appusers/GetAppUsers',
            method: 'GET',
            success: (result) => {
                $.each(result, (k, v) => {
                    tr += `<tr>
                        <td>${v.fullName}</td>
                        <td>${v.address}</td>
                        <td>${v.email}</td>
                        <td>${v.password}</td>
                        <td>
                            <a href='../appusers/Edit?id=${v.userId}'> Edit </a> |
                            <a href='../appusers/Details?id=${v.userId}'> Details </a> |
                            <a href='../appusers/Delete?id=${v.userId}'> Delete </a>
                        </td>
                      </tr>`;
                });
                $("#tableUser").html(tr);
            },
            error: (error) => {
                console.log(error)
            }
        });
    }
})