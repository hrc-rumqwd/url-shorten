function onSubmitShortenLink(e) {
    e.preventDefault();
    const input = $("form#shortenForm input[name=sourceLink]").val();
    if (input.length === 0) {
        alert("Please enter a URL to shorten.");
        return false;
    }

    $.ajax({
        type: 'POST',
        url: `/api/shorten-link?url=${input}`,
        success: function (data) {
            const domItem = $("<a>").attr("href", data.data)
                .text(data.data);
            $("#output").append(domItem);
        }
    })
}