function createCommentItem(form, path) {
    var service = new ItemService({ url: '/sitecore/api/ssc/item' });
    var obj = {
        ItemName: 'comment - ' + form.name.value,
        TemplateID: '{C4E3CBA0-EEA3-4BC2-8CAA-0BCE266A97B4}',
        Name: form.name.value,
        Comment: form.comment.value
    };
    service.create(obj).path(path).execute()
    .then(function (item) {
        form.name.value = form.comment.value = '';
        window.alert('Thanks. Your message will show on the site shortly');
    })
    .fail(function (err) { window.alert(err); });
    event.preventDefault();
    return false;
}
function createCommentItemXHR(form, path) {

    var xhr = new XMLHttpRequest();
    var url = "http://events.tac.local/sitecore/api/ssc/item" + path;
    //alert(url);
    //xhr.open("POST", "http://events.tac.local/sitecore/api/ssc/item" + path);
    //xhr.open("POST", "http://events.tac.local/sitecore/api/ssc/item/sitecore%2Fcontent%2Fhome ");
    //xhr.open("POST", "http://events.tac.local/sitecore/api/ssc/item/sitecore/content/home ");
    xhr.open("POST", "http://events.tac.local/sitecore/api/ssc/item/sitecore/content/Events/Home/Events/Climbing/Climb Mount Everest");
    xhr.setRequestHeader("Content-Type", "application/json");
    xhr.onreadystatechange = function () {
        if (this.readyState == 4) {
            alert('Status: ' + this.status + '\nHeaders: ' + JSON.stringify(this.getAllResponseHeaders()) + '\nBody: ' + this.responseText);
        }
    };
    var rdata = '\"{' + '\n';
    rdata = rdata + '\"ItemName\": ' + '\"' + form.name.value +'\",' + '\n';
    rdata = rdata + '\"TemplateID\": ' + '\"' + 'C4E3CBA0-EEA3-4BC2-8CAA-0BCE266A97B4' + '\",' + '\n';
    rdata = rdata + '\"Name\": ' + '\"' + form.name.value + '\",' + '\n';
    rdata = rdata + '\"Comment\": ' + '\"' + form.comment.value + '\"' + '\n';
    rdata = rdata + '\n' + '}\"';
    alert(rdata);
    //xhr.send("{ \n    \"ItemName\": \"Home\", \n    \"TemplateID\": \"76036f5e-cbce-46d1-af0a-4143f9b557aa\", \n    \"Title\": \"Sitecore\", \n    \"Text\": \"\\r\\n\\t\\t\u003Cp\u003EWelcome to Sitecore\u003C/p\u003E\\r\\n\" \n}");
    xhr.send(rdata);
}