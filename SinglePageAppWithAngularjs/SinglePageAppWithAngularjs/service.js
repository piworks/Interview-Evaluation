var IntervieweeService = angular.module('IntervieweeService', ['ngMessages']);

IntervieweeService.factory('EmpApi', function ($http) {
    var urlBase = "http://localhost:51275/api";

    var EmpApi = {};

    // EXTRA NOTE
    EmpApi.getExtraNotes = function () {
        return $http.get(urlBase + '/ExtraNote');
    };

    EmpApi.AddExtraNotes = function (emp) {
        return $http.post(urlBase + '/ExtraNote/', emp);
    };

    EmpApi.EditExtraNotes = function (emp) {
        return $http.put(urlBase + '/ExtraNote/', emp);
    };

    // EXTRA COLUMN
    EmpApi.getExtraColumns = function () {
        return $http.get(urlBase + '/ExtraColumn');
    };

    EmpApi.AddExtraColumns = function (emp) {
        return $http.post(urlBase + '/ExtraColumn/', emp);
    }


    // INTERVIEWEE
    EmpApi.getInterviewees = function () {
        return $http.get(urlBase + '/Interviewee')
    };

    EmpApi.getIntervieweeById = function (emp) {
        return $http.get(urlBase + '/Interviewee/' + emp)
    };

    EmpApi.AddInterviewee = function (emp) {
        return $http.post(urlBase + '/Interviewee/', emp);
    }

    EmpApi.EditInterviewee = function (empToUpdate) {
        var request = $http({
            method: 'PUT',
            url: urlBase + '/Interviewee/' + empToUpdate.id,
            data: empToUpdate
        });
        return request;
    }

    EmpApi.DeleteInterviewee = function (empToDelete) {
        var request = $http({
            method: 'delete',
            url: urlBase + '/Interviewee/' + empToDelete.id,
            data: empToDelete
        });
        return request;
    }

    return EmpApi;
});