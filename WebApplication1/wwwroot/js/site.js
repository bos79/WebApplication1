// Write your JavaScript code.
$.getJSON( "Ajax/JsonFile.json", function( data ) {
    var items = [];
    
    items = data;
    function getCountryByCode(code, data) {
        console.log(data);
        return data.filter(
            function (data) { return data.code == code }
        );
    }

    var found = getCountryByCode('Förfallodatum', data);
    console.log(found);
});
