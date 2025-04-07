import { apiBase, fetchUsers, fileApi, getCurrentUserId, getUsernameByUuid } from './globals.js';
import { deleteChat, deleteMessage, editMessage, getMsgs, leaveGroup, readMessages, sendMsg, typing } from './signalR.js'


// some of this variables are outdated/unused 🤭
// its just for showcase, make your own chat system

export let dm_to_user = {};
export var user_to_dm = {};
export let chats = new Set();
var current_chat = null;
let chat_messages = [];
var dms = new Set();
let fetching_history = false
var end_of_dm = false
// We are sending message to server, and they instantly sends response that message is handled. So we need to store it locally for exclude dublicates on newMsg. Hashset is much faster than array
let sent_msgs = new Set();

export function flush(){
    dms = new Set();
    chats = new Set();
    sent_msgs = new Set();
    dm_to_user = {};
    user_to_dm = {};
    current_chat = null;
    chat_messages = [];
    fetching_history = false;
    end_of_dm = false;
    clearChat();
}


document.getElementById("close_chat").addEventListener("click", function() {
    closeChat();
})
const MessagesContainer = document.getElementById("messages_container");

var last_messages = {};
var unread_counts = {};

export async function replaceUUIDs(input) { 
    const matches = [...input.matchAll(/\[([0-9a-fA-F-]{36})\]/g)];
    const replacements = await Promise.all(
        matches.map(async ([fullMatch, uuid]) => {
            const username = await getUsername(uuid);
            return { fullMatch, replacement: `${username}` };
        })
    );
    let result = input;
    for (const { fullMatch, replacement } of replacements) {
        result = result.replace(`${fullMatch}`, replacement);
    }
    return result;
}

export async function getUsername(userid) {
    let username = getUsernameByUuid(userid);
    if (username == null) {
        await fetchUsers();
        username = getUsernameByUuid(userid);
        if (username == null) {
            console.log("Can't find username");
            username = "Unknown";
        }
    }
    return username
}

function buildNewChat(chatId, chatName) {
    const newChat = document.createElement("div");
    newChat.className = "chat-item flex flex-col overflow-hidden";
    newChat.id = "chat_" + chatId;
    newChat.addEventListener("click", () => {
        openChat(chatId, chatName);
    });
    return newChat
}

async function getChatName(chatId, chatName, dmUser) {
    let chat_name = chatName;
    if (dmUser != null) {
        dms.add(chatId);
        dm_to_user[chatId] = dmUser;
        user_to_dm[dmUser] = chatId;
        chat_name = "Dm with " + await getUsername(dmUser);
    }
    return chat_name
}

function buildChatHeader(chatId, chatName, dmUser) {
    const chatHeader = document.createElement("div");
    chatHeader.className = "flex items-center w-full";
    if (dmUser != null) {
        const onlineStatus = document.createRange().createContextualFragment(`<svg class="absolute overflow-visible -translate-[10px] -translate-y-[15px]" xmlns="http://www.w3.org/2000/svg" width="12" height="12" color="#646cff"><circle cy="4" r="8" fill="currentColor"/></svg>`);
        onlineStatus.innerHTML = `<circle cy="4" r="8" fill="currentColor"/>`;
        const svgElement = onlineStatus.querySelector('svg');
        svgElement.id = "online_status_" + dmUser;
        svgElement.style.display = "none";
        chatHeader.appendChild(onlineStatus);
    }
    const chatNameElement = document.createElement("a");
    chatNameElement.className ="truncate";
    chatNameElement.textContent = chatName;
    chatNameElement.id = "chat_name_" + chatId;
    chatHeader.appendChild(chatNameElement);
    return chatHeader
}

async function msgToLastMessage(msg) {
    if (msg.isFunctional){
        return replaceUUIDs(msg.msg, dm_to_user);
    }
    return (msg.senderId == getCurrentUserId() ? "You: " : (await getUsername(msg.senderId) + ": ")) + msg.msg;
}

