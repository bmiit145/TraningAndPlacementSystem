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


function extractVideoId(url) {
    var match = url.match(/(?:https?:\/\/)?(?:www\.)?(?:youtube\.com\/(?:[^\/\n\s]+\/\S+\/|(?:v|e(?:mbed)?)\/|\S*?[?&]v=)|youtu\.be\/)([a-zA-Z0-9_-]{11})/);
    return match ? match[1] : null;
}