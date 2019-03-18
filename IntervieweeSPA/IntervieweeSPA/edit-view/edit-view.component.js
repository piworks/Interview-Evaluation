angular.
    module('editView').
    component('editView', {
        templateUrl: 'edit-view/edit-view.template.html',
        controller: ['$scope', '$routeParams', 'EmpApi',
            function EditViewController($scope, $routeParams, EmpApi) {
                // get the interviewee by id
                var intervieweeId = $routeParams.id;
                var outerScope = this;
                outerScope.listOfExtraNoteIds = [];
                getIntervieweesById();
                function getIntervieweesById() {
                    EmpApi.getIntervieweeById(intervieweeId).then(function (emps) {
                        $scope.emps = emps.data;

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
                    });
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
                        }, {
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
                    for (var i = 0; i < 8 - numberOfExtraColumns; ++i) {
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

                    EmpApi.EditInterviewee(empToEdit, $scope.extraColumnNotes, $scope.cols)
                        .then(function (response) {
                            alert("user edit");

                            // extra column operation names for extranotes
                            var length = $scope.cols.length;
                            for (var i = 0; i < length; ++i) {
                                // console.log($scope.cols[i].columnname);
                                $scope.extraColumnNotes[i].columnname = $scope.cols[i].columnname;
                            }

                            // if it has no extra notes previously, we need to call POST
                            if (outerScope.listOfExtraNoteIds.length == 0) {
                                EmpApi.AddExtraNotes($scope.extraColumnNotes)
                                    .then(function (response) {
                                        alert("note added");
                                    });
                            }
                            // if it has a previous record, we call PUT to edit
                            else {
                                EmpApi.EditExtraNotes($scope.extraColumnNotes)
                                    .then(function (response) {
                                        alert("note edited");
                                    });
                            }
                        });
                }

                // creating input fields dynamically
                // every create a field button click must create two input fields
                // which are for column name and note respectively
                getColumns();
                function getColumns() {
                    EmpApi.getExtraColumns().then(function (cols) {
                        $scope.cols = cols.data;
                    });
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
                    }, {
                        columnname: 'columnname',
                        note: 'note'
                    }, {
                        columnname: 'columnname',
                        note: 'note'
                    }, {
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
            }
        ]
    });