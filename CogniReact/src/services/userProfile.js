import axios from "axios"

export const API_URL = "https://localhost:7055";

const $api = axios.create({
    baseURL: API_URL,
    withCredentials: true,
});

$api.interceptors.request.use((config) => {
    config.headers.Authorization = `Bearer ${localStorage.getItem('aToken')}`
    return config;
});

$api.interceptors.response.use((config) => {
    return config;
}, async (error) => {
    console.log(error.config);
});

export const getUserById = async (userId) => {
    try {
        let response = await $api.get("/User/GetUserById", { params: { id: userId } });
        return response;
    } catch(e) {
        console.log(1);
    }
}
