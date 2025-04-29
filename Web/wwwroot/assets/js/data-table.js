// npm package: datatables.net-bs5
// github link: https://github.com/DataTables/Dist-DataTables-Bootstrap5

'use strict';

(function () {

  $('#dataTableExample').DataTable({
    layout: {
      topEnd: {
        search: {
          placeholder: 'جستجو'
        }
      }
    },
    "aLengthMenu": [
      [5, 10, 30, 50, -1],
      [5, 10, 30, 50, "همه"]
    ],
    "iDisplayLength": 10,
    "language": {
      search: ""
    },
    paginationType: 'simple_numbers'
  });

})();