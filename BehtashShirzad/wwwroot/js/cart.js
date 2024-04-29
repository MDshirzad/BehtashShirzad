function addToCart() {

    
    var price = $("#ProductPrice").text()
    var Title = $("#ProductTitle").text()
    var url = window.location.href;

    dto = { "Price": price, "Title": Title, "url": url }
    // Retrieve cart from localStorage
    var cart = localStorage.getItem("cart");

    // Check if cart exists in localStorage
    if (cart != null) {
        // Parse the cart JSON string into an array
        cart = JSON.parse(cart);

        // Check if the item with the specified ID is already in the cart
        var isInCart = cart.some(item => item.Title === dto.Title);

        if (!isInCart) {
            // Add the new item to the cart if it's not already in the cart
            cart.push(dto);

            // Update the cart in localStorage by converting it back to JSON string
            localStorage.setItem("cart", JSON.stringify(cart));
        } else {
            alert("محصول مورد نظر در سبد موجود میباشد")
        }
    } else {
        // If cart doesn't exist, initialize an array with the new item
        cart = [dto];

        // Update the cart in localStorage with the new item
        localStorage.setItem("cart", JSON.stringify(cart));
    }
    
}

