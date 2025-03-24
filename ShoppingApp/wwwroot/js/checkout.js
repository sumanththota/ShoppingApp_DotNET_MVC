// This test secret API key is a placeholder. Don't include personal details in requests with this key.
// To see your test secret API key embedded in code samples, sign in to your Stripe account.
// You can also find your test secret API key at https://dashboard.stripe.com/test/apikeys.
const stripe = Stripe("pk_test_51R5zvrHcMSUslYQYxEDMDkpsXoWGYMmFeG0Cw2vHJkuePKSc7mnsYBSV5fBpdz9ulLwKjxB4uehBWZbQNa8kvaoE00fWqciFwt"); // Replace with your real key

// document.getElementById("checkout-button").addEventListener("click", initialize);
// Create a Checkout Session
initialize();
async function initialize() {
    const fetchClientSecret = async () => {
        const response = await fetch("/create-checkout-session", {
            method: "POST",
        });
        const { clientSecret } = await response.json();
        return clientSecret;
    };

    const checkout = await stripe.initEmbeddedCheckout({
        fetchClientSecret,
    });

    // Mount Checkout
    checkout.mount('#checkout');
    console.log("mount succesful");
}