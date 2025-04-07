export const DEV = false; // import.meta.env.VITE_DEV == null ? true : import.meta.env.VITE_DEV === 'true';
console.log("ISDEV: ", DEV)
export const apiBase = DEV ? "http://127.0.0.1:5108/api" : "/api"
export const fileApi =  DEV ? "http://127.0.0.1:9000" : ""

let uuid_to_username = {}


export function addUsernameRelation(username, uuid) {
    uuid_to_username[uuid] = username;
}

export function getUsernameByUuid(uuid) {
    return uuid_to_username[uuid];
}

export function removeUsernameRelation(uuid) {
    delete uuid_to_username[uuid];
}

export function clearUsernameRelations() {
    uuid_to_username = {};
}

export async function fetchUsers() {
    const response = await fetch(`${apiBase}/auth/users`, {
        method: "GET",
        headers: {
            "Content-Type": "application/json"
        }
    });
    if (response.ok) {
        const users = await response.json();
        clearUsernameRelations();
        users.forEach(user => {
            addUsernameRelation(user.username, user.id);
        });
        return users
    } else {
        showToast("Failed to fetch users.");
    }
}

let currentUser = {
    userId: null
};

export function setCurrentUser(userId) {
    currentUser.userId = userId;
}

export function getCurrentUserId() {
    return currentUser.userId;
}

export function logout() {
    currentUser.userId = null;
}

export function uuidv4() {
    return "10000000-1000-4000-8000-100000000000".replace(/[018]/g, c =>
        (+c ^ crypto.getRandomValues(new Uint8Array(1))[0] & 15 >> +c / 4).toString(16)
    );
}

export function showToast(message) {
    let toast = document.createElement("div");
    toast.textContent = message;
    toast.style.position = "fixed";
    toast.style.bottom = "20px";
    toast.style.left = "50%";
    toast.style.transform = "translateX(-50%)";
    toast.style.background = "rgba(0, 0, 0, 0.7)";
    toast.style.color = "#fff";
    toast.style.padding = "10px 20px";
    toast.style.borderRadius = "5px";
    toast.style.zIndex = "1000";
    document.body.appendChild(toast);
    setTimeout(() => toast.remove(), 2000);
}