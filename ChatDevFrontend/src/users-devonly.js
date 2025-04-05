import { fetchUsers } from './globals.js';

document.getElementById("get_users").addEventListener("click", async function() {
    // Side effect is update uuid_to_username
    let users = await fetchUsers();
    const usersContainer = document.getElementById("users_container");
    usersContainer.innerHTML = '';
    users.forEach(user => {
        const userDiv = document.createElement("a");
        userDiv.classList.add("w-full", "break-word", "text-center");
        userDiv.classList.add("user-item");
        userDiv.style.color = "#646cff";
        userDiv.textContent = `name: ${user.username} - id: ${user.id}`;
        userDiv.addEventListener("click", function() {
            navigator.clipboard.writeText(user.id).then(() => {}).catch(err => {
                alert("Failed to copy text: " + err);
            });
        });
        usersContainer.appendChild(userDiv);
    });
});
