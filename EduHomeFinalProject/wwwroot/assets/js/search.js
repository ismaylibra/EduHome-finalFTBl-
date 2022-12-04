var searchInput = document.getElementById("search-courses");
if (searchInput) {
    searchInput.addEventListener("keyup", function () {

        let text = this.value
        let productList = document.querySelector("#product-list")

        fetch('Courses/Search?searchText=' + text)
            .then((response) => response.text())
            .then((data) => {
                console.log(data)
                productList.innerHTML = data
            });

    });
}