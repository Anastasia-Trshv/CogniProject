import React from 'react';
import Profile from "../Profile/Profile"
import Header from "../Layouts/Header/Header"
import Navigation from "../Layouts/Navigation/Navigation"
import { BrowserRouter, Route, Routes, useLocation } from 'react-router-dom';
import './Home.css';

function Home() {
    const location = useLocation();

    return (
        <div className="home">
            <Header></Header>
            <div className="home__bg">
                <div className="home__wrapper">
                    <Navigation></Navigation>
                    {location.pathname === "/profile" && <Profile />}
                </div>
            </div>
        </div>
    );
};

export default Home;

