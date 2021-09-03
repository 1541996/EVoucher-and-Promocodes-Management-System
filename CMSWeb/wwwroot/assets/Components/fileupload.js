function ShowImagePreview(imageUploader, previewImage, fileid) {


    if (imageUploader.files && imageUploader.files[0]) {
        var files = $(".file_" + fileid).get(0).files;
        var file = files[0];

        var img = document.createElement("img");
        var reader = new FileReader();
        reader.onload = function (e) {
            $(previewImage).attr('src', e.target.result);

            img.src = e.target.result;
            var mime_type = "image/jpeg";
            if (typeof output_format !== "undefined" && output_format == "png") {
                mime_type = "image/png";
            }
            img.onload = function () {
                var canvas = document.createElement("canvas");
                if (navigator.userAgent.toLowerCase().indexOf('firefox') > -1) {
                    alert('Photo is uploading...');
                }
                //set max height width
                var gg = maxwidthheight(img.width, img.height);
                width = gg.width;
                height = gg.height;
                canvas.width = width;
                canvas.height = height;
                var ctx = canvas.getContext("2d");
                ctx.drawImage(img, 0, 0, canvas.width, canvas.height);
                var dataurl = canvas.toDataURL(mime_type, 60 / 100);
                var base64data = dataurl;

                $('#photo_' + fileid).val(base64data);


            }
        }
        reader.readAsDataURL(file);
    }
}

function maxwidthheight(width, height) {
    var MAX_WIDTH = 1200;  // to remove maximum width height
    var MAX_HEIGHT = 1200;
    var width = width;
    var height = height;

    if (width > height) {
        if (width > MAX_WIDTH) {
            height *= MAX_WIDTH / width;
            width = MAX_WIDTH;
        }
    } else {
        if (height > MAX_HEIGHT) {
            width *= MAX_HEIGHT / height;
            height = MAX_HEIGHT;
        }
    }
    var a = { width: width, height: height };
    return a
}

function dataURItoBlob(dataURI) {
    var binary = atob(dataURI.split(',')[1]);
    var array = [];
    for (var i = 0; i < binary.length; i++) {
        array.push(binary.charCodeAt(i));
    }
    return new Blob([new Uint8Array(array)], { type: 'image/png' });
}
