'use strict';

describe('component test : add', function () {

    // add module reference you want to test
    beforeEach(module('addView'));

    // add templates [from karma]
    beforeEach(module('templates'));

    var $mockScope = {};
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

        element = angular.element('<add-view></add-view>');
        scope.$apply(function () {
            template = $compile(element)(scope);
        });

        $httpBackend.flush();
    }));


    // tests

    it('extra column http get', function () {
        $httpBackend.expectGET('http://localhost:51275/api/ExtraColumn')
            .respond(200, {
                id: 3,
                columnname: 'toefl'
            });
        
    });

    it('interviewees http get', function () {
        $httpBackend.expectGET('http://localhost:51275/api/Interviewee');      
    });

    it('header text', function () {
        var title = element.find('h1');
        expect(title.text()).toContain('Add a new interviewee');
    });

    it('form validation', function () {
        expect(element.isolateScope().intervieweeFrom.$valid).toBe(false);
    });
});
