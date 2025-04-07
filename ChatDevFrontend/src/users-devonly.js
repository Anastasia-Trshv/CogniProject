import { fetchUsers } from './globals.js';

document.getElementById("get_users").addEventListener("click", async function() {
    // Side effect: update uuid_to_username
    let users = await fetchUsers();
    const usersContainer = document.getElementById("users_container");
    usersContainer.innerHTML = '';

    users.forEach(user => {
        const userDiv = document.createElement("button");
        userDiv.classList.add("w-full", "break-word", "text-center", "user-item");
        userDiv.style.color = "#646cff";
        userDiv.textContent = `name: ${user.username} - id: ${user.id}`;

        userDiv.addEventListener("click", function() {
            navigator.permissions.query({ name: "clipboard-write" }).then(function(result) {
                if (result.state === "granted" || result.state === "prompt") {
                    navigator.clipboard.writeText(user.id).then(() => {
                        alert("User ID copied to clipboard!");
                    }).catch(err => {
                        alert("Failed to copy text: " + err);
                    });
                } else {
                    alert("Clipboard access denied. Please grant permission.");
                }
            }).catch(err => {
                alert("Error checking clipboard permission: " + err);
            });
        });

        usersContainer.appendChild(userDiv);
    });
});
