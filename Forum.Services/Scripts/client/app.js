/// <reference path="../_references.js" />


(function () {
	var appLayout =
		new kendo.Layout('<div id="main-content"></div>');
	var data = persisters.get("api/");
	vmFactory.setPersister(data);

	var router = new kendo.Router();
	router.route("/", function () {
	    viewsFactory.getCategoriesView()
			.then(function (categoriesHtml) {
			    vmFactory.getCategoriesVM()
				.then(function (vm) {
					var view =
						new kendo.View(categoriesHtml,
						{ model: vm });
					appLayout.showIn("#main-content", view);
				}, function (err) {
				    console.log(err);
				});
			});
	});

	router.route("/login-or-register", function () {
		if (data.users.currentUser()) {
			router.navigate("/");
		}
		else {
			viewsFactory.getLoginView()
				.then(function (loginViewHtml) {
					var loginVm = vmFactory.getLoginVM(
						function () {
							router.navigate("/");
						});
					var view = new kendo.View(loginViewHtml,
						{ model: loginVm });
					appLayout.showIn("#main-content", view);
				});
		}
	});

	router.route("/category/:id", function (id) {
	    if (isNaN(id)) {
	        router.navigate("/error");
	    }
	    else {
	        viewsFactory.getPostsView()
               .then(function (postsHtml) {
                   vmFactory.getPostsVM(id)
                   .then(function (vm) {
                       var view =
                           new kendo.View(postsHtml,
                           { model: vm });
                       appLayout.showIn("#main-content", view);
                   }, function (err) {
                       console.log(err);
                   });
               });
	    }


	});

	router.route("/post/:id", function (id) {
	    if (isNaN(id)) {
	        router.navigate("/error");
	    }
	    else {
	        viewsFactory.getCurrentPostView()
              .then(function (currentPostHtml) {
                  vmFactory.getCurrentPostVM(id)
                  .then(function (vm) {
                      var view =
                          new kendo.View(currentPostHtml,
                          { model: vm });
                      appLayout.showIn("#main-content", view);
                  }, function (err) {
                      console.log(err);
                  });
              });
	    }
	});

	router.route("/logout", function () {
	    data.users.logout()
            .then(function () {
                router.navigate("/");
            });
	});

	$(function () { 
		appLayout.render("#app");
		router.start();
	});
}());