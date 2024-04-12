$.validator.addMethod("noSpecialChars", function (value, element) {
    return this.optional(element) || /^[a-zA-Z0-9\s\-/.|]+$/.test(value);
}, "Special characters are not allowed except '-', '.', '/', '|'");
