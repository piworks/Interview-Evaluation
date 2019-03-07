exports.config = {
    framework: 'jasmine',
    seleniumAddress: 'http://localhost:4444/wd/hub',
    specs: ['spec.js'],
    //SELENIUM_PROMISE_MANAGER: false,
    getPageTimeout: 10000,
    multiCapabilities: [
        {
            browserName: 'firefox'
        }, {
            browserName: 'chrome'
        }
    ]
}