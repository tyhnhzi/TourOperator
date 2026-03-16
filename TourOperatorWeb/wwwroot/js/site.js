// Please see documentation at https://learn.microsoft.com/aspnet/core/client-side/bundling-and-minification
// for details on configuring this project to bundle and minify static web assets.

// Write your JavaScript code.

// Điều khiển nút tiến/lùi cho danh sách tour đề cử xếp chồng ngang
// Toàn bộ các thẻ xoay vòng và luôn đưa một thẻ ra giữa, nổi bật trên cùng
document.addEventListener("DOMContentLoaded", function () {
	var wrappers = document.querySelectorAll(".tour-stack-wrapper");
	wrappers.forEach(function (wrapper) {
		var track = wrapper.querySelector(".tour-stack-track");
		var prevBtn = wrapper.querySelector(".tour-stack-prev");
		var nextBtn = wrapper.querySelector(".tour-stack-next");

		if (!track || !prevBtn || !nextBtn) {
			return;
		}

		function updateActive() {
			var cards = track.querySelectorAll(".tour-stack-card");
			if (cards.length === 0) return;

			var mid = Math.floor(cards.length / 2);

			cards.forEach(function (card, index) {
				if (index === mid) {
					card.classList.add("tour-stack-card--active");
				} else {
					card.classList.remove("tour-stack-card--active");
				}
			});
		}

		prevBtn.addEventListener("click", function () {
			var last = track.lastElementChild;
			if (!last) return;
			track.insertBefore(last, track.firstElementChild); // xoay vòng về trước
			updateActive();
		});

		nextBtn.addEventListener("click", function () {
			var first = track.firstElementChild;
			if (!first) return;
			track.appendChild(first); // xoay vòng về sau
			updateActive();
		});

		// Thiết lập thẻ ở giữa là nổi bật ban đầu
		updateActive();
	});
});
