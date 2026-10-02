document.addEventListener("DOMContentLoaded", function () {

    const loginForm = document.getElementById("loginForm");

    const emailInput = document.getElementById("email");
    const passwordInput = document.getElementById("password");

    const emailError = document.getElementById("emailError");
    const passwordError = document.getElementById("passwordError");
    const captchaError = document.getElementById("captchaError");
    const message = document.getElementById("message");

    const loginButton = document.getElementById("loginButton");
    const togglePassword = document.getElementById("togglePassword");
    const passwordIcon = document.getElementById("passwordIcon");


    // Password show / hide
    togglePassword.addEventListener("click", function () {

        if (passwordInput.type === "password") {

            passwordInput.type = "text";

            passwordIcon.classList.remove("fa-eye");
            passwordIcon.classList.add("fa-eye-slash");

        }
        else {

            passwordInput.type = "password";

            passwordIcon.classList.remove("fa-eye-slash");
            passwordIcon.classList.add("fa-eye");
        }
    });


    // Login form submit
    loginForm.addEventListener("submit", async function (event) {

        // Prevent normal form submission / page refresh
        event.preventDefault();


        // Clear previous messages
        emailError.textContent = "";
        passwordError.textContent = "";
        captchaError.textContent = "";
        message.textContent = "";

        message.className = "mt-2";


        const email = emailInput.value.trim();
        const password = passwordInput.value;

        let isValid = true;


        // Email validation
        if (!email) {

            emailError.textContent =
                "Email address is required.";

            isValid = false;
        }
        else {

            const emailPattern =
                /^[^\s@]+@[^\s@]+\.[^\s@]+$/;

            if (!emailPattern.test(email)) {

                emailError.textContent =
                    "Please enter a valid email address.";

                isValid = false;
            }
        }


        // Password validation
        if (!password) {

            passwordError.textContent =
                "Password is required.";

            isValid = false;
        }
        else if (password.length < 6) {

            passwordError.textContent =
                "Password must be at least 6 characters.";

            isValid = false;
        }


        // CAPTCHA validation
        const captchaToken = grecaptcha.getResponse();

        if (!captchaToken) {

            captchaError.textContent =
                "Please verify the CAPTCHA.";

            isValid = false;
        }


        // Stop here if validation failed
        if (!isValid) {
            return;
        }


        // Disable button while login request is running
        loginButton.disabled = true;

        loginButton.querySelector("span").textContent =
            "Signing in...";


        try {

            const response = await fetch("/Account/Login", {

                method: "POST",

                headers: {
                    "Content-Type": "application/json"
                },

                body: JSON.stringify({

                    Email: email,

                    Password: password,

                    CaptchaToken: captchaToken

                })
            });


            const result = await response.json();


            if (result.success) {

                message.textContent =
                    result.message;

                message.className =
                    "mt-2 text-success";


                // Store JWT
                sessionStorage.setItem(
                    "jwtToken",
                    result.token
                );


                // Store user information
                sessionStorage.setItem(
                    "userRole",
                    result.user.role
                );

                sessionStorage.setItem(
                    "userName",
                    result.user.name
                );


                console.log("JWT Token:", result.token);


                // Redirect after successful login
                setTimeout(function () {

                    window.location.href =
                        "/Default/Index";

                }, 500);

            }
            else {

                message.textContent =
                    result.message;

                message.className =
                    "mt-2 text-danger";


                // Reset CAPTCHA
                grecaptcha.reset();
            }

        }
        catch (error) {

            console.error(
                "Login Error:",
                error
            );

            message.textContent =
                "Unable to connect to the server. Please try again.";

            message.className =
                "mt-2 text-danger";


            grecaptcha.reset();
        }
        finally {

            loginButton.disabled = false;

            loginButton.querySelector("span").textContent =
                "Sign In";
        }

    });

});