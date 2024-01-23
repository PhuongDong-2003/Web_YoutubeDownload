function downloadVideo(link) {
    Swal.fire({
        title: 'Download Confirmation',
        text: 'Are you sure you want to download this video?',
        icon: 'question',
        showCancelButton: true,
        confirmButtonText: 'Yes, download it!',
        cancelButtonText: 'Cancel',
        allowOutsideClick: () => !Swal.isLoading(),
        preConfirm: () => {
            Swal.showLoading();
            return fetch('/Home/DownloadJs?link=' + encodeURIComponent(link), {
                method: 'GET',
                headers: {
                    'Content-Type': 'application/json',
                },
            })
                .then(async (response) => {

                    if (response.status === 400) {
                        Swal.fire({
                            icon: "error",
                            title: "Oops...",
                            text: "You do not have permission to download!",
                            timer: 2500,
                            allowOutsideClick: false,
                        })
                        throw 1;
                    } else {
                        // download(response.blob(), "v.mp4", "video/mp4");
                        // var file = window.URL.createObjectURL(blob);
                        // window.location.assign(file);
                        var blob = await response.blob();
                        const titleResponse = await fetch('https://www.youtube.com/oembed?url=' + encodeURIComponent(link) + '&format=json');
                        const videoData = await titleResponse.json();
                        const title = videoData.title;
                        download(blob,`${title}.mp4`, "video/mp4");
                        return;
                    }

                });
            // .then((blob) => {

            //     var title = "";
            //     return fetch('https://www.youtube.com/oembed?url=' + encodeURIComponent(link) + '&format=json')
            //         .then((response) => response.json())
            //         .then((videoData) => {
            //             title = videoData.title;
            //             var filename = title + '.mp4';

            //             var downloadLink = document.createElement('a');
            //             downloadLink.href = window.URL.createObjectURL(blob);
            //             downloadLink.download = filename;

            //             document.body.appendChild(downloadLink);
            //             downloadLink.click();

            //             document.body.removeChild(downloadLink);

            //             Swal.close();
            //         })
            //         .catch(() => {
            //             console.log('Failed to get video data');
            //             Swal.fire('Error', 'Failed to get video data', 'error');

            //             Swal.close();
            //         });
            // })
            // .catch(() => {
            //     console.log('Failed to connect to server');
            //     Swal.fire('Error', 'Failed to connect to server', 'error');

            //     Swal.close();
            // });
        },
    });
}



function DownloadAudio(link) {

    Swal.fire({
        title: 'Download Confirmation',
        text: 'Are you sure you want to download this Audio?',
        icon: 'question',
        showCancelButton: true,
        confirmButtonText: 'Yes, download it!',
        cancelButtonText: 'Cancel',
        allowOutsideClick: () => !Swal.isLoading(),
        preConfirm: () => {
            Swal.showLoading();
            return fetch('/Home/DownloadJs?link=' + encodeURIComponent(link), {
                method: 'GET',
                headers: {
                    'Content-Type': 'application/json',
                },
            })
                .then((response) => response.blob())
                .then((data) => {

                    var blob = new Blob([data], { type: 'Audio/mp4' });
                    fetch('https://www.youtube.com/oembed?url=' + encodeURIComponent(link) + '&format=json')
                        .then((response) => response.json())
                        .then((audioData) => {
                            var title = audioData.title;
                            var filename = title + '.mp4';

                            var downloadLink = document.createElement('a');
                            downloadLink.href = window.URL.createObjectURL(blob);
                            downloadLink.download = filename;

                            document.body.appendChild(downloadLink);
                            downloadLink.click();
                            document.body.removeChild(downloadLink);


                            Swal.close();
                        })
                        .catch(() => {
                            console.log('Failed to get audio data');
                            Swal.fire('Error', 'Failed to get audio data', 'error');

                            Swal.close();
                        });
                }).catch(() => {
                    console.log('Failed to connect to server');
                    Swal.fire('Error', 'Failed to connect to server', 'error');

                    Swal.close();
                });
        }
    });
}

var input = document.getElementById("keyword");
var button = document.querySelector("#searchForm button");


input.addEventListener("keydown", function (event) {
    if (event.key === "Enter") {
        searchVideos();
    }
});

function searchVideos() {
    var keyword = $('#keyword').val();
    $.ajax({
        url: '/Home/FindJs',
        type: 'POST',
        data: { keyword: keyword },
        beforeSend: function () {
            Swal.showLoading(); // Show loading indicator before the request is sent
        },
        success: function (data) {
            $('#searchResults').html(data);
        },
        error: function () {
            console.log('Failed to connect to server');
            Swal.fire('Error', 'Failed to connect to server', 'error');
        }
    }).always(function () {
        Swal.close(); // Close SweetAlert2 modal after the request is complete
    });
}


// function DownloadAudio(link) {
//     Swal.fire({
//         title: 'Download Confirmation',
//         text: 'Are you sure you want to download this Audio?',
//         icon: 'question',
//         showCancelButton: true,
//         confirmButtonText: 'Yes, download it!',
//         cancelButtonText: 'Cancel',
//     }).then((result) => {
//         if (result.isConfirmed) {

//             Swal.showLoading();
//             $.ajax({
//                 url: '/Home/DownloadAudio',
//                 type: 'GET',
//                 data: { link: link },
//                 xhrFields: {
//                     responseType: 'blob'
//                 },
//                 xhr: function () {
//                     var xhr = $.ajaxSettings.xhr();

//                     // Attach an event listener to track progress
//                     xhr.addEventListener('progress', function (e) {
//                         if (e.lengthComputable) {
//                             var percentage = Math.round((e.loaded / e.total) * 100);
//                             Swal.update({
//                                 title: 'Downloading...',
//                                 text: percentage + '% complete',
//                                 showConfirmButton: false,
//                             });
//                         }
//                     });

//                     return xhr;
//                 },
//                 success: function (data) {
//                     var blob = new Blob([data], { type: 'Audio/mp4' });

//                     $.ajax({
//                         url: 'https://www.youtube.com/oembed?url=' + link + '&format=json',
//                         type: 'GET',
//                         success: function (videoData) {
//                             var title = videoData.title;
//                             var filename = title + '.mp4';

//                             // Create download link
//                             var downloadLink = document.createElement('a');
//                             downloadLink.href = window.URL.createObjectURL(blob);
//                             downloadLink.download = filename;

//                             // Append download link to body and trigger click event
//                             document.body.appendChild(downloadLink);
//                             downloadLink.click();

//                             // Remove download link from body
//                             document.body.removeChild(downloadLink);

//                             // Close SweetAlert2 modal
//                             Swal.close();
//                         },
//                         error: function () {
//                             console.log('Failed to get Audio data');
//                             Swal.fire('Error', 'Failed to get Audio data', 'error');
//                             // Close SweetAlert2 modal in case of error
//                             Swal.close();
//                         }
//                     });
//                 },
//                 error: function () {
//                     console.log('Failed to connect to server');
//                     Swal.fire('Error', 'Failed to connect to server', 'error');
//                     // Close SweetAlert2 modal in case of error
//                     Swal.close();
//                 }
//             });
//         }
//     });

// }








