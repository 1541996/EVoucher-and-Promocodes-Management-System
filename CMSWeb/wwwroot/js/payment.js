

var stripe = Stripe("pk_test_51JUs7cGyxIDm1WhZRN6NSGPMSnCJlVfj0PihRYRepawtOeGQXLWIADqI6Sy6oyHuicY6W2qVbJ6mvh7yFO4sFO6K00vJMYl9cv");



//document.querySelector("button").disabled = true;
var style = {
    base: {
        color: "#32325d",
        fontFamily: 'Arial, sans-serif',
        fontSmoothing: "antialiased",
        fontSize: "16px",
        "::placeholder": {
            color: "#32325d"
        }
    },
    invalid: {
        fontFamily: 'Arial, sans-serif',
        color: "#fa755a",
        iconColor: "#fa755a"
    }
};
var elements = stripe.elements();
card = elements.create("card", { style: style });
// Stripe injects an iframe into the DOM
card.mount("#card-element");

card.on("change", function (event) {
    // Disable the Pay button if there are no card details in the Element
  //  document.querySelector("button").disabled = event.empty;
  //  document.querySelector("#card-error").textContent = event.error ? event.error.message : "";
});
// Calls stripe.confirmCardPayment
// If the card requires authentication Stripe shows a pop-up modal to
// prompt the user to enter authentication details without leaving your page.
var payWithCard = function (stripe, card, clientSecret, myData) {
  //  loading(true);
    stripe
        .confirmCardPayment(clientSecret, {
            payment_method: {
                card: card
            }
        })
        .then(function (result) {
            if (result.error) {
                // Show error to your customer
                showError(result.error.message);
            } else {
                alert("Please check for payment success " + "https://dashboard.stripe.com/test/payments/" + result.paymentIntent.id);
                // The payment succeeded!
                orderComplete(result.paymentIntent.id, myData);
            }
        });
};

/* ------- UI helpers ------- */

// Shows a success message when the payment is complete
var orderComplete = function (paymentIntentId, myData) {
  //  loading(false);
    //document
    //    .querySelector(".result-message a")
    //    .setAttribute(
    //        "href",
    //        "https://dashboard.stripe.com/test/payments/" + paymentIntentId
    //);

    $('#tbody_purchase').empty();
    for (var i = 0; i < myData.length; i++) {
        $('#tbody_purchase').append(`<tr>
            <td>${myData[i].CustomerName}</td>
            <td>${myData[i].VoucherName}</td>
            <td>${myData[i].PaymentMethodName}</td>
            <td>${myData[i].PromoCodes}</td>
            <td>$ ${myData[i].Amount}</td>
            <td>${myData[i].Discount}%</td>
            <td>$ ${myData[i].TotalAmount}</td>
            <td class="qrcode" onclick="QRScan('${myData[i].PromoCodes}')">Check qrcodes</td>


        </tr>`
        )
    };


    $('#purchase_modal').modal({ backdrop: 'static' }, 'show');

    $('#txt_cusname').val('');
    $('#txt_cusphone').val('');


  //  document.querySelector(".result-message").classList.remove("hidden");
  //  document.querySelector("button").disabled = true;
};

// Show the customer the error from Stripe if their card fails to charge
var showError = function (errorMsgText) {
  //  loading(false);
    alert(errorMsgText);
    //var errorMsg = document.querySelector("#card-error");
    //errorMsg.textContent = errorMsgText;
    //setTimeout(function () {
    //    errorMsg.textContent = "";
    //}, 4000);
};

