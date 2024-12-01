import React from "react";
import LoginForm from "./Components/Auth/Login/LoginForm";
import RegisterForm from "./Components/Auth/Register/RegisterForm";
import Home from "./Components/Home/Home";
import Profile from "./Components/Profile/Profile";
import Settings from "./Components/Settings/Settings";
import About from "./Components/About/About";
import { BrowserRouter, Route, Routes, Item } from 'react-router-dom';
import './App.css';
function App() {

  return (
    <>
      <BrowserRouter>
        <Routes>
          <Route path="login" element={<LoginForm />}/>
          <Route path="register" element={<RegisterForm />}/>
          <Route path="/" element={<Home />}>
            <Route path="profile" element={<Profile />}/>
            <Route path="settings" element={<Settings />}/>
            <Route path="about" element={<About />}/>
          </Route>
        </Routes>
      </BrowserRouter>
    </>
  );
}

export default App;
