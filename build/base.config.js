const args = process.argv
const isProduction = args.indexOf('--prod', 0) >= 0

let CurrentEnvironment = require('./base.dev.config.js')
if (isProduction) {
  CurrentEnvironment = require('./base.prod.config.js')
}

// Please understand that the default are for the production. It can be overriden in the specific
// configuration file such as :
// - base.dev.config: Development configuration overrides
// - base.prod.config: Production configuration overrides
module.exports = {
  // Define if the build is in production or development. Please don't override this value.
  // to trigger a build as in production, use: npm run build -- --prod
  isProduction: isProduction,

  // Define what is your base URI. Normally, if your site is http://www.something.com/ you don't
  // have anything to change here. Otherwise if your site is http://www.something.com/my-application/
  // then you would put the base uri as '/my-application'.
  //
  // default: '/'
  baseUriPath: CurrentEnvironment.hasOwnProperty('baseUriPath') ? CurrentEnvironment.baseUriPath : '/',

  // Define if you want to have the map file generated in order to mainly debug your application. Don't
  // forget that normally you don't want this in production.
  //
  // default: false
  generateMapFiles: CurrentEnvironment.hasOwnProperty('generateMapFiles') ? CurrentEnvironment.baseUriPath : false
}
