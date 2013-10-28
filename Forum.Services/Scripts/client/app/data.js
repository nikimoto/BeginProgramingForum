/// <reference path="../../_references.js" />

window.persisters = (function () {
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


	var DataPersister = Class.create({
	    init: function (apiUrl) {
	        this.apiUrl = apiUrl;
	        this.users = new UsersPersister(apiUrl + "users/");
	        this.posts = new PostsPersister(apiUrl + "posts/");
	        this.categories = new CategoriesPersister(apiUrl + "categories/");
	        this.comments = new CommentsPersister(apiUrl + "comments/");
	    }
	});

	var UsersPersister = Class.create({
		init: function (apiUrl) {
			this.apiUrl = apiUrl;
		},
		login: function (username, password) {
			var user = {
				username: username,
				authCode: CryptoJS.SHA1(password).toString()
			};
			return httpRequester.POST(this.apiUrl + "login", user)
				.then(function (data) {
				    saveUserData(data);
				});
		},
		register: function (username, password) {
			var user = {
				username: username,
				authCode: CryptoJS.SHA1(password).toString()
			};
			return httpRequester.POST(this.apiUrl + "register", user)
				.then(function (data) {
				    saveUserData(data);
					return data.username;
				});
		},
		logout: function () {
		    var url = this.apiUrl + "logout?sessionKey=" + SessionKey;
		    return httpRequester.PUT(url)
				.then(function () {
				    clearUserData();
				}, function(ex) {
				    console.log(ex);
				});
		},
		currentUser: function () {
		    return Username != null && Username != "" && SessionKey != null && SessionKey != "";
		}
	});

	
	var PostsPersister = Class.create({
	    init: function (apiUrl) {
	        this.apiUrl = apiUrl;
	    },
	    getAll: function () {
	        var url = this.apiUrl + "all";
	        return httpRequester.GET(url)
                .then(function (data) {
                    return data;
                }, function (ex) {
                    console.log(ex);
            });
	    },
	    getByCategoryId: function (id) {
	        var url = this.apiUrl + "by-category/" + id;

	        return httpRequester.GET(url)
                         .then(function (data) {
                             return data;
                         }, function (ex) {
                             console.log(ex);
                         });
	    },
	    getById: function(id) {
	        var url = this.apiUrl + "by-id/" + id;

	        return httpRequester.GET(url)
                         .then(function (data) {
                             return data;
                         }, function (ex) {
                             console.log(ex);
                         });
	    },
	    create: function (post) {
	        var url = this.apiUrl + "create?sessionKey=" + SessionKey;

	        return httpRequester.POST(url, post)
                       .then(function (data) {
                           return data;
                       }, function (ex) {
                           console.log(ex);
                       });
	    }
	});

	var CategoriesPersister = Class.create({
	    init: function (apiUrl) {
	        this.apiUrl = apiUrl;
	    },
	    getAll: function () {
	        return httpRequester.GET(this.apiUrl + "all")
               .then(function (data) {
                   return data;
               }, function (ex) {
        })
	    },
	    getNameById: function (id) {
	        return httpRequester.GET(this.apiUrl + "by-id?id=" + id)
                .then(function (data) {
                    return data;
                }, function (ex) {
                    console.log(ex);
                });
	    }
	});


	var CommentsPersister = Class.create({
	    init: function (apiUrl) {
	        this.apiUrl = apiUrl;
	    },
	    create: function (comment) {
	        var url = this.apiUrl + "create?sessionKey=" + SessionKey;

	        return httpRequester.POST(url, comment)
                    .then(function (data) {
                        return data;
                    }, function (ex) {
                        console.log(ex);
                    });
	    }
	});

	return {
		get: function (apiUrl) {
			return new DataPersister(apiUrl);
		}
	}
}());