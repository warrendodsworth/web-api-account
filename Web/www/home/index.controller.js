(function () {
  'use strict';

  angular.module('controllers').controller('IndexController', IndexController);

  IndexController.$inject = ['$scope', '$http', '$location'];

  function IndexController($scope, $http, $location) {
    var vm = $scope;
    vm.$parent.title = 'Wow';

    $http.get('/api/posts').then(function (res) {
      vm.posts = res.data;
    });
  }
}());