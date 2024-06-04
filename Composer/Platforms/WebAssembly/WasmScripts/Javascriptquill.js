function insertTextAtCursor(text) {
    quill.focus();
    const selection = quill.getSelection();
    if (selection && selection.length === 0) {
        // Insert text at cursor
        quill.insertText(selection.index, ' ' + text + ' ', 'api');
        quill.formatText(selection.index + 1, 1, 'font', 'scheherazade-new');
        quill.formatText(selection.index + 1, 1, 'size', parseInt(formats.size) + 4 + 'px');

        quill.formatText(selection.index + 2, 1, 'font', formats.font);
        quill.formatText(selection.index + 2, 1, 'size', formats.size);
        // Move cursor to the end of the inserted text
        quill.setSelection(selection.index + 3);
    }
}

function captureDivToClipboard() {
    var div = document.querySelector('#editor .ql-editor');
    var originalHeight = div.style.height;

    // Ensure all content is visible by setting the height to the scroll height
    div.style.position = 'absolute';
    div.style.height = div.scrollHeight + 'px';
    div.style.top = '-9999px';
    div.style.left = '-9999px';
    div.style.width = 700 + 'px';

    html2canvas(div, {
        scale: 1.5,
        useCORS: true,

    }).then(function (canvas) {
        // Reset the div's style back to its original state
        div.style.height = originalHeight;
        div.style.position = 'initial';
        // Create a temporary canvas to crop the image
        var tempCanvas = document.createElement('canvas');
        tempCanvas.width = canvas.width - 4; // Crop 2 pixels on the right
        tempCanvas.height = canvas.height - 2; // Crop 2 pixels at the bottom
        var ctx = tempCanvas.getContext('2d');
        ctx.drawImage(canvas, 2, 0, canvas.width - 4, canvas.height - 2, 0, 0, tempCanvas.width, tempCanvas.height);

        tempCanvas.toBlob(function (blob) {
            navigator.clipboard.write([
                new ClipboardItem({ 'image/png': blob })
            ]).then(function () {
                console.log('Image copied to clipboard');
            }).catch(function (err) {
                console.error('Failed to copy image to clipboard: ', err);
            });
        }, 'image/png');
    }).catch(function (err) {
        console.error('Failed to capture div: ', err);
    });
}

function saveContentAsHTML() {

    var htmlContent = quill.root.innerHTML;

    var blob = new Blob([htmlContent], { type: 'text/html' });

    var a = document.createElement('a');
    a.href = URL.createObjectURL(blob);
    a.download = 'content.html';
    document.body.appendChild(a);
    a.click();

    // Remove the link from the document
    document.body.removeChild(a);
}

function saveContentAsDOCX() {
    // Get the HTML content from the Quill editor
    var htmlContent = quill.root.innerHTML;

    // Add necessary HTML structure for a valid DOCX conversion
    var fullHtml = `
            <!DOCTYPE html>
            <html>
            <head>
                <meta charset="utf-8">
                <title>Document</title>
            </head>
            <body>${htmlContent}</body>
            </html>`;

    // Create a Blob from the HTML content using html-docx-js
    var converted = htmlDocx.asBlob(fullHtml);

    // Create a link element
    var a = document.createElement('a');

    // Create a URL for the Blob and set it as the href attribute
    a.href = URL.createObjectURL(converted);

    // Set the download attribute to the desired file name
    a.download = 'content.docx';

    // Append the link to the body (required for Firefox)
    document.body.appendChild(a);

    // Programmatically click the link to trigger the download
    a.click();

    // Remove the link from the document
    document.body.removeChild(a);
}
