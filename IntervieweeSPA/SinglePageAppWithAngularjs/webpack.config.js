var HtmlWebpackPlugin = require('html-webpack-plugin');

module.exports = {
    
    entry: "./main.js", //relative to root of the application
    output: {
        path: __dirname,
        filename: "app.bundle.js" //relative to root of the application
    },
    watchOptions: {
        aggregateTimeout: 300,
        poll: 1000
    },
    plugins: [
        new HtmlWebpackPlugin({
            hash: true,
            title: 'My Awesome application',
            myPageHeader: 'interviewee',
            template: './_index.html',
            filename: 'index.html' //relative to root of the application
        })
    ]
}



// module.exports = {
//     entry: './main.js',
//     output: {
//         filename: './bundle.js'
//     }
// };