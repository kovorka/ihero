// prevent global execution using IFFE (iffy) - self exectuing anonymous function

(function () {
    "use strict"
    var app = angular.module("myApp", ['ngResource', 'ui.router', 'ui.bootstrap', 'ngAnimate']);

    app.config(['$stateProvider', '$urlRouterProvider',
      function ($stateProvider, $urlRouterProvider) {
          $urlRouterProvider.otherwise("/facialrecognition");

                    $stateProvider
                       .state("FacialRecognition", {
                            url: "/facialrecognition",
                            templateUrl: "app/facialrecognition.html",
                            controller: "FacialRecognitionController as vm"
                        })

      }]);
}());