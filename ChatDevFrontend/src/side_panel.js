import { getCurrentUserId, showToast } from './globals.js';
import { addToGroup, createGroup, renameGroup, startDm } from './signalR.js';
const groupUsers = new Set();

document.getElementById("add_id_to_group_list").addEventListener("click", function() {
    const userIdInput = document.getElementById("group_user");
    const userId = userIdInput.value.trim();

    if (!userId) return;

    if (groupUsers.has(userId)) {
        // groupUsers.delete(userId);
    } else {
        groupUsers.add(userId);
    }

    const groupUsersContainer = document.getElementById("group_users");
    groupUsersContainer.innerHTML = '';
    groupUsers.forEach(user => {
        const userItem = document.createElement("button");
        userItem.classList.add("user-item");
        userItem.textContent = user;

        userItem.addEventListener("click", () => {
            groupUsers.delete(user);
            userItem.remove();
        });

        groupUsersContainer.appendChild(userItem);
    });

    userIdInput.value = '';
});

document.getElementById("create_group").addEventListener("click", async function() {
    const groupName = document.getElementById("group_name").value.trim();
    if (!groupName || groupUsers.size < 1) {
        showToast("Please provide a group name and add at least one user.");
        return;
    }
    const myId = getCurrentUserId();
    if (!myId) {
        showToast("User is not logged in.");
        return;
    }
    groupUsers.add(myId)
    createGroup(groupName, Array.from(groupUsers));
});

document.getElementById("send_dm").addEventListener("click", function() {
    const user = document.getElementById("direct_username").value;
    const message = document.getElementById("direct_message").value.trim();
    if (message && user) {
        startDm(user, message)
    }
});

document.getElementById("add_to_group").addEventListener("click", function() {
    const user = document.getElementById("add_to_group_user_id").value.trim();
    const chatId = document.getElementById("add_to_group_id").value.trim();
    if (user && chatId) {
        addToGroup(chatId, user);
    }
});


document.getElementById("rename_group").addEventListener("click", function() {
    const chatId = document.getElementById("rename_group_id").value.trim();
    const newName = document.getElementById("rename_group_name").value.trim();
    if (chatId && newName) {
        renameGroup(chatId, newName);
    } else {
        showToast("Please provide a chat ID and a new name.");
    }
});


let guide_opened = false;
const guide = document.getElementById("guide");

function toggleGuide() {
    guide_opened = !guide_opened;
    if (guide_opened) {
        guide.style.opacity = "1";
        guide.style.visibility = "visible";
    } else {
        guide.style.opacity = "0";
        guide.style.visibility = "hidden";
    }
}

document.getElementById("open_guide").addEventListener("click", toggleGuide);

document.addEventListener("keydown", function (event) {
    if (event.key === "Escape" && guide_opened) {
        toggleGuide();
    }
});