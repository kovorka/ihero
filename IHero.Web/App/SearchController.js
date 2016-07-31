(function () {
    "use strict"
    angular.module("myApp").controller("SearchController", SearchController);


    function SearchController($http, $stateParams) {
        var vm = this;
        vm.test = 1;



    }//end controller

}
());