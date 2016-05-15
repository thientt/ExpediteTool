/***************************************************
* htmlspecialchars
****************************************************/
function removeHtmlSpecialChars(str) {
    var replaceChar = "-";
    str = str.trim();
    return str.replace(/&/g, replaceChar).replace(/</g, replaceChar).replace(/>/g, replaceChar).replace("\"", replaceChar).replace("*", replaceChar).replace("?", replaceChar).replace("\\", replaceChar).replace("/", replaceChar).replace("|", replaceChar).replace(":", replaceChar).replace("*", replaceChar);
}


