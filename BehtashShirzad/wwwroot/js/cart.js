function addToCart() {

    
    var price = $("#ProductPrice").text()
    var Title = $("#ProductTitle").text()
    var url = window.location.href;
    var Image = $("#productImage").prop("src") 
    dto = { "Price": price, "Title": Title, "Url": url, image: Image }
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
            return;
        }
    } else {
        // If cart doesn't exist, initialize an array with the new item
        cart = [dto];

        // Update the cart in localStorage with the new item
        localStorage.setItem("cart", JSON.stringify(cart));

       
    }
    alert(`${Title} به سبد خرید اضافه شد `)
}

function removeFromCart(nameToRemove) {
     
    var items = JSON.parse(localStorage.getItem('cart'));

    
    var itemNameToRemove = nameToRemove;
    items = items.filter(function (item) {
        return item.Title !== itemNameToRemove;
    });

    
    localStorage.setItem('cart', JSON.stringify(items));
    fetchCartData()
}



function fetchCartData() {
   
    $(".CartData").empty()

    var basket = localStorage.getItem("cart");
    
    if (basket == null || basket == "" || basket == "[]") {

        $(".CartData").append(`<li> <span>سبد خرید خالی است</span></li>`)
        $(".checkout-btn").hide();
        $("#totalPrice").text(0) 
        return
    }

    total = 0
     
    data = $.parseJSON(basket)
    
    
    $.each(data, function (index, element) {

        var carddata =
                        `<div class="cart-items">
                            <div class="cart-item">
                                <img src="${element["image"]}" alt="${element["Title"]}" style="margin-left:10px"/>
                                    <div class="item-details">
                                    <a href="${element["Url"]}">${element["Title"]}</a>
                                        <p>${parseInt(element["Price"])} تومان</p>
                                        <button class="remove-btn" onclick="removeFromCart('${element["Title"]}')">حذف</button>
                                     </div>
                            </div>
                        </div>`

        $(".CartData").append(carddata)
        total += parseInt(element["Price"])
    });

   
    $("#totalPrice").text(total) 
    if (total == null || total == 0   ) {
        $(".checkout-btn").hide();
      
    }
   

     


}


function pay() {
    var names = [];
    var data = JSON.parse(localStorage.getItem("cart"));

    console.log("Retrieved data:", data); // Debug output

    if (data) {
        data.forEach(function (element) {
            names.push(element["Title"]);
        });
    } else {
        console.log('No data found in local storage');
        return; // Exit function if no data
    }

    var dataToSend = JSON.stringify({ products: names });
    console.log("Sending data:", dataToSend); // More debug output

    $.ajax({
        url: "/Invoice/Create",
        type: "POST",
        contentType: "application/json",
        data: dataToSend
    }).done(function (data, textStatus, jqXHR) {
        debugger
        console.log(textStatus)
        if ("success" == String(textStatus)) {
            window.location = "/User"
        }
        if (data.includes("html")) {
            window.location = "/Login";
        }
       
    });
}