async function msgToLastMessageRaw(msg, sender) {
    return (sender == getCurrentUserId() ? "You: " : (await getUsername(sender) + ": ")) + msg;
}

const DEFAULT_CHAT_PREVIEW_CLASS = "flex items-center gap-1 overflow-hidden";

async function buildChatPreviewContent(chatId, lastMessage, unreadCount){
    const chatPreviewContent = document.createElement("div");
    chatPreviewContent.className = DEFAULT_CHAT_PREVIEW_CLASS
    chatPreviewContent.id = "chat_preview_content_" + chatId;

    const lastMessageEl = document.createElement("div");
    lastMessageEl.className = "truncate w-full chat-extra last_message";
    lastMessageEl.id = "last_message_" + chatId;
    lastMessageEl.textContent = await msgToLastMessage(lastMessage);
    lastMessageEl.setAttribute("data-message-id", lastMessage.messageId);
    chatPreviewContent.appendChild(lastMessageEl);

    const chatTypingEl = document.createElement("div");
    chatTypingEl.className = "flex w-full chat-extra chat_typing gap-1";
    chatTypingEl.id = "chat_typing_" + chatId;
    const typingAnim = document.createElement("img");
    typingAnim.src = "/assets/SvgSpinners3DotsScale.svg";
    typingAnim.alt = "/src/SvgSpinners3DotsScale.svg";
    chatTypingEl.appendChild(typingAnim);
    const chatTypingName = document.createElement("span");
    chatTypingName.className = "truncate";
    chatTypingName.id = "chat_typing_name_" + chatId;
    chatTypingEl.appendChild(chatTypingName);

    const unreadCountEl = document.createElement("div");
    unreadCountEl.className = "unreaden-count";
    unreadCountEl.id = "unreaden_count_" + chatId;
    unreadCountEl.textContent = unreadCount;
    if (unreadCount > 0) unreadCountEl.style.display = "block";
    else unreadCountEl.style.display = "none";
    
    chatPreviewContent.appendChild(chatTypingEl);
    chatPreviewContent.appendChild(unreadCountEl);
    return chatPreviewContent
}

export async function addChat(chatName, chatId, isDm, members, lastMessage, unreadCount) {
    console.log("Adding chat: ", chatName, chatId, isDm, members, lastMessage, unreadCount)
    let dmUser = isDm ? (members[0] == getCurrentUserId() ? members[1] : members[0]) : null;
    chats.add(chatId);
    console.log("dmUser: ", dmUser)
    let chat_name = await getChatName(chatId, chatName, dmUser);
    console.log("chat_name: ", chat_name)
    const chatContainer = document.getElementById("chats_container");
    const newChat = buildNewChat(chatId, chat_name);
    last_messages[chatId] = lastMessage;
    unread_counts[chatId] = unreadCount;
    newChat.appendChild(buildChatHeader(chatId, chat_name, dmUser));
    newChat.appendChild(await buildChatPreviewContent(chatId, lastMessage, unreadCount));
    chatContainer.appendChild(newChat);
}

export function removeChat(chatId) {
    if (dms.has(chatId)) {
        dms.delete(chatId)
        delete user_to_dm[dm_to_user[chatId]]
        delete dm_to_user[chatId]
    }
    chats.delete(chatId);
    const chat = document.getElementById("chat_" + chatId);
    console.log("Removing chat: ", chatId, chat)
    chat.remove();
    if (current_chat == chatId) closeChat();
}


export function closeChat(){
    clearChat();
    const name = document.getElementById("chat_name");
    name.textContent = "";
    current_chat = null;
}

export function openChat(chatId) {
    clearChat();
    const nameSrc = document.getElementById("chat_name_" + chatId);
    const name = document.getElementById("chat_name");
    name.textContent = nameSrc.textContent;
    current_chat = chatId;
    setUnreadenCounter(chatId, 0);
    readMessages(chatId, -1);
    getMsgs(chatId, -1, false, 30);
}

