// Function to validate YouTube URL
function isValidYouTubeUrl(url) {
    var youtubeRegex = /^(https?\:\/\/)?(www\.)?(youtube\.com|youtu\.?be)\/.+$/;
    return youtubeRegex.test(url);
}

function getYouTubeVideoInfo(url, callback) {
    if (!isValidYouTubeUrl(url)) {
        callback("Invalid YouTube URL", null);
        return;
    }
    
    $.ajax({
        url: "https://www.googleapis.com/youtube/v3/videos",
        dataType: "json",
        data: {
            part: "snippet",
            id: url.match(/(youtu\.be\/|youtube\.com\/(watch\?(.*&)?v=|(embed|v)\/))([^?&\"'>]+)/)[5],
            key: "AIzaSyCI8DwPQ752bIEjvmaYSxuCv_T9n4cs8Zc"
        },
        success: function(response) {
            if (response.items.length > 0) {
                // return all the information
                callback(null, response.items[0]);
            } else {
                callback("Video not found", null);
            }
        },
        error: function(xhr, status, error) {
            callback(error, null);
        }
    });
}


