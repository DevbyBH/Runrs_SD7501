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
    placeholder: 'Type your announcement here:',
    body_class: 'announcement-editor',
    statusbar: false,
    paste_data_images: false,
    content_style: `
        @import url('https://fonts.googleapis.com/css2?family=Rajdhani:wght@400;500;600&display=swap');
        body {
            font-family: 'Rajdhani', sans-serif;
            font-size: 16px;
            color: #ffffff;
            background-color: rgba(0, 0, 0, 0.5) !important;
            padding: 10px;
            margin: 0;
        }

        .mce-content-body[data-mce-placeholder]::before {
             color: rgba(255, 255, 255, 0.52) !important;
             opacity: 1;
             padding-left: 10px;
        }

        p {
            margin: 0 0 8px 10px;
        }

        h1 {
            margin: 0 0 8px 10px;
        }

        h2 {
            margin: 0 0 8px 10px;
        }

        h3 {
            margin: 0 0 8px 10px;
        }

        h4 {
            margin: 0 0 8px 10px;
        }

        h5 {
            margin: 0 0 8px 10px;
        }

        h6 {
            margin: 0 0 8px 10px;
        }
    `,
    setup: function (editor) {
        editor.on('change', function () {
            editor.save();
        });

        editor.on('init', function () {
            const iframe = editor.getContentAreaContainer().querySelector('iframe');
            if (iframe) {
                iframe.style.background = 'transparent';
                iframe.contentDocument.documentElement.style.background = 'transparent';
                iframe.contentDocument.body.style.backgroundColor = 'transparent';
            }
            const container = editor.getContainer();
            if (container) {
                container.style.background = 'transparent';
                container.style.border = '4px solid #440746';
                container.style.borderRadius = '10px';
                container.style.overflow = 'hidden';
            }
            const editArea = container?.querySelector('.tox-edit-area');
            if (editArea) {
                editArea.style.background = 'transparent';
            }
            const editAreaIframe = container?.querySelector('.tox-edit-area__iframe');
            if (editAreaIframe) {
                editAreaIframe.style.background = 'transparent';
            }
        });
    }
});