export function clearChat(){
    MessagesContainer.innerHTML = "";
    chat_messages = [];
    sent_msgs = new Set();
    fetching_history = false;
    end_of_dm = false;
}


// handler for msgSent to prevent duplicates, you can remove it, but this may increase the delay as the message will go through rabbitmq instead of insta-response
export function addSentMessage(message){ 
    // i will remove it too because sometimes it cause duplicate messages and i dont wanna fix it :)
    return;
    if (current_chat == null || current_chat != message.chatId) return
    sent_msgs.add(message.messageId)
    chat_messages.push(message)
    console.log("Adding message: ", message)
    showMessages()
}

export async function addMessages(messages) {
    if (messages.length == 0) end_of_dm = true
    if (current_chat == null || messages.length == 0 || current_chat != messages[0].chatId) return
    console.log("Before filter: ", messages)
    console.log("Sent msgs: ", sent_msgs)
    messages = messages.filter(v => !sent_msgs.has(v.messageId))
    console.log("Adding messages: ", messages)
    messages = await Promise.all(
        messages.map(async (m) => {
            if (m.isFunctional) {
                m.msg = await replaceUUIDs(m.msg);
            }
            return m;
        })
    );
    chat_messages = chat_messages.concat(messages)
    showMessages()
    fetching_history = false;
}


export async function handleNewMessage(message) {
    addMessages([message])
    console.log("Handling new message: ", message)
    console.log("Chatid: ", message.chatId)

    const chatElement = document.getElementById("chat_preview_content_" + message.chatId);
    if (chatElement) {
        chatElement.classList.remove("typing");
        console.log("rm typing")
    }

    const lastMessageEl = document.getElementById("last_message_" + message.chatId);
    lastMessageEl.textContent = await msgToLastMessage(message);
    lastMessageEl.setAttribute("data-message-id", message.messageId);
    if (message.senderId == getCurrentUserId()) {return;}
    if (message.chatId == current_chat && isScrolledToBottom(MessagesContainer)) {
        console.log("Scrolled to bottom, read all messages")
        readMessages(message.chatId, message.messageId);
    } else {
        console.log("Adding to unread count")
        addUnreadCount(message.chatId, 1)
    }
}

export function addUnreadCount(chatId, unreadCount){
    const unreadCountEl = document.getElementById("unreaden_count_" + chatId);
    console.log("Adding unread count: ", unreadCount)
    console.log("Current unread count: ", unreadCountEl.textContent)
    setUnreadenCounter(chatId, parseInt(unreadCountEl.textContent) + unreadCount)
}

export function setUnreadenCounter(chatId, newCount) {
    console.log("Setting unread count: ", chatId, newCount)
    const unreadCountEl = document.getElementById("unreaden_count_" + chatId);
    if (newCount <= 0) {
        unreadCountEl.style.display = "none";
        unreadCountEl.textContent = "0";
    } else {
        unreadCountEl.style.display = "block";
        unreadCountEl.textContent = newCount;
    }
}


export function clearUserChats() {
    const chatContainer = document.getElementById("chats_container");
    chatContainer.innerHTML = "";
}

function getFileExtension(filename) {
    const match = filename.match(/\.([^.]+)$/);
    return match ? match[1] : "txt";
}

function removeUUID(filename) {
    return filename.replace(/^[a-f0-9-]+_/, '');
}

function getFilename(filepath) {
    return filepath.split('/').pop();
}

