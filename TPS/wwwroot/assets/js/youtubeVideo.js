var key = "AIzaSyCI8DwPQ752bIEjvmaYSxuCv_T9n4cs8Zc";

// Function to validate YouTube URL
function isValidYouTubeUrl(url) {
    var youtubeRegex = /^(https?\:\/\/)?(www\.)?(youtube\.com|youtu\.?be)\/.+$/;
    return youtubeRegex.test(url);
}

//isValidYouTubePlaylistUrl
function isValidYouTubePlaylistUrl(url) {
    // var youtubeRegex = /^(https?\:\/\/)?(www\.)?(youtube\.com|youtu\.?be)\/playlist\?list=.+$/;
    var youtubeRegex = /(?:https?:\/\/)?(?:www\.)?(?:youtube\.com\/(?:[^\/\n\s]+\/\S+\/|(?:v|e(?:mbed)?)\/|\S*?[?&]list=))([^\/\n\s&]+)/;
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
            key: key
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


// function to get the details from youtube playlist url
function getYouTubePlaylistInfo(url, callback) {
    if (!isValidYouTubeUrl(url)) {
        callback("Invalid YouTube URL", null);
        return;
    }

    // ckech as link has list parameter or not
    if (!url.match(/list=/)) {
        callback("Invalid YouTube Playlist URL", null);
        return;
    }

    $.ajax({
        url: "https://www.googleapis.com/youtube/v3/playlists",
        dataType: "json",
        data: {
            part: "snippet",
            id: url.match(/list=([^&]+)/)[1],
            key: key
        },
        success: function(response) {
            if (response.items.length > 0) {
                // return all the information
                callback(null, response.items[0]);
            } else {
                callback("Playlist not found", null);
            }
        },
        error: function(xhr, status, error) {
            callback(error, null);
        }
    });
}

function getYouTubePlaylistVideos(playlistId, callback) {
    $.ajax({
        url: "https://www.googleapis.com/youtube/v3/playlistItems",
        dataType: "json",
        data: {
            part: "snippet",
            playlistId: playlistId,
            maxResults: 500,// adjust as needed
            key: key
        },
        success: function(response) {
            var videos = response.items.map(function(item) {
                return {
                    id: item.snippet.resourceId.videoId,
                    title: item.snippet.title
                };
            });
            callback(null, videos);
        },
        error: function(xhr, status, error) {
            callback(error, null);
        }
    });
}