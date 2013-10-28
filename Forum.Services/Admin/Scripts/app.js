/// <reference path="../libs/underscore.js" />
/// <reference path="../libs/angular.js" />

angular.module("forum", [])
	.config(["$routeProvider", function ($routeProvider) {
	    //console.log($routeProvider);
		$routeProvider
			.when("/", { templateUrl: "Scripts/partials/login.html", controller: LoginController })
			.when("/options", { templateUrl: "Scripts/partials/options.html", controller: OptionsController })
			.when("/posts", { templateUrl: "Scripts/partials/posts.html", controller: PostsController })
			//.when("/comments", { templateUrl: "Scripts/partials/comments.html", controller: CommentsController })
			//.when("/categories", { templateUrl: "Scripts/partials/categories.html", controller: CategoryController })
			.when("/users", { templateUrl: "Scripts/partials/users.html", controller: UserController })
			.when("/posts/edit/:id", { templateUrl: "Scripts/partials/edit-post.html", controller: EditPostsController })
			.when("/users/edit/:id", { templateUrl: "Scripts/partials/edit-user.html", controller: EditUserController })
			.otherwise({ redirectTo: "/" });
	}]);
