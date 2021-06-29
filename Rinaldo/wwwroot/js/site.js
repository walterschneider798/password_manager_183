// Please see documentation at https://docs.microsoft.com/aspnet/core/client-side/bundling-and-minification
// for details on configuring this project to bundle and minify static web assets.

// Write your JavaScript code.

var userEmail;
var userPassword;
var userHashedPassword;
var userId;
var userSalt;

$(document).ready(function () {
    if (localStorage.getItem("email") != null) {
        $.ajax('/login/authenticateuser?email=' + localStorage.getItem("email") + '&password=' + localStorage.getItem("password"),   // request url
            {
                dataType: 'json',
                success: function (data, status, xhr) {// success callback function
                    if (data != "No User") {
                        userEmail = localStorage.getItem("email");
                        userHashedPassword = data.password;
                        userPassword = localStorage.getItem("password");
                        userId = data.id;
                        userSalt = data.salt;

                        localStorage.setItem("email", userEmail);
                        localStorage.setItem("hashedPassword", userHashedPassword);
                        localStorage.setItem("password", userPassword);
                        localStorage.setItem("id", userId);
                        localStorage.setItem("salt", userSalt);

                        $(".loginBox").fadeOut(function () {
                            $(".passwordList").fadeIn();
                        });
                        loadAllPasswords(userEmail, userPassword);
                    }

                },
                error: function () {
                    localStorage.clear();
                }
            });
    }
    else {
        $("#loginButton").click(function () {
            var email = $("#emailInput").val();
            var password = $("#passwordInput").val();
            $.ajax('/login/authenticateuser?email=' + email + '&password=' + password,   // request url
                {
                    dataType: 'json',
                    success: function (data, status, xhr) {// success callback function
                        userEmail = data.email;
                        userHashedPassword = data.password;
                        userPassword = password;
                        userId = data.id;
                        userSalt = data.salt;

                        localStorage.setItem("email", userEmail);
                        localStorage.setItem("hashedPassword", userHashedPassword);
                        localStorage.setItem("password", userPassword);
                        localStorage.setItem("id", userId);
                        localStorage.setItem("salt", userSalt);

                        $(".loginBox").fadeOut(function () {
                            $(".passwordList").fadeIn();
                        });
                        loadAllPasswords(userEmail, userPassword);
                        $("#loginMessage").html("");

                    },
                    error: function () {
                        $("#loginMessage").html("Login Credentials are wrong");
                    }
                });
        });

        $("#createUser").click(function () {
            var email = $("#newEmailInput").val();
            var password = $("#newPasswordInput").val();
            $.ajax('/login/AddUser?email=' + email + '&password=' + password,   // request url
                {
                    dataType: 'json',
                    success: function (data, status, xhr) {// success callback function
                        userEmail = data.email;
                        userHashedPassword = data.password;
                        userPassword = password;
                        userId = data.id;
                        userSalt = data.salt;

                        localStorage.setItem("email", userEmail);
                        localStorage.setItem("hashedPassword", userHashedPassword);
                        localStorage.setItem("password", userPassword);
                        localStorage.setItem("id", userId);
                        localStorage.setItem("salt", userSalt);

                        $(".loginBox").fadeOut(function () {
                            $(".passwordList").fadeIn();
                        });
                        loadAllPasswords(userEmail, userPassword);
                        $("#registerMessage").html("");

                    },
                    error: function () {
                        $("#registerMessage").html("Ein Benutzer mit die gleiche E-Mail Adresse existiert bereits.");
                    }
                });
        });

        $(".addPasswordBox button").click(function () {
            var name = $("#passwordName").val();
            var password = $("#passwordValue").val();
            $.ajax('/passwordchecker/AddPassword?email=' + userEmail + '&password=' + userPassword + '&name=' + name + '&addedPassword=' + password,   // request url
                {
                    success: function (data, status, xhr) {// success callback function
                        loadAllPasswords(userEmail, userPassword);
                    },
                    error: function () {
                    }
                });
        });
        $("#createNewUser").click(function () {
            $(".newUserBox").slideDown();
        });

    }
    $('input').keypress(function (e) {
        if (e.which == 13) {
            $(this).parent().siblings("button").eq(0).click();
        }
    });
    function loadAllPasswords(email, password) {
        $.ajax('/passwordchecker/GetAllPasswords?email=' + email + '&password=' + password,   // request url
            {
                dataType: 'json',
                success: function (data, status, xhr) {// success callback function
                    $(".passwordUl").html("");
                    $.each(data, function (i, item) {
                        $(".passwordUl").append("<li><div class='passwordName'>" + item.name + "</div><div class='passwordValue'>" + item.password + "</div></li>")
                    });
                },
                error: function () {

                }
            });
    }
    $("#logout").click(function () {
        logOut();
    });
    function logOut() {
        $(".passwordList").fadeOut(function () {
            $(".passwordUl").html("");
            localStorage.clear();
            $(".loginBox").fadeIn();
        });
    }
});
