var MyApp = angular.module("MyApp", [
    'ngRoute',
    'ngMessages',
    'IntervieweeService'
    ]
);

MyApp.config(['$routeProvider',
    function ($routeProvider) {
        $routeProvider.
            when('/Add', {
                templateUrl: 'Views/add.html',
                controller: 'AddController'
            }).
            when('/Edit/:id', {
                templateUrl: 'Views/edit.html',
                controller: 'EditController'
            }).
            when('/Delete', {
                templateUrl: 'Views/delete.html',
                controller: 'DeleteController'
            }).
            when('/Home', {
                templateUrl: 'Views/home.html',
                controller: 'HomeController'
            }).
            otherwise({
                redirectTo: '/Home'
            });
    }]
);

MyApp.controller("AddController", function ($scope, EmpApi) {

    $scope.addEmp = function (numberOfExtraColumns) {
 
        //maximum number of extra columns are limited by 3
        $scope.extraColumnNotes = [
            {
                'intervieweeid': -1,
                'columnname': 'columnname',
                'note': 'note'
            }, {
                'intervieweeid': -1,
                'columnname': 'columnname',
                'note': 'note'
            }, {
                'intervieweeid': -1,
                'columnname': 'columnname',
                'note': 'note'
            }
        ];

        // to add the items from view
        for (var i = 0; i < numberOfExtraColumns; ++i) {
            $scope.extraColumnNotes[i].note = $scope.choices[i].note;
        }


        // to remove the unnecessary items
        for (var i = 0; i < 3 - numberOfExtraColumns; ++i) {
            $scope.extraColumnNotes.splice(-1, 1);
        }

        // these values come from the view
        var empToAdd = {
            'firstname': $scope.firstname,
            'lastname': $scope.lastname,
            'email': $scope.email,
            'university': $scope.university,
            'githublink': $scope.githublink,
            'bamboolink': $scope.bamboolink,
            'backendnote': $scope.backendnote,
            'frontend': $scope.frontend,
            'algorithms': $scope.algorithms,
            'specialnote': $scope.specialnote
        };
    
        EmpApi.AddInterviewee(empToAdd, $scope.extraColumnNotes)
            .success(function (response) {
                alert("user edited");

                // add the note
                EmpApi.AddExtraNotes($scope.extraColumnNotes)
                    .success(function (response) {
                        alert("note edited");
                    }).
                    error(function (response) {
                        alert("note error");
                    });

            }).
            error(function (response) {
                alert("error in adding");
            });

    }


    // creating input fields dynamically
    // every create a field button click must create two input fields
    // which are for column name and note respectively
    getColumns();
    function getColumns() {
        EmpApi.getExtraColumns().success(function (cols) {
            $scope.cols = cols;
        }).error(function (error) {
            $scope.status = "unable to load columns";
        })
    }
   
    // initially empmty
    $scope.choices = [
        {
            columnname: 'columnname',
            note: 'note'
        }, {
            columnname: 'columnname',
            note: 'note'
        }, {
            columnname: 'columnname',
            note: 'note'
        }
    ];
  
});

MyApp.controller("DeleteController", function ($scope, EmpApi) {

    console.log("delete");
    $scope.deleteEmp = function () {
        var empToDelete = {
            'id': $scope.id
        };
        console.log($scope.id + " is being deleted");
        EmpApi.DeleteInterviewee(empToDelete)
            .success(function (response) {
                alert("user deleted");
                $scope.id = undefined;
            }).
            error(function (response) {
                alert('error at deleting');
            });
    }

});

MyApp.controller("HomeController", function ($scope, $route, EmpApi) {


    // for refreshing the page
    var vm = this;

    // lists all the interviewees registered to the system
    getInterviewees();
    function getInterviewees() {
        EmpApi.getInterviewees().success(function (emps) {
            $scope.emps = emps;
        })
            .error(function (error) {
                $scope.status = 'unable to load ' + error.message;
            })
    }

    // gets the list of column that are added as extra columns 
    getColumns();
    function getColumns() {
        EmpApi.getExtraColumns().success(function (cols) {
            $scope.cols = cols;
        }).error(function (error) {
            $scope.status = "unable to load columns";
        })
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

        EmpApi.DeleteInterviewee(empToDelete)
            .success(function (response) {

                $scope.id = undefined;
            }).
            error(function (response) {
                alert('error at deleting');
            });

        // refreshes the list of interviewees after the deletion
        vm.reloadData = function () {
            $route.reload();
        }
        vm.reloadData();
    }

});

