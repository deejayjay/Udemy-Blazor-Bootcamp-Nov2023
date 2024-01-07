function redirectToCheckout(sessionId) {
    const publicKey = "pk_test_51OVoeQKneXMBJTDIHpeVkZt0TuZf8BWqC44EtFEhxeCTewh8V81F8nN93u8e9PNCr5lBNjgCDAtRr8otEJRZ29mn00OfUvH1oD";
    const stripe = Stripe(publicKey);

    stripe.redirectToCheckout({ sessionId });
}