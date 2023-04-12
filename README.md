# ImaginationStation
An Instagram-esque app that allows users to post, view, and like art work. This C# project utilized ASP.NET Core framework in order to increase performance and provide built-in secutrity features. I also used Entity Framework to easily connect to MySQL to help with query optimization and scalability.

! [gif of a demo of the app] (http://i.imgur.com/V3N0II3h.gif)

# Features
* Ability to register and login
* Validations on registration include fields must be of a certain length, must input a proper email address, and email address must not already exist within the database
* User must be logged in, in order to create, read, update, and delete posts. Otherwise they will be redirected to the login page
* Users are only able to edit or delete their own posts
* Users are only able to like other users posts
* Ability to view the amount of likes on each post
* When editing a post, the user's original submitted content automatically populates in the input field
