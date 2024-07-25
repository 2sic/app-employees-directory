<image src="app-icon.png" align="right" width="200px">

# People Directory 4 App for .net CMSs

> This is a [2sxc](https://2sxc.org) App for [DNN ☢️](https://www.dnnsoftware.com/) and [Oqtane 💧](https://www.oqtane.org/)

Employees Directory App for 2sxc / DNN

| Aspect              | Status | Comments or Version
| ------------------- | :----: | -------------------
| 2sxc                | ✅    | requires 2sxc v17.07.00
| Dnn                 | ✅    | For v9.6.1+
| Oqtane              | ✅    | Requires v05.00+
| No jQuery           | ✅    |
| Live Demo           | ➖    |
| Install Checklist   | ✅    | See [Installation](https://azing.org/2sxc/r/2Qsd-qum) on [azing.org](https://azing.org/2sxc)
| Source & License    | ✅    | included, ISC/MIT
| App Catalog         | ✅    | See [app catalog](https://2sxc.org/en/apps/app/people-directory-v4-hybrid-for-dnn-and-oqtane)
| Screenshots         | ✅    | See [app catalog](https://2sxc.org/en/apps/app/people-directory-v4-hybrid-for-dnn-and-oqtane)
| Best Practices      | ✅    | Uses v13.10 conventions
| Bootstrap 3         | ✔️    | Works but not optimized
| Bootstrap 4         | ✅    |
| Bootstrap 5         | ✅    |

## Customize the App

This App uses Google-Maps, and you will need to use your own (free) Maps API key. See the ["change Google Maps API Key" checklist](https://azing.org/2sxc/r/ApSwlItl).

If you want to customize the CSS, you will usually follow the ["Create Custom Styles in a Standard 2sxc App" checklist](https://azing.org/2sxc/r/gg_aB9FD)

## History

* v.4 2021-10
  * Updated to standard v12
  * Tested / works on Oqtane
  * no more jQuery
  * vCard generation now in custom API controller
* v.4.01.03 2022-03
  * enabled data-optimizations
* v.4.02.00 2022-04
  * Changed all access to services to ToSic.Sxc.Services
  * Changed all image tags to use the IImageService and the picture feature
* v.4.03.00 2022-06
  * Replaced all base classes with their 2sxc 14 equivalents
  * Replaced GetService<> with the new ServiceKit14
  * Updated webpack
  * Changed the toolbar configurations to use the IToolbarService
* v.4.04.00 2022-08
  * Added a new details view which can be used on its ow or as details view
  * Added a app settings parameter which decides which details view should be used
* v.04.05.00 2023-02
  * Replaced turnOn Tag with `Kit.Page.TurnOn`
  * Enhanced Kit.Image with `imgAltFallback`
  * Removed _ from Filenames
  * Code in one file the bs5, less duplicated code
* v04.06.00 2023-07
  * 2sxc 16.02 coding conventions
  * everything typed
* v04.17.00 2024-04
  * Strong Typed
  * Auto Generated Class
  * Typed MyItem
* v04.17.01 2024-07
  * minor bugfix incorrect razor which only worked in DNN
* v04.17.02 2024-07
  * Update app.sln and app.csproj