function createMediaComponent(fileLink) {
    const extension = getFileExtension(fileLink);
    let mediaElement;
    if (['jpg', 'jpeg', 'png', 'gif', 'bmp', 'webp'].includes(extension)) {
        mediaElement = `<div class="media-component image flex"><img src="${fileLink}" alt="Image" class="media-image" /></div>`;
    } else if (['mp4', 'webm', 'ogg', 'avi'].includes(extension)) {
        mediaElement = `<div class="media-component video flex" width="100%"><video controls width="100%"><source src="${fileLink}" type="video/${extension}">Your browser does not support the video tag.</video></div>`;
    } else if (['mp3', 'ogg', 'wav', 'aac'].includes(extension)) {
        mediaElement = `<div class="media-component audio flex"><audio controls><source src="${fileLink}" type="audio/${extension}">Your browser does not support the audio element.</audio></div>`;
    } else {
        mediaElement = `<div class="media-component unsupported">Unsupported media type: ${extension}</div>`;
    }
    return mediaElement;
}

async function showMessages() {
    chat_messages.sort((a, b) => b.messageId - a.messageId)
    const msgsContainer = MessagesContainer;
    msgsContainer.innerHTML = "";
    console.log("showing", chat_messages)
    console.log("showing", chat_messages.length)

    for (var msg of chat_messages) {
        console.log(msg)
        const msgEl = document.createElement("div");
        msgEl.id = "chat_message_" + msg.messageId;
        if (msg.isFunctional) {
            msgEl.className = "functional_message"
            msgEl.textContent = msg.msg;
        } else {
            let own_message = msg.senderId == getCurrentUserId()
            if (own_message) {
                msgEl.className = "message own_message"
            } else {
                msgEl.className = "message other_message"
                const usernameEl = document.createElement("div");
                usernameEl.textContent = await getUsername(msg.senderId);
                usernameEl.className = "username";
                msgEl.appendChild(usernameEl);
            }
            const textEl = document.createElement("div");
            textEl.id = "chat_message_text_" + msg.messageId;
            textEl.textContent = msg.msg;
            const date = new Date(msg.date);
            // to 24 hr daytime
            const dateTimeStr = date.toLocaleString('en-US', { hour: 'numeric', minute: 'numeric', hour12: false });
            const timeEl = document.createElement("div");
            timeEl.textContent = dateTimeStr;
            timeEl.className = "font-mono color-gray time";
            msgEl.appendChild(textEl);
            if (msg.attachments != null && msg.attachments.length != 0){
                msg.attachments.forEach(file_link => {
                    if (file_link.startsWith("FILE::")) {
                        const link = fileApi + file_link.substring(6);
                        //fileApi
                        const name = removeUUID(getFilename(link));
                        const fileEl = document.createElement("div");
                        fileEl.className = "file"
                        const fileExt = document.createElement("a");
                        fileExt.className = "file_ext";
                        fileExt.download = name;
                        // MAY WORK ONLY ON SAME-DOMAIN HOSTED FILES!!!!
                        fileExt.href = link
                        fileExt.textContent = "." + getFileExtension(link)
                        const fileName = document.createElement("div");
                        fileName.className = "file_name";
                        fileName.textContent = name
                        fileEl.appendChild(fileExt);
                        fileEl.appendChild(fileName);
                        msgEl.appendChild(fileEl);
                    } else {
                        const mediaEl = document.createRange().createContextualFragment(createMediaComponent(fileApi + file_link));
                        msgEl.appendChild(mediaEl);
                    }
                });
            }
            msgEl.appendChild(timeEl);
            if (own_message) {
                const msgId = msg.messageId; 
                textEl.addEventListener("click", () => beginEditMessage(msgEl, textEl, msgId));
            }
        }
        msgsContainer.appendChild(msgEl);
    }
}

