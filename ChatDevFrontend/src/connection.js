import { flush } from './chats.js';
import { setCurrentUser, logout, fetchUsers, apiBase } from './globals.js';
import { stopSignalRConnection, startSignalRConnection, updateStatus } from "./signalR.js";

let username;
let userId;

document.getElementById("connect").addEventListener("click", async function() {
    username = document.getElementById("username").value.trim();
    if (!username) {
        alert("Please enter a username.");
        return;
    }
    const response = await fetch(`${apiBase}/auth/register`, {
        method: "POST",
        headers: {
            "Content-Type": "application/json"
        },
        body: JSON.stringify({
            username: username
        })
    });
    if (response.ok) {
        const jwtImitation = await response.json();
        userId = jwtImitation.userId;
        console.log("JWT imitation: ", jwtImitation);
        await fetchUsers();
        flush();
        setCurrentUser(jwtImitation.userId);
        connectSignalR(jwtImitation.userId);
    } else {
        const error = await response.json();
        alert(`Error: ${error.message}`);
    }
});



async function connectSignalR(userId) {
    try {
        await startSignalRConnection(userId);
    } catch (err) {
        console.log("SignalR Connection Error: ", err);
        updateStatus(false);
    }
}

document.getElementById("disconnect").addEventListener("click", async function() {
    logout();
    await stopSignalRConnection();
});

