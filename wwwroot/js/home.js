function downloadVideo(link) {
    $.ajax({
        url: '/Home/DownloadJs',
        type: 'GET',
        data: { link: link },
        xhrFields: {
            responseType: 'blob'
        },
        success: function (data) {
            var blob = new Blob([data], { type: 'video/mp4' });
            
            // Lấy thông tin về video
            $.ajax({
                url: 'https://www.youtube.com/oembed?url=' + link + '&format=json',
                type: 'GET',
                success: function (videoData) {
                    var title = videoData.title;
                    var filename = title + '.mp4';

                    // Tạo đối tượng <a> để tải xuống
                    var downloadLink = document.createElement('a');
                    downloadLink.href = window.URL.createObjectURL(blob);
                    downloadLink.download = filename;

                    // Thêm đối tượng <a> vào body và kích hoạt sự kiện click
                    document.body.appendChild(downloadLink);
                    downloadLink.click();

                    // Loại bỏ đối tượng <a> khỏi body
                    document.body.removeChild(downloadLink);
                },
                error: function () {
                    console.log('Failed to get video data');
                }
            });
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



