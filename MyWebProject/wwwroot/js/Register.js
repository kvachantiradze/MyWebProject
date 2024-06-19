function togglePasswordVisibility(inputId, buttonId) {
    var input = document.getElementById(inputId);
    var button = document.getElementById(buttonId);

    if (input.type === "password") {
        input.type = "text";
        button.textContent = "Hide";
    } else {
        input.type = "password";
        button.textContent = "Show";
    }
}


document.getElementById("togglePassword").addEventListener("click", function () {
    togglePasswordVisibility("passwordInput", "togglePassword");
});

document.getElementById("toggleConfirmPassword").addEventListener("click", function () {
    togglePasswordVisibility("confirmPasswordInput", "toggleConfirmPassword");
});












