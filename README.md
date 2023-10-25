# LetsGame

A board game group event scheduling app. Made to help busy people find more time to play board games.

The idea was inspired by noticing how much time my friends and I would spend deciding on which games to play instead of actually playing them.

## Scope of Project

I undertook this project as part of my learning ASP.NET and web app development. For this reason covering some of the most fundemental elements was my true goal and I learned more from this single project than I ever could have from simply reading the docs. 

### Areas I wanted to cover:

  - [x] Entity Framework
    - [x] SQL Server
    - [x] Fluid API
  - [x] ASP.NET Core + Razor Pages
    - [x] Partial views
    - [x] Layout pages
    - [x] Routing
    - [x] Dependency Injection
      - [x] CRUD Event Manager Class
      - [x] CRUD Friend Manager Class
    - [x] Site Areas
    - [ ] REST API: This is not fully implemented yet. Original intention was for data to be accesible for an associated Discord bot.
    - [ ] Allow Interaction with BGG API: https://boardgamegeek.com/wiki/page/BGG_XML_API2. Users can manage collections of games they have or want to play.
    - [ ] Bussiness Logic
      - [x] Create Database Schema
      - [x] Create Object Models for Entity Framework.
      - [ ] User Notification System: One of the first things I'd add.
      - [x] Create Events, View Event, Modify Event if event creator
      - [ ] Event Roles: Creator can assign trusted permissions to some participants. Would like to add this feature.
      - [x] Invite to Events accept invites/join Events
      - [x] Create, Vote on Polls
      - [ ] Notify event participants of updates regarding events.
      - [ ] Allow user to change user profile picture and other app specific details.
      - [ ] Allow selection of an image for the event
      - [x] Send/Recieve/Accept friend requests. Invite friends to events you are part of.
      - [ ] User chat. Not something originally considered to be within the scope of the project. As the project developed it became clear chat would be necessary.
    
### I even learned a few things I didn't set out to:

- AJAX
- JQuery
- DOM manipulation
      
### Things I did **not** intend to accomplish.

- UI Focus. In my learning timeline I has just finsihed a pure HTML/CSS project and wanted to concentrate more on developing my backend understanding. The site is rendered using the boostrap css library.

## State of the project

The project is currently unfinished. I implemented all of the *"core"* functionality. However, some big areas such as user chat and user notifications remain un-implemented.

In addition, much of the code is in need of a refactor. My lack of knowledge at the time means that the database queries are not properly optimised.

Updating the UI would be a huge must. I might consider a clientside framework to create the UI.

Implementing the ability to fecth games from BGG (Board game geek has a public API) would make the polls feel more important.

Logic that calculates which games can be played during the event duration would help guide the selection choices of the group.

## Conclusion

I hope/plan to return to this project in the future but for now I learned a lot and that was my main goal.
