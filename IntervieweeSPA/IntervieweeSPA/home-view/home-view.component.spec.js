'use strict';

describe('component test : home', function () {

    // add module reference you want to test
    beforeEach(module('homeView'));

    // add templates [from karma]
    beforeEach(module('templates'));

    beforeEach(module('IntervieweeService'));

    var element;
    var scope;
    var $httpBackend;
    var template;
    var res = [
        {
            id: 3,
            columnname: 'toefl'
        }
    ];
    var $rootScope;
   

    beforeEach(inject(function (_$rootScope_, $compile, _$httpBackend_) {
        $httpBackend = _$httpBackend_;
        $rootScope = _$rootScope_;

        $httpBackend.when('GET', 'http://localhost:51275/api/ExtraColumn')
            .respond(200, res);

        $httpBackend.when('GET', 'http://localhost:51275/api/Interviewee')
            .respond(200, res);

        scope = $rootScope.$new();

        element = angular.element('<home-view></home-view>');
        scope.$apply(function () {
            template = $compile(element)(scope);
        });

        $rootScope.$digest();

    }));


    // tests

    it('extra column http get', function () {
        $httpBackend.expectGET('http://localhost:51275/api/ExtraColumn');

    });

    it('interviewees http get', function () {
        $httpBackend.expectGET('http://localhost:51275/api/Interviewee');

    });


    it('header text', function () {
        var title = element.find('h1');
        expect(title.text()).toContain('Interviewees');
    });


});
