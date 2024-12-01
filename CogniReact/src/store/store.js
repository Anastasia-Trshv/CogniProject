import { createUser, loginUser } from "../services/auth.js";
import { getUserById } from "../services/userProfile.js";

export default class Store {

    async register(user) {
        try {
            const response = await createUser(user);
            localStorage.setItem('aToken', response.data.aToken);
            localStorage.setItem('rToken', response.data.rToken);
            localStorage.setItem('userId', response.data.id);
            return true;
        } catch (e) {
            console.log(e);
        }
    }

    async login(user) {
        try {
            const response = await loginUser(user);
            localStorage.setItem('aToken', response.data.aToken);
            localStorage.setItem('rToken', response.data.rToken);
            localStorage.setItem('userId', response.data.id);
            return true;
        } catch (e) {
            console.log(e);
        }
    }

    async logout() {
            localStorage.removeItem('aToken');
            localStorage.removeItem('rToken');
            localStorage.removeItem('userId');
    }

    async userInfo(userId) {
        try {
            const response = await getUserById(userId);
            return response.data;
        } catch (e) {
            console.log(e);
        }
    }
}