MyApp.controller("EditController", function ($scope, $routeParams, EmpApi) {


    // get the interviewee by id
    var intervieweeId = $routeParams.id;
    var outerScope = this;
    outerScope.listOfExtraNoteIds = [];
    getIntervieweesById();
    function getIntervieweesById() {
        EmpApi.getIntervieweeById(intervieweeId).success(function (emps) {
            $scope.emps = emps;

            // here we initialize the fields with previous records
            $scope.firstname = $scope.emps.firstname;
            $scope.lastname = $scope.emps.lastname; 
            $scope.email = $scope.emps.email;
            $scope.university = $scope.emps.university;
            $scope.githublink = $scope.emps.githublink;
            $scope.bamboolink = $scope.emps.bamboolink;
            $scope.backendnote = $scope.emps.backendnote;
            $scope.frontend = $scope.emps.frontend;
            $scope.algorithms = $scope.emps.algorithms;
            $scope.specialnote = $scope.emps.specialnote;

            for (var i = 0; i < $scope.emps.extranotes.length; ++i) {    
                $scope.choices[i].note = $scope.emps.extranotes[i].note;
                outerScope.listOfExtraNoteIds.push($scope.emps.extranotes[i].id);
            }
        })
            .error(function (error) {
                $scope.status = 'unable to load ' + error.message;
            })
    }


    $scope.editEmp = function (numberOfExtraColumns) {
    
        //maximum number of extra columns are limited by 3
        $scope.extraColumnNotes = [
            {
                'intervieweeid': $routeParams.id,
                'columnname': 'columnname',
                'note': 'note'
            }, {
                'intervieweeid': $routeParams.id,
                'columnname': 'columnname',
                'note': 'note'
            }, {
                'intervieweeid': $routeParams.id,
                'columnname': 'columnname',
                'note': 'note'
            }
        ];

        // to add the items from view
        for (var i = 0; i < numberOfExtraColumns; ++i) {
            $scope.extraColumnNotes[i].note = $scope.choices[i].note;
        }


        // to remove the unnecessary items
        for (var i = 0; i < 3 - numberOfExtraColumns; ++i) {
            $scope.extraColumnNotes.splice(-1, 1);
        }

        var empToEdit = {
            'id': $routeParams.id,
            'firstname': $scope.firstname,
            'lastname': $scope.lastname,
            'email': $scope.email,
            'university': $scope.university,
            'githublink': $scope.githublink,
            'bamboolink': $scope.bamboolink,
            'backendnote': $scope.backendnote,
            'frontend': $scope.frontend,
            'algorithms': $scope.algorithms,
            'specialnote': $scope.specialnote
        };

        EmpApi.EditInterviewee(empToEdit, $scope.extraColumnNotes)
            .success(function (response) {
                alert("user edit");

                console.log("length = " + outerScope.listOfExtraNoteIds.length);

                // if it has no extra notes previously, we need to call POST
                if (outerScope.listOfExtraNoteIds.length == 0) {
                    EmpApi.AddExtraNotes($scope.extraColumnNotes)
                        .success(function (response) {
                            alert("note added");
                        }).
                        error(function (response) {
                            alert("note error");
                        });
                }
                // if it has a previous record, we call PUT to edit
                else {
                    EmpApi.EditExtraNotes($scope.extraColumnNotes)
                        .success(function (response) {
                            alert("note edited");
                        }).
                        error(function (response) {
                            alert("note error");
                        });
                }
                

            }).
            error(function (response) {
                alert('error at editing');
            });


    }

    // creating input fields dynamically
    // every create a field button click must create two input fields
    // which are for column name and note respectively
    getColumns();
    function getColumns() {
        EmpApi.getExtraColumns().success(function (cols) {
            $scope.cols = cols;
        }).error(function (error) {
            $scope.status = "unable to load columns";
        })
    }

    // initially empmty
    $scope.choices = [
        {
            columnname: 'columnname',
            note: 'note'
        }, {
            columnname: 'columnname',
            note: 'note'
        }, {
            columnname: 'columnname',
            note: 'note'
        }
    ];

});