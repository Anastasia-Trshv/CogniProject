import LoginForm from "./Components/Auth/Login/LoginForm"
import RegisterForm from "./Components/Auth/Register/RegisterForm"
import Profile from "./Components/Profile/Profile"
import React from "react";
import { BrowserRouter, Route, Routes } from 'react-router-dom';
import './App.css';

function App() {
  return (
    <>
      <BrowserRouter>
        <Routes>
          <Route path="login" element={<LoginForm />}/>
          <Route path="register" element={<RegisterForm />}/>
          <Route path="profile" element={<Profile />}/>
        </Routes>
      </BrowserRouter>
    </>
  );
}

export default App;
