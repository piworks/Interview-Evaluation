angular.
    module('addView').
    component('addView', {
        templateUrl: 'add-view/add-view.template.html',
        controller: ['$scope', 'EmpApi',
            function AddViewController($scope, EmpApi) {
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
                        .then(function (response) {
                            alert("user edited");

                            // add the note
                            EmpApi.AddExtraNotes($scope.extraColumnNotes)
                                .then(function (response) {
                                    alert("note edited");
                                });;

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
                    }
                ];
            }
        ]
    });