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
    const response = await fetch(`/api/auth/users`, {
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
        alert("Failed to fetch users.");
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