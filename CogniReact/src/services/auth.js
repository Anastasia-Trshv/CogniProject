import axios from "axios"
import { API_URL } from "../http";

const api = axios.create({
    baseURL: API_URL,
    withCredentials: true,
});

export const createUser = async (user) => {
    try {
        let response = await api.post("/User/CreateUser", user);
        return response;
    } catch(e) {
        console.log(e);
    }
}

export const loginUser = async (user) => {
    try {
        let response = await api.post("/User/GetUserByEmail", user);
        return response;
    } catch(e) {
        console.log(e);
    }
}

//Валидация
export const isFormValid = (user, passwordRepeat) => {
    var reply = {status: true, name: false, surname: false, email: false, password: false, passwordRepeat: false};

    if (!validateName(user?.name)) {
        reply.name = true;
        reply.status = false;
    }
    if (!validateSurname(user?.surname)) {
        reply.surname = true;
        reply.status = false;
    }
    if (!validateEmail(user?.email)) {
        reply.email = true;
        reply.status = false;
    }
    if (!validatePassword(user?.password)) {
        reply.password = true;
        reply.status = false;
    }
    if (!validatePasswordRepeat(user?.password, passwordRepeat)) {
        reply.passwordRepeat = true;
        reply.status = false;
    }
    return reply;
};


//Валидация имени
const validateName = (name) => {
    var re = /^[\u0400-\u04FF]+$/;
    return re.test(name) && name[0] == name[0].toUpperCase();
};

//Валидация Фамилии
const validateSurname = (surname) => {
    var re = /^[\u0400-\u04FF]+$/;
    return re.test(surname) && surname[0] == surname[0].toUpperCase();
};

//Валидация email
const validateEmail = (email) => {
    return email.match(
      /^(([^<>()[\]\\.,;:\s@\"]+(\.[^<>()[\]\\.,;:\s@\"]+)*)|(\".+\"))@((\[[0-9]{1,3}\.[0-9]{1,3}\.[0-9]{1,3}\.[0-9]{1,3}\])|(([a-zA-Z\-0-9]+\.)+[a-zA-Z]{2,}))$/
    );
};

//Валидация password
const validatePassword = (password) => {
    var re = /^(?=.*\d)(?=.*[!@#$%^&*])(?=.*[a-zA-Z]).{8,}$/;
    return re.test(password);
};

//Проверка повтора пароля
const validatePasswordRepeat = (password, passwordRepeat) => {
    return password == passwordRepeat && !!passwordRepeat.length;
};