function beginEditMessage(msgEl, textEl, msgId) {
    console.log("Editing", msgId);
    const originalText = textEl.textContent;
    const input = document.createElement("textarea");
    // input.type = "text";
    input.value = originalText;
    input.className = "edit_input";
    input.id = "edit_input_" + msgId;
    const saveButton = document.createElement("button");
    saveButton.textContent = "Save";
    saveButton.className = "edit_button";
    saveButton.id = "edit_button_" + msgId;
    saveButton.addEventListener("click", () => saveEditedMessage(input, textEl, msgEl, msgId));

    const deleteButton = document.createElement("button");
    deleteButton.textContent = "Delete";
    deleteButton.className = "edit_delete_button";
    deleteButton.id = "edit_delete_button_" + msgId;
    deleteButton.addEventListener("click", () => deleteMessage(msgId));

    const cancelButton = document.createElement("button");
    cancelButton.textContent = "Cancel";
    cancelButton.className = "edit_cancel_button";
    cancelButton.id = "edit_cancel_button_" + msgId;
    cancelButton.addEventListener("click", () => cancelEdit(msgEl, textEl, input, saveButton, deleteButton, cancelButton));


    msgEl.replaceChild(input, textEl);
    msgEl.appendChild(saveButton);
    msgEl.appendChild(deleteButton);
    msgEl.appendChild(cancelButton);

    input.focus();
}

function cancelEdit(msgEl, textEl, input, saveButton, deleteButton, cancelButton) {
    msgEl.replaceChild(textEl, input);
    saveButton.remove();
    deleteButton.remove();
    cancelButton.remove();
}

function saveEditedMessage(input, textEl, msgEl, msgId) {
    const newText = input.value.trim();
    if (newText === "") {
        deleteMessage(msgId);
        return;
    }
    msgEl.replaceChild(textEl, input);
    editMessage(msgId, newText);
    document.getElementById("edit_button_" + msgId).remove();
    document.getElementById("edit_delete_button_" + msgId).remove();
    document.getElementById("edit_cancel_button_" + msgId).remove();
}



const messageInput = document.getElementById("message_input");
messageInput.addEventListener("keydown", function(event) {
    if (event.key === "Enter") {
        if (!event.shiftKey) {
            event.preventDefault();
            sendMessage();
        }
    }
});

document.getElementById("send_message").addEventListener("click", function() {
    sendMessage()
});

document.getElementById("send_message_media").addEventListener("click", function() {
    document.getElementById("media-input").click();
});

document.getElementById("send_message_file").addEventListener("click", function() {
    document.getElementById("files-input").click();
});

document.getElementById("files-input").addEventListener("change", function(e) {
    sendFiles(e);
});

document.getElementById("media-input").addEventListener("change", function(e) {
    sendFiles(e, true);
});

function sendFiles(e, as_media=false) {
    const files = e.target.files;
    if (files.length > 9) {
        alert("You can select up to 9 files only.");
        return null;
    }
    for (let i = 0; i < files.length; i++) {
        if (files[i].size > 1 * 1024 * 1024 * 1024) {
            alert(`File ${files[i].name} is too large. Max file size is 1GB.`);
            return null;
        }
    }
    uploadFiles(files, as_media);
}
async function uploadFiles(files, as_media=false) {
    const formData = new FormData();
    for (let i = 0; i < files.length; i++) {
        formData.append("files", files[i]);
    }
    try {
        const response = await fetch(`${apiBase}/Files/Upload`, {
            method: "POST",
            body: formData
        });
        if (!response.ok) {
            throw new Error("Upload failed");
        }
        const data = await response.json();
        console.log("Files uploaded successfully:", data);
        var links = data.links
        if (!as_media) {links = data.links.map(l => "FILE::" + l);}
        sendMessage(links);
    } catch (error) {
        alert("Error uploading files: " + error.message);
    }
}


function sendMessage(ext=null){
    const message = messageInput.value.trim();
    messageInput.value = "";
    if (current_chat == null) return
    if (message != "" || (ext != null && ext.length > 0)) {
        scrollToBottom();
        readMessages(current_chat, -1);
        sendMsg(current_chat, message, ext)
    }
}

document.getElementById("leave_chat").addEventListener("click", function() {
    if (current_chat == null) return;
    leaveGroup(current_chat);
})

document.getElementById("delete_chat").addEventListener("click", function() {
    if (current_chat == null) return;
    deleteChat(current_chat);
})

document.getElementById("copy_chat_id").addEventListener("click", function() {
    if (current_chat == null) {
        alert("No chat selected");
        return;
    }
    navigator.permissions.query({ name: "clipboard-write" }).then(function(result) {
        if (result.state === "granted" || result.state === "prompt") {
            navigator.clipboard.writeText(current_chat).then(() => {
                alert("Chat ID copied to clipboard!");
            }).catch(err => {
                alert("Failed to copy text: " + err);
            });
        } else {
            alert("Clipboard access denied. Please grant permission.");
        }
    }).catch(err => {
        alert("Error checking clipboard permission: " + err);
    });
})

MessagesContainer.addEventListener("scroll", () => {
    if (MessagesContainer.scrollTop < 200 && !fetching_history && !end_of_dm && current_chat != null && chat_messages.length > 0) {
        console.log("Fetching more messages");
        getMsgs(current_chat, chat_messages[chat_messages.length - 1].messageId, false, 30);
        fetching_history = true;
    }
    if (isScrolledToBottom(MessagesContainer)){
        readMessages(chatId, -1);
    }
});

function isScrolledToBottom(container) {
    return container.scrollHeight - container.scrollTop <= container.clientHeight + 1;
}


export function updateOnlineUsers(users) {
    for (const key in users) {
        const status = document.getElementById("online_status_" + key);
        if (status == null) {continue;}
        if (!users[key]) {
            status.style.display = "none";
        } else {
            status.style.display = "block";
        }
    }
}


let lastSentTime = 0; 
document.getElementById("message_input").addEventListener("keydown", function(event) {
    if (current_chat == null) return;
    const currentTime = Date.now();
    if (currentTime - lastSentTime >= 500) {
        typing(current_chat);
        lastSentTime = currentTime;
    }
});


export async function UpdateChatTyping(chatTyping) {
    for (const key of chats) {
        var val = chatTyping[key];
        const chatElement = document.getElementById("chat_preview_content_" + key);
        if (val == null || val == "0") {
            if (chatElement) {
                chatElement.classList.remove("typing");
            }
            continue;
        }
        const chatTypingText = document.getElementById("chat_typing_name_" + key);
        if (chatElement == null || chatTypingText == null) {
            continue;
        }
        var t = "";
        var i = parseInt(val);
        if (i) {
            t = i + " users typing";
        } else {
            t = await replaceUUIDs(val) + " is typing";
        }
        chatTypingText.textContent = t;
        chatElement.classList.add("typing");
    }
}


export function RenameGroupChat(chatId, newName) {
    const chatElement = document.getElementById("chat_preview_content_" + chatId);
    if (chatElement == null) {
        return;
    }
    const chatName = document.getElementById("chat_name_" + chatId);
    if (chatName == null) {
        return;
    }
    if (current_chat == chatId) {
        const name = document.getElementById("chat_name");
        name.textContent = newName;
    }
    chatName.textContent = newName;
}

function scrollToBottom() {
    MessagesContainer.scrollTop = MessagesContainer.scrollHeight;
}

export function deleteMessageHandler(senderId, chatId, messageId) {
    const lastMessageEl = document.querySelector(`.last_message[data-message-id="${messageId}"]`);
    if (lastMessageEl) {
        lastMessageEl.textContent = "[DELETED]";
        // TODO: request last message?
    }
    const messageEl = document.getElementById("chat_message_" + messageId);
    if (messageEl) {
        messageEl.remove();
    }
}

export async function editMessageHandler(senderId, chatId, messageId, newMessage) {
    const lastMessageEl = document.querySelector(`.last_message[data-message-id="${messageId}"]`);
    if (lastMessageEl) {
        lastMessageEl.textContent = await msgToLastMessageRaw(newMessage, senderId);
    }
    const messageEl = document.getElementById("chat_message_text_" + messageId);
    if (messageEl) {
        messageEl.textContent = newMessage;
    }
}

