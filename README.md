# Asp.NETCore 2.0 Vue 2 with WebApi Authorization Starter template

This repo contains an AspNetCore 2 + Vue.js 2 starter template (VS2017) + NetCore WebAPI + OpenIddict authorization. 
---

# Table of Contents

* [Features](#features)
* [TechStack](#techstack)
* [Prerequisites](#prerequisites)
* [Installation - Getting Started!](#installation)
* [Extras](#extras)
* [Special Thanks](#special-thanks)
* [License](#license)
* [Vue & ASP.NET Consulting & training](##looking-for-vue--javascript-or-aspnet-consulting--training--support)

# Features

* Full responsive app accessed by Web or WebAPI (eg. postman client)

![Full responsive app](/Docs/1_start.png)

* Role level permission (admin, manager, user)

![Role level permission](/Docs/2_login_admin.png)

* Role context page display

![Role context page display](/Docs/3_admin_page.png)

* Admin can manage users, users groups, file categories, check users activity logs

![Admin can manage](/Docs/5_users_ibn_groups.png)

* User with role 'manager' has rigth to upload files to selected group and file category or private files with drag and drop functionality

![Manager role](/Docs/7_manager_upload.png)

* Users can download files from groups they belongs to

![Manager role](/Docs/8_user_download.png)


# TechStack

- **ASP.NET Core 2.0**
  - Web API
- **VueJS 2**
  - Vuex (State Store)
- **OpenIddict 2**
  - Easy-to-use OpenID Connect server for ASP.NET Core
- **Webpack 2**
  - HMR (Hot Module Replacement/Reloading)
- **Vuetifyjs**
  - Material Component Framework for Vue

# Prerequisites:
 * nodejs > 6
 * VS2017
 * dotnet core 2.0

# Installation / Getting Started:
 * Clone this repo
 * At the repo's root directory run `dotnet restore`
 * Restore Node dependencies by running `npm install` in ClientApp folder (there is package.json and wepack config files) and this way install all required modules
 * Build the Vue web application (`npm run build`)
 
finally
 
 * Run the application in VSCode or Visual Studio 2017 (Hit `F5`) or command ('dotnet run') - Vue Dev Tools are enebled
 * Browse to [http://localhost:53703](http://localhost:53703)

# Extras

- Get Chrome DevTools for Vue [here](https://chrome.google.com/webstore/detail/vuejs-devtools/nhdogjmejiglipccpnnnanhbledajbpd)

### Special Thanks

List of github repos which have been my inspiration to build this template and may contains part of their source code.

https://github.com/MarkPieszak/aspnetcore-Vue-starter

https://github.com/andersco/FreedomCalculator2

https://github.com/bradyholt/aspnet-core-vuejs-template

https://github.com/0xFireball/PenguinUpload

----

# Found a Bug? Want to Contribute?

Nothing's ever perfect, but please let me know by creating an issue (make sure there isn't an existing one about it already), and we'll try and work out a fix for it! If you have any good ideas, or want to contribute, feel free to either make an Issue with the Proposal, or just make a PR from your Fork.

----

# License

[![MIT License](https://img.shields.io/badge/license-MIT-blue.svg?style=flat)](/LICENSE) 


----

# Looking for Vue / JavaScript or ASP.NET Consulting + Training + support?

Contact me @ <herbat73@yahoo.com>, and let's talk about your projects needs!
