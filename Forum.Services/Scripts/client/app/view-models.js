/// <reference path="../libs/_references.js" />

window.vmFactory = (function () {
	var data = null;

	function getLoginViewModel(successCallback) {		
		var viewModel = {
			username: "",
			password: "",
            message: "",
            login: function () {
                var self = this;
				data.users.login(this.get("username"), this.get("password"))
					.then(function () {
						if (successCallback) {
							successCallback();
						}
					}, function (err) {
					    debugger;
					    var text = JSON.parse(err.responseText);
					    self.set("message", text.Message);
					});
			},
			register: function () {
				data.users.register(this.get("username"), this.get("password"))
					.then(function () {
						if (successCallback) {
							successCallback();
						}
					});
			}
		};
		return kendo.observable(viewModel);
	};

	function getCategoriesViewModel() {
	    return data.categories.getAll()
            .then(function (categories) {
                
                var viewModel = {
                    postTitle: "",
                    postContent: "",
                    categories: categories,
                    loggedUsername: localStorage.getItem("username"),
                    message: "",
                    isUserLogged: function () {
                        return data.users.currentUser();
                    },
                    addPost: function () {
                        //data.posts.create();
                    }
                };
                return kendo.observable(viewModel);
            });

	};

	function getPostsViewModel(id) {
	    return data.posts.getByCategoryId(id)
            .then(
                function (postsWithCategoryName) {
                var viewModel = {
                    postTitle: "",
                    postContent: "",
                    postTags: "",
                    categoryName: postsWithCategoryName.Title,
                    categoryDescription: postWithCategoryName.Description,
                    loggedUsername: localStorage.getItem("username"),
                    posts: postsWithCategoryName.Posts,
                    message: "",
                    isUserLogged: function () {
                        return data.users.currentUser();
                    },
                    createPost: function () {
                        var self = this;

                        tags = this.get("postTags").split(',');

                        for (var i in tags) {
                            tags[i] = $.trim(tags[i]).toLowerCase();
                        }
                        
                        var post = {
                            Title: this.get("postTitle"),
                            Content: this.get("postContent"),
                            Tags: tags,
                            CurrentCategoryId: id
                        };

                        if (!post.Title || !post.Content) {
                            this.set("message", "Content or title is empty!");
                            return;
                        }

                        data.posts.create(post)
                            .then(function (createdPost) {
                                self.set("postTitle", "");
                                self.set("postContent", "");
                                self.set("postTags", "");
                                self.get("posts").push(createdPost);
                            }, function (err) {
                                debugger;
                                var text = JSON.parse(err.responseText);
                                self.set("message", text.Message);
                            });
                    }
                };

                return kendo.observable(viewModel);
            });
	}

	function getCurrentPostViewModel(id) {
	    return data.posts.getById(id)
            .then(function (post) {
              
               post.creationDate = post.creationDate.toString().substring(0, 10);

               for (var i in post.comments)
               {
                   post.comments[i].creationDate = post.comments[i].creationDate.toString().substring(0, 10)
               }
                var viewModel = {
                    commentContent: "",
                    post: post,
                    comments: post.comments,
                    loggedUsername: localStorage.getItem("username"),
                    message: "",
                    isUserLogged: function () {
                        return data.users.currentUser();
                    },
                    createComment: function () {
                        var self = this;
                        var comment = {
                            Content: this.get("commentContent"),
                            PostId: this.post.id
                        };

                        if (!comment.Content) {
                            this.set("message", "Content is empty!");
                            return;
                        }
                        data.comments.create(comment)
                            .then(function (data) {
                                data.creationDate = data.creationDate.toString().substring(0, 10);
                                self.set("commentContent", "");
                                self.get("comments").push(data);
                            });
                    }
                };

                return kendo.observable(viewModel);
            });
	}

	return {
	    getLoginVM: getLoginViewModel,
	    getCategoriesVM: getCategoriesViewModel,
	    getPostsVM: getPostsViewModel,
        getCurrentPostVM: getCurrentPostViewModel,
		setPersister: function (persister) {
			data = persister
		}
	};
}());