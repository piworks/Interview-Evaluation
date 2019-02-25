angular.
    module('homeView').
    component('homeView', {
        templateUrl: 'home-view/home-view.template.html',
        controller: ['$scope', '$route', 'EmpApi',
            function HomeViewController($scope, $route, EmpApi) {
                // for refreshing the page
                var vm = this;

                // lists all the interviewees registered to the system
                getInterviewees();
                function getInterviewees() {
                    EmpApi.getInterviewees().then(function (emps) {
                        $scope.emps = emps.data;
                    });
                }

                // gets the list of column that are added as extra columns 
                getColumns();
                function getColumns() {
                    EmpApi.getExtraColumns().then(function (cols) {
                        $scope.cols = cols.data;
                    });
                }


                // adds an extra column to the table with the given name
                $scope.addColumn = function () {
                    var colToAdd = {
                        'columnname': $scope.newcolumn,
                    };

                    EmpApi.AddExtraColumns(colToAdd)
                        .success(function (response) {
                            alert("column added");
                        }).
                        error(function (response) {
                            alert("error in adding");
                        }
                        );
                }


                // adds the ability to sort the table by clicking the desired column
                $scope.sortColumn = "name";
                $scope.reverseSort = false;
                $scope.sortData = function (column) {
                    $scope.reverseSort = ($scope.sortColumn == column) ? !$scope.reverseSort : false;
                    $scope.sortColumn = column;
                }
                $scope.getSortClass = function (column) {
                    if ($scope.sortColumn == column) {
                        return $scope.reverseSort ? 'arrow-down' : 'arrow-up';
                    }
                    return '';
                }


                // removes an interviewee from the table
                $scope.deleteEmpById = function (idToBeDeleted) {
                    var empToDelete = {
                        'id': idToBeDeleted
                    };

                    EmpApi.DeleteInterviewee(empToDelete).then(function (response) {
                        $scope.id = undefined;
                    });

                    // refreshes the list of interviewees after the deletion
                    vm.reloadData = function () {
                        $route.reload();
                    }
                    vm.reloadData();
                }
            }
        ]
    });