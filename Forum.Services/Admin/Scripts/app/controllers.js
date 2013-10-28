var Username = localStorage.getItem("username");
var SessionKey = localStorage.getItem("sessionKey");

function saveUserData(userData) {
    localStorage.setItem("username", userData.username);
    localStorage.setItem("sessionKey", userData.sessionKey);
    Username = userData.username;
    SessionKey = userData.sessionKey;
}

function clearUserData() {
    localStorage.removeItem("username");
    localStorage.removeItem("sessionKey");
    Username = null;
    SessionKey = null;
}

function AddGreet() {

    $('#log-out-btn').show();
}

function RemoveGreet() {
    $('#log-out-btn').hide();
}

function LoginController($scope, $http) {
    RemoveGreet();

    $('#login-btn').on('click', function () {

        var currentUser = {
            Username: $('#tb-username').val(),
            AuthCode: CryptoJS.SHA1($('#tb-password').val()).toString()
        };

        $http({
            method: "POST",
            url: "..//api/users/login",
            headers: { "Authorization": "Bearer " },
            contentType: "application/json",
            data: currentUser
        })
           .success(function (data) {
               saveUserData(data);
               AddGreet();
               window.location.href = "/Admin/index.html#/options";
           });
    })

    $('#register-btn').on('click', function () {

        var currentUser = {
            Username: $('#tb-username').val(),
            AuthCode: CryptoJS.SHA1($('#tb-password').val()).toString()
        };

        $http({
            method: "POST",
            url: "..//api/users/register",
            headers: { "Authorization": "Bearer " },
            contentType: "application/json",
            data: currentUser
        })
           .success(function (data) {
               saveUserData(data);
               AddGreet();
               window.location.href = "/Admin/index.html#/options";
           });
    })

    $('#log-out-btn').on('click', function () {
        $http({
            method: "PUT",
            url: "..//api/users/logout?sessionKey=" + SessionKey,
            headers: { "Authorization": "Bearer " },
            contentType: "application/json"
        })
       .success(function (data) {
           RemoveGreet();
           clearUserData();
       })
    })
}

function OptionsController($scope, $http) {
    //automatically included
}

function PostsController($scope, $http) {
    $scope.newPost = {
        Title: "",
        Tontent: "",
        Category: ""
    };

    $http({
        method: "GET",
        url: "..//api/posts",
        headers: { "Authorization": "Bearer " },
        contentType: "application/json"
    })
    .success(function (posts) {
        console.log(posts);
        $scope.posts = posts;
        $scope.categories = _.uniq(_.pluck(posts.Result, "Category"));
        $scope.selectedCategory = $scope.categories[0];
    });

    $scope.addPost = function () {
        $http({
            method: "POST",
            url: "..//api/posts/create",
            data: $scope.newPost,
            contentType: "application/json"
        })
        $scope.posts.push($scope.newPost);
        var category = $scope.newPost.Category;
        if (!_.contains($scope.categories, category)) {
            $scope.categories.push(category);
        }

        $scope.newPost = {
            Title: "",
            Tontent: "",
            Category: ""
        };
    }

    $('#wrapper').on('click', '.remove-post', function (ev) {
        var id = ev.target.id;
        $http({
            method: "DELETE",
            url: "..//api/posts/delete-by-id?sessionKey=" + SessionKey + "&id=" + id,
            headers: { "Authorization": "Bearer " },
            contentType: "application/json"
        })
        .success(function (categories) {
            console.log("REMOVED");
            window.location.href = "/Admin/index.html#/options";
        });
    })

    //$('#remove-post').on('click', function () {
    //    alert('remove post clicked');

    //})
}

function UserController($scope, $http) {
    $http({
        method: "GET",
        url: "..//api/users/all",
        headers: { "Authorization": "Bearer " },
        contentType: "application/json"
    })
	.success(function (users) {
	    console.log(users);
	    $scope.users = users;
	});

    $('#wrapper').on('click', '.remove-user', function (ev) {
        var id = ev.target.id;

        $http({
            method: "DELETE",
            url: "..//api/users/delete-by-id?sessionKey=" + SessionKey + "&id=" + id,
            headers: { "Authorization": "Bearer " },
            contentType: "application/json"
        })
        .success(function (categories) {
            console.log("REMOVED");
            window.location.href = "/Admin/index.html#/options";
        });
    })
};

function EditPostsController($scope, $http, $routeParams) {
    var id = $routeParams.id;

    $scope.postID = id;

    $('#change-post-btn').on('click', function () {
        var edited = {
            "Id": id,
            "Content": $('#tb-newContent').val()
        };

        $http({
            method: "PUT",
            url: "..//api/posts/edit-by-id?sessionKey=" + SessionKey,
            headers: { "Authorization": "Bearer " },
            contentType: "application/json",
            data: edited
        })
        .success(function (users) {
            alert('Edited!');
            window.location.href = "/Admin/index.html#/options";
        });
    })
};

function EditUserController($scope, $http, $routeParams) {
    var id = $routeParams.id;

    $scope.postID = id;

    $('#change-user-btn').on('click', function () {

        var content;
        if ($('#ban').val() == 'true') {
            content = true;
        }
        else {
            content = false;
        }

        var edited = {
            "Id": id,
            "IsBanned": content
        };

        $http({
            method: "PUT",
            url: "..//api/users/edit-by-id?sessionKey=" + SessionKey,
            headers: { "Authorization": "Bearer " },
            contentType: "application/json",
            data: edited
        })
        .success(function (users) {
            alert('Edited!');
            window.location.href = "/Admin/index.html#/options";
        });
    })


};

//function CommentsController($scope, $http) {
//    $http({
//        method: "GET",
//        url: "..//api/comments",
//        headers: { "Authorization": "Bearer " },
//        contentType: "application/json"
//    })
//	.success(function (comments) {
//	    console.log(comments);
//	    $scope.comments = comments;
//	});
//}

//function CategoryController($scope, $http) {
//    $http({
//        method: "GET",
//        url: "..//api/categories/all",
//        headers: { "Authorization": "Bearer " },
//        contentType: "application/json"
//    })
//	.success(function (categories) {
//	    console.log(categories);
//	    $scope.categories = categories;
//	});
//}

//----------------other click events------------------





$(document).ready(function () {

});