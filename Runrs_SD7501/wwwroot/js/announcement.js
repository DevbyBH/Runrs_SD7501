tinymce.init({
    selector: '#announcementContent',
    min_height: 200,
    max_height: 600, 
    height: 200,
    menubar: false,
    plugins: [
        'advlist', 'autolink', 'link', 'image', 'lists', 'charmap', 'preview', 'anchor', 'pagebreak',
        'searchreplace', 'wordcount', 'visualblocks', 'visualchars', 'code', 'fullscreen', 'insertdatetime',
        'media', 'table', 'emoticons', 'help', 'autoresize'
    ],
    toolbar: 'undo redo | tinymceai-chat tinymceai-quickactions tinymceai-review | blocks fontfamily fontsize | bold italic underline strikethrough | link media table mergetags | addcomment showcomments | spellcheckdialog a11ycheck typography uploadcare | align lineheight | checklist numlist bullist indent outdent | emoticons charmap | removeformat',
    skin: 'oxide-dark',
    content_css: 'light',
    placeholder: 'Type your announcement here..',
    body_class: 'announcement-editor',
    statusbar: false,
    paste_data_images: false,
    content_style: `
        body {
            font-family: 'Rajdhani', sans-serif;
            font-size: 16px;
            color: #000;
            background-color: #fff;
            padding: 10px;
        }
        p { margin: 0 0 8px 0; }
    `,
    setup: function (editor) {
        editor.on('change', function () {
            editor.save();
        });
    }
});

