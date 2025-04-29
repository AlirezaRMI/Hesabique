// npm package: typeahead.js
// github link: https://github.com/twitter/typeahead.js

'use strict';

(function () {

  const statesArray = ['مرکزی', 'اردبیل', 'آذربایجان غربی', 'اصفهان', 'خوزستان',
    'ایلام', 'خراسان شمالی', 'هرمزگان', 'بوشهر', 'خراسان جنوبی', 'آذربایجان شرقی',
    'تهران', 'لرستان', 'گیلان', 'سیستان و بلوچستان', 'زنجان', 'مازندران', 'سمنان',
    'کردستان', 'چهارمحال و بختیاری', 'فارس', 'قزوین', 'قم',
    'البرز', 'کرمان', 'کرمانشاه', 'گلستان', 'خراسان رضوی', 'همدان',
    'چهارمحال و بختیاری', 'کهکیلویه و بویراحمد', 'یزد'
  ];


  // 1. Basic Example
  const substringMatcher = function (strs) {
    return function findMatches(q, cb) {
      let matches, substringRegex;

      // an array that will be populated with substring matches
      matches = [];

      // regex used to determine if a string contains the substring `q`
      var substrRegex = new RegExp(q, 'i');

      // iterate through the pool of strings and for any string that
      // contains the substring `q`, add it to the `matches` array
      for (var i = 0; i < strs.length; i++) {
        if (substrRegex.test(strs[i])) {
          matches.push(strs[i]);
        }
      }

      cb(matches);
    };
  };

  $('#the-basics .typeahead').typeahead({
    hint: true,
    highlight: true,
    minLength: 1
  }, {
    name: 'states',
    source: substringMatcher(statesArray)
  });




  // 2. Bloodhound (Suggestion Engine) Example
  // constructs the suggestion engine
  const states = new Bloodhound({
    datumTokenizer: Bloodhound.tokenizers.whitespace,
    queryTokenizer: Bloodhound.tokenizers.whitespace,
    // `statesArray` is an array of state names defined in "The Basics"
    local: statesArray
  });

  $('#bloodhound .typeahead').typeahead({
    hint: true,
    highlight: true,
    minLength: 1
  }, {
    name: 'states',
    source: states
  });

})();