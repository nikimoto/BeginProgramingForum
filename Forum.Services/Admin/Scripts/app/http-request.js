﻿(function () {
	function performGetRequest(url, headers) {
		var promise = new RSVP.Promise(function (resolve, reject) {
			$.ajax({
				type: "GET",
				url: url,
				headers: headers,
				contentType: "application/json",
				success: function (data) {
					resolve(data);
				},
				error: function (err) {
					reject(err);
				}
			});
		});
		return promise;
	}

	function performPostRequest(url, requestData, headers) {
		var promise = new RSVP.Promise(function (resolve, reject) {
			$.ajax({
				type: "POST",
				url: url,
				contentType: "application/json",
				headers: headers,
				data: JSON.stringify(requestData),
				success: function (data) {
					resolve(data);
				},
				error: function (err) {
					reject(err);
				}
			});
		});
		return promise;
	}
	window.request = {
		get: performGetRequest,
		post: performPostRequest
	}

}());