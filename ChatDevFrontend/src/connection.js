import { flush } from './chats.js';
import { setCurrentUser, logout, fetchUsers, apiBase, showToast, getCurrentUserId } from './globals.js';
import { stopSignalRConnection, startSignalRConnection, updateStatus } from "./signalR.js";

let username;
let userId;

document.getElementById("connect").addEventListener("click", async function() {
    let token = document.getElementById("token").value.trim();
    if (!token) {
        showToast("Please enter a token.");
        return;
    }
    let id;
    try {
        const payload = JSON.parse(atob(token.split('.')[1]));
        console.log(payload);
        id = payload["userId"];
        if (!id) {
            showToast("Invalid token.");
            return;
        }
    } catch (err) {
        showToast("Invalid token.");
        return;
    }
    
    setCurrentUser(id);
    connectSignalR(token);
});



async function connectSignalR(token) {
    try {
        await startSignalRConnection(token);
    } catch (err) {
        console.log("SignalR Connection Error: ", err);
        updateStatus(false);
    }
}

document.getElementById("disconnect").addEventListener("click", async function() {
    logout();
    await stopSignalRConnection();
});

