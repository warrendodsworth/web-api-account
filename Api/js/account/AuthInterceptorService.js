﻿(function () {
  'use strict';

  angular
    .module('app')
    .factory('AuthInterceptorService', AuthInterceptorService);

  AuthInterceptorService.$inject = ['$q', '$location', 'localStorageService'];

  function AuthInterceptorService($q, $location, localStorageService) {

    var service = {};

    service.request = function (config) {

      config.headers = config.headers || {};

      var authData = localStorageService.get('authorizationData');
      if (authData) {
        config.headers.Authorization = 'Bearer ' + authData.token;
      }

      return config;
    };

    service.responseError = function (rejection) {
      if (rejection.status === 401) {
        $location.path('/login');
      }
      return $q.reject(rejection);
    };

    return service;
  }
})();