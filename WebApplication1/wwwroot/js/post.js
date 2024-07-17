$(() => {
    var currentPage = 1;
    var totalPages = 1;
    function LoadProdData(page) {


        $.ajax({
            url: '/posts/Getposts',
            method: 'GET',
            data: { pageIndex: page },
            success: (result) => {

                var tr = '';
                $.each(result.posts, (k, v) => {
                    tr += `<tr>
                            <td>${v.authorName}</td>
                            <td>${v.createDate}</td>
                            <td>${v.updateDate}</td>
                            <td>${v.title}</td>
                            <td>${v.content}</td>
                            <td>${v.status}</td>
                            <td>${v.category}</td>
                            <td>
                                <a href='../posts/Edit?id=${v.postId}'> Edit </a> |
                                <a href='../posts/Details?id=${v.postId}'> Details </a> |
                                <a href='../posts/Delete?id=${v.postId}'> Delete </a>
                            </td>
                        </tr>`;
                });
                $("#tablePost").html(tr);
                totalPages = result.totalPages;
                $("#pageInfo").text(`Page ${currentPage} of ${result.totalPages}`);
            },
            error: (error) => {
                console.log(error);
            }
        });
    }

    var connection = new signalR.HubConnectionBuilder().withUrl("/signalrServer").build();

    connection.start();
    connection.on("LoadPosts", function () {

        LoadProdData(currentPage);
    });

    $("#prevPage").click(() => {
        if (currentPage > 1 ) {
            currentPage--;
            LoadProdData(currentPage);
        }
    });


    $("#nextPage").click(() => {
        if (currentPage < totalPages) {
            currentPage++;
            LoadProdData(currentPage);
        }
    });

    LoadProdData(currentPage);
});