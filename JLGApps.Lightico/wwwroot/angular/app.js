var templateDatagrid = angular.module('templateDatagrid', []);

templateDatagrid.controller('templateController', ['$scope', '$http', function ($scope, $http) {


    $scope.rows = [{ id: 1,  Category: "Plantiff Profile Form", DocumentTitle: "RUP Update and Review Letter", Docket: "RUP", DateCreated: "08/29/2020" },
        { id: 2, Category: "Plantiff Fact Sheet", DocumentTitle: "Talc Update and Review Letter", Docket: "TLC", DateCreated: "08/01/2020" }];

    $http({

        method: 'GET',

        url: '/api/EmailTemplateDetails'


    }).then(function success(response) {

        // this function will be called when the request is success
        $scope.rows=response.data
     
    }, function error(response) {

        // this function will be called when the request returned error status
    });

}]);