// spec.js
describe('Protractor Demo App', function() {

    describe('home-view', function(){
        
        beforeEach(async function(){

            await browser.get('http://localhost:49335/index.html#!/home');

        });

        it('page title test', function() {

            expect(browser.getTitle()).toEqual('Interviewee Evaluation');

        });
    
    
        it('number of interviewees', function() {

            var numberOfInterviewees = element.all(by.repeater('a in emps'));
            expect(numberOfInterviewees.count()).toEqual(4);

        });

        it('sorted by firstname', async function(){

            await element(by.css("[ng-click=\"sortData('firstname')\"]")).click();
            var firstname = element.all(by.repeater('a in emps')).all(by.css('td'));     
            expect(await firstname.get(0).getText()).toEqual('abraham');

        });

    })

    describe('add-view', function(){

        beforeEach(async function(){

            await browser.get('http://localhost:49335/index.html#!/add');

        });

        it('add interviewee button', async function() {

            var addButton = await element(by.id('addButton'));
            expect(addButton.isEnabled()).toEqual(false);

        });

        it('add from validation', async function() {
            
            await element(by.model('firstname')).sendKeys('ibrahim');
            await element(by.model('lastname')).sendKeys('yazici');
            await element(by.model('email')).sendKeys('ibrahim@gmail.com');
            await element(by.model('university')).sendKeys('GTU');
            await element(by.model('githublink')).sendKeys('github.com/ibrhmyzc');
            await element(by.model('bamboolink')).sendKeys('bamboo/ibrahimyazici');
            await element(by.model('backendnote')).sendKeys(50);
            await element(by.model('frontend')).sendKeys(50);
            await element(by.model('algorithms')).sendKeys(50);
            await element(by.model('specialnote')).sendKeys('e2e');

            var addButton = element(by.id('addButton'));
            expect(await addButton.isEnabled()).toEqual(true);
           
        });

    })

});