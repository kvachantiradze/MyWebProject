function togglePasswordVisibility() {
    var input = document.getElementById("passwordInput");
    var button = document.getElementById("togglePassword");

    if (input.type === "password") {
        input.type = "text";
        button.textContent = "Hide";
    } else {
        input.type = "password";
        button.textContent = "Show";
    }
}


document.getElementById("togglePassword").addEventListener("click", function () {
    togglePasswordVisibility();
});

document.getElementById("loginForm").addEventListener("submit", function () {
    document.getElementById("loadingSpinner").classList.remove("d-none");
});








