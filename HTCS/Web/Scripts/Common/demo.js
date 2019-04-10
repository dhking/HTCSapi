/*jslint  browser: true, white: true, plusplus: true */
/*global $, countries */

$(function () {
   
    'use strict';
    var countries = {
        "id": "B00155L1OZ",
        "name": "鑫峰苑",
        "district": "上海市闵行区",
        "address": "鑫都路2535弄"
    };
    var countriesArray = $.map(countries, function (value, key) { return { value: value, data: key }; });

    $('#autocomplete-ajax').autocomplete({
        
        lookup: function (query, done) {
            //doc.objectQuery("http://localhost:4527/api/Map/Querybuxiaoqu?xiaoqu=鑫&city=上海市", function (result) {
            //    if (result.numberData != null) {
            //        var realresut = [];
            //        var val = {};
            //        $.each(result.numberData, function (tindex,tvalue) {
            //            val.value = tvalue.name + "-" + tvalue.district + tvalue.address;


            //        })
            //    }
            //})
            var result = {
                suggestions: [
                    { "value": "United Arab Emirates", "data": "AE" },
                    { "value": "United Kingdom", "data": "UK" },
                    { "value": "United States", "data": "US" }
                ]
            };

            done(result);
        },
        lookupFilter: function(suggestion, originalQuery, queryLowerCase) {
            var re = new RegExp('\\b' + $.Autocomplete.utils.escapeRegExChars(queryLowerCase), 'gi');
            return re.test(suggestion.value);
        },
        onSelect: function(suggestion) {
            $('#selction-ajax').html('You selected: ' + suggestion.value + ', ' + suggestion.data);
        },
        onHint: function (hint) {
            $('#autocomplete-ajax-x').val(hint);
        },
        onInvalidateSelection: function() {
            $('#selction-ajax').html('You selected: none');
        }
    });

    var nhlTeams = ['Anaheim Ducks', 'Atlanta Thrashers', 'Boston Bruins', 'Buffalo Sabres', 'Calgary Flames', 'Carolina Hurricanes', 'Chicago Blackhawks', 'Colorado Avalanche', 'Columbus Blue Jackets', 'Dallas Stars', 'Detroit Red Wings', 'Edmonton OIlers', 'Florida Panthers', 'Los Angeles Kings', 'Minnesota Wild', 'Montreal Canadiens', 'Nashville Predators', 'New Jersey Devils', 'New Rork Islanders', 'New York Rangers', 'Ottawa Senators', 'Philadelphia Flyers', 'Phoenix Coyotes', 'Pittsburgh Penguins', 'Saint Louis Blues', 'San Jose Sharks', 'Tampa Bay Lightning', 'Toronto Maple Leafs', 'Vancouver Canucks', 'Washington Capitals'];
    var nbaTeams = ['Atlanta Hawks', 'Boston Celtics', 'Charlotte Bobcats', 'Chicago Bulls', 'Cleveland Cavaliers', 'Dallas Mavericks', 'Denver Nuggets', 'Detroit Pistons', 'Golden State Warriors', 'Houston Rockets', 'Indiana Pacers', 'LA Clippers', 'LA Lakers', 'Memphis Grizzlies', 'Miami Heat', 'Milwaukee Bucks', 'Minnesota Timberwolves', 'New Jersey Nets', 'New Orleans Hornets', 'New York Knicks', 'Oklahoma City Thunder', 'Orlando Magic', 'Philadelphia Sixers', 'Phoenix Suns', 'Portland Trail Blazers', 'Sacramento Kings', 'San Antonio Spurs', 'Toronto Raptors', 'Utah Jazz', 'Washington Wizards'];
    var nhl = $.map(nhlTeams, function (team) { return { value: team, data: { category: 'NHL' }}; });
    var nba = $.map(nbaTeams, function (team) { return { value: team, data: { category: 'NBA' } }; });
    var teams = nhl.concat(nba);

    // Initialize autocomplete with local lookup:
    $('#autocomplete').devbridgeAutocomplete({
        lookup: function (query, done) {
            doc.objectQuery("http://localhost:4527/api/Map/Querybuxiaoqu?xiaoqu=鑫&city=上海市", function (result) {
                if (result.numberData != null) {
                    var realresut = [];
                    var val = {};
                    $.each(result.numberData, function (tindex, tvalue) {
                        val.value = tvalue.name + "-" + tvalue.district + tvalue.address;
                        val.data = tvalue.name + "-" + tvalue.district + tvalue.address;
                        realresut.push(val);
                    });
                    var result = {
                        suggestions: [
                            { "value": "United Arab Emirates", "data": "AE" },
                            { "value": "United Kingdom", "data": "UK" },
                            { "value": "United States", "data": "US" }

                        ]
                    };

                    done(result);
                }
            })
            
        },
        minChars: 1,
        onSelect: function (suggestion) {
            if (suggestion.data == "找不到小区点击这添加小区") {
                alert();
            };
        },
        showNoSuggestionNotice: true,
        noSuggestionNotice: function () {
            return '<span onclick="f();">找不到小区,可手动添加</span>'
            
        },
        groupBy: 'category'
    });
    
    // Initialize autocomplete with custom appendTo:
    $('#autocomplete-custom-append').autocomplete({
        lookup: countriesArray,
        appendTo: '#suggestions-container'
    });

    // Initialize autocomplete with custom appendTo:
    $('#autocomplete-dynamic').autocomplete({
        lookup: countriesArray
    });
    $("#addnew").click(function () {
        alert();
    })
    function f() {
        alert();
    }
});