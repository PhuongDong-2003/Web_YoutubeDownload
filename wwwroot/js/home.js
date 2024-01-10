
function downloadVideo(link) {
    $.ajax({
        url: '/Home/DownloadJs',
        type: 'POST',
        data: { link: link },
        success: function (data) {
            if (data.success) {
                window.open(data.fileResult.virtualPath);
                // window.location.href = data.fileResult.virtualPath;
            } else {
                
                console.log('Error downloading video');
            }
        },
        error: function () {

            console.log('Failed to connect to server');
        }
    });
}


function searchVideos() {
    var keyword = $('#keyword').val();;
    $.ajax({
        url: '/Home/FindJs',
        type: 'POST',

        data: { keyword: keyword },
        success: function (data) {

            $('#searchResults').html(data);
        },
        error: function () {
            console.log('Failed to connect to server');
        }
    });

}



