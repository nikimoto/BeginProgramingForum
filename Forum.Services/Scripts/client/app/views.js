/// <reference path="../libs/_references.js" />

window.viewsFactory = (function () {
    //Change root url
	var rootUrl = "Scripts/client/partials/";

	var templates = {};

	function getTemplate(name) {
		var promise = new RSVP.Promise(function (resolve, reject) {
			if (templates[name]) {
				resolve(templates[name])
			}
			else {
				$.ajax({
					url: rootUrl + name + ".html",
					type: "GET",
					success: function (templateHtml) {
						templates[name] = templateHtml;
						resolve(templateHtml);
					},
					error: function (err) {
						reject(err)
					}
				});
			}
		});
		return promise;
	}

	function getLoginView() {
		return getTemplate("login-form");
	}

	function getCategoriesView() {
	    return getTemplate("categories-list");
	}
    
	function getPostsView() {
        return getTemplate("posts-list")
	}

	function getCurrentPostView() {
	    return getTemplate("current-post");
	}

	return {
	    getLoginView: getLoginView,
	    getCategoriesView: getCategoriesView,
	    getPostsView: getPostsView,
        getCurrentPostView: getCurrentPostView
	};
}());