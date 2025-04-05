import * as signalR from "@microsoft/signalr";
import { addChat, addMessages, addSentMessage, clearUserChats, removeChat, handleNewMessage, user_to_dm, updateOnlineUsers, chats, UpdateChatTyping, setUnreadenCounter, RenameGroupChat, deleteMessageHandler, editMessageHandler } from "./chats.js";

export function updateStatus(status) {
    const statusDiv = document.getElementById("status");
    if (status) {
        statusDiv.classList.remove("text-red-500");
        statusDiv.classList.remove("text-yellow-500");
        statusDiv.classList.add("text-green-500");
        statusDiv.textContent = "CONNECTED";
    } else {
        statusDiv.classList.remove("text-green-500");
        statusDiv.classList.remove("text-yellow-500");
        statusDiv.classList.add("text-red-500");
        statusDiv.textContent = "DISCONNECTED";
    }
}


let connection = null;

export async function startSignalRConnection(userId) {
    if (connection) {
        console.warn("SignalR connection already exists. Closing existing connection...");
        await stopSignalRConnection();
    }

    connection = new signalR.HubConnectionBuilder()
        .withUrl(`/api/chathub?userId=${userId}`)
        .build();

    connection.onclose(() => {
        console.warn("SignalR Disconnected.");
        clearUserChats();
        imHereDaemon.stop();
        fetchDmsDaemon.stop();
        updateStatus(false);
    });

    await connection.start().then(() => {
        imHereDaemon.start();
        fetchDmsDaemon.start();
        getChatList();
    });

    console.log("SignalR Connected");
    updateStatus(true);

    connection.on("ChatList", async function (notification) {
        for (const chat of notification) {
            console.log(chat)
            await addChat(
                chat.name, 
                chat.id, 
                chat.isDm, 
                chat.members,
                chat.lastMessage, 
                chat.unreadCount
            );
        }
    });

    connection.on("ChatAdded", async function (notification) {
        notification = JSON.parse(notification);
        await addChat(
            notification.name, 
            notification.id, 
            notification.isDm, 
            notification.members, 
            notification.lastMessage, 
            notification.unreadCount);
    });
    connection.on("ChatRemoved", function (notification) {
        console.log(notification)
        notification = JSON.parse(notification);
        console.log(notification)
        removeChat(notification.id);
    });
    connection.on("Msgs", async function (messages) {
        console.log(messages)
        await addMessages(messages)
    });
    connection.on("MsgSent", function (message) {
        addSentMessage(message) 
    });
    connection.on("NewMsg", async function (notification) {
        notification = JSON.parse(notification);
        console.log("Newmsg: ", notification)
        await handleNewMessage(notification);
    });
    connection.on("UsersOnline", function (notification) {
        updateOnlineUsers(notification)
    });
    connection.on("ChatsTyping", async function (notification) {
        await UpdateChatTyping(notification)
    });
    connection.on("ErrorResponse", function (notification) {
        console.log(notification)
        alert(notification);
    });
    connection.on("MsgsReaden", function (notification) {
        console.log("READEN: ", notification)
        notification = JSON.parse(notification);
        setUnreadenCounter(notification.chatId, notification.unreadCount);
    });
    connection.on("GroupRenamed", function (notification) {
        notification = JSON.parse(notification);
        RenameGroupChat(notification.chatId, notification.newName)
    });
    connection.on("MsgDeleted", function (notification) {
        notification = JSON.parse(notification);
        deleteMessageHandler(notification.senderId, notification.chatId, notification.messageId)
    });
    connection.on("MsgEdited", async function (notification) {
        notification = JSON.parse(notification);
        console.log("EDITED ", notification)
        await editMessageHandler(notification.senderId, notification.chatId, notification.messageId, notification.newMessage);
    });
}

export async function stopSignalRConnection() {
    if (connection) {
        await connection.stop();
        updateStatus(false);
        console.log("Disconnected from SignalR");
        connection = null;
    }
}

// signalR isnt case-sensitive, u can use any of this: GetMsgs/getmsgs/getMsgs/GeTmSgS

export function imHere() {
    connection.invoke("imHere");
}

export function createGroup(group, users) {
    connection.invoke("createGroup", group, users);
}

export function getMsgs(chatId, startId, toNew, amount) {
    connection.invoke("getMsgs", chatId, startId, toNew, amount);
}

export function startDm(user_id, msg) {
    connection.invoke("startDm", user_id, msg);
}

export function sendMsg(chatId, msg, ext=null) {
    connection.invoke("sendMsg", chatId, msg, ext);
}

export function getChatList() {
    connection.invoke("getChatList");
}

export function deleteChat(chatId) {
    connection.invoke("deleteChat", chatId);
}

export function leaveGroup(chatId) {
    connection.invoke("leaveGroup", chatId);
}

export function addToGroup(chatId, userId) {
    connection.invoke("addToGroup", chatId, userId);
}

export function getUsersOnline(userIds) {
    connection.invoke("getUsersOnline", userIds);
}
export function getChatsTyping(chatIds) {
    connection.invoke("getChatsTyping", chatIds);
}

export function readMessages(chatId, lastMsgId) {
    connection.invoke("readMessages", chatId, lastMsgId);
}

export function renameGroup(chatId, newName) {
    connection.invoke("renameGroup", chatId, newName);
}

export function typing(chatId) {
    if (connection == null) return;
    console.log("Im typing ", chatId)
    connection.invoke("typing", chatId);
}

export function editMessage(messageId, newMessage) {
    connection.invoke("editMessage", messageId, newMessage);
}

export function deleteMessage(messageId) {
    console.log("Deleting ", messageId)
    connection.invoke("deleteMessage", messageId);
}

class ImHereDaemon {
    constructor(interval = 1000) {
        this.interval = interval;
        this.running = false;
        this.loop();
    }
    start() {
        this.running = true;
    }
    stop() {
        this.running = false;
    }

    loop() {
        if (this.running){
            imHere();
        }
        setTimeout(() => this.loop(), this.interval);
    }
}

var imHereDaemon = new ImHereDaemon(1000);

class FetchDmsDaeman {
    constructor(interval = 1000) {
        this.interval = interval;
        this.running = false;
        this.loop();
    }
    start() {
        this.running = true;
    }
    stop() {
        this.running = false;
    }

    loop() {
        if (this.running){
            getUsersOnline(Object.keys(user_to_dm));
            getChatsTyping(Array.from(chats))
        }
        setTimeout(() => this.loop(), this.interval);
    }
}
var fetchDmsDaemon = new FetchDmsDaeman(1000);


