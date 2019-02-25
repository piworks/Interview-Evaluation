'use strict';

angular.
  module('intervieweeApp').
  config(['$routeProvider',
    function config($routeProvider) {
      $routeProvider.
        when('/home', {
          template: '<home-view></home-view>'
        }).
        when('/edit/:id', {
            template: '<edit-view></edit-view>'
        }).
        when('/add', {
            template: '<add-view></add-view>'
        }).
        otherwise('/home');
    }
    ]);

