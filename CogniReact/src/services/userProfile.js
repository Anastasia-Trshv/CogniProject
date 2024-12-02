import axios from "axios"

const API_URL = "https://localhost:7055";

const $api = axios.create({
    baseURL: API_URL,
    withCredentials: true,
});

$api.interceptors.request.use((config) => {
    config.headers.Authorization = `Bearer ${localStorage.getItem('aToken')}`
    config.headers["Refresh-Token"] = `${localStorage.getItem('rToken')}`
    return config;
});

const refreshAccessToken = async (userId) => {
    try {
        const userId = localStorage.getItem('userId');
        const response = await $api.get(`${API_URL}/Token/Refresh`, { params: { id: userId} });
        var aToken = response.data;
        localStorage.setItem("aToken", aToken);
        return aToken;
    } catch (error) {
        console.error("Ошибка при обновлении токена:", error);
    }
};

$api.interceptors.response.use((config) => {
    return config;
}, async (error) => {
    const originalRequest = error.config;
    if (error.response.status == 401 && !originalRequest._retry) {
        originalRequest._retry = true;
        try {
            const newAccessToken = await refreshAccessToken();
            originalRequest.headers.Authorization = `Bearer ${newAccessToken}`;
            return $api.request(originalRequest);
        } catch (e) {
            console.error("Не удалось обновить токен. Переход на логин.");
        }
    }
    console.log(error.config);
});

export const getUserById = async (userId) => {
    try {
        let response = await $api.get("/User/GetUserById", { params: { id: userId } });
        return response;
    } catch(e) {
        console.error(1);
    }
}

export const getUserPosts = async (userId) => {
    try {
        let response = await $api.get("/Cogni/GetAllPosts");
        console.log(response);
        return response;
    } catch(e) {
        console.error(1);
    }
}
