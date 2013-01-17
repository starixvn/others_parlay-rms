/* File Created: November 7, 2012 */
//function MovePage(routeValue) {
//    location.href = url_domain(routeValue, $('#pagesize').val());
//}

//function url_domain(page, pagesize) {
//    var subStringUrl = window.location.href.substr(0, window.location.href.indexOf('?') + 1);
//    if (subStringUrl == "")
//        return window.location.href + '?page=' + page + '&pagesize=' + pagesize;
//    var hashes = window.location.href.slice(window.location.href.indexOf('?') + 1).split('&');
//    subStringUrl += 'page=' + page + '&pagesize=' + pagesize + '&';
//    for (var i = 0; i < hashes.length; i++) {
//        hash = hashes[i].split('=');
//        if (hash[0] != 'page' && hash[0] != 'pagesize' && hash[0] != "") {
//            subStringUrl += hash[0] + '=' + hash[1] + '&';
//        }
//    }
//    return subStringUrl.substr(0, subStringUrl.length - 1);
//}

//function pagesize_onclick(el) {
//    location.href = url_domain(1, el.value);
//}

//function MovePage(routeValue) {
//    location.href = url_domain(routeValue);
//}

//function url_domain(page, pagesize) {
//    var subStringUrl = window.location.href.substr(0, window.location.href.indexOf('?') + 1);
//    if (subStringUrl == "")
//        return window.location.href + '?page=' + page;
//    var hashes = window.location.href.slice(window.location.href.indexOf('?') + 1).split('&');
//    subStringUrl += 'page=' + page + '&';
//    for (var i = 0; i < hashes.length; i++) {
//        hash = hashes[i].split('=');
//        if (hash[0] != 'page' && hash[0] != 'pagesize' && hash[0] != "") {
//            subStringUrl += hash[0] + '=' + hash[1] + '&';
//        }
//    }
//    return subStringUrl.substr(0, subStringUrl.length - 1);
//}