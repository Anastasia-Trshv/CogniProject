console.log("HI FROM INJECTION!");
var inited = false;
document.addEventListener("DOMContentLoaded", function () {
    const checkSwaggerLoaded = setInterval(function () {
        console.log("Checking for Swagger UI...");
        // Only execute once when the Swagger UI is loaded
        if (!inited && document.querySelector('.opblock-tag')) {
            inited = true;
            console.log("Swagger UI is loaded!");

            // Apply the initial replacement
            replaceText();

            // Add event listeners to handle future updates or interactions
            const els = document.getElementsByClassName('opblock-tag');
            Array.from(els).forEach(element => {
                element.addEventListener('click', function () {
                    // Add a slight delay to ensure elements have been updated
                    setTimeout(replaceText, 100);
                });
            });

            clearInterval(checkSwaggerLoaded);
        }
    }, 100);
});

function replaceText() {
    console.log("Replacing method names...");
    const opblocks = document.querySelectorAll('.opblock-summary-control');
    opblocks.forEach(opblock => {
        const pathElement = opblock.querySelector('[data-path^="/listen"]');
        if (pathElement) {
            const methodText = opblock.querySelector('.opblock-summary-method');
            if (methodText) {
                methodText.textContent = 'LISTEN';
            }
        }
        const pathElementI = opblock.querySelector('[data-path^="/invoke"]');
        if (pathElementI) {
            const methodText = opblock.querySelector('.opblock-summary-method');
            if (methodText) {
                methodText.textContent = 'INVOKE';
            }
        }
        const pathElementR = opblock.querySelector('[data-path^="/invoke-response"]');
        if (pathElementR) {
            const methodText = opblock.querySelector('.opblock-summary-method');
            if (methodText) {
                methodText.textContent = 'RESPONSE';
            }
        }
        const pathElementC = opblock.querySelector('[data-path^="/ChatHub"]');
        if (pathElementC) {
            const methodText = opblock.querySelector('.opblock-summary-method');
            if (methodText) {
                methodText.textContent = 'CONN';
            }
        }
    });
